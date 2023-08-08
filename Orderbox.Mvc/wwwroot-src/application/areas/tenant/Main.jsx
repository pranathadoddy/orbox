import './Main.css';

import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import {
    Page,
    Tabbar,
    Tab
} from 'react-onsenui';

import { FetchCustomer, FetchTenant, PushScreen, PopScreen, Screens } from './redux/actions/AppAction';

import HomeScreen from './HomeScreen';
import OrderScreen from './OrderScreen';
import Splash from './component/Splash';

const indexByScreen = {
    HOME: 0,
    CART: 1,
    CHECKOUT: 1,
    PRODUCT: 1
}

const Main = ({ cart, customer, fetchCustomer, fetchTenant, navigator, pushScreen, popScreen, screenStack, tenant, user }) => {
    useEffect(() => {
        if (!tenant) {
            fetchTenant();
        }
    }, []);

    useEffect(() => {
        if (!tenant) {
            return;
        }

        if (!user) {
            return;
        }

        fetchCustomer();
    }, [tenant, user])

    if (!tenant) {
        return <Splash />;
    }
    return (        
        <Page>
            <Tabbar
                onPreChange={({ index }) => {
                    if (index === 0 && screenStack.length > 1) {
                        popScreen();
                    }
                    else if (index === 1 && screenStack.length === 1) {
                        pushScreen(Screens.Cart);
                    }
                }}
                position='bottom'
                index={indexByScreen[screenStack[screenStack.length - 1]]}
                renderTabs={(activeIndex, tabbar) => [
                    {
                        content: <HomeScreen key="Screen2" active={activeIndex === 0} tabbar={tabbar} navigator={navigator} />,
                        tab: <Tab key="Tab0" label="Toko" icon="md-home" />
                    },
                    {
                        content: <OrderScreen key="Screen1" active={activeIndex === 1} tabbar={tabbar} navigator={navigator} />,
                        tab: <Tab key="Tab1" label="Keranjang" icon="md-shopping-cart" badge={cart.length > 0 ? cart.length : ""} />
                    }
                ]}
            />
        </Page>
    );
};

const mapStateToProps = state => ({
    cart: state.CartReducer.cart,
    customer: state.AppReducer.customer,
    screenStack: state.AppReducer.screenStack,
    tenant: state.AppReducer.tenant,
    user: state.AppReducer.user
});

const mapDispatchToProps = dispatch => ({
    fetchCustomer: () => dispatch(FetchCustomer()),
    fetchTenant: () => dispatch(FetchTenant()),
    pushScreen: (screen) => dispatch(PushScreen(screen)),
    popScreen: () => dispatch(PopScreen())
});

export default connect(mapStateToProps, mapDispatchToProps)(Main);