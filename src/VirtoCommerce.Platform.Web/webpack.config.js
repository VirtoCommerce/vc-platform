// webpack.config.js
const path = require('path');
const glob = require('glob');
const webpack = require('webpack');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const {CleanWebpackPlugin} = require('clean-webpack-plugin');

const rootPath = path.resolve(__dirname, 'wwwroot/dist');

module.exports = (env, argv) => {
    const isProduction = argv.mode === 'production';

    return {
        entry: {
            vendor: ['./wwwroot/src/js/vendor.js', './wwwroot/css/themes/main/sass/main.sass'],
            app: glob.sync('./wwwroot/js/**/*.js'),
        },
        devtool: isProduction ? 'source-map' : 'eval-source-map',
        output: {
            path: rootPath,
            filename: '[name].js'
        },
        module: {
            rules: [{
                    test: /.css$/,
                    use: [MiniCssExtractPlugin.loader, 'css-loader'],
                },
                {
                    test: /.(jpe?g|png|gif)$/i,
                    type: 'asset/resource',
                    generator: {
                        filename: 'images/[name][ext]',
                    },
                },
                {
                    test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
                    type: 'asset/resource',
                    generator: {
                        filename: 'fonts/[name][ext]',
                    },
                },
                {
                    test: /.(sass|scss)$/,
                    use: [{
                            loader: MiniCssExtractPlugin.loader
                        },
                        {
                            loader: 'css-loader'
                        },
                        {
                            loader: 'sass-loader',
                            options: {
                                implementation: require('sass'),
                            },
                        },
                    ],
                },
            ],
        },
        plugins: [
            new CleanWebpackPlugin(),
            new MiniCssExtractPlugin({
                filename: 'style.css',
            }),
            new webpack.ProvidePlugin({
                $: 'jquery',
                jQuery: 'jquery',
                'window.jQuery': 'jquery',
                _: 'underscore',
            }),
        ],
        resolve: {
            extensions: ['.js', '.json'],
            alias: {
                Vendor: path.resolve(__dirname, 'wwwroot/vendor')
            },
            fallback: { fs: false }
        },
    };
};
