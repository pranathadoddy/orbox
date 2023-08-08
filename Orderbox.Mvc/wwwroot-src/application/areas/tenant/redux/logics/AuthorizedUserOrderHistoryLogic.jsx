import { createLogic } from 'redux-logic';

import {
    REQUEST_ORDER_HISTORY_DATA,
    SetOrderHistory,
    SetOrderHistoryError,
    SetOrderHistoryLoading,
    PageSize
} from "../actions/AuthorizedUserOrderHistoryAction";

import OrderService from '../../services/OrderService';
const _orderService = new OrderService();

export const OrderHistoryLogic = createLogic({
    type: REQUEST_ORDER_HISTORY_DATA,
    process({ getState, action }, dispatch, done) {
        let state = getState();
        let credential = state.AppReducer.user.credential;
        let tenant = state.AppReducer.tenant;
        let currentPage = state.AuthorizedUserOrderHistoryReducer.currentPage;

        dispatch(SetOrderHistoryLoading());

        _orderService.PagedSearch({ rowCount: PageSize, current: currentPage+1 }, tenant.shortName, credential)
            .then((response) => {
                let result = response.data;
                dispatch(SetOrderHistory({
                    data: result.rows,
                    currentPage: result.current,
                    totalItems: result.total
                }));
            })
            .catch(error => {
                dispatch(SetOrderHistoryError(error.message));
            })
            .then(() => done());
    }
});

export default [
    OrderHistoryLogic
];