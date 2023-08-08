const config = {
    development: {
        apiUrl: 'https://api.orderbox.id:5001/api',
        stateCookieDomain: 'https://orderbox.id:5001'
    },
    staging: {
        apiUrl: 'https://api-staging.orbox.id',
        stateCookieDomain: 'https://orbox.id'
    },
    production: {
        apiUrl: 'https://api.orbox.id',
        stateCookieDomain: 'https://orbox.id'
    }
};

export default config[process.env.NODE_ENV];