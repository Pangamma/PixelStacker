import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

// https://vitejs.dev/config/
export default defineConfig({
  "base": "/projects/pixelstacker/demo/",
  plugins: [
    react()
  ],
  resolve: {
    alias: [
      { find: '@', replacement: path.resolve(__dirname, 'src') },
      { find: '@utils', replacement: path.resolve(__dirname, 'src/utils') }
    ]
  },
  build: {
    rollupOptions: {
      output: {
        manualChunks: (id: string) => {
          if (id.includes("node_modules")) {
            if (id.includes("react")) {
                return "react";
            }

            return `vendor`; // all other package goes here
          }
        }
      }
    }
  }
})
