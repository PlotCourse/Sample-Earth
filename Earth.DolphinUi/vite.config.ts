import { defineConfig } from 'vite';
import { resolve } from 'path';
import { autoDefs } from './vite-defs.g';

export default defineConfig(({ command }) => {
  const outDir = 'dist';
  const isProd = command === 'build';

  const input: Record<string, string> = {};
  for (const inputPath of autoDefs.inputFilePaths) {
    var outName = inputPath
      .replace(/\.ts$/, '')
      .replace(/\.html$/, '');

    if (outName.startsWith('src/')) {
      outName = outName.substring(4);
    }

    input[outName] = resolve(__dirname, inputPath);
  }

  return {
    resolve: {
      alias: {
        '@node_modules': resolve(__dirname, 'node_modules'),
      },
    },
    plugins: [
      {
        name: 'html-script-transform',
        transformIndexHtml(html) {
          if (!isProd) {
            return html.replace(
              '<script type="module" src="/src/',
              '<script type="module" src="/src/dev/' //adds the dev dashboard
            );
          }
          return html;
        }
      }
    ],
    build: {
      outDir,
      emptyOutDir: true,
      rollupOptions: {
        tsconfig: resolve(__dirname, 'tsconfig.json'),
        input,
      },
    },
    base: '/',
    server: {
      host: '127.0.0.1',
      port: process.env.PORT ? parseInt(process.env.PORT) : 5173,
      strictPort: true,
      proxy: autoDefs.proxy
    },
    define: autoDefs.define
  };
});
