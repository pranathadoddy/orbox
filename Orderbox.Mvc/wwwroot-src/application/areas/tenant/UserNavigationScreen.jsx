import './UserNavigationScreen.css';

import React from 'react';
import { connect } from 'react-redux';
import {
    Page,
    List,
    ListItem
} from 'react-onsenui';

import HistoryScreen from './AuthorizedUserHistoryScreen';
import VoucherScreen from './AuthorizedUserVoucherScreen';

import {
    PushScreen,
    Screens,
    FetchUserComplete
} from './redux/actions/AppAction';

const UserNavigationScreen = ({ customer, logout, navigator, pushScreen }) => {
    const dataSource = [
        {
            label: 'Transaction',
            onClick: () => {
                navigator.pushPage({ component: HistoryScreen, props: { key: 'HISTORY', navigator } }, { animation: 'slide' });
                pushScreen(Screens.History);
            }
        },
        {
            label: 'Voucher',
            onClick: () => {
                navigator.pushPage({ component: VoucherScreen, props: { key: 'VOUCHER', navigator } }, { animation: 'slide' });
                pushScreen(Screens.Voucher);
            }
        },
        {
            label: 'Logout',
            onClick: () => {
                logout();
                window.google.accounts.id.disableAutoSelect();
                window.location.reload();
            }
        }
    ]
    const _renderHeader = () => {
        return (
            <div className="user-navigation-header">
                <img src={customer?.profilePicture} alt="Profile Picture" />
                <h5>{`${customer?.firstName} ${customer?.lastName}`}</h5>
                <span>{customer?.emailAddress}</span>
            </div>
        );
    }

    return (
        <Page>
            {_renderHeader()}
            <List
                dataSource={dataSource}
                renderRow={(row, idx) => (
                    <ListItem key={idx} onClick={row.onClick}>
                        <div>
                            {row.label}
                        </div>
                    </ListItem>
                )}
            />
        </Page>
    );
};

const mapStateToProps = state => ({
    customer: state.AppReducer.customer
});

const mapDispatchToProps = dispatch => ({
    pushScreen: (screen) => dispatch(PushScreen(screen)),
    logout: () => dispatch(FetchUserComplete(null))
});

export default connect(mapStateToProps, mapDispatchToProps)(UserNavigationScreen);