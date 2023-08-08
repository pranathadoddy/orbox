import './RegistrationScreen.css';

import React, { useState, useEffect } from 'react';
import { BottomToolbar, Button, Card, Input, Page, Toolbar } from 'react-onsenui';
import { connect } from 'react-redux';

import BlockLoader from './component/BlockLoader';
import { PopScreen, SetCustomer } from './redux/actions/AppAction';

import UserService from './services/UserService';
const _userService = new UserService();

const RegistrationScreen = ({ customer, navigator, popScreen, setCustomer, tenant, user }) => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [email, setEmail] = useState('');
    const [phone, setPhone] = useState('');
    const [profilePicture, setProfilePicture] = useState('');
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        _userService
            .Read('', tenant.shortName, user.credential)
            .then(response => {
                var result = response.data;
                if (result.isSuccess) {
                    setFirstName(result.value.firstName);
                    setLastName(result.value.lastName);
                    setEmail(result.value.email);
                    setProfilePicture(result.value.picture);
                    setIsLoading(false);
                }
            });
    }, [])

    useEffect(() => {
        if (customer) {
            navigator.popPage();
            popScreen();
        }
    }, [customer])

    const onSubmit = () => {
        setCustomer({
            firstName,
            lastName,
            emailAddress: email,
            phone
        });
        setIsLoading(true);
    }

    const _renderToolbar = () => {
        return (
            <Toolbar>
                <div className="left"></div>
                <div className="center">Pendaftaran</div>
            </Toolbar>
        );
    }

    const _renderBottomToolbar = () => {
        return (
            <BottomToolbar className='container-price-cart product-screen-footer'>
                <div className='cart-container-price'>
                    <span>&nbsp;</span>
                </div>
                <div className='cart-container-btn'>
                    <Button disabled={firstName === '' || email === '' || phone === ''} onClick={onSubmit} modifier="large--cta" style={{ backgroundColor: '#00ACA1' }} ripple>Register</Button> :
                </div>
            </BottomToolbar>
        );
    }

    if (isLoading) {
        return (
            <Page>
                <Card>
                    <BlockLoader />
                </Card>
            </Page>
        );
    }

    return (
        <Page renderToolbar={_renderToolbar} renderBottomToolbar={_renderBottomToolbar}>
            <Card>
                <div className="registration-card">
                    <div className="image-container"><img src={profilePicture} /></div>
                    <Input modifier="material" float type="text" placeholder="First Name" value={firstName} onChange={(event) => { setFirstName(event.target.value); }} style={{ width: '100%', marginTop: '20px' }} />
                    <Input modifier="material" float type="text" placeholder="Last Name" value={lastName} onChange={(event) => { setLastName(event.target.value); }} style={{ width: '100%', marginTop: '20px' }} />
                    <Input modifier="material" float type="email" placeholder="Email" value={email} onChange={(event) => { setEmail(event.target.value); }} style={{ width: '100%', marginTop: '20px' }} />
                    <Input modifier="material" float type="number" placeholder="No. Telepon / Whatsapp" value={phone} onChange={(event) => { setPhone(event.target.value); }} style={{ width: '100%', marginTop: '20px' }} />
                </div>
            </Card>
        </Page>
    );
};

const mapStateToProps = state => ({
    tenant: state.AppReducer.tenant,
    user: state.AppReducer.user,
    customer: state.AppReducer.customer
});

const mapDispatchToProps = dispatch => ({
    popScreen: () => dispatch(PopScreen()),
    setCustomer: (data) => dispatch(SetCustomer(data))
});

export default connect(mapStateToProps, mapDispatchToProps)(RegistrationScreen);