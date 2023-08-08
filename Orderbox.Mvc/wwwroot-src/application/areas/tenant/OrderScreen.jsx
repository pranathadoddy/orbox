import './OrderScreen.css';

import React from 'react';
import { connect } from 'react-redux';
import {
    Page, Toolbar, Button, Icon, Card
} from 'react-onsenui';

import ProductList from './component/ProductList';
import OrboxBottomToolbar from './component/OrboxBottomToolbar';
import CheckoutOrderScreen from './CheckoutOrderScreen';

import {
    PushScreen,
    Screens
} from './redux/actions/AppAction';

const OrderScreen = ({ tabbar, navigator, cart, pushScreen }) => {
    const _continueOrder = () => {
        navigator.pushPage({ component: CheckoutOrderScreen, props: { key: 'CHECKOUT', navigator } }, { animation: 'slide' });
        pushScreen(Screens.Checkout);
    };

    const _renderToolbar = () => {
        return (
            <Toolbar>
                <div className="left">
                    <Button modifier="quiet" onClick={() => tabbar._tabbar.setActiveTab(0)}><Icon icon="md-arrow-left" size={24} style={{ color: '#00aca1' }} /></Button>
                </div>
                <div className="center">
                    Keranjang
                </div>
            </Toolbar>    
        );
    }

    const _renderBottomToolbar = () => {
        return <OrboxBottomToolbar onButtonClick={_continueOrder} disabled={cart.length === 0} />;        
    }

    return (
        <Page renderToolbar={_renderToolbar} renderBottomToolbar={_renderBottomToolbar}>
            {
                cart.length === 0 ?
                    <Card style={{ textAlign: 'center' }}>
                        <p>Anda tidak memiliki pesanan di keranjang belanja</p>
                    </Card> :
                    <ProductList
                        className="ordered-product"
                        data={cart}
                        showTotalPricePerListItem={true}
                    />
            }
        </Page>
    );
};

const mapStateToProps = state => ({
    cart: state.CartReducer.cart
});

const mapDispatchToProps = dispatch => ({
    pushScreen: (screen) => dispatch(PushScreen(screen))
});

export default connect(mapStateToProps, mapDispatchToProps)(OrderScreen);