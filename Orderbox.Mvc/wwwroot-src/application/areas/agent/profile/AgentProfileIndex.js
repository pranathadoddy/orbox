import '../../../../lib/croppie/croppie.min.js';
import '../../../../lib/croppie/croppie.css';
import '../../../../lib/select2/select2.min.css';
import '../../../../lib/select2/select2.min.js';
import autosize from '../../../../lib/autosize/autosize.min.js';
import CrudHelper from '../../../component/CrudHelper';
import UiHelper from '../../../component/UiHelper';
import { CountryPhoneCodes } from '../../../component/CountryPhoneCodes';

const crudHelper = new CrudHelper();
const uiHelper = new UiHelper();

class AgentProfileIndex {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.initialize();
        self.formInitialize();
        self.registerCroppie();
        self.registerWallpaperCroppie();
        self.registerValidation();
        self.registerButtonSave();
    }

    initialize() {
        var self = this;
        // Activate menu
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.merchant').addClass('active');
        $('textarea').each(function () {
            autosize(this);
        });
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
            $("#Phone", self.element).val('');
        });
    }

    registerValidation() {
        var self = this;
        crudHelper.initializeValidation($('#form-profile', self.element));
    }

    registerButtonSave() {
        var self = this;
        crudHelper.buttonSaveClicked($('#btn-save', self.element));
    }

    registerCroppie() {
        var self = this;

        var logoCroppie = $('#logo-croppie').croppie({
            viewport: {
                width: 300,
                height: 300,
                type: 'square'
            },
            enableExif: true,
            enableResize: true,
            enableOrientation: true
        });

        $('#logo-modal').on('shown.bs.modal',
            function () {
                logoCroppie.croppie('bind');
            });

        $('#logo-modal #btn-upload').off('click').on('click',
            function () {
                logoCroppie.croppie('result',
                    {
                        type: 'canvas',
                        size: 'viewport',
                        format: 'png' //this is to make sure, the result is in png format
                    })
                    .then(function (resp) {
                        $('#logo-modal').modal('hide');
                        uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

                        var fileName = $("#LogoName").val();
                        var url = $('#form-logo').attr('action');
                        var tenantId = $('#TenantId', $('#form-logo')).val();
                        $.post(url, { Base64File: resp, FileName: fileName, TenantId: tenantId }, function (result) {
                            crudHelper.onSaveReturnResponse.call(self, result);
                            $('.profile-logo').css('background-image', 'url(' + resp + ')');
                            $('#logo-placeholder-nav').attr('style', '').css('background-image', 'url(' + resp + ')');
                        }).fail(crudHelper.onSaveReturnResponse.bind(self));
                    });
            });


        $("input[name=Logo]", self.element)
            .on('change', function () { self.readUploadedFile(this, logoCroppie, $('#logo-modal'), $("#LogoName")); });
    }

    registerWallpaperCroppie() {
        var self = this;

        var containerWidth = $('#wallpaper-croppie').closest('.modal-dialog').css('max-width').replace('px', '');
        var width = parseInt(containerWidth);
        if (!width) {
            width = screen.width;
        }
        width = width - 56;
        var height = (2 / 5) * width;

        var wallpaperCroppie = $('#wallpaper-croppie').croppie({
            viewport: {
                width,
                height,
                type: 'square'
            },
            enableExif: true,
            enableResize: true,
            enableOrientation: true
        });

        $('#wallpaper-modal').on('shown.bs.modal',
            function () {
                wallpaperCroppie.croppie('bind');
            });

        $('#wallpaper-modal #btn-upload-wallpaper').off('click').on('click',
            function () {
                wallpaperCroppie.croppie('result',
                    {
                        type: 'canvas',
                        size: { width: 750, height: 300 },
                        format: 'png' //this is to make sure, the result is in png format
                    })
                    .then(function (resp) {
                        $('#wallpaper-modal').modal('hide');
                        uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

                        var fileName = $("#WallpaperName").val();
                        var url = $('#form-wallpaper').attr('action');
                        var tenantId = $('#TenantId', $('#form-wallpaper')).val();
                        $.post(url, { Base64File: resp, FileName: fileName, TenantId: tenantId }, function (result) {
                            crudHelper.onSaveReturnResponse.call(self, result);
                            $('.profile-wallpaper').css('background-image', 'url(' + resp + ')');
                            $('[btn-unset-wallpapper]', self.element).parent().removeClass('d-none');
                        }).fail(crudHelper.onSaveReturnResponse.bind(self));
                    });
            });


        $("input[name=Wallpaper]", self.element)
            .on('change', function () { self.readUploadedFile(this, wallpaperCroppie, $("#wallpaper-modal"), $("#WallpaperName")); });

        crudHelper.buttonDeleteClicked($('[btn-unset-wallpapper]', self.element));
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

export default AgentProfileIndex;