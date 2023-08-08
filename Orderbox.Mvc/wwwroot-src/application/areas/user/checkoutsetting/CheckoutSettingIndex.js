import CrudHelper from '../../../component/CrudHelper';

const crudHelper = new CrudHelper();

class CheckoutSettingIndex {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.registerValidation();
        self.registerButtonSave();
        self.registerCustomerMustLoginToggle();
        self.registerPayments();
    }

    registerValidation() {
        var self = this;
        crudHelper.initializeValidation($('#form-checkout-setting', self.element));
    }

    registerButtonSave() {
        var self = this;
        crudHelper.buttonSaveClicked($('#btn-save', self.element));
    }

    registerCustomerMustLoginToggle() {
        var self = this;

        if ($('#CustomerMustLogin').prop('checked')) {
            $('#GoogleOAuthClientId-container').removeClass('d-none');
        }

        $('#CustomerMustLogin', self.element).change(function (e) {
            if (e.target.checked) {
                $('#GoogleOAuthClientId-container').removeClass('d-none');
            }
            else {
                $('#GoogleOAuthClientId-container').addClass('d-none');
            }
        });
    }

    registerPayments() {
        var self = this;

        crudHelper.buttonDeleteClickedWithCallback($('.btn-remove-payment'), function (buttonEl) {
            buttonEl.closest('.list-separated-item').first().remove();
        });

        crudHelper.submitFormWithFile($('.btn-submit-payment'), "/User/Payment/Create", function (data) {
            populatePaymentListItem(data.value);
            $("#paymentModal").modal("hide");
            $('#form-payment').find("input").val("");
        });

        $('.btn-add-payment').click(function (e) {
            e.preventDefault();
            $("#paymentModal").modal("show");
        });

        $('#PaymentOptionCode').change(function () {
            if ($(this).val() === "COD") {
                $('.panel-bank').hide();
                $('.panel-wallet').hide();
                $('.panel-account').hide();
                $('.panel-payment-gateway').hide();
                $('.panel-payment-gateway-api-key').hide();
                $('.panel-payment-gateway-webhook-secret').hide();
                $('.panel-payment-gateway-active-duration').hide();
            }
            if ($(this).val() === "BNK") {
                $('.panel-bank').show();
                $('.panel-wallet').hide();
                $('.panel-account').show();
                $('.panel-payment-gateway').hide();
                $('.panel-payment-gateway-api-key').hide();
                $('.panel-payment-gateway-webhook-secret').hide();
                $('.panel-payment-gateway-active-duration').hide();
            }
            else if ($(this).val() === "WLT") {
                $('.panel-wallet').show();
                $('.panel-bank').hide();
                $('.panel-account').show();
                $('.panel-payment-gateway').hide();
                $('.panel-payment-gateway-api-key').hide();
                $('.panel-payment-gateway-webhook-secret').hide();
                $('.panel-payment-gateway-active-duration').hide();
            }
            else if ($(this).val() === "PMGW") {
                $('.panel-bank').hide();
                $('.panel-wallet').hide();
                $('.panel-account').hide();
                $('.panel-payment-gateway').show();
                $('.panel-payment-gateway-api-key').show();
                $('.panel-payment-gateway-webhook-secret').show();
                $('.panel-payment-gateway-active-duration').show();
            }
        });

        function populatePaymentListItem(data) {

            var providerName = data.providerName;
            if (data.paymentOptionCode === "BNK") {
                providerName = "Bank " + data.providerName;
            }
            else if (data.paymentOptionCode === "COD") {
                providerName = "Bayar di Tempat (COD)";
            }

            var paymentListItem = '';
            paymentListItem += '<li class="list-separated-item">';
            paymentListItem += '    <form data-delete-url="/User/Payment/Delete?Id=' + data.id + '"';
            paymentListItem += '        data-delete-confirmation-message="Pesan Konfirmasi"';
            paymentListItem += '        data-delete-warning="Lanjutkan menghapus data?"';
            paymentListItem += '        data-custom-delete-yes-button="Ya">';
            paymentListItem += '        <div class="row align-items-center">';
            paymentListItem += '            <div class="col-auto">';
            paymentListItem += '                <span class="avatar avatar-md d-block">' + providerName[0] + '</span>';
            paymentListItem += '            </div>';
            paymentListItem += '            <div class="col">';
            paymentListItem += '                <div>';
            if (data.paymentOptionCode === "COD" || data.paymentOptionCode === "PMGW") {
                paymentListItem += '                    <a href="javascript:void(0)" class="text-inherit">' + providerName + '</a>';
            }
            else {
                paymentListItem += '                    <a href="javascript:void(0)" class="text-inherit">' + providerName + " - " + data.accountName + '</a>';
            }
            paymentListItem += '                </div>';
            if (data.paymentOptionCode !== "COD" && data.paymentOptionCode !== "PMGW") {
                paymentListItem += '                <small class="d-block item-except text-sm text-muted h-1x">' + data.accountNumber + '</small>';
            }
            paymentListItem += '            </div>';
            paymentListItem += '            <div class="col-auto">';
            paymentListItem += '                <button class="btn btn-danger btn-remove-payment"><i class="fa fa-1x fa-trash"></i></button>';
            paymentListItem += '            </div>';
            paymentListItem += '        </div>';
            paymentListItem += '    </form>';
            paymentListItem += '</li>';

            $('#paymentGroup').append($(paymentListItem));
            crudHelper.buttonDeleteClickedWithCallback($('.btn-remove-payment'), function (buttonEl) {
                buttonEl.closest('.list-separated-item').first().remove();
            });
        }
    }
}

export default CheckoutSettingIndex;