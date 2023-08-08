import './OrderCounter.css';

import React from 'react';
import { Icon } from 'react-onsenui';

const OrderCounter = (props) => {
    return (
        <div className="order-counter">
            <a onClick={() => { props.minAction(); }} className="order-counter__icon-button">
                <Icon icon='fa-minus-square' className="icon" style={{ color: '#F37021' }} />
            </a>
            <input type="text" value={props.quantity} onChange={(e) => setQuantity(e.target.value)} className="order-counter__qty" />
            <a onClick={() => { props.plusAction(); }} className="order-counter__icon-button">
                <Icon icon="fa-plus-square" className="icon" style={{ color: '#F37021'}} />
            </a>
        </div>
    );
};

export default OrderCounter;