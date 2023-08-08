import { createLogic } from 'redux-logic';

import {
    REQUEST_VOUCHER_HISTORY_DATA,
    SetVoucherHistory,
    SetVoucherHistoryError,
    SetVoucherHistoryLoading,
    PageSize
} from "../actions/AuthorizedUserVoucherHistoryAction";

import VoucherService from '../../services/VoucherService';
const _voucherService = new VoucherService();

export const AuthorizedUserVoucherHistoryLogic = createLogic({
    type: REQUEST_VOUCHER_HISTORY_DATA,
    process({ getState, action }, dispatch, done) {
        let state = getState();
        let credential = state.AppReducer.user.credential;
        let tenant = state.AppReducer.tenant;
        let currentPage = state.AuthorizedUserVoucherHistoryReducer.currentPage;

        dispatch(SetVoucherHistoryLoading());

        _voucherService.PagedSearch({ rowCount: PageSize, current: currentPage+1 }, tenant.shortName, credential)
            .then((response) => {
                let result = response.data;
                dispatch(SetVoucherHistory({
                    data: result.rows,
                    currentPage: result.current,
                    totalItems: result.total
                }));
            })
            .catch(error => {
                dispatch(SetVoucherHistoryError(error.message));
            })
            .then(() => done());
    }
});

export default [
    AuthorizedUserVoucherHistoryLogic
];