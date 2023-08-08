import { combineReducers } from 'redux';

import AppReducer from './AppReducer.jsx';
import AuthorizedUserOrderHistoryReducer from './AuthorizedUserOrderHistoryReducer';
import AuthorizedUserVoucherHistoryReducer from './AuthorizedUserVoucherHistoryReducer';
import CartReducer from './CartReducer.jsx';
import BuyerReducer from './BuyerReducer.jsx';
import OrderHistoryReducer from './OrderHistoryReducer.jsx';
import VoucherHistoryReducer from './VoucherHistoryReducer.jsx';

const reducer = combineReducers({
    AppReducer,
    AuthorizedUserOrderHistoryReducer,
    AuthorizedUserVoucherHistoryReducer,
    CartReducer,
    BuyerReducer,
    OrderHistoryReducer,
    VoucherHistoryReducer
});

export default reducer;