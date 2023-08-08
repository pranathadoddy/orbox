import './ons-textarea.css';

import React from 'react';

const OnsTextarea = ({ onChange, placeholder, rows, style, value }) => {
    const handleKeyDown = (e) => {
        let additionalHeight = 5;
        if (e.key === 'Enter') {
            additionalHeight = 20;
        }
        e.target.style.height = 'inherit';
        e.target.style.height = `${e.target.scrollHeight+additionalHeight}px`;
    }

    return (
        <ons-textarea modifier="material" float="" style={style}>
            <textarea
                className="text-input text-input--material"
                rows={rows}
                value={value}
                onChange={onChange}
                onKeyDown={handleKeyDown}
                style={{ width: '100%', height: 'inherit' }}
            ></textarea>
            <span
                className={`text-input__label text-input--material__label ${value && value !== "" ? "text-input--material__label--active" : ""}`}
            >{placeholder}</span>
        </ons-textarea>
    );
}

export default OnsTextarea;