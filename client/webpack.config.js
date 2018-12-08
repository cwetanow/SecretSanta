const HtmlWebPackPlugin = require("html-webpack-plugin");
const path = require('path');

const htmlPlugin = new HtmlWebPackPlugin({
  template: "./src/index.html",
  filename: "./index.html"
});

module.exports = {
  entry: ["babel-polyfill", "./src/app.js"],
  output: {
    path: path.resolve(__dirname, '../server/SecretSanta.Web/wwwroot'),
    publicPath: '/'
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: {
          loader: "babel-loader"
        }
      },
      {
        test: /\.css$/,
        use: ["style-loader", "css-loader"]
      },
      {
        test: /\.jsx?$/,
        loader: 'babel-loader',
        exclude: /node_modules/,
        query: {
          cacheDirectory: true,
          presets: ['@babel/react']
        }
      }
    ]
  },
  resolve: {
    extensions: ['*', '.js', '.jsx']
  },
  plugins: [htmlPlugin],
  devServer: {
    proxy: {
      '/api/message': { target: 'http://localhost:1235/', secure: false },
      '/api': { target: 'http://localhost:5000/', secure: false },
      changeOrigin: true
    },
    historyApiFallback: true
  }
};