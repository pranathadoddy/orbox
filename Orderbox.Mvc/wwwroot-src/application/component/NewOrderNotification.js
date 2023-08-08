import * as firebase from 'firebase/app';
import 'firebase/messaging';
import UiHelper from './UiHelper';

const uiHelper = new UiHelper(); 

class NewOrderNotification {
    register() {
        const firebaseConfig = {
            apiKey: "AIzaSyDxAJ9nn7v_VrSZ2_IT4rQlNO_4xdd4h8k",
            authDomain: "orderbox-web.firebaseapp.com",
            databaseURL: "https://orderbox-web.firebaseio.com",
            projectId: "orderbox-web",
            storageBucket: "orderbox-web.appspot.com",
            messagingSenderId: "150143395622",
            appId: "1:150143395622:web:6e7d410ea4215b2e2a03e7",
            measurementId: "G-14600H7HSC"
        };
        firebase.initializeApp(firebaseConfig);
        const messaging = firebase.messaging();
        messaging.usePublicVapidKey("BMFuawCOCysJG7zZ3j17eLbpqUo88TMM6VrSVRTzh2umgXda8-TUpkIVyc6EnFsDJ3v0DfIuZqV66AW5KLKggIQ");
        messaging.getToken().then((currentToken) => {
            if (currentToken) {
                $.post(
                    '/User/Tenant/AddOrUpdatePostNotificationToken',
                    { token: currentToken },
                    function (data) {} //just simply do nothing
                );
            }
        });
        messaging.onMessage(function (payload) {
            const payloadData = payload.data;
            uiHelper.notyf.success(`${payloadData.Title} ${payloadData.Body}`);
            if ($('#order[grid]').length > 0) {
                $('#order[grid]').bootgrid('reload');
            }
        });
    }
}

export default NewOrderNotification;