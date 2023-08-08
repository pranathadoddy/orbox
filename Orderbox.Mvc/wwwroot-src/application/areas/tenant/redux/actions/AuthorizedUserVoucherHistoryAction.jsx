export const REQUEST_VOUCHER_HISTORY_DATA = 'REQUEST_VOUCHER_HISTORY_DATA';

export const SET_VOUCHER_HISTORY = 'SET_VOUCHER_HISTORY';
export const SET_VOUCHER_HISTORY_ERROR = 'SET_VOUCHER_HISTORY_ERROR';
export const SET_VOUCHER_HISTORY_LOADING = 'SET_VOUCHER_HISTORY_LOADING';

export const FetchVoucherHistory = () => ({ type: REQUEST_VOUCHER_HISTORY_DATA });

export const SetVoucherHistory = (item) => ({ type: SET_VOUCHER_HISTORY, ...item });
export const SetVoucherHistoryError = (error) => ({ type: SET_VOUCHER_HISTORY_ERROR, error });
export const SetVoucherHistoryLoading = (item) => ({ type: SET_VOUCHER_HISTORY_LOADING });

export const PageSize = 10;