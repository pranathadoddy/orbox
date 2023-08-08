import './Splash.css';

import React from 'react';

const Splash = ({ message }) => {
    return (
        <div className="splash">
            <img src='https://cdn.orbox.id/obox/assets/img/logo.png' />
            <span className="appsname">Orderbox</span>
            <span className="small">{message || "loading..."}</span>
        </div>
    );
};

export default Splash;