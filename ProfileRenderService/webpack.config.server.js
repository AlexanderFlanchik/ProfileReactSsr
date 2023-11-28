const nodeExternals = require('webpack-node-externals');
const path = require('path');

module.exports = {
  name: 'server',
  entry: {
    server: path.resolve(__dirname, 'server.js'),
  },
  mode: 'production',
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: '[name].js',
  },
  resolve: {
    extensions: ['.js', '.jsx', '.less', '.css'],
  },
  externals: [nodeExternals()],
  target: 'node',
  node: {
    __dirname: false,
  },
  module: {
    rules: [
      {
        test: /\.jsx?$/,
        loader: 'babel-loader',
        exclude: [/node_modules/],
        options: { 
          presets: ['@babel/env','@babel/preset-react'] 
        }
      },
      {
        test: /\.less$/i,
        use: [
          {
            loader: 'css-loader',
            options: {
              modules: {
                exportOnlyLocals: true,
                exportLocalsConvention: 'camelCase',
                localIdentName: '[local]_[hash:base64:5]'
              },
            }
          },
        ],
      },
    ],
  },
}