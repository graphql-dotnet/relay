const path = require('path')
const RelayCompilerWebpackPlugin = require('@dhau/relay-compiler-webpack-plugin')
const { plugins, rules } = require('webpack-atoms')

module.exports = {
  devtool: "cheap-module-source-map",
  entry: './ClientApp/app.js',
  output: {
    path: `${__dirname}/wwwroot/dist`,
    filename: 'bundle.js',
    publicPath: '/'
  },
  module: {
    rules: [
      rules.js(),
      rules.css(),
      rules.images(),
    ]
  },
  plugins: [
    plugins.extractText({ disable: true }),
    new RelayCompilerWebpackPlugin({
      schema: path.resolve(__dirname, 'wwwroot/schema.json'),
      src: path.resolve(__dirname, './ClientApp'),
    })
  ]
}
