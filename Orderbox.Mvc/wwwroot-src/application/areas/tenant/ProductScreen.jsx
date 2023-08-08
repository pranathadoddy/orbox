import './ProductScreen.css';

import React, { useState, useEffect } from 'react';
import { Button, BackButton, Carousel, CarouselItem, Page, Toolbar, BottomToolbar } from 'react-onsenui';
import { connect } from 'react-redux';

import OrderCounter from './component/OrderCounter';
import Main from './Main';

import {
    AddItemToCart,
    UpdateItemQuantity,
    RemoveItemFromCart
} from './redux/actions/CartAction';
import {
    FetchTenant,
    HideProductDetail,
    PopScreen,
    PushScreen,
    SetScreen,
    Screens
} from './redux/actions/AppAction';

const ProductScreen = ({
    addItemToCart,
    cart,
    hideProductDetail,
    isProductScreenDirectlyLoaded,
    navigator,
    popScreen,
    removeItemFromCart,
    setScreen,
    tenant,
    updateItemQuantity,
    viewedData
}) => {
    const [pictureIndex, setPictureIndex] = useState(0);

    if (!viewedData || !tenant) {
        return null;
    }

    let data = viewedData;
    let cartData = cart.find(item => item.id === viewedData.id);
    if (cartData) {
        data = cartData;
    }
    let currentPrice = data.price;
    if (data.discount > 0) {
        currentPrice = data.price - (data.discount * data.price / 100);
    }
    const quantity = data.quantity || 0;

    const _continueOrder = () => {
        hideProductDetail('VIEW_CART');
        if (isProductScreenDirectlyLoaded) {
            window.history.replaceState({}, "", "/");
        }
        else {
            window.history.go(-1);
        }
        setTimeout(() => navigator.resetPage({ component: Main, props: { key: `MAIN-${new Date().getTime()}` } }), 0);
        setScreen([Screens.Home, Screens.Cart]);
    };

    const _addButtonClicked = () => {
        const addedItem = { ...data, quantity: (data.quantity || 0) + 1 };
        addItemToCart(addedItem);
    };

    const _plusAction = () => {
        const updatedItem = { ...data, quantity: data.quantity + 1 };
        updateItemQuantity(updatedItem);
    };

    const _minusAction = () => {
        const quantity = data.quantity - 1;
        const updatedItem = { ...data, quantity };

        if (quantity > 0) {
            updateItemQuantity(updatedItem);
            return;
        }

        removeItemFromCart(updatedItem);
    };

    const _renderPrice = () => {
        if (data.discount > 0) {
            return (
                <div className="product-screen__price">
                    {tenant ? tenant.currency : ''}
                    &nbsp;
                    <span className="old-price ml-1 mr-1">{data.price.toLocaleString("id-ID")}</span>
                    &nbsp;
                    {currentPrice.toLocaleString("id-ID")}
                    /
                    {data.unit}
                </div>
            );
        }

        return (
            <div className="product-screen__price">
                {tenant ? tenant.currency : ''} {data.price.toLocaleString("id-ID")}/{data.unit}
            </div>
        );
    }

    const _renderToolbar = () => {
        return (
            <Toolbar>
                <div className="left">
                    <BackButton
                        onClick={() => {
                            hideProductDetail();
                            if (isProductScreenDirectlyLoaded) {
                                window.history.replaceState({}, "", "/");
                            }
                            else {
                                window.history.back();
                            }
                            navigator.popPage();
                            popScreen();
                        }} />
                </div>
                <div className="center"></div>
            </Toolbar>
        );
    }

    const _renderBottomToolbar = () => {
        return (
            <BottomToolbar className='container-price-cart product-screen-footer'>
                <div className='cart-container-price'>
                    <div className='left price-left'>
                        {
                            quantity > 0 ?
                                <OrderCounter
                                    quantity={quantity}
                                    plusAction={_plusAction}
                                    minAction={_minusAction}
                                /> :
                                <span>&nbsp;</span>
                        }
                    </div>
                    <div className='right price-right'>
                        {quantity > 0 ? `${tenant ? tenant.currency : ''} ${(currentPrice * quantity).toLocaleString("id-ID")}` : <span>&nbsp;</span>}
                    </div>
                </div>
                <div className='cart-container-btn'>  
                    {
                        quantity > 0 ?
                            <Button onClick={_continueOrder} modifier="large--cta" style={{ backgroundColor: '#00ACA1' }} ripple>Keranjang</Button> :
                            <Button onClick={_addButtonClicked} modifier="large--cta" style={{ backgroundColor: '#00ACA1' }} ripple>Tambahkan</Button>
                    }
                </div>
            </BottomToolbar>
        );
    }

    return (
        <Page renderToolbar={_renderToolbar} renderBottomToolbar={_renderBottomToolbar}>
            {
                data.productImages.length > 0 ?
                    <div className="product-screen__carousel-container">
                        <Carousel
                            onPostChange={(item) => setPictureIndex(item.activeIndex)}
                            index={pictureIndex}
                            swipeable
                            fullscreen
                            autoScroll
                            autoScrollRatio={0.2}
                        >
                            {
                                data.productImages.map((v, i) => {
                                    return (
                                        <CarouselItem key={i}>
                                            <div className="product-screen__carousel-item" style={{ backgroundImage: `url(${v.fileName})` }}></div>
                                        </CarouselItem>
                                    );
                                })
                            }
                        </Carousel>
                        <span className="product-screen__carousel-counter">{pictureIndex + 1}/{data.productImages.length}</span>
                    </div> : null
            }
            <div className="product-screen__title">
                <h3>{data ? data.name : ''}</h3>
                {_renderPrice()}
            </div>
            {
                data && data.description ?
                    <div className="product-screen__description">
                        {data.description}
                    </div> : null
            }
        </Page>
    );
};

const mapStateToProps = state => ({
    tenant: state.AppReducer.tenant,
    viewedData: state.AppReducer.viewedData,
    cart: state.CartReducer.cart,
    screenStack: state.AppReducer.screenStack
});

const mapDispatchToProps = dispatch => ({
    addItemToCart: (item) => dispatch(AddItemToCart(item)),
    fetchTenant: () => dispatch(FetchTenant()),
    updateItemQuantity: (item) => dispatch(UpdateItemQuantity(item)),
    removeItemFromCart: (item) => dispatch(RemoveItemFromCart(item)),
    hideProductDetail: (item) => dispatch(HideProductDetail(item)),
    popScreen: () => dispatch(PopScreen()),
    pushScreen: (screen) => dispatch(PushScreen(screen)),
    setScreen: (screens) => dispatch(SetScreen(screens))
});

export default connect(mapStateToProps, mapDispatchToProps)(ProductScreen);