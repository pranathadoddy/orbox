export const REQUEST_TENANT_DATA = 'REQUEST_TENANT_DATA';
export const RECEIVE_TENANT_DATA = 'RECEIVE_TENANT_DATA';
export const RECEIVE_TENANT_ERROR = 'RECEIVE_TENANT_ERROR';

export const REQUEST_USER_DATA = 'REQUEST_USER_DATA';
export const RECEIVE_USER_DATA = 'RECEIVE_USER_DATA';
export const RECEIVE_USER_ERROR = 'RECEIVE_USER_ERROR';

export const REQUEST_CUSTOMER_DATA = 'REQUEST_CUSTOMER_DATA';
export const RECEIVE_CUSTOMER_DATA = 'RECEIVE_CUSTOMER_DATA';
export const RECEIVE_CUSTOMER_ERROR = 'RECEIVE_CUSTOMER_ERROR';
export const SET_CUSTOMER_NOT_REGISTERED = 'SET_CUSTOMER_NOT_REGISTERED';
export const SET_CUSTOMER = 'SET_CUSTOMER';
export const SET_CUSTOMER_COMPLETE = 'SET_CUSTOMER_COMPLETE';
export const SET_CUSTOMER_ERROR = 'SET_CUSTOMER_ERROR';

export const SHOW_PRODUCT_DETAIL = "SHOW_PRODUCT_DETAIL";
export const HIDE_PRODUCT_DETAIL = "HIDE_PRODUCT_DETAIL";

export const PUSH_SCREEN = "PUSH_SCREEN";
export const POP_SCREEN = "POP_SCREEN";
export const SET_SCREEN = "SET_SCREEN";

export const SET_USER_AUTHENTICATION_READY = "SET_USER_AUTHENTICATION_READY";

export const Screens = {
    Home: 'HOME',
    Cart: 'CART',
    Product: 'PRODUCT',
    Registration: 'REGISTRATION',
    Checkout: 'CHECKOUT',
    History: 'HISTORY',
    Voucher: 'VOUCHER'
};

export const AuthProvider = {
    google: 'https://accounts.google.com'
};

export const FetchTenant = () => ({ type: REQUEST_TENANT_DATA });
export const FetchTenantComplete = (data) => ({ type: RECEIVE_TENANT_DATA, data });
export const FetchTenantFailed = (error) => ({ type: RECEIVE_TENANT_ERROR, error });

export const FetchUser = () => ({ type: REQUEST_USER_DATA });
export const FetchUserComplete = (data) => ({ type: RECEIVE_USER_DATA, data });
export const FetchUserFailed = (error) => ({ type: RECEIVE_USER_ERROR, error });

export const FetchCustomer = () => ({ type: REQUEST_CUSTOMER_DATA });
export const FetchCustomerComplete = (data) => ({ type: RECEIVE_CUSTOMER_DATA, data });
export const FetchCustomerFailed = (error) => ({ type: RECEIVE_CUSTOMER_ERROR, error });
export const SetCustomerNotRegistered = () => ({ type: SET_CUSTOMER_NOT_REGISTERED });
export const SetCustomer = (data) => ({ type: SET_CUSTOMER, data });
export const SetCustomerComplete = (data) => ({ type: SET_CUSTOMER_COMPLETE, data });
export const SetCustomerFailed = (error) => ({ type: SET_CUSTOMER_ERROR, error })

export const ShowProductDetail = (item) => ({ type: SHOW_PRODUCT_DETAIL, item });
export const HideProductDetail = () => ({ type: HIDE_PRODUCT_DETAIL });

export const PushScreen = (screen) => ({ type: PUSH_SCREEN, screen });
export const PopScreen = () => ({ type: POP_SCREEN });
export const SetScreen = (screens) => ({ type: SET_SCREEN, screens });

export const SetUserAuthenticationReady = () => ({ type: SET_USER_AUTHENTICATION_READY });