import './VoucherScreen.css';

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
import { FetchVoucherHistory, PageSize } from './redux/actions/AuthorizedUserVoucherHistoryAction';

const AuthorizedUserVoucherScreen = ({
    navigator,
    popScreen,
    data,
    currentPage,
    totalItems,
    isLoading,
    fetchVoucherHistory
}) => {
    useEffect(() => {
        fetchVoucherHistory();
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
                    Voucher History
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
                            <span>{row.code}</span>
                            <span>&nbsp;</span>
                            <span className={`badge ${row.badgeColor}`}>{row.statusDescription}</span>
                            <div>{row.name}</div>
                            {_renderTime(row.createdDateTime)}
                        </div>
                    </ListItem>
                )}
            />
            {
                (currentPage * PageSize < totalItems) &&
                    (isLoading ?
                        <Button modifier="large--quiet" disabled>Loading</Button> :
                        <Button modifier="large--quiet" onClick={() => fetchVoucherHistory()}>Load More</Button>)
            }
        </Page>
    );
};

const mapStateToProps = state => ({
    data: state.AuthorizedUserVoucherHistoryReducer.data,
    currentPage: state.AuthorizedUserVoucherHistoryReducer.currentPage,
    totalItems: state.AuthorizedUserVoucherHistoryReducer.totalItems,
    isLoading: state.AuthorizedUserVoucherHistoryReducer.isLoading
});

const mapDispatchToProps = dispatch => ({
    popScreen: () => dispatch(PopScreen()),
    fetchVoucherHistory: () => dispatch(FetchVoucherHistory())
});

export default connect(mapStateToProps, mapDispatchToProps)(AuthorizedUserVoucherScreen);