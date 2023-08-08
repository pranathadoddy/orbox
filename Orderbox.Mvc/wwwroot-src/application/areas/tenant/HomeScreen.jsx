import './HomeScreen.css';

import React, { useState, useEffect, useRef } from 'react';
import { isNullOrUndefined } from 'util';
import { connect } from 'react-redux';

import {
    Page,
    List,
    ListItem,
    Icon,
    Button,
    Splitter,
    SplitterSide,
    SplitterContent
} from 'react-onsenui';

import HistoryScreen from './HistoryScreen';
import VoucherScreen from './VoucherScreen';
import UserNavigationScreen from './UserNavigationScreen';
import ProductScreen from './ProductScreen';
import RegistrationScreen from './RegistrationScreen';
import ProductSearch from './component/ProductSearch';
import ProductList from './component/ProductList';
import BlockLoader from './component/BlockLoader';
import GoogleLoginButton from './component/GoogleLoginButton';

import {
    HideProductDetail,
    ShowProductDetail,
    PushScreen,
    Screens
} from './redux/actions/AppAction';

const DefaultTenantLogo = 'data:image/png;base64,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';

const InitialProductPageIndex = 1;
const ProductPageSize = 20;

const HomeScreen = ({
    cart,
    cartLastAction,
    cartLastActionData,
    customer,
    hideProductDetail,
    isRegisteredCustomer,
    navigator,
    pushScreen,
    showProductDetail,
    tenant,
    user,
    userAuthenticationReady,
    viewedData
}) => {
    const [isFixedHeader, setIsFixedHeader] = useState(false);
    const [isProductScreenDirectlyLoaded, setIsProductScreenDirectlyLoaded] = useState(false);
    const [content, setContent] = useState({
        isLoading: true,
        isCategoryLoading: false,
        categories: [],
        selectedCategory: null,
        isProductLoading: false,
        products: [],
        productPageIndex: InitialProductPageIndex,
        productIsMore: false,
        productSearch: '',
        productLoadingDone: () => { }
    });
    const [splitterOpen, setSplitterOpen] = useState(false);
    const productSearchRef = useRef(null);
    const googleLoginButtonRef = useRef();

    useEffect(() => {
        let hash = window.location.hash;
        let params = hash.split('/');
        if (params.length > 1 && !isNaN(params[1])) {
            fetch(`/product/get/${params[1]}`, { method: 'GET' })
                .then(result => result.json())
                .then(result => {
                    if (result.isSuccess) {
                        setIsProductScreenDirectlyLoaded(true);
                        showProductDetail(result.value);
                        return;
                    }
                    hideProductDetail();
                })
                .catch(error => hideProductDetail());
        }
        _init();
    }, []);

    useEffect(() => {
        if (content.selectedCategory !== null && (content.isLoading || content.isProductLoading)) {
            _getProductByCategory(content.productLoadingDone);
        }
    }, [content]);

    useEffect(() => {
        if (cartLastAction !== '') {
            switch (cartLastAction) {
                case 'ADD_ITEM_TO_CART':
                    _itemAddedToCart(cartLastActionData);
                    break;
                case 'UPDATE_ITEM_QUANTITY':
                case 'REMOVE_ITEM_FROM_CART':
                    _itemQuantityUpdated(cartLastActionData);
                    break;
                case 'ADD_ITEM_NOTE':
                    _itemNoteAdded(cartLastActionData);
                    break;
            }

            viewedData && showProductDetail(cartLastActionData);
        }
    }, [cartLastAction, cartLastActionData])

    useEffect(() => {
        if (tenant.customerMustLogin && userAuthenticationReady) {
            window.google.accounts.id.renderButton(googleLoginButtonRef.current, { type: 'standard', theme: 'outline', shape: 'square', size: 'large' });
        }
    }, [userAuthenticationReady])

    useEffect(() => {
        if (!viewedData) {
            setIsProductScreenDirectlyLoaded(false);
            return;
        }
        _goToProductScreen(viewedData.id);
    }, [viewedData?.id])

    useEffect(() => {
        if (!isRegisteredCustomer) {
            _goToRegistrationScreen();
        }
    }, [isRegisteredCustomer])

    const _handleScroll = (e) => {
        let element = e.target;
        let wallpapper = element.getElementsByClassName('wallpaper')[0];
        let height = wallpapper.offsetHeight - 71;
        if (element.classList.contains('page__content')) {
            if (element.scrollTop > height) {
                setIsFixedHeader(true);
            }
            else {
                setIsFixedHeader(false);
            }
        }
    };

    const _init = () => {
        fetch("/product/init", { method: 'GET' })
            .then(result => result.json())
            .then(result => {
                setContent({
                    ...content,
                    isLoading: false,
                    categories: result.value.categories,
                    selectedCategory: result.value.categories.length > 0 ? result.value.categories[0] : null,
                    products: result.value.products,
                    productIsMore: content.productPageIndex * ProductPageSize < result.value.totalProductItem
                });
            });
    };

    const _infiniteScroll = (done) => {
        if (!content.productIsMore) {
            done();
            return;
        }
        setContent({ ...content, isProductLoading: true, productPageIndex: content.productPageIndex + 1, productLoadingDone: done });
    }

    const _changeCategory = (category) => {
        const selectedCategory = { ...category };

        setContent({
            ...content,
            isProductLoading: true,
            products: [],
            productPageIndex: InitialProductPageIndex,
            productSearch: '',
            selectedCategory
        });
        productSearchRef.current.clearText();
    }

    const _searchProduct = (keyword) => {
        setContent({
            ...content,
            isProductLoading: true,
            products: [],
            productSearch: keyword,
            productPageIndex: InitialProductPageIndex
        });
    }

    const _getProductByCategory = (callback) => {
        fetch(
            `/product/getbycategory?cid=${content.selectedCategory.id}&pi=${content.productPageIndex}&k=${content.productSearch}`,
            { method: 'GET' }
        )
            .then(result => result.json())
            .then(result => {
                const clonedProducts = [...content.products];
                let newProducts = [];

                if (result.rows.length > 0) {
                    const justLoadedProducts = result.rows;
                    for (var idx = 0; idx < cart.length; idx++) {
                        let sameProductIndex = justLoadedProducts.findIndex(item => item.id === cart[idx].id);
                        if (sameProductIndex > -1) {
                            justLoadedProducts.splice(sameProductIndex, 1, cart[idx]);
                        }
                    }
                    newProducts = clonedProducts.concat(justLoadedProducts);
                }

                setContent({
                    ...content,
                    isLoading: false,
                    isProductLoading: false,
                    products: newProducts,
                    productIsMore: content.productPageIndex * ProductPageSize < result.total
                });

                if (typeof callback === 'function') callback();
            });
    }

    const _itemAddedToCart = (addedItem) => {
        const newProducts = [...content.products];
        let sameProductIndex = newProducts.findIndex(item => item.id === addedItem.id);
        newProducts.splice(sameProductIndex, 1, addedItem);
        setContent({ ...content, products: newProducts });
    }

    const _itemQuantityUpdated = (updatedItem) => {
        const newProducts = [...content.products];
        let product = newProducts.find(item => item.id === updatedItem.id);
        if (product) {
            product.quantity = updatedItem.quantity;
            setContent({ ...content, products: newProducts });
        }
    }

    const _itemNoteAdded = (updatedItem) => {
        const newProducts = [...content.products];
        let product = newProducts.find(item => item.id === updatedItem.id);
        if (product) {
            product.note = updatedItem.note;
            setContent({ ...content, products: newProducts });
        }
    }

    const _renderBurgerMenu = () => {
        return (
            <div className="burger-menu" id="burgermenu-button">
                <Button modifier="quiet" fixed-width onClick={() => setSplitterOpen(true)}><Icon icon="md-menu" size={32} className="text-white" /></Button>
            </div>
        );
    }
    const _renderGoogleAuth = () => {
        return (
            customer ?
                <GoogleLoginButton onClick={() => setSplitterOpen(true)} /> :
                <div className="google-auth" ref={googleLoginButtonRef}></div>
        );
    }

    const _goToProductScreen = (productId) => {
        navigator.pushPage({ component: ProductScreen, props: { key: 'PRODUCT', navigator, isProductScreenDirectlyLoaded } }, { animation: 'slide' });        
        window.history.pushState({}, '', `#/${productId}`);
        pushScreen(Screens.Product);
    }

    const _goToRegistrationScreen = () => {
        navigator.pushPage({ component: RegistrationScreen, props: { key: 'REGISTRATION', navigator }}, { animation: 'slide' });
        pushScreen(Screens.Registration);
    }

    return (
        <Splitter>
            <SplitterSide
                side="left"
                width="75%"
                collapse={true}
                isOpen={splitterOpen}
                swipeable={false}
                onClose={() => setSplitterOpen(false)}
            >
                {
                    tenant.customerMustLogin ?
                        <UserNavigationScreen navigator={navigator} pushScreen={pushScreen} /> :
                        <HistoryScreen navigator={navigator} />
                }
            </SplitterSide>
            <SplitterContent>
                <Page onInfiniteScroll={_infiniteScroll} onScroll={_handleScroll} className={isFixedHeader ? 'home-screen fixed-header' : 'home-screen'}>
                    <div className="wallpaper" style={isNullOrUndefined(tenant.wallpaper) ? {} : { backgroundImage: `url(${tenant.wallpaper})` }}>
                        <div className="menu">
                            {tenant.customerMustLogin ? _renderGoogleAuth() : _renderBurgerMenu()}
                        </div>
                        <div className="tenant-profile">
                            <div className="tenant-profile-picture" style={{ backgroundImage: `url(${isNullOrUndefined(tenant.logo) ? DefaultTenantLogo : tenant.logo})` }}></div>
                            <h5 style={{ marginTop: '10px' }}>{tenant.name}</h5>
                            <p>{tenant.address}</p>
                            <p>{tenant.additionalInformation}</p>
                            {
                                !tenant.agencyId && tenant.phone &&
                                [
                                    <a key="whatsapp" href={`https://api.whatsapp.com/send?phone=${tenant.phoneAreaCode}${tenant.phone}`} className="button" target="_blank"><Icon icon="fa-whatsapp" size={16} style={{ color: '#FFFFFF' }} /> Whatsapp</a>,
                                    <a key="phone" href={`tel:${tenant.phoneAreaCode}${tenant.phone}`} className="button"><Icon icon="fa-phone" size={16} style={{ color: '#FFFFFF' }} /> Phone</a>
                                ]
                            }
                        </div>
                        <ProductSearch searchProduct={_searchProduct} ref={productSearchRef} />
                    </div>
                    {
                        content.isLoading ?
                            <BlockLoader /> :
                            [
                                <div key="category" className="category">
                                    <List
                                        dataSource={content.categories}
                                        renderRow={(row, idx) => (
                                            <ListItem key={idx}>
                                                <a href="#"
                                                    className={row.id === content.selectedCategory.id ? 'active' : ''}
                                                    onClick={(e) => {
                                                        e.preventDefault();
                                                        _changeCategory(row);
                                                    }}
                                                >{row.name}</a>
                                            </ListItem>
                                        )}
                                    />
                                </div>,
                                <ProductList
                                    key="home-product"
                                    className="home-product"
                                    data={content.products}
                                    isLoading={content.isProductLoading}
                                />
                            ]
                    }
                </Page>
            </SplitterContent>
        </Splitter>
    );
};

const mapStateToProps = state => ({
    cart: state.CartReducer.cart,
    cartLastAction: state.CartReducer.lastAction,
    cartLastActionData: state.CartReducer.lastActionData,
    customer: state.AppReducer.customer,
    isRegisteredCustomer: state.AppReducer.isRegisteredCustomer,
    tenant: state.AppReducer.tenant,
    user: state.AppReducer.user,
    userAuthenticationReady: state.AppReducer.userAuthenticationReady,
    viewedData: state.AppReducer.viewedData
});

const mapDispatchToProps = dispatch => ({
    hideProductDetail: () => dispatch(HideProductDetail()),
    showProductDetail: (item) => dispatch(ShowProductDetail(item)),
    pushScreen: (screen) => dispatch(PushScreen(screen))
});

export default connect(mapStateToProps, mapDispatchToProps)(HomeScreen);