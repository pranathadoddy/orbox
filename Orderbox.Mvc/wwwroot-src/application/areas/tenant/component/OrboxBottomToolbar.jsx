import './OrboxBottomToolbar.css';

import React from 'react';
import { BottomToolbar, Button } from 'react-onsenui';
import { connect } from 'react-redux';

const OrboxBottomToolbar = ({ tenant, cart, onButtonClick, disabled }) => {
    let totalPrice =
        cart.length === 0 ?
            0 :
            cart.map(item => {
                if (item.discount > 0) {
                    let price = item.price - (item.discount * item.price / 100);
                    return item.quantity * price;
                }

                return item.quantity * item.price;
            }).reduce((a, c) => a + c);

    return (
        <BottomToolbar className='container-price-cart'>
            <div className='cart-container-price'>
                <div className='left price-left'>
                    Total Belanja
                </div>
                <div className='right price-right'>
                    {tenant.currency} {totalPrice.toLocaleString("id-ID")}
                </div>
            </div>
            <div className='cart-container-btn'>
                <Button disabled={disabled} onClick={onButtonClick} modifier="large--cta" style={{ backgroundColor: '#00ACA1' }} ripple>Lanjutkan Pesanan</Button>
            </div>
        </BottomToolbar>
    );
};

const mapStateToProps = state => ({
    tenant: state.AppReducer.tenant,
    cart: state.CartReducer.cart
});

const mapDispatchToProps = dispatch => ({});

export default connect(mapStateToProps, mapDispatchToProps)(OrboxBottomToolbar);