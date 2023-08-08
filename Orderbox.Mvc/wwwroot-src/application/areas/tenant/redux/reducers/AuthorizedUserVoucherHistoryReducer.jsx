import {
    SET_VOUCHER_HISTORY,
    SET_VOUCHER_HISTORY_LOADING,
    SET_VOUCHER_HISTORY_ERROR
} from '../actions/AuthorizedUserVoucherHistoryAction';

const initialState = {
    error: '',
    data: [],
    totalItems: 0,
    currentPage: 0,
    isLoading: true
};

const AuthorizedUserVoucherHistoryReducer = (state = initialState, action = {}) => {
    switch (action.type) {
        case SET_VOUCHER_HISTORY: {
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

        case SET_VOUCHER_HISTORY_LOADING: {
            return {
                ...state,
                error: '',
                isLoading: true
            };
        }

        case SET_VOUCHER_HISTORY_ERROR: {
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

export default AuthorizedUserVoucherHistoryReducer;