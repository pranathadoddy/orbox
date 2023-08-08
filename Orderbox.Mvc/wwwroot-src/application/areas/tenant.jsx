import 'onsenui/css/onsenui.css';
import 'onsenui/css/onsen-css-components.css';

import './tenant.css';

import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { Navigator } from 'react-onsenui';
import ons from 'onsenui';
import { GoogleReCaptchaProvider } from 'react-google-recaptcha-v3';
import { PersistGate } from 'redux-persist/integration/react';

import Main from './tenant/Main.jsx';
import { FetchUser, SetUserAuthenticationReady } from './tenant/redux/actions/AppAction';

import configureStore from './tenant/redux/configureStore.jsx';
const { store, persistor } = configureStore();

const App = ({ fetchUser, setUserAuthenticationReady, tenant }) => {
    useEffect(() => {
        ons.platform.select('android');
    });

    useEffect(() => {
        if (tenant && tenant.customerMustLogin) {
            const script = document.createElement('script');
            const onLoadCallback = () => {
                setUserAuthenticationReady();
                fetchUser();
            }
            script.src = "https://accounts.google.com/gsi/client";
            script.async = true;
            script.defer = true;
            script.onload = onLoadCallback;
            document.body.appendChild(script);

            return () => {
                window.google?.accounts.id.cancel();
                document.body.removeChild(script);
            }
        }
    }, [tenant]);

    return (
        <Navigator
            renderPage={(route, navigator) => {
                let Component = route.component;
                return <Component {...route.props} navigator={navigator} />
            }}
            initialRoute={{ component: Main, props: { key: 'MAIN' } }}
        />
    );
};

const mapStateToProps = state => ({
    user: state.AppReducer.user,
    tenant: state.AppReducer.tenant
});

const mapDispatchToProps = dispatch => ({
    fetchUser: () => dispatch(FetchUser()),
    setUserAuthenticationReady: () => dispatch(SetUserAuthenticationReady())
});

const WrappedApp = connect(mapStateToProps, mapDispatchToProps)(App);

ReactDOM.render(
    <Provider store={store}>
        <GoogleReCaptchaProvider reCaptchaKey="6LdzDeoeAAAAAFibvAaUJ8YTJ79W8FVuGYx8xNdZ">
            <PersistGate loading={null} persistor={persistor}>
                <WrappedApp />
            </PersistGate>
        </GoogleReCaptchaProvider>
    </Provider>,
    document.getElementById('root')
);