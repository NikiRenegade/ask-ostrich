import { defineConfig } from 'vite';
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api/Auth': {
        target: 'http://localhost:8080/security',
        changeOrigin: true,
        secure: false,
      }
    }
  },
  build: {
    sourcemap: true,
  },
})