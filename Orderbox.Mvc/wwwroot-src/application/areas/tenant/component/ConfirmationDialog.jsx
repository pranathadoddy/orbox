import React from 'react';

import {
    Dialog, Button
} from 'react-onsenui';

const ConfrimationDialog = (props) => {
    return (
        <Dialog
            isOpen={props.isShow}
            isCancelable={true}
            onCancel={() => { props.onCancel(); }}
        >
            <div style={{ textAlign: 'center', margin: '20px' }}>
                <p>{props.message}</p>
                <p>
                    <Button onClick={() => { props.onContinue(); }} className='btn btn-primary'>Lanjutkan</Button>
                    <Button onClick={() => { props.onCancel(); }} modifier='quiet' className='btn btn-close'>Tutup</Button>
                </p>
            </div>
        </Dialog>
    );
};

export default ConfrimationDialog;