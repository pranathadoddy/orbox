import React, { useState } from 'react';
import { List } from 'react-onsenui';

import './ProductList.css';
import OrderItem from './OrderItem';
import OrderItemNoteModal from './OrderItemNoteModal';
import BlockLoader from './BlockLoader';

const ProductList = ({
    className,
    data,
    isLoading,
    showTotalPricePerListItem,
    readOnlyListItem
}) => {

    const [isOpenNoteModal, setIsOpenNoteModal] = useState(false);
    const [selectedOrderItem, setSelectedOrderItem] = useState(undefined);

    const _showNoteModal = (item) => {
        setIsOpenNoteModal(true);
        setSelectedOrderItem(item);
    };

    const _hideNoteModal = () => {
        setIsOpenNoteModal(false);
        setSelectedOrderItem(undefined);
    }

    return (
        <div className={`product ${className}`}>
            <List
                dataSource={data}
                renderRow={(row, index) => {
                    return (
                        <OrderItem
                            key={index}
                            product={row}
                            showTotal={showTotalPricePerListItem}
                            readOnly={readOnlyListItem}
                            showNoteModal={_showNoteModal}
                        />
                    );
                }}
            />
            {isLoading ? <div className="loader"><BlockLoader /></div> : null}
            {isOpenNoteModal ?
                <OrderItemNoteModal
                    data={selectedOrderItem}
                    hideModal={_hideNoteModal}
                    readOnly={readOnlyListItem}
                /> : null}
        </div>
    );
};

export default ProductList;