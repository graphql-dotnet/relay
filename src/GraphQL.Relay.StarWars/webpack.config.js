


module.exports = {
  entry: './app/entry.js',
  output: {
    path: `${__dirname}/wwwroot`,
    filename: 'bundle.js',
    publicPath: '/'
  },
  module: {
    rules: [
      { test: /\.css/, use: ['style-loader', 'css-loader'] }
    ]
  }
}