<template>
  <div class="chat-container">
    <!-- فضای نمایش پیام‌ها -->   
    <div class="chat-area">  
      <div class="message-list">
        <div v-for="(message, index) in messages" :key="index"
          :class="['message', message.from.guid === me.guid ? 'mine' : 'theirs']">
          <span class="from">{{ message.from.name }}</span>
          <div class="content">{{ message.message }}</div>
          <span class="time">{{ message.sendTime }}</span>
        </div>
      </div>

      <!-- فرم ورود -->
      <div v-if="!loggedIn" class="message-input">
        <input type="text" v-model="name" placeholder="نام خود را بنویسید..." />
        <button :disabled="name.length < 3" @click="login">شروع</button>
      </div>

      <!-- فرم ارسال پیام -->
      <div v-else class="message-input">
        <input type="text" v-model="newMessage" placeholder="پیام خود را بنویسید..." />
        <button :disabled="!newMessage.trim()" @click="sendMessage">ارسال</button>
      </div>
    </div>
  </div>
</template>

<script>

import * as signalR from "@microsoft/signalr";
import { v4 as uuidv4 } from 'uuid';
import { format } from 'date-fns';

export default {
  data() {
    return {
      connection: null, // اتصال SignalR
      name: "", // نام کاربر
      newMessage: "", // پیام جدید
      messages: [], // لیست پیام‌ها
      loggedIn: false, // وضعیت ورود
      me: null, // اطلاعات کاربر
    };
  },
  methods: {
    async login() {
      try {
        this.me =
        {
          guid: uuidv4(),
          name: this.name,
          device: "hi"
        }; // مشخصات کاربر

        console.log(this.me);

        var url = window.location.origin;

        url = url.replace(/:\d+/, ':8080');

        this.connection = new signalR.HubConnectionBuilder()
          .withUrl(url + "/chathub") // آدرس SignalR
          .configureLogging(signalR.LogLevel.Information)
          .build();

        // تنظیمات دریافت پیام
        this.connection.on("ReceiveMessage", (message) => {
          this.messages.push(message);
        });

        await this.connection.start();
        this.loggedIn = true;
      } catch (err) {
        console.error("خطا در اتصال:", err);
      }
    },
    async sendMessage() {
      if (this.connection) {
        try {
          const message = {
            from: this.me,
            message: this.newMessage,
            sendTime: format(new Date(), 'HH:mm:ss'),
          };

          console.log(message);

          await this.connection.invoke("SendMessage", message);
          this.messages.push(message);
          this.newMessage = "";

        } catch (err) {
          console.error("خطا در ارسال پیام:", err);
        }
      }
    },
  },
};
</script>