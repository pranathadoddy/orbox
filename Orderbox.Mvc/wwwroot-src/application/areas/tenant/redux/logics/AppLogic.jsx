import { createLogic } from 'redux-logic';

import {
    REQUEST_CUSTOMER_DATA,
    REQUEST_TENANT_DATA,
    REQUEST_USER_DATA,
    SET_CUSTOMER,
    AuthProvider,
    FetchCustomerComplete,
    FetchCustomerFailed,
    SetCustomerNotRegistered,
    FetchTenantComplete,
    FetchTenantFailed,
    FetchUserComplete,
    FetchUserFailed,
    SetCustomerComplete,
    SetCustomerFailed
} from "../actions/AppAction";

import config from '../../config';

import CustomerService from '../../services/CustomerService';
const _customerService = new CustomerService();

export const FetchCustomerLogic = createLogic({
    type: REQUEST_CUSTOMER_DATA,
    process({ getState, action }, dispatch, done) {
        const state = getState();
        const tenant = state.AppReducer.tenant;
        const accessToken = state.AppReducer.user.credential;

        if (!tenant.customerMustLogin) {
            done();
        }

        _customerService.GetByExternalProvider(encodeURIComponent(AuthProvider.google), tenant.shortName, accessToken)
            .then(response => {
                let result = response.data;
                if (result.isSuccess) {
                    dispatch(FetchCustomerComplete(result.value));
                }
                else {
                    dispatch(FetchCustomerFailed(result.messageErrorTextArray[0]));
                }
            })
            .catch(error => {
                if (error.response.status === 404) {
                    dispatch(SetCustomerNotRegistered());
                    return;
                }
                dispatch(FetchCustomerFailed(error.message));
            })
            .then(() => done());
    }
});

export const FetchTenantLogic = createLogic({
    type: REQUEST_TENANT_DATA,
    process({ getState, action }, dispatch, done) {
        fetch("/home/info", { method: 'GET' })
            .then(result => result.json())
            .then(result => {
                if (result.isSuccess) {
                    dispatch(FetchTenantComplete(result.value));
                }
                else {
                    dispatch(FetchTenantFailed(result.messageErrorTextArray[0]));
                }
            })
            .catch(error => dispatch(FetchTenantFailed(error)))
            .then(() => done());
    }
});

export const FetchUserLogic = createLogic({
    type: REQUEST_USER_DATA,
    process({ getState, action }, dispatch, done) {
        const state = getState();
        const tenant = state.AppReducer.tenant;

        if (!tenant.customerMustLogin) {
            done();
        }

        window.google.accounts.id.initialize({
            auto_select: true,
            callback: (res) => {
                if (!res.clientId || !res.credential) {
                    dispatch(FetchUserFailed("Unauthorized"));
                    done();
                    return;
                }
                dispatch(FetchUserComplete({ credential: res.credential }));
                done();
            },
            client_id: tenant.googleOauthClientId,
            state_cookie_domain: config.stateCookieDomain
        });
        window.google.accounts.id.prompt((notification) => {
            if (notification.isNotDisplayed() || notification.isSkippedMoment()) {
                // try next provider if OneTap is not displayed or skipped
            }
        });
        return;
    }
});

export const SetCustomerLogic = createLogic({
    type: SET_CUSTOMER,
    process({ getState, action }, dispatch, done) {
        const state = getState();
        const tenant = state.AppReducer.tenant;
        const accessToken = state.AppReducer.user.credential;

        if (!tenant.customerMustLogin) {
            done();
        }

        _customerService.Insert(action.data, tenant.shortName, accessToken)
            .then((response) => {
                const result = response.data;
                if (result.isSuccess) {
                    dispatch(SetCustomerComplete(result.value));
                }
                else {
                    dispatch(SetCustomerFailed(result.messageErrorTextArray[0]));
                }
            })
            .catch(error => {
                dispatch(SetCustomerFailed(error.message));
            })
            .then(() => done());
    }
});

export default [
    FetchCustomerLogic,
    FetchTenantLogic,
    FetchUserLogic,
    SetCustomerLogic
];
