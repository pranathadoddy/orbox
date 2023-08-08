import AppLogic from './AppLogic';
import AuthorizedUserOrderHistoryLogic from './AuthorizedUserOrderHistoryLogic';
import AuthorizedUserVoucherHistoryLogic from './AuthorizedUserVoucherHistoryLogic';

export default [
    ...AppLogic,
    ...AuthorizedUserOrderHistoryLogic,
    ...AuthorizedUserVoucherHistoryLogic
];