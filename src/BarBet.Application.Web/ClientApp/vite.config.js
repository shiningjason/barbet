import path from 'node:path'
import viteReactPlugin from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

const env = {
  ...(() => {
    const match = /:\/\/([^:]+):(\d+)/.exec(process.env.SPA_SERVER_URL)
    return { spaServerHost: match?.[1], spaServerPort: match?.[2] }
  })(),
  apiServerUrl: process.env.ASPNETCORE_URLS?.split(';').find(url =>
    url.startsWith('https://')
  )
}

export default defineConfig({
  resolve: {
    alias: {
      '@app': path.join(import.meta.dirname, './src')
    }
  },
  plugins: [
    viteReactPlugin({
      babel: {
        plugins: [
          ['@babel/plugin-proposal-decorators', { version: '2023-11' }],
          '@babel/plugin-transform-class-static-block',
          '@babel/plugin-proposal-class-properties'
        ]
      }
    })
  ],
  build: { outDir: 'build' },
  server: {
    host: env.spaServerHost,
    port: env.spaServerPort,
    proxy: {
      '/api': {
        target: env.apiServerUrl,
        changeOrigin: true,
        secure: false
      }
    }
  }
})
