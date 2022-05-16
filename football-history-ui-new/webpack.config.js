const HtmlWebpackPlugin = require("html-webpack-plugin");
const HTMLWebpackPlugin = require("html-webpack-plugin");
const path = require("path");

module.exports = {
  entry: "./src/index.tsx",
  mode: "development",
  devtool: "source-map",
  output: {
    path: path.resolve(__dirname, "dist"),
    filename: "bundle.js",
    publicPath: "/"
  },
  plugins: [new HtmlWebpackPlugin({ template: "public/index.html" })],
  module: {
    rules: [
      { test: /\.js$/, loader: "babel-loader", exclude: /node_modules/ },
      { test: /\.(ts|tsx)?$/, loader: "ts-loader", exclude: /node_modules/ }
    ]
  },
  resolve: { extensions: [".ts", ".tsx", ".js", ".json"] },
  devServer: {
    port: 3000,
    open: true,
    hot: true,
    historyApiFallback: true
  }
};
