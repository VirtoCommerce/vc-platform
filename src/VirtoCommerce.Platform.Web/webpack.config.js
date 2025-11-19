// webpack.config.js
const path = require('path');
const glob = require('glob');
const webpack = require('webpack');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

const rootPath = path.resolve(__dirname, 'wwwroot/dist');

module.exports = (env, argv) => {
    const isProduction = argv.mode === 'production';

    return {
        entry: {
            vendor: ['./wwwroot/src/js/vendor.js', './wwwroot/css/themes/main/sass/main.sass'],
            app: [
                ...glob.sync('./wwwroot/js/**/*.js'),
                ...(isProduction ? glob.sync('./wwwroot/js/**/*.html', { nosort: true }): [])
            ]
        },
        devtool: isProduction ? 'source-map' : 'eval-source-map',
        output: {
            path: rootPath,
            filename: '[name].js',
            clean: true
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
                {
                    test: /\.html$/,
                    use: [
                        {
                            loader: 'ngtemplate-loader',
                            options: {
                                relativeTo: path.resolve(__dirname, './wwwroot/js/'),
                                prefix: "$(Platform)/Scripts/",
                            }
                        },
                        {
                            loader: "html-loader",
                            options: {
                                sources: false,
                            }
                        }
                    ]
                },
            ],
        },
        plugins: [
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
