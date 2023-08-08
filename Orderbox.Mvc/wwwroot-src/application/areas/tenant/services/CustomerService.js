import BaseService from './BaseService';

export default class CustomerService extends BaseService {
    constructor() {
        super('Customer');
    }

    GetByExternalProvider(provider, tenantName, accessToken) {
        return this._Get(`${this.url}/external/${provider}`, tenantName, accessToken);
    }
}