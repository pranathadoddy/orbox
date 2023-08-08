import './SimpleInquiry.css';

import React, { useEffect, useState, useRef } from 'react';
import { connect } from 'react-redux';

import {
    Input,
    Card
} from 'react-onsenui';
import OnsTextarea from '../../element/ons-textarea';

const SimpleInquiry = ({ buyers, customer, onChange, tenant, user, userAuthenticationReady }) => {
    const googleLoginButtonRef = useRef();

    const [phone, setPhone] = useState();
    const [email, setEmail] = useState();
    const [name, setName] = useState();
    const [profilePicture, setProfilePicture] = useState();
    const [description, setDescription] = useState();

    useEffect(() => {
        if (tenant.customerMustLogin && userAuthenticationReady) {
            window.google.accounts.id.renderButton(googleLoginButtonRef.current, { type: 'standard', theme: 'outline', shape: 'square', size: 'large' });
        }
    }, [userAuthenticationReady])

    useEffect(() => {
        if (customer) {
            setPhone(customer.phone);
            setEmail(customer.emailAddress);
            setName(`${customer.firstName} ${customer.lastName}`);
            setProfilePicture(customer.profilePicture);
        }
    }, [customer])

    useEffect(() => {
        onChange({ phone, email, name, description })
    }, [phone, email, name, description])

    const getBuyerInformation = (phone) => {
        const buyer = buyers.find(item => item.phone === phone);

        if (buyer) {
            setName(buyer.name);
            setEmail(buyer.email);
        }
    }

    return (
        <div className="simple-inquiry">
            <Card>
                <div className="title">
                    Pembeli
                </div>
                {
                    tenant.customerMustLogin && !user ?
                        <div className="google-auth" ref={googleLoginButtonRef}></div> :
                        <>
                            {
                                profilePicture ?
                                    <img src={profilePicture} alt="Profile Picture" style={{ borderRadius: '50%', width: '60px' }} /> :
                                    null
                            }
                            <Input
                                modifier="material"
                                float
                                type="number"
                                placeholder="No. Telepon / Whatsapp"
                                value={phone}
                                onBlur={(event) => { getBuyerInformation(event.target.value); }}
                                onChange={(event) => { setPhone(event.target.value); }}
                                style={{ width: '100%', marginTop: '20px' }}
                                disabled={tenant.customerMustLogin}
                            />
                            <Input
                                modifier="material"
                                float
                                type="email"
                                placeholder="Email"
                                value={email}
                                onChange={(event) => { setEmail(event.target.value); }}
                                style={{ width: '100%', marginTop: '20px' }}
                                disabled={tenant.customerMustLogin}
                            />
                            <Input
                                modifier="material"
                                float
                                type="text"
                                placeholder="Nama"
                                value={name}
                                onChange={(event) => { setName(event.target.value); }}
                                style={{ width: '100%', marginTop: '20px' }}
                                disabled={tenant.customerMustLogin}
                            />
                            <OnsTextarea
                                rows={8}
                                placeholder='Description'
                                value={description}
                                onChange={(event) => setDescription(event.target.value)}
                                style={{ width: '100%', marginTop: '20px', borderColor: 'transparent' }}
                            />
                        </>
                }
            </Card>
        </div>
    );
}

const mapStateToProps = state => ({
    buyers: state.BuyerReducer.buyers,
    customer: state.AppReducer.customer,
    tenant: state.AppReducer.tenant,
    user: state.AppReducer.user,
    userAuthenticationReady: state.AppReducer.userAuthenticationReady
});

const mapDispatchToProps = dispatch => ({});

export default connect(mapStateToProps, mapDispatchToProps)(SimpleInquiry);