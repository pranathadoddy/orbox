import {
    SET_ORDER_HISTORY,
    SET_ORDER_HISTORY_LOADING,
    SET_ORDER_HISTORY_ERROR
} from '../actions/AuthorizedUserOrderHistoryAction';

const initialState = {
    error: '',
    data: [],
    totalItems: 0,
    currentPage: 0,
    isLoading: true
};

const AuthorizedUserOrderHistoryReducer = (state = initialState, action = {}) => {
    switch (action.type) {
        case SET_ORDER_HISTORY: {
            const newData = [...state.data];
            const concatedData = newData.concat(action.data);
            return {
                ...state,
                error: '',
                data: concatedData,
                currentPage: action.currentPage,
                totalItems: action.totalItems,
                isLoading: false
            };
        }

        case SET_ORDER_HISTORY_LOADING: {
            return {
                ...state,
                error: '',
                isLoading: true
            };
        }

        case SET_ORDER_HISTORY_ERROR: {
            return {
                ...state,
                error: action.error,
                isLoading: false
            }
        }

        default: {
            return state;
        }
    }
};

export default AuthorizedUserOrderHistoryReducer;