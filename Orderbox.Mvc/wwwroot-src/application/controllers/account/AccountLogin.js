﻿import $ from 'jquery';

import Alert from '../../component/Base/Alert';
import Spinner from '../../component/Base/Spinner';
import CrudHelper from '../../component/CrudHelper';

const crudHelper = new CrudHelper();

class AccountLogin
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

        self.registerValidation();
        self.registerLoginButtonClick();
    }

    registerValidation()
    {
        let self = this;

        crudHelper.initializeValidation($('form', self.element));
    }

    registerLoginButtonClick()
    {
        let self = this;

        $('#btn-login', self.element).off('click').on('click',
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

export default AccountLogin;