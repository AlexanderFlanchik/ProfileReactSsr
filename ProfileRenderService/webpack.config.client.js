const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const { WebpackManifestPlugin } = require('webpack-manifest-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = {
    name: 'client',
    entry: {
        client: path.resolve(__dirname, 'client/client.jsx'),
    },
    mode: 'production',
    output: {
        path: path.resolve(__dirname + '/dist/static'),
        filename: '[name].[contenthash].js',
        publicPath: '',
    },
    resolve: {
        extensions: ['.jsx', '.js', '.less', '.css'],
    },
    target: 'web',
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
                    MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: {
                            modules: {
                                exportLocalsConvention: 'camelCase',
                                localIdentName: '[local]',
                            },
                        },
                    },
                    'less-loader',
                ],
            },
        ],
    },
    plugins: [
        new MiniCssExtractPlugin({ 
            filename: '[name].[contenthash].css',
        }),
        new CleanWebpackPlugin(),
        new WebpackManifestPlugin(),
    ],
}