import '../../../../lib/croppie/croppie.min.js';
import '../../../../lib/croppie/croppie.css';

import CrudHelper from '../../../component/CrudHelper';
import UiHelper from '../../../component/UiHelper';

const crudHelper = new CrudHelper();
const uiHelper = new UiHelper();

class TenantOrderOrderDetail {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.registerCroppie();
    }

    registerCroppie() {
        var self = this;

        var elementCroppie = $('#payment-proof-croppie').croppie({
            viewport: {
                width: 300,
                height: 300,
                type: 'square'
            },
            enableExif: true,
            enableResize: true,
            enableOrientation: true
        });

        $('#payment-proof-modal').on('shown.bs.modal',
            function () {
                elementCroppie.croppie('bind');
            });

        $('#payment-proof-modal #btn-upload').off('click').on('click',
            function () {
                elementCroppie.croppie('result',
                    {
                        type: 'canvas',
                        size: 'viewport',
                        format: 'png' //this is to make sure, the result is in png format
                    })
                    .then(function (resp) {
                        $('#payment-proof-modal').modal('hide');
                        uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

                        var fileName = $("#FileName").val();
                        var url = $('#form-payment-proof').attr('action');
                        var orderId = $('#OrderId', $('#form-payment-proof')).val();
                        $.post(url, { Base64File: resp, FileName: fileName, OrderId: orderId }, function (result) {
                            window.location.reload();
                        }).fail(crudHelper.onSaveReturnResponse.bind(self));
                    });
            });


        $("input[name=PaymentProof]", self.element)
            .on('change', function () { self.readUploadedFile(this, elementCroppie, $('#payment-proof-modal'), $("#FileName")); });

        $(".btn-upload-payment-proof", self.element)
            .click(function (e) {
                e.preventDefault();
                $("input[name=PaymentProof]", self.element).trigger('click');
            })
    }

    readUploadedFile(input, $croppieElement, $modal, $nameElement) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $croppieElement
                    .croppie('bind', { url: e.target.result })
                    .then(function () {
                        $modal.modal('show');
                        $nameElement.val(input.files[0].fileName);
                    });
            }

            reader.readAsDataURL(input.files[0]);
        }
    }
}

export default TenantOrderOrderDetail;