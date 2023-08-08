import './HistoryScreen.css';

import React, { useEffect } from 'react';
import moment from 'moment';
import { connect } from 'react-redux';
import {
    Button,
    Page,
    Toolbar,
    BackButton,
    List,
    ListItem
} from 'react-onsenui';

import { PopScreen } from './redux/actions/AppAction';
import { FetchOrderHistory, PageSize } from './redux/actions/AuthorizedUserOrderHistoryAction';

const HistoryScreen = ({
    navigator,
    popScreen,
    data,
    currentPage,
    totalItems,
    isLoading,
    fetchOrderHistory
}) => {
    useEffect(() => {
        fetchOrderHistory();
    }, []);

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

    const _renderToolbar = () => {
        return (
            <Toolbar>
                <div className="left">
                    <BackButton
                        onClick={() => {
                            navigator.popPage();
                            popScreen();
                        }} />
                </div>
                <div className="center">
                    Transaction History
                </div>
            </Toolbar>
        );
    }

    return (
        <Page renderToolbar={_renderToolbar}>
            <List
                dataSource={data}
                renderRow={(row, idx) => (
                    <ListItem key={idx} onClick={() => window.open(row.url, '_blank')}>
                        <div id={idx}>
                            {row.orderNumber}
                            <span>&nbsp;</span>
                            {row.paymentStatus == "PAID" && <span className="badge badge-green">Paid</span>}
                            {_renderTime(row.date)}
                        </div>
                    </ListItem>
                )}
            />
            {
                (currentPage * PageSize < totalItems) &&
                    (isLoading ?
                        <Button modifier="large--quiet" disabled>Loading</Button> :
                        <Button modifier="large--quiet" onClick={() => fetchOrderHistory()}>Load More</Button>)
            }
        </Page>
    );
};

const mapStateToProps = state => ({
    data: state.AuthorizedUserOrderHistoryReducer.data,
    currentPage: state.AuthorizedUserOrderHistoryReducer.currentPage,
    totalItems: state.AuthorizedUserOrderHistoryReducer.totalItems,
    isLoading: state.AuthorizedUserOrderHistoryReducer.isLoading
});

const mapDispatchToProps = dispatch => ({
    popScreen: () => dispatch(PopScreen()),
    fetchOrderHistory: () => dispatch(FetchOrderHistory())
});

export default connect(mapStateToProps, mapDispatchToProps)(HistoryScreen);