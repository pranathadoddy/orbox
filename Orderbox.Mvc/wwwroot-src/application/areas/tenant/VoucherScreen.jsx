import './VoucherScreen.css';

import React from 'react';
import moment from 'moment';
import { connect } from 'react-redux';
import {
    Page,
    List,
    ListItem
} from 'react-onsenui';

const VoucherScreen = ({ histories }) => {
    const _renderHeader = () => {
        return (
            <div className="splitter-header">
                <h2>Voucher History</h2>
            </div>
        );
    }

    const _renderTime = (dateTime) => {
        const currentDateTime = moment();
        const orderDateTime = moment(dateTime);

        const differentDay = currentDateTime.diff(orderDateTime, 'days');

        if (differentDay < 3) {
            return (
                <small>{orderDateTime.from(currentDateTime)}</small>
            );
        }

        return (
            <small>{orderDateTime.format('DD MMM YYYY, HH:mm')}</small>
        );
    }
    return (
        <Page>
            {_renderHeader()}
            <List
                dataSource={histories}
                renderRow={(row, idx) => (
                    <ListItem key={idx} onClick={() => window.open(`/voucher/get/${row.token}`, '_blank')}>
                        <div>
                            {row.orderNumber}
                            {_renderTime(row.date)}
                        </div>
                    </ListItem>
                )}
            />
        </Page>
    );
};

const mapStateToProps = state => ({
    histories: state.VoucherHistoryReducer.voucherHistories
});

const mapDispatchToProps = dispatch => ({});

export default connect(mapStateToProps, mapDispatchToProps)(VoucherScreen);