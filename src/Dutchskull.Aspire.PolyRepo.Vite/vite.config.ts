import { defineConfig, loadEnv, type ConfigEnv } from 'vite'
import react from '@vitejs/plugin-react'

export default ({ mode }: ConfigEnv) => {
  const env = loadEnv(mode, process.cwd(), '');
  console.log(env.services__apiservice__http__0)
  return defineConfig({
    define: {
      plugins: [react()],
      'import.meta.env.VITE_WEATHER_API': JSON.stringify(env.services__apiservice__http__0),
    },
  });
};