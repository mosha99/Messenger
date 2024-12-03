const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
   devServer: {
    port: 8081, // شماره پورت جدید
    open: true, // به طور خودکار مرورگر را باز کند
  }
})