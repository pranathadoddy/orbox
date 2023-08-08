import {
    ADD_ITEM_TO_CART,
    UPDATE_ITEM_QUANTITY,
    REMOVE_ITEM_FROM_CART,
    ADD_ITEM_NOTE,
    RESET_CART
} from '../actions/CartAction';

const initialState = {
    cart: [],
    lastAction: '',
    lastActionData: null
};

const CartReducer = (state = initialState, action = {}) => {
    switch (action.type) {
        case ADD_ITEM_TO_CART: {
            const newCart = [...state.cart];
            newCart.push(action.item);

            return {
                ...state,
                cart: newCart,
                lastAction: 'ADD_ITEM_TO_CART',
                lastActionData: action.item
            };
        }

        case UPDATE_ITEM_QUANTITY: {
            const newCart = [...state.cart];
            const item = newCart.find(item => item.id === action.item.id);
            item.quantity = action.item.quantity;

            return {
                ...state,
                cart: newCart,
                lastAction: 'UPDATE_ITEM_QUANTITY',
                lastActionData: action.item
            }
        }

        case REMOVE_ITEM_FROM_CART: {
            const newCart = [...state.cart];
            const index = newCart.findIndex(item => item.id === action.item.id);
            newCart.splice(index, 1);

            return {
                ...state,
                cart: newCart,
                lastAction: 'REMOVE_ITEM_FROM_CART',
                lastActionData: action.item
            }
        }

        case ADD_ITEM_NOTE: {
            const newCart = [...state.cart];
            const item = newCart.find(item => item.id === action.item.id);
            item.note = action.item.note;

            return {
                ...state,
                cart: newCart,
                lastAction: 'ADD_ITEM_NOTE',
                lastActionData: action.item
            }
        }

        case RESET_CART: {
            return {
                ...state,
                cart: [],
                lastAction: '',
                lastActionData: null
            }
        }

        default: {
            return state;
        }
    }
};

export default CartReducer;