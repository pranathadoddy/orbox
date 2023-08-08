import './BlockLoader.css';

import React, { Component } from 'react';

class BlockLoader extends Component {
    render() {
        return (
            <div className="loading-shading">
                <div className="loading-indicator">
                    <div className="lds-ripple"><div /><div /></div>
                </div>
            </div>
        );
    }
}

export default BlockLoader;