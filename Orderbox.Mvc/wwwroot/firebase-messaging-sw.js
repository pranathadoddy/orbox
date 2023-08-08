importScripts('https://www.gstatic.com/firebasejs/7.15.0/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/7.15.0/firebase-messaging.js');

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

messaging.setBackgroundMessageHandler(function (payload) {
    const payloadData = payload.data;

    const notificationTitle = payloadData.Title;
    const notificationOptions = {
        body: payloadData.Body,
        icon: payloadData.Icon,
        data: {
            url: payloadData.RedirectUrl
        }
    };

    return self.registration.showNotification(notificationTitle, notificationOptions);
});

self.addEventListener('notificationclick', function (event) {
    clients.openWindow(event.notification.data.url);
}, false);