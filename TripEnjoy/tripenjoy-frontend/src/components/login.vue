<template>
  <div class="login">
    <h2>Login</h2>
    <form @submit.prevent="login">
      <div>
        <label>Email:</label>
        <input type="email" v-model="email" required />
      </div>
      <div>
        <label>Password:</label>
        <input type="password" v-model="password" required />
      </div>
      <button type="submit">Login</button>
    </form>
  </div>
</template>

<script>
import api from '@/services/api'; // Import API service

export default {
  data() {
    return {
      email: '',
      password: ''
    };
  },
  methods: {
    async login() {
      try {
        const response = await api.login({
          email: this.email,
          password: this.password
        });
        console.log('Login successful:', response.data);
        // Thực hiện hành động sau khi đăng nhập thành công
      } catch (error) {
        console.error('Login failed:', error.response.data);
        // Xử lý lỗi đăng nhập
      }
    }
  }
};
</script>

<style scoped>
/* CSS tùy chỉnh nếu cần */
</style>
