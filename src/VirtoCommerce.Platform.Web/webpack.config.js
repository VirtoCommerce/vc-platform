const path = require('path');
const glob = require('glob');
const webpack = require('webpack');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');

const rootPath = path.resolve(__dirname, 'wwwroot/dist');

function isProductionEnvironment(env) {
    return env && env.prod === true;
}

module.exports = env => {
    return [
        {
            entry: {
                main: ['./wwwroot/src/js/vendor.js', './wwwroot/css/themes/main/sass/main.sass']
            },
            mode: isProductionEnvironment(env) ? 'production' : 'development',
            output: {
                path: rootPath,
                filename: 'vendor.js',
                publicPath: '/dist/'
            },
            module: {
                rules: [
                    {
                        test: /\.js$/,
                        use: {
                            loader: 'babel-loader'
                        }
                    },
                    {
                        test: /\.css$/,
                        loaders: [MiniCssExtractPlugin.loader, "css-loader"]
                    },
                    {
                        test: /\.(jpe?g|png|gif)$/i,
                        loader: "file-loader",
                        options: {
                            name: '[name].[ext]',
                            outputPath: 'images/'
                        }
                    },
                    {
                        test: /\.modernizrrc\.js$/,
                        loader: 'webpack-modernizr-loader'
                    },
                    {
                        test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
                        use: [
                            {
                                loader: 'file-loader',
                                options: {
                                    name: '[name].[ext]',
                                    outputPath: 'fonts/'
                                }
                            }
                        ]
                    },
                    {
                        test: /\.(sass|scss)$/,
                        use: [
                            { loader: MiniCssExtractPlugin.loader },
                            { loader: 'css-loader' },
                            { loader: 'sass-loader', options: { includePaths: [require('node-bourbon').includePaths] } }
                        ]
                    }
                ]
            },
            plugins: [
                new CleanWebpackPlugin(rootPath, { verbose: isProductionEnvironment(env) ? false : true }),
                new MiniCssExtractPlugin({
                    filename: 'style.css'
                }),
                new webpack.ProvidePlugin({
                    $: 'jquery',
                    jQuery: 'jquery',
                    'window.jQuery': 'jquery'
                })
            ],
            resolve: {
                alias: {
                    Vendor: path.resolve(__dirname, 'wwwroot/vendor'),
                    modernizr$: path.resolve(__dirname, ".modernizrrc.js")
                }
            },
            node: {
                fs: 'empty'
            }
        },
        {
            entry: glob.sync('./wwwroot/js/**/*.js'),
            mode: isProductionEnvironment(env) ? 'production' : 'development',
            output: {
                path: rootPath,
                filename: 'app.js',
                publicPath: '/dist/'
            },
            devtool: false,
            plugins: [
                new webpack.SourceMapDevToolPlugin({
                    namespace: 'VirtoCommerce.Platform'
                }),
                new webpack.ProvidePlugin({
                    _: 'underscore'
                })
            ],
            module: {
                rules: [
                    {
                        test: /\.js$/,
                        use: {
                            loader: 'babel-loader'
                        }
                    }
                ]
            },
            node: {
                fs: 'empty'
            }
        }
    ];
};
