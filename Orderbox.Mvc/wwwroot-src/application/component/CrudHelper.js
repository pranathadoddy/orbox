import $ from 'jquery';
import '../../lib/jquery-validate/jquery.validate';
import UiHelper from './UiHelper';

var uiHelper = new UiHelper();

class CrudHelper
{
    initializeValidation($form)
    {
        $form.validate({
            highlight: function (element) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element) {
                $(element).removeClass('is-invalid');
            },
            errorPlacement: function (error, element) { }
        });
    }

    buttonSaveClicked(button, urlDestination, onSaveReturnCallback)
    {
        var self = this;

        button.off('click').on('click',
            function (e) {
                e.preventDefault();

                var isValid = $(this).closest('form').valid();
                if (!isValid) return;

                uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

                var url = urlDestination || $(this).closest('form').attr('action');
                var data = $(this).closest('form').serialize();

                var onResponse = onSaveReturnCallback || self.onSaveReturnResponse;
                var onRequestFailed = self.onSaveDoesNotReturnResponse;

                $.post(url, data, onResponse).fail(onRequestFailed);
            });
    }

    submitFormWithFile(button, urlDestination, onSaveReturnCallback) {
        var self = this;

        button.off('click').on('click',
            function (e) {
                e.preventDefault();

                var isValid = $(this).closest('form').valid();
                if (!isValid) return;

                uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

                var url = urlDestination || $(this).closest('form').attr('action');
                var data = new FormData($(this).closest('form')[0]);

                $.ajax({
                    type: 'POST',
                    url: url,
                    data: data,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        if (data.isSuccess) {
                            onSaveReturnCallback(data);
                            self.onSaveReturnResponse(data);
                        }
                        else {
                            self.onSaveDoesNotReturnResponse(data);
                        }
                    }
                });
            });
    }

    buttonDeleteClicked(button)
    {
        var self = this;
        console.log('delete cliked')
        button.off('click').on('click',
            function (e) {
                e.preventDefault();

                var form = $(this).closest('form');

                uiHelper.dangerAlert.fire({
                    title: form.data('delete-confirmation-message'),
                    text: form.data('delete-warning'),
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonText: form.data('custom-delete-yes-button'),
                    focusCancel: true,
                    footer: " "
                }).then(function (result) {
                    if (!result.value) return;

                    uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

                    var url = form.data('delete-url');

                    var onResponse = self.onDeleteReturnResponse;
                    var onRequestFailed = self.onDeleteDoesNotReturnResponse;

                    $.post(url, onResponse).fail(onRequestFailed);
                });
            });
    }

    buttonDeleteClickedWithCallback(button, callback) {
        var self = this;

        button.off('click').on('click',
            function (e) {
                e.preventDefault();

                var form = $(this).closest('form');
                var buttonEl = $(this);

                uiHelper.dangerAlert.fire({
                    title: form.data('delete-confirmation-message'),
                    text: form.data('delete-warning'),
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonText: form.data('custom-delete-yes-button'),
                    focusCancel: true,
                    footer: " "
                }).then(function (result) {
                    if (!result.value) return;

                    uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

                    var url = form.data('delete-url');

                    callback(buttonEl);
                    var onResponse = self.onDeleteReturnResponse;
                    var onRequestFailed = self.onDeleteDoesNotReturnResponse;

                    $.post(url, onResponse).fail(onRequestFailed);
                });
            });
    }

    onSaveReturnResponse(result)
    {
        uiHelper.loadingOverlay('hide');

        if (!result.isSuccess) {
            result.messageErrorTextArray.forEach(function (v) {
                uiHelper.notyf.error(v);
            });
            return;
        }

        if (result.messageInfoTextArray) {
            result.messageInfoTextArray.forEach(function (v) {
                uiHelper.notyf.success(v);
            });
        }

        if (result.redirectUrl) {            
            window.location = result.redirectUrl;
            return;
        }

        return;
    }

    onSaveDoesNotReturnResponse()
    {
        uiHelper.loadingOverlay('hide');
        uiHelper.notyf.error($('[generalmessage]').data('network-error'));
    }

    onDeleteReturnResponse(result)
    {
        uiHelper.loadingOverlay('hide');

        if (!result.isSuccess) {
            result.messageErrorTextArray.forEach(function (v) {
                uiHelper.notyf.error(v);
            });
            return;
        }

        if (result.redirectUrl) {
            window.location = result.redirectUrl;
            return;
        }

        result.messageInfoTextArray.forEach(function (v) {
            uiHelper.notyf.success(v);
        });
    }

    onDeleteDoesNotReturnResponse()
    {
        uiHelper.loadingOverlay('hide');
        uiHelper.notyf.error($('[generalmessage]').data('network-error'));
    }
}

export default CrudHelper;