import './CheckoutOrderScreen.css';

import React, { useState } from 'react';
import { connect } from 'react-redux';
import { useGoogleReCaptcha } from 'react-google-recaptcha-v3';

import {
    Page,
    Card,
    Toolbar,
    BackButton,
    List,
    ListItem,
    Radio,
    AlertDialog,
    AlertDialogButton
} from 'react-onsenui';

import ProductList from './component/ProductList.jsx';
import OrboxBottomToolbar from './component/OrboxBottomToolbar';
import Splash from './component/Splash';
import SimpleInquiry from './component/CheckoutForm/SimpleInquiry';

import { PopScreen } from './redux/actions/AppAction';
import { SetBuyerData } from './redux/actions/BuyerAction';
import { ResetCart } from './redux/actions/CartAction';
import { SetOrderHistory } from './redux/actions/OrderHistoryAction';

import OrderService from './services/OrderService';
const _orderService = new OrderService();

const forms = {
    SIMPLE_INQUIRY: SimpleInquiry
}

const CheckoutOrderScreen = ({ cart, navigator, saveBuyerData, saveOrderHistory, popScreen, resetCart, tenant, user }) => {
    const [buyerInfo, setBuyerInfo] = useState();
    const [paymentMethod, setPaymentMethod] = useState(tenant.paymentDtos.length > 0 ? tenant.paymentDtos[0] : null);
    const [dialog, setDialog] = useState({ type: '', message: '' });
    const [isLoading, setIsLoading] = useState(false);

    const { executeRecaptcha } = useGoogleReCaptcha();

    const _showSubmitConfirmationDialog = () => {
        setDialog({ type: 'SUBMIT_CONFIRMATION', message: 'Pastikan barang yang anda beli sudah benar. Apakah anda ingin checkout sekarang?' });
    };

    const _showErrorDialog = (message) => {
        setDialog({ type: 'ERROR', message });
    }

    const _closedialog = () => {
        setDialog({ type: '', message: '' });
    };

    const _showLoadingIndicator = () => {
        setIsLoading(true);
    }

    const _hideLoadingIndicator = () => {
        setIsLoading(false);
    }

    const _populateThenSendOrder = (callback) => {
        executeRecaptcha("checkout_order_screen")
            .then((token) => {
                let orderItems = [];
                cart.forEach(item => {
                    orderItems.push({
                        productId: item.id,
                        quantity: item.quantity,
                        note: item.note
                    });
                });
                let order = {
                    tenantId: tenant.id,
                    buyerName: buyerInfo.name,
                    buyerPhone: buyerInfo.phone,
                    buyerEmail: buyerInfo.email,
                    description: buyerInfo.description,
                    paymentMethodId: paymentMethod !== null ? paymentMethod.id : 0,
                    captchaToken: token,
                    orderItems: orderItems
                };
                callback(order);
            })
    }

    const _submit = () => {
        _closedialog();
        _showLoadingIndicator();

        _setBuyerInformation();

        _populateThenSendOrder((order) => {
            if (tenant.customerMustLogin) {
                _orderService.Insert(order, tenant.shortName, user.credential)
                    .then((response) => {
                        const result = response.data;
                        if (!result.isSuccess) {
                            _hideLoadingIndicator();
                            _showErrorDialog(result.messageErrorTextArray[0]);
                            return;
                        }

                        resetCart();
                        window.location = `/order/get/${result.value.token}`;
                    })
                    .catch((error) => {
                        _hideLoadingIndicator();
                        alert(error);
                    });
                return;
            }

            fetch("/order/submitorder", {
                method: 'POST',
                cache: 'no-cache',
                credentials: 'same-origin',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'dataType': 'json'
                },
                body: JSON.stringify(order)
            })
                .then(result => result.json())
                .then((result) => {
                    if (!result.isSuccess) {
                        _hideLoadingIndicator();
                        _showErrorDialog(result.messageErrorTextArray[0]);
                        return;
                    }

                    saveOrderHistory(result.value);
                    resetCart();

                    window.location = `/order/get/${result.value.token}`;
                })
                .catch((error) => {
                    _hideLoadingIndicator();
                    alert(error);
                });
        });
    }

    const _isDisabled = () => {
        return buyerInfo &&
            (
                buyerInfo.phone === undefined ||
                buyerInfo.phone === '' ||
                buyerInfo.email === undefined ||
                buyerInfo.email === '' ||
                buyerInfo.name === undefined ||
                buyerInfo.name === ''
            );
    }

    const _setBuyerInformation = () => {
        const data = {
            phone: buyerInfo.phone,
            email: buyerInfo.email,
            name: buyerInfo.name
        };

        saveBuyerData(data);
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
                    Pemesanan
                </div>
            </Toolbar>
        );
    }

    const _renderBottomToolbar = () => {
        return <OrboxBottomToolbar onButtonClick={_showSubmitConfirmationDialog} disabled={_isDisabled()} />;
    }

    const _renderConfirmationDialog = (isOpen, message) => {
        return (
            <AlertDialog isOpen={isOpen}>
                <div className="alert-dialog-title">Checkout</div>
                <div className="alert-dialog-content">
                    {message}
                </div>
                <div className="alert-dialog-footer">
                    <AlertDialogButton onClick={_submit} style={{ backgroundColor: '#00ACA1', color: '#FFFFFF' }}>
                        Checkout Sekarang
                    </AlertDialogButton>
                    <AlertDialogButton onClick={_closedialog}>
                        Batal
                    </AlertDialogButton>
                </div>
            </AlertDialog>
        );
    }

    const _renderErrorDialog = (isOpen, message) => {
        return (
            <AlertDialog isOpen={isOpen}>
                <div className="alert-dialog-title">Error</div>
                <div className="alert-dialog-content">
                    {message}
                </div>
                <div className="alert-dialog-footer">
                    <AlertDialogButton onClick={_closedialog}>
                        Tutup
                    </AlertDialogButton>
                </div>
            </AlertDialog>
        );
    }

    const _renderPayment = () => {
        if (!paymentMethod) return null;

        let description = `<h3>${paymentMethod.providerName}</h3><p>${paymentMethod.accountName}<br />${paymentMethod.accountNumber}</p>`;
        if (paymentMethod.paymentOptionCode === 'COD') {
            description = `<h3>${paymentMethod.providerName}</h3><p>${paymentMethod.description}</p>`;
        }

        return (
            <Card>
                <div className="title">
                    Pembayaran
                </div>
                <div
                    className="content alert alert-info"
                    dangerouslySetInnerHTML={{ __html: description }}
                >
                </div>
                <List
                    dataSource={tenant.paymentDtos}
                    renderRow={(row, idx) => (
                        <ListItem key={idx} onClick={event => setPaymentMethod(row)}>
                            <label className="left">
                                <Radio
                                    value={row.id.toString()}
                                    checked={paymentMethod.id === row.id}
                                    modifier='material'
                                />
                            </label>
                            <label htmlFor={row.paymentOptionCode.toLowerCase()} className="center">
                                {row.providerName}
                            </label>
                        </ListItem>
                    )}
                />
            </Card>
        );
    }

    if (isLoading) {
        return <Page><Splash message="Mengirim pesanan..." /></Page>;
    }

    const Form = forms[tenant.checkoutForm];

    return (
        <Page renderToolbar={_renderToolbar} renderBottomToolbar={_renderBottomToolbar}>
            <Form onChange={setBuyerInfo} />
            {_renderPayment()}
            <Card style={{ marginBottom: '65px' }}>
                <div className="title">
                    Rincian
                </div>
                <ProductList className="checkout" data={cart} showTotalPricePerListItem={true} readOnlyListItem={true} />
            </Card>
            {_renderConfirmationDialog(dialog.type === 'SUBMIT_CONFIRMATION', dialog.message)}
            {_renderErrorDialog(dialog.type === 'ERROR', dialog.message)}
        </Page>
    );
};

const mapStateToProps = state => ({
    cart: state.CartReducer.cart,
    tenant: state.AppReducer.tenant,
    user: state.AppReducer.user
});

const mapDispatchToProps = dispatch => ({
    saveBuyerData: (item) => dispatch(SetBuyerData(item)),
    saveOrderHistory: (item) => dispatch(SetOrderHistory(item)),
    popScreen: () => dispatch(PopScreen()),
    resetCart: () => dispatch(ResetCart())
});

export default connect(mapStateToProps, mapDispatchToProps)(CheckoutOrderScreen);