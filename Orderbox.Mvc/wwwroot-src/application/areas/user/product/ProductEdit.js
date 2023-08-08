import $ from 'jquery';
import '../../../../lib/croppie/croppie.min.js'
import '../../../../lib/croppie/croppie.css'
import CrudHelper from '../../../component/CrudHelper';
import UiHelper from '../../../component/UiHelper';

const crudHelper = new CrudHelper();
const uiHelper = new UiHelper();

class ProductEdit {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.initialize();
        self.registerValidation();
        self.registerButtonSave();
        self.registerEvents();
        self.registerCroppie();
        self.registerProductImageControl();
    }

    registerValidation() {
        var self = this;
        crudHelper.initializeValidation($('form', self.element));
    }

    registerButtonSave() {
        var self = this;
        crudHelper.submitFormWithFile($('#btn-save', self.element), undefined, function (data) {
            $('#image-panel').show();
            $('#product-image').attr('src', data.value.imageUrl);
        });
    }

    registerEvents() {
        $("#File").on('change', function () {
            var filename = $('#File').val();
            $('.form-file-text').html(filename);
        });
    }

    initialize() {
        // Activate menu
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.product').addClass('active');
    }

    registerCroppie() {
        var self = this;

        $('.product-image-add a', self.element)
            .click(function (e) {
                e.preventDefault();
                $('.product-image-add input[type="file"]', self.element).trigger('click');
            })

        var productImageCroppie = $('#product-croppie').croppie({
            viewport: {
                width: 300,
                height: 300,
                type: 'square'
            },
            enableExif: true,
            enableResize: true,
            enableOrientation: true
        });

        $('#product-modal').on('shown.bs.modal',
            function () {
                productImageCroppie.croppie('bind');
            });

        $('#product-modal #btn-upload').off('click').on('click',
            function () {
                productImageCroppie.croppie('result',
                    {
                        type: 'canvas',
                        size: 'viewport',
                        format: 'png'
                    })
                    .then(function (resp) {
                        $('#product-modal').modal('hide');
                        uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

                        var fileName = $("#ImageName").val();
                        var productId = $("#ProductId").val();
                        var url = $('#product-modal').data('upload-url');

                        $.post(url, {
                            Base64Logo: resp,
                            FileName: fileName,
                            ProductId: productId
                        }, function (result) {
                            crudHelper.onSaveReturnResponse.call(self, result);
                            self.renderUploadedImage(result.value);
                        }).fail(crudHelper.onSaveReturnResponse.bind(self));
                    });
            });

        $('.product-image-add input[type="file"]', self.element)
            .on('change', function () { self.readUploadedFile(this, productImageCroppie); });
    }

    readUploadedFile(input, productImageCroppie) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                productImageCroppie
                    .croppie('bind', { url: e.target.result })
                    .then(function () {
                        $('#product-modal').modal('show');
                        $("#ImageName").val(input.files[0].name);
                    });
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    renderUploadedImage(data) {
        var self = this;

        var image = `
            <div class="col-12 col-md-3 product-image-container mb-3" data-id="${data.id}">
                <div class="card product-image" style="background-image: url(${data.fileName})"></div>
                <div class="m-0 d-flex">
                    ${
                        !data.isPrimary ?
                            '<a href="/User/Product/SetPrimary?id=' + data.id +'" class="btn btn-primary btn-set-primary"><i class="fa fa-check mr-1"></i> Set Primary</a>' :
                            ''
                    }
                    <a href="/User/Product/DeleteImage?id=${data.id}" class="btn btn-danger btn-remove-product-image"><i class="fa fa-trash mr-1"></i> Remove</a>
                </div>
            </div>`;
        $('.product-image-add').parent().before(image);

        self.registerProductImageControl();

        if ($('.product-image-container').length === 10) {
            $('.product-image-add').parent().addClass("d-none");
        }
    }

    registerProductImageControl() {
        var self = this;

        $('.btn-remove-product-image', self.element).each(function (i, v) {
            $(v).off('click').on('click', self.registerDeleteButton);
        });

        $('.btn-set-primary', self.element).each(function (i, v) {
            $(v).off('click').on('click', self.registerSetPrimary);
        });
    }

    registerDeleteButton(e) {
        e.preventDefault();
        var thisButton = this;

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

            var url = $(thisButton).attr('href');

            var onResponse = function (result) {
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

                if (result.value.isPrimary) {
                    window.location.reload();
                    return;
                }

                $(thisButton).closest('.product-image-container').remove();
                $('.product-image-add').parent().removeClass("d-none");
            };
            var onRequestFailed = crudHelper.onDeleteDoesNotReturnResponse;

            $.post(url, onResponse).fail(onRequestFailed);
        });
    }

    registerSetPrimary(e) {
        e.preventDefault();
        var thisButton = this;

        var form = $(this).closest('form');

        uiHelper.successAlert.fire({
            title: form.data('set-primary-confirmation-message'),
            showCancelButton: true,
            focusCancel: true,
            footer: " "
        }).then(function (result) {
            if (!result.value) return;

            uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

            var url = $(thisButton).attr('href');

            var onResponse = function (result) {
                uiHelper.loadingOverlay('hide');

                if (!result.isSuccess) {
                    result.messageErrorTextArray.forEach(function (v) {
                        uiHelper.notyf.error(v);
                    });
                    return;
                }

                window.location.reload();
            };
            var onRequestFailed = crudHelper.onDeleteDoesNotReturnResponse;

            $.post(url, onResponse).fail(onRequestFailed);
        });
    }
}

export default ProductEdit;