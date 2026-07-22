export const autoDefs = {
  proxy: {
    '/earth-support': {
      target: process.env["services__earth-support__https__0"] || process.env["services__earth-support__http__0"],
      changeOrigin: true,
      rewrite: path => path.replace(/^\/earth-support/, '')
    },
    '/earth-main': {
      target: process.env["services__earth-main__https__0"] || process.env["services__earth-main__http__0"],
      changeOrigin: true,
      rewrite: path => path.replace(/^\/earth-main/, '')
    },
  },
  define: {
    SERVER_URL_EARTH_SUPPORT: JSON.stringify(
      process.env["services__earth-support__https__0"] ||
      process.env["services__earth-support__http__0"])
  },
  inputFilePaths: [
    'index.html',
  ]
};
