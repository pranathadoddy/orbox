import './GoogleLoginButton.css';

import React from 'react';
import { connect } from 'react-redux';

import {
    Button
} from 'react-onsenui';

const GoogleLoginButton = ({ customer, onClick }) => {
    return (
        <Button onClick={onClick} className='btn btn-primary google-button'>
            <img src={customer.profilePicture} />
        </Button>
    );
};

const mapStateToProps = state => ({
    customer: state.AppReducer.customer
});

const mapDispatchToProps = dispatch => ({});

export default connect(mapStateToProps, mapDispatchToProps)(GoogleLoginButton);