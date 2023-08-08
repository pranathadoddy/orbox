import './ProductSearch.css';

import React, { useState, forwardRef, useImperativeHandle } from 'react';
import { Input } from 'react-onsenui';

const ProductSearch = ({ searchProduct }, ref) => {
    const [searchText, setSearchText] = useState('');

    useImperativeHandle(ref, () => ({
        clearText: () => {
            setSearchText('');
        }
    }));

    let timeout = null;

    return (
        <div className="product-search-container">
            <div className="product-search">
                <img src="https://cdn.orbox.id/obox/assets/img/logo.png" />
                <Input
                    modifier="material"
                    value={searchText}
                    onKeyUp={(event) => {
                        clearTimeout(timeout);
                        let keyword = event.target.value;
                        timeout = setTimeout(() => {
                            setSearchText(keyword);
                            searchProduct(keyword);
                            clearTimeout(timeout);
                        }, 550)
                    }}
                    placeholder='Cari produk'
                />
            </div>
        </div>
    );
};

export default forwardRef(ProductSearch);