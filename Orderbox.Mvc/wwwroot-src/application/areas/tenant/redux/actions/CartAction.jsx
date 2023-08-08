export const ADD_ITEM_TO_CART = 'ADD_ITEM_TO_CART';
export const UPDATE_ITEM_QUANTITY = 'UPDATE_ITEM_QUANTITY';
export const REMOVE_ITEM_FROM_CART = 'REMOVE_ITEM_FROM_CART';
export const ADD_ITEM_NOTE = "ADD_ITEM_NOTE";
export const RESET_CART = "RESET_CART";

export const AddItemToCart = (item) => ({ type: ADD_ITEM_TO_CART, item });
export const UpdateItemQuantity = (item) => ({ type: UPDATE_ITEM_QUANTITY, item });
export const RemoveItemFromCart = (item) => ({ type: REMOVE_ITEM_FROM_CART, item });
export const AddItemNote = (item) => ({ type: ADD_ITEM_NOTE, item });
export const ResetCart = () => ({ type: RESET_CART });