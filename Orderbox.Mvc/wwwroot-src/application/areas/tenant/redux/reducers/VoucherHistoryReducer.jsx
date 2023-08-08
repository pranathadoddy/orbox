import {
    SET_VOUCHER_HISTORY
} from '../actions/VoucherHistoryAction';

const initialState = {
    voucherHistories: []
};

const VoucherHistoryReducer = (state = initialState, action = {}) => {
    switch (action.type) {
        case SET_VOUCHER_HISTORY: {
            const newVoucherHistories = [...state.voucherHistories];
            if (action.item) {
                newVoucherHistories.unshift(action.item);
            }
            return {
                ...state,
                voucherHistories: newVoucherHistories
            };
        }

        default: {
            return state;
        }
    }
};

export default VoucherHistoryReducer;