import $ from 'jquery';

import Alert from '../../component/Base/Alert';
import Spinner from '../../component/Base/Spinner';
import CrudHelper from '../../component/CrudHelper';
import { CountryPhoneCodes } from '../../component/CountryPhoneCodes';
import '../../../lib/select2/select2.min.css';
import '../../../lib/select2/select2.min.js';

const crudHelper = new CrudHelper();

class AccountActivation
{
    constructor(element)
    {
        this.element = element;
        this.spinner = new Spinner($('[spinnerpane]', element).first());
        this.alert = new Alert($('[errorpane]', element).first());
    }

    register()
    {
        let self = this;

        self.formInitialize();
        self.registerValidation();
        self.registerConfirmButtonClick();
    }

    formInitialize() {
        var self = this;
        var countryCode = $("#CountryId", self.element).val();

        if ($("#Phone", self.element).val().length === 0) {
            $("#AreaCode", self.element)
                .val(CountryPhoneCodes[countryCode]);
            $(".area-code", self.element)
                .text(CountryPhoneCodes[countryCode]);
        }

        function format(state) {
            if (!state.id) return state.text; // optgroup
            return $("<span><img class='flag' src='https://cdn.orbox.id/obox/assets/img/flags/" + state.id.toLowerCase() + ".png'/>" + state.text + "</span>");
        }

        $("#CountryId", self.element).select2({
            templateResult: format,
            templateSelection: format
        });

        $("#CountryId", self.element).on("change", function (e) {
            $("#AreaCode", self.element)
                .val(CountryPhoneCodes[$(this).val()]);
            $(".area-code", self.element)
                .text(CountryPhoneCodes[$(this).val()]);
        });

        self.tenantDomainEventControl();
    }

    tenantDomainEventControl() {
        var self = this;

        $("#TenantDomain", self.element).on("keypress", function (e) {

            if (!e.key.match(/^[a-zA-Z0-9]$/)) {
                e.preventDefault();
            }

        });

        $("#TenantDomain", self.element).on("keyup", function () {
            $("#tenant-domain-result", self.element).html($(this).val().toLowerCase());
        });
    }

    registerValidation()
    {
        let self = this;

        crudHelper.initializeValidation($('form', self.element));
    }
    
    registerConfirmButtonClick()
    {
        let self = this;

        $('#btn-activate', self.element).off('click').on('click',
            function (e) {
                e.preventDefault();

                var isValid = $(this).closest('form').valid();
                if (!isValid) return;

                var btnSubmit = this;

                $(btnSubmit).hide();
                self.spinner.show();

                var form = $(this).closest('form');
                $.post(form.attr('action'),
                    form.serialize(),
                    function (data) {
                        if (!data.isSuccess) {
                            $(btnSubmit).show();
                            self.spinner.hide();
                            self.alert.dismissableDanger(data.messageErrorTextArray[0]);
                            return;
                        }

                        window.location = data.redirectUrl;
                    });
            });
    }
}

export default AccountActivation;