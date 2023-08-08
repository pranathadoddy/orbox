import {
    SET_ORDER_HISTORY
} from '../actions/OrderHistoryAction';

const initialState = {
    orderHistories: []
};

const OrderHistoryReducer = (state = initialState, action = {}) => {
    switch (action.type) {
        case SET_ORDER_HISTORY: {
            const newOrderHistories = [...state.orderHistories];
            if (action.item) {
                newOrderHistories.unshift(action.item);
            }
            return {
                ...state,
                orderHistories: newOrderHistories
            };
        }

        default: {
            return state;
        }
    }
};

export default OrderHistoryReducer;