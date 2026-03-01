import axios, { AxiosError, type AxiosInstance } from 'axios';

const axiosInstance: AxiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
    timeout: 30000,
});

axiosInstance.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

axiosInstance.interceptors.response.use(
  res => res,
  async error => {
    const originalRequest = error.config;
    
    if (error.response?.status === 401) {
        
        const message = error.response.data?.message || error.message;
        
        if (originalRequest.url.includes("/refresh")) {
            return Promise.reject(new Error(message));
        }

        if (originalRequest._retry) {
            return Promise.reject(new Error(message));
        }

        originalRequest._retry = true;

        try {
            await axiosInstance.post("/security/api/Auth/refresh", null, {
                withCredentials: true
            });

            return axiosInstance(originalRequest);

        } catch (refreshError) {
            return Promise.reject(refreshError);
        }
    }
    return Promise.reject(error);
  }
);

axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    const message = error.response?.data?.message || error.message;
    return Promise.reject(new Error(message));
  }
);

export default axiosInstance;
