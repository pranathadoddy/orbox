import {
    SET_BUYER_DATA
} from '../actions/BuyerAction';

const initialState = {
    buyers: []
};

const BuyerReducer = (state = initialState, action = {}) => {
    let newBuyers;
    let buyerIndex;

    switch (action.type) {
        case SET_BUYER_DATA:
            newBuyers = [...state.buyers];
            buyerIndex = newBuyers.findIndex(item => item.phone === action.data.phone);

            if (buyerIndex === -1) {
                newBuyers.push(action.data);
            } else {
                newBuyers.splice(buyerIndex, 1, action.data);
            }

           return {
               ...state,
               buyers: newBuyers
            };
        default:
            return state;
    }
};

export default BuyerReducer;