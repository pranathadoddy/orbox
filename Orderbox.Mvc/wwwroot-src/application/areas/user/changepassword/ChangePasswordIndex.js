import $ from 'jquery';
import CrudHelper from '../../../component/CrudHelper';
import UiHelper from '../../../component/UiHelper';

const crudHelper = new CrudHelper();
const uiHelper = new UiHelper();

class ChangePasswordIndex {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.registerChangePassword();
        self.viewPasswordButtonClick();
    }

    registerChangePassword() {
        var self = this;
        crudHelper.buttonSaveClicked($('#btn-change-password', self.element), undefined, function (result) {
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

            $("#OldPassword", self.element).val('');
            $("#NewPassword", self.element).val('');
        });
    }

    viewPasswordButtonClick() {
        var self = this;
        $(".view-password", self.element).off("click").on("click", function (event) {

            $(this).closest(".form-group").find(".form-control").attr("type", "text");
            $(this).removeClass("view-password");
            $(this).addClass("hide-password");
            var icon = $(this).find("i");
            icon.removeClass("fa-eye");
            icon.addClass("fa-eye-slash");

            self.hidePasswordButtonClick();
        });
    }

    hidePasswordButtonClick() {
        var self = this;
        $(".hide-password", self.element).off("click").on("click", function (event) {

            $(this).closest(".form-group").find(".form-control").attr("type", "password");
            $(this).addClass("view-password");
            $(this).removeClass("hide-password");
            var icon = $(this).find("i");
            icon.removeClass("fa-eye-slash");
            icon.addClass("fa-eye");

            self.viewPasswordButtonClick();

        });
    }
}

export default ChangePasswordIndex;