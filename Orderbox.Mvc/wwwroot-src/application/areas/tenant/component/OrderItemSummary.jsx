import React, { useState } from 'react';
import { ListItem } from 'react-onsenui';

import './OrderItem.css';

const OrderItemSummary = (object) => {
    let data = object.data;
    let totalPayment = parseFloat(data.Price) * parseInt(data.Amount);

    return (
        <ListItem id={data.Id} className='order-item order-item-summary'>
            <div className='order-item-left'>
                <img src={data.Image} className='list-item__thumbnail' />
            </div>
            <div className='order-item-right'>
                <div className='order-item-title'>{data.Name}</div>
                <div className='order-item-price'>{data.Amount} x {data.Price} = Rp {totalPayment}</div>
            </div>
        </ListItem>
    );
};

export default OrderItemSummary;