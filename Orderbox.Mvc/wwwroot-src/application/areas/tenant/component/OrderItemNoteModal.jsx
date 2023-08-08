import './OrderItemNoteModal.css';

import React, { useState, useEffect, useRef } from 'react';
import { Modal, Card, Button, Icon } from 'react-onsenui';
import TextareaAutosize from 'react-autosize-textarea';

import { connect } from 'react-redux';

import { AddItemNote } from '../redux/actions/CartAction';

const OrderItemNoteModal = ({
    data,
    hideModal,
    addItemNote,
    readOnly
}) => {

    const [note, setNote] = useState(data.note);

    const _onAddButtonClick = () => {
        addItemNote({
            id: data.id,
            note: note
        });

        hideModal();
    };

    return (<Modal isOpen={true} className="order-item-note-modal">
        <Card>
            <div className="order-item-modal__header">
                <Button modifier="quiet" onClick={() => hideModal()}><Icon icon="md-arrow-left" size={24} style={{ color: '#00aca1' }} /></Button>
                <span className="title">Catatan Untuk Penjual</span>
            </div>
            <TextareaAutosize
                value={note}
                onChange={(event) => setNote(event.target.value)}
                placeholder="Tambahkan Catatan Untuk Penjual (contoh: 'Ukuran XL' untuk produk pakaian, atau 'Agak pedas' untuk produk makanan)"
                disabled={readOnly}
                style={{ borderColor: '#00aca1', minHeight: 150 }}
            />
            {
                readOnly ? null :
                    <div className="note-container-button">
                        <Button modifier="large--cta" onClick={_onAddButtonClick} style={{ backgroundColor: '#F37021' }} ripple>Tambah</Button>
                    </div>
            }

        </Card>
    </Modal>);

}

const mapStateToProps = state => ({});

const mapDispatchToProps = dispatch => ({
    addItemNote: (item) => dispatch(AddItemNote(item)),
});

export default connect(mapStateToProps, mapDispatchToProps)(OrderItemNoteModal);