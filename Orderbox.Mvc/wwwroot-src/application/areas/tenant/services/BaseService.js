import axios from 'axios';
import config from '../config';

export default class BaseService {
    constructor(baseController) {
        this.baseController = baseController;
        this.url = `${config.apiUrl}/${baseController}`;
    }

    _Delete(url, tenantName, accessToken) {
        return axios.put(url, {
            headers: {
                'X-Tenant-Name': tenantName,
                'Authorization': `Bearer ${accessToken}`
            }
        });
    }

    _Get(url, tenantName, accessToken) {
        return axios.get(url, {
            headers: {
                'X-Tenant-Name': tenantName,
                'Authorization': `Bearer ${accessToken}`
            }
        });
    }

    _Patch(url, data, tenantName, accessToken) {
        return axios.patch(url, data, {
            headers: {
                'X-Tenant-Name': tenantName,
                'Authorization': `Bearer ${accessToken}`
            }
        });
    }

    _Post(url, data, tenantName, accessToken) {
        return axios.post(url, data, {
            headers: {
                'X-Tenant-Name': tenantName,
                'Authorization': `Bearer ${accessToken}`
            }
        });
    }

    _Put(url, data, tenantName, accessToken) {
        return axios.put(url, data, {
            headers: {
                'X-Tenant-Name': tenantName,
                'Authorization': `Bearer ${accessToken}`
            }
        });
    }

    Delete(id, tenantName, accessToken) {
        return this._Delete(`${this.url}/${id}`, tenantName, accessToken);
    }

    Insert(model, tenantName, accessToken) {
        return this._Post(`${this.url}`, model, tenantName, accessToken);
    }

    PagedSearch(model, tenantName, accessToken) {
        let params = new URLSearchParams(model).toString();
        return this._Get(`${this.url}?${params}`, tenantName, accessToken);
    }

    Read(id, tenantName, accessToken) {
        return this._Get(`${this.url}/${id}`, tenantName, accessToken);
    }

    Update(id, model, tenantName, accessToken) {
        return this._Put(`${this.url}/${id}`, model, tenantName, accessToken);
    }
}