import './OrderItem.css';

import React from 'react';
import { ListItem, Button, Icon } from 'react-onsenui';
import { connect } from 'react-redux';

import {
    AddItemToCart,
    UpdateItemQuantity,
    RemoveItemFromCart
} from '../redux/actions/CartAction';
import {
    PushScreen,
    ShowProductDetail,
    Screens
} from '../redux/actions/AppAction';
import OrderCounter from './OrderCounter';

const defaultProductImage = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAOEAAADhCAMAAAAJbSJIAAAAMFBMVEXh6vH////5+vrl7fPv8/f0+fzt9Pbf6vD+/f/j6/Lf6vL///3f6fLr8fbv8/by9vm0HxD7AAACbUlEQVR4nO3c65abIBRAYVHTDALx/d+2VBuDII6zLHro2t/fmEn2HETn2jQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADkG29irNHa4I9G9+qu83B2BTauu01JIIYUUUkghhbcX/ipFTGH3LKOTUmi6Qq/QGQrLovA8Ckuj8LzqC/98A2ZX5YXd2Lajf6beOaTmwm5+tnnsBNZc+NTt/OaNanWT/U5hzYWjWjzyh1VSqK1ONpTPW/fyz66kcHCqixJtHwSqPvvUKgq1D/Rjik61MZzhWPd5aJ3fTfwR6yn+P4XzBFU8RevCwvzPJCoo9IFmepdmPUUdFuqaZzgt0fcUw8T+88Ar/wrSC/UywemgcKE+7Tg/ZNRY8V2bDScYT9G6r+mdu+fOK8guXE8wmaK/Vet6980PPmUXDvEEVXLRCL942lwGkguny0RSmF76//ILuh/SE1JyYXIObk/xzfn17NJH5BZ+LvSHpmjno10yRbmFySYTDHFjivONnZ9iHC+3MLNE31NcJy7zNsn9m9DC9DIRTXG9UD977jTF1UIVWpjbZDJTDM/YeLsRWfjdBKMprj8d8RRFFm5d6Dca31OM99z1diOwMHehT01TTBe0WV00BBYem+AyRbdxcDhFgYX6YN80RZu5LTCSZ9gcLzSqz+xIZvloAgt/MsN8u+QZUkghhRRSSCGFFFJIIYWXFKqv85Towr3fcDpKSy78xygsjcLzKCyNwvPEFCpthxKsVlIKXVeGE1NYHoUUUkghhRRSeFfh2D6u0o53BOpmuOz/RA17f8MHAAAAAAAAAAAAAAAAAAAAAAAAAAAAALjcb3yLQG5tF3tgAAAAAElFTkSuQmCC';

const OrderItem = ({
    tenant,
    cart,
    product,
    addItemToCart,
    updateItemQuantity,
    removeItemFromCart,
    showProductDetail,
    showTotal,
    readOnly,
    pushScreen,
    showNoteModal
}) => {
    let data = product;
    let cartData = cart.find(item => item.id === product.id);
    if (cartData) {
        data = cartData;
    }
    let currentPrice = data.price;
    if (data.discount > 0) {
        currentPrice = data.price - (data.discount * data.price / 100);
    }
    const quantity = data.quantity || 0;
    const primaryImage = data.productImages.find(item => item.isPrimary);
    let backgroundImage = defaultProductImage;
    if (primaryImage) {
        backgroundImage = primaryImage.fileName;
    }

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
                <div className="order-item__price">
                    <div className="order-item__old-price-container">
                        {tenant.currency}
                        &nbsp;
                        <span className="old-price ml-1 mr-1">{data.price.toLocaleString("id-ID")}</span>
                    </div>
                    <div className="order-item__discounted-price-container">{currentPrice.toLocaleString("id-ID")}/{data.unit}</div>
                </div>
            );
        }

        return (
            <div className="order-item__price">
                {tenant.currency} {data.price.toLocaleString("id-ID")}/{data.unit}
            </div>
        );
    }

    const _showProductDetail = () => {
        const dataInCart = cart.find(item => item.id === data.id);
        let newData;
        if (dataInCart) {
            newData = { ...dataInCart, readOnly };
        }
        else {
            newData = { ...data, readOnly };
        }        
        showProductDetail(newData);
    }

    return (
        <ListItem id={data.id} className="order-item">
            <div className="order-item__info">
                <div className="order-item__left" style={{ backgroundImage: `url(${backgroundImage})` }} onClick={_showProductDetail}>
                    {
                        data.discount > 0 ? <span className="discount">-{data.discount}%</span> : null
                    }
                </div>
                <div className="order-item__right">
                    <div className="order-item__title">{data.name}</div>
                    {_renderPrice()}
                    <div className="order-item__qty">
                        {readOnly ?
                            <>
                                {
                                    data.note ?
                                        <div className="order-item__notes">
                                            <a
                                                className="order-item__notes-button"
                                                onClick={() => { showNoteModal(data) }}
                                                modifier="light">
                                                <Icon
                                                    size={{ material: 17 }}
                                                    icon={{ material: 'fa-pencil-square-o' }}
                                                    style={{ color: data.note ? '#00aca1' : '#F37021' }} />
                                            </a>
                                        </div> : null
                                }
                                <span className="label">Jumlah {quantity}</span>
                            </>
                            :
                            quantity > 0 ?
                                <>
                                    <div className="order-item__notes">
                                        <a
                                            className="order-item__notes-button"
                                            onClick={() => { showNoteModal(data) }}
                                            modifier="light">
                                            <Icon
                                                size={{ material: 17 }}
                                                icon={{ material: 'fa-pencil-square-o' }}
                                                style={{ color: data.note ? '#00aca1' : '#F37021' }} />
                                        </a>
                                    </div>
                                    <OrderCounter
                                        quantity={quantity}
                                        plusAction={_plusAction}
                                        minAction={_minusAction}
                                    />
                                </> :
                                <Button modifier="cta" onClick={_addButtonClicked}>Tambahkan</Button>
                        }
                    </div>
                </div>
            </div>
            {
                showTotal &&
                <div className="order-item__total">
                    <div className="order-item__left">Total</div>
                    <div className="order-item__right">{tenant.currency} {(quantity * currentPrice).toLocaleString("id-ID")}</div>
                </div>
            }

        </ListItem>
    );
};

const mapStateToProps = state => ({
    tenant: state.AppReducer.tenant,
    cart: state.CartReducer.cart
});

const mapDispatchToProps = dispatch => ({
    addItemToCart: (item) => dispatch(AddItemToCart(item)),
    updateItemQuantity: (item) => dispatch(UpdateItemQuantity(item)),
    removeItemFromCart: (item) => dispatch(RemoveItemFromCart(item)),
    showProductDetail: (item) => dispatch(ShowProductDetail(item)),
    pushScreen: (screen) => dispatch(PushScreen(screen))
});

export default connect(mapStateToProps, mapDispatchToProps)(OrderItem);