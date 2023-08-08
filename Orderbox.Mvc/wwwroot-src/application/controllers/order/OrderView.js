import $ from 'jquery';
import UiHelper from '../../component/UiHelper';

var uiHelper = new UiHelper();

class OrderView {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.initialize();
        self.registerUpdateStatusEvent();
    }

    registerUpdateStatusEvent() {

        $("input[name='Status']").change(function () {
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
}

export default OrderView;