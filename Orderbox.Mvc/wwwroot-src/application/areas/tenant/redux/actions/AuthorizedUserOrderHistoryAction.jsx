export const REQUEST_ORDER_HISTORY_DATA = 'REQUEST_ORDER_HISTORY_DATA';

export const SET_ORDER_HISTORY = 'SET_ORDER_HISTORY';
export const SET_ORDER_HISTORY_ERROR = 'SET_ORDER_HISTORY_ERROR';
export const SET_ORDER_HISTORY_LOADING = 'SET_ORDER_HISTORY_LOADING';

export const FetchOrderHistory = () => ({ type: REQUEST_ORDER_HISTORY_DATA });

export const SetOrderHistory = (item) => ({ type: SET_ORDER_HISTORY, ...item });
export const SetOrderHistoryError = (error) => ({ type: SET_ORDER_HISTORY_ERROR, error });
export const SetOrderHistoryLoading = (item) => ({ type: SET_ORDER_HISTORY_LOADING });

export const PageSize = 10;