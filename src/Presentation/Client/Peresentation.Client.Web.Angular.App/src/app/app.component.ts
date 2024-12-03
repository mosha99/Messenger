import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { v4 as uuidv4 } from 'uuid';
import { format } from 'date-fns';
import { RouterOutlet } from '@angular/router';
import {FormsModule  } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-root',
  imports: [RouterOutlet,CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent {
  connection!: HubConnection | null; // اتصال SignalR
  name: string = ''; // نام کاربر
  newMessage: string = ''; // پیام جدید
  messages: Message[] = []; // لیست پیام‌ها
  loggedIn: boolean = false; // وضعیت ورود
  me?: SenderUser; // اطلاعات کاربر

  async login() {
    try {
      this.me = {
        guid: uuidv4(),
        name: this.name,
        device: 'hi'
      }; // مشخصات کاربر

      console.log(this.me);

      var url = window.location.origin;
      url = url.replace(/:\d+/, ':8080');

      this.connection = new HubConnectionBuilder()
        .withUrl(url + '/chathub') // آدرس SignalR
        .configureLogging(LogLevel.Information)
        .build();

      // تنظیمات دریافت پیام
      this.connection.on('ReceiveMessage', (message) => {
        this.messages.push(message);
      });

      await this.connection.start();
      this.loggedIn = true;
    } catch (err) {
      console.error('خطا در اتصال:', err);
    }
  }

  async sendMessage() {
    if (this.connection) {
      try {
        const message : Message= {
          from: this.me!,
          message: this.newMessage,
          sendTime: format(new Date(), 'HH:mm:ss')
        };

        console.log(message);

        await this.connection.invoke('SendMessage', message);
        this.messages.push(message);
        this.newMessage = '';
      } catch (err) {
        console.error('خطا در ارسال پیام:', err);
      }
    }
  }
}

export interface SenderUser {
  guid: string;
  name: string;
  device: string;
}

export interface Message {
  from: SenderUser;
  message: string;
  sendTime: string;
}