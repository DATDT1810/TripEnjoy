import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'http://localhost:5000/api', // Địa chỉ của Web API ASP.NET Core
    headers: {
        'Content-Type': 'application/json',
    },
});

export default {
    // Ví dụ: Gọi API đăng nhập
    login(user) {
        return apiClient.post('/account/login', user);
    },

    // Ví dụ: Gọi API đăng ký
    register(user) {
        return apiClient.post('/account/register', user);
    },

    // Các hàm API khác tùy vào yêu cầu
};
