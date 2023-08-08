import {
    RECEIVE_TENANT_DATA,
    RECEIVE_TENANT_ERROR,
    RECEIVE_USER_DATA,
    RECEIVE_USER_ERROR,
    SHOW_PRODUCT_DETAIL,
    HIDE_PRODUCT_DETAIL,
    PUSH_SCREEN,
    POP_SCREEN,
    SET_CUSTOMER_NOT_REGISTERED,
    SET_SCREEN,
    SET_USER_AUTHENTICATION_READY,
    Screens,
    RECEIVE_CUSTOMER_DATA,
    RECEIVE_CUSTOMER_ERROR,
    SET_CUSTOMER_COMPLETE,
    SET_CUSTOMER_ERROR
} from '../actions/AppAction';

const initialState = {
    error: '',
    customer: null,
    isRegisteredCustomer: true,
    tenant: null,
    user: null,
    userAuthenticationReady: false,
    viewedData: null,
    screenStack: [Screens.Home]
};

const AppReducer = (state = initialState, action = {}) => {
    switch (action.type) {
        case RECEIVE_CUSTOMER_DATA:
            return {
                ...state,
                customer: action.data
            };

        case RECEIVE_TENANT_DATA:
            return {
                ...state,
                tenant: action.data
            };

        case RECEIVE_TENANT_ERROR:
        case RECEIVE_USER_ERROR:
        case RECEIVE_CUSTOMER_ERROR:
            return {
                ...state,
                error: action.error
            };

        case RECEIVE_USER_DATA:
            return {
                ...state,
                user: action.data
            };

        case SHOW_PRODUCT_DETAIL: {
            return {
                ...state,
                viewedData: action.item
            }
        }

        case HIDE_PRODUCT_DETAIL: {
            return {
                ...state,
                viewedData: null
            }
        }

        case PUSH_SCREEN: {
            let newScreenStack = [...state.screenStack];
            newScreenStack.push(action.screen);
            return {
                ...state,
                screenStack: newScreenStack
            }
        }

        case POP_SCREEN: {
            let newScreenStack = [...state.screenStack];
            newScreenStack.pop();
            return {
                ...state,
                screenStack: newScreenStack
            }
        }

        case SET_SCREEN: {
            return {
                ...state,
                screenStack: action.screens
            }
        }

        case SET_USER_AUTHENTICATION_READY: {
            return {
                ...state,
                userAuthenticationReady: true
            }
        }

        case SET_CUSTOMER_NOT_REGISTERED: {
            return {
                ...state,
                isRegisteredCustomer: false
            }
        }

        case SET_CUSTOMER_COMPLETE: {
            return {
                ...state,
                customer: action.data,
                isRegisteredCustomer: true
            }
        }

        case SET_CUSTOMER_ERROR: {
            return {
                ...state,
                error: action.error
            }
        }

        default:
            return state;
    }
};

export default AppReducer;