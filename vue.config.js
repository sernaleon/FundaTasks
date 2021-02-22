module.exports = {
  "devServer": {
    proxy: {
      '/api': {
        target: 'http://localhost:7071',
        logLevel: 'debug',
        ignorePath: false,
        ws: true
      }
    },
  },
  "transpileDependencies": [
    "vuetify"
  ]
}