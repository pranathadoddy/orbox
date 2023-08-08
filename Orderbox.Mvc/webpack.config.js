const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const TerserJSPlugin = require('terser-webpack-plugin');
const OptimizeCssAssetsPlugin = require('optimize-css-assets-webpack-plugin');

var path = require('path');

const bundlePrefixName = {
    development: "dev",
    staging: "staging",
    production: "prod"
};

var config = {
    entry: {
        'landingpage': './wwwroot-src/application/landingpage.js',
        'notloggedin': './wwwroot-src/application/notloggedin.js',
        'app-administrator': './wwwroot-src/application/areas/administrator.js',
        'app-agent': './wwwroot-src/application/areas/agent.js',
        'app-user': './wwwroot-src/application/areas/user.js',
        'app-tenant': './wwwroot-src/application/areas/tenant.jsx'
    },
    resolve: {
        extensions: ['*', '.js', '.jsx']
    },
    module: {
        rules: [
            {
                test: /\.(js)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader",
                    options: {
                        "presets": ["@babel/preset-env"],
                        "plugins": [["@babel/plugin-proposal-class-properties"]]
                    }
                }
            },
            {
                test: /\.(jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader",
                    options: {
                        "presets": ["@babel/preset-env", "@babel/preset-react"],
                        "plugins": [["@babel/plugin-proposal-class-properties"]]
                    }
                }
            },
            {
                test: /\.(css|scss)$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    "sass-loader"
                ]
            },
            {
                test: /\.svg$/,
                loader: "svg-inline-loader"
            },
            {
                test: /\.(woff(2)?|ttf|eot|svg|gif|png)(\?v=\d+\.\d+\.\d+)?$/,
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
            { test: require.resolve('jquery'), use: [{ loader: 'expose-loader', options: 'jQuery' }, { loader: 'expose-loader', options: '$' }] }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: `[name]-${bundlePrefixName[process.env.NODE_ENV]}.min.css`
        })
    ]
};

module.exports = (env, argv) => {
    if (process.env.NODE_ENV === 'development') {
        config.output = {
            filename: `[name]-${bundlePrefixName[process.env.NODE_ENV]}.js`,
            path: __dirname + '/wwwroot/dist',
            publicPath: '/dist/'
        };
        config.devtool = 'source-map';
        config.mode = 'development';
    }

    if (process.env.NODE_ENV === 'staging' || process.env.NODE_ENV === 'production') {
        config.output = {
            filename: `[name]-${bundlePrefixName[process.env.NODE_ENV]}.min.js`,
            path: __dirname + '/wwwroot/dist',
            publicPath: '/dist/'
        };
        config.mode = 'production';
        config.optimization = {
            namedModules: false,
            namedChunks: true,
            nodeEnv: process.env.NODE_ENV,
            flagIncludedChunks: true,
            occurrenceOrder: true,
            sideEffects: true,
            usedExports: true,
            concatenateModules: true,
            splitChunks: {
                hidePathInfo: true,
                minSize: 30000,
                maxAsyncRequests: 5,
                maxInitialRequests: 3
            },
            noEmitOnErrors: true,
            checkWasmTypes: true,
            minimize: true,
            minimizer: [
                new TerserJSPlugin({
                    cache: true,
                    parallel: true,
                    terserOptions: { output: { comments: false } }
                }),
                new OptimizeCssAssetsPlugin({
                    cssProcessorPluginOptions: {
                        preset: ['default', { discardComments: { removeAll: true } }]
                    }
                })
            ]
        };
    }

    return config;
};