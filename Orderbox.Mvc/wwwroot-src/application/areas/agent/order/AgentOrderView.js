import $ from 'jquery';
import CrudHelper from '../../../component/CrudHelper';
import UiHelper from '../../../component/UiHelper';

var crudHelper = new CrudHelper();
var uiHelper = new UiHelper();

class AgentOrderView {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.initialize();
        self.registerPaymentResetButton();
        self.registerPaymentRejectButton();
        self.registerPaymentAcceptButton();
        self.registerUpdateStatusEvent();        
    }

    registerUpdateStatusEvent() {
        $("input[name='Status']")
            .change(function () {
                uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

                var form = $(this).closest('form');
                $.post(form.data('url'),
                    form.serialize(),
                    function (data) {
                        uiHelper.loadingOverlay('hide');

                        if (!data.isSuccess) {
                            self.alert.dismissableDanger(data.messageErrorTextArray[0]);
                            return;
                        }                 
                    
                        if (data.messageInfoTextArray) {
                            data.messageInfoTextArray.forEach(function (v) {
                                uiHelper.notyf.success(v);
                            });
                        }
                    });
            });
    }

    initialize() {
        // Activate menu
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.order').addClass('active');
    }

    registerPaymentResetButton() {
        var self = this;

        $('.btn-reset', self.element)
            .click(function (e) {
                e.preventDefault();
                self.onAction.call(this, { alertType: 'warningAlert' });
            })
    }

    registerPaymentRejectButton() {
        var self = this;

        $('.btn-reject', self.element)
            .click(function (e) {
                e.preventDefault();
                self.onAction.call(this, { alertType: 'dangerAlert' });
            })
    }

    registerPaymentAcceptButton() {
        var self = this;

        $('.btn-accept', self.element)
            .click(function (e) {
                e.preventDefault();
                self.onAction.call(this, {});
            })
    }

    onAction({ type = 'warning', alertType = 'successAlert' }) {
        var button = $(this);

        const onSaveReturnResponse = (result) => {
            uiHelper.loadingOverlay('hide');

            if (!result.isSuccess) {
                result.messageErrorTextArray.forEach(function (v) {
                    uiHelper.notyf.error(v);
                });
                return;
            }

            window.location.reload();
        }

        uiHelper[alertType].fire({
            title: button.data('title'),
            text: button.data('confirmation-message'),
            type: type,
            showCancelButton: true,
            focusCancel: true,
            footer: " "
        }).then(function (result) {
            if (!result.value) return;

            uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

            var url = button.attr('href');

            var onResponse = onSaveReturnResponse;
            var onRequestFailed = crudHelper.onSaveDoesNotReturnResponse;

            $.post(url, { tenantId: button.data('tenant-id') }, onResponse).fail(onRequestFailed);
        });
    }
}

export default AgentOrderView;