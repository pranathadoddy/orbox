import $ from 'jquery';
import UiHelper from './UiHelper';
import CrudHelper from './CrudHelper';

var uiHelper = new UiHelper();
var crudHelper = new CrudHelper();

class SweetAlertHelper
{
    alertConfirmation(optParam)
    {
        var self = this;

        var options = {
            title: optParam.title,
            text: optParam.warning,
            type: optParam.type,
            showCancelButton: true,
            confirmButtonText: optParam.confirmButtonText,
            focusCancel: true,
            footer: " "
        };

        var alert;

        switch (optParam.theme) {
            case 'danger':
                alert = uiHelper.dangerAlert(options);
                break;
            case 'success':
                alert = uiHelper.successAlert(options);
                break;
            default:
                alert = uiHelper.alert(options);
        }

        alert.then(function (result) {
            if (!result.value) return;

            uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

            var onResponse = crudHelper.onSaveReturnResponse;
            var onRequestFailed = crudHelper.onSaveDoesNotReturnResponse;

            $.post(optParam.url, onResponse).fail(onRequestFailed);
        });
    }

    alertConfirmationWithReason(optParam) {
        var self = this;

        var options = {
            title: optParam.title,
            html: '' +
                '<div class="mb-3">' +
                    '<div id="swal2-content" class="d-block mb-3">' +
                        optParam.warning +
                    '</div>' +
                    '<form id="reason-form"><textarea class="form-control" id="Reason" name="Reason" placeholder="Reason" required=""></textarea></form>' +
                '</div>',
            type: optParam.type,
            showCancelButton: true,
            confirmButtonText: optParam.confirmButtonText || 'Yes',
            focusCancel: true,
            footer: " ",
            preConfirm: function () {
                var $popupForm = $('#reason-form');
                crudHelper.initializeValidation($popupForm);
                $('#Reason', $popupForm).off('focus').on('focus',
                    function () {
                        window.swal.resetValidationError();
                    });

                var promise = (new Promise(function (resolve) {
                    var isValid = $popupForm.valid();
                    if (isValid) resolve($popupForm.serialize());
                    else throw new Error(optParam.reasonErrorMessage || 'Reason is required!');
                })).catch(function (error) {
                    window.swal.showValidationError(error);
                });

                return promise;
            }
        };

        var alert;

        switch (optParam.theme) {
            case 'danger':
                alert = uiHelper.dangerAlert(options);
                break;
            case 'success':
                alert = uiHelper.successAlert(options);
                break;
            default:
                alert = uiHelper.alert(options);
        }

        alert.then(function (result) {
            if (!result.value) return;

            uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

            var onResponse = crudHelper.onSaveReturnResponse;
            var onRequestFailed = crudHelper.onSaveDoesNotReturnResponse;

            $.post(optParam.url, result.value, onResponse).fail(onRequestFailed);
        });
    }
}

export default SweetAlertHelper;