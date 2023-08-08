﻿import $ from 'jquery';
import '../../../../lib/croppie/croppie.min.js';
import '../../../../lib/croppie/croppie.css';
import '../../../../lib/bootstrap-datepicker/bootstrap-datepicker.css';
import '../../../../lib/bootstrap-datepicker/bootstrap-datepicker-custom.css';
import '../../../../lib/bootstrap-datepicker/bootstrap-datepicker.js';
import moment from 'moment';
import CrudHelper from '../../../component/CrudHelper';
import UiHelper from '../../../component/UiHelper';
import '../../../../lib/select2/select2.min.css';
import '../../../../lib/select2/select2.min.js';

const crudHelper = new CrudHelper();
const uiHelper = new UiHelper();

class AgentVoucherEdit {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.initialize();
        self.registerValidation();
        self.initializeAgencyCategoryMultipleSelect();
        self.initializeStoreMultipleSelect();
        self.registerButtonSave();
        self.registerEvents();
        self.registerCroppie();
        self.registerProductImageControl();
        self.initializeDatePicker($("#ValidPeriodStart", self.element), (datePicked) => {
            $("#ValidPeriodEnd", self.element).datepicker('setStartDate', datePicked);
        });

        self.initializeDatePicker($("#ValidPeriodEnd", self.element), (datePicked) => {
            $("#ValidPeriodStart", self.element).datepicker('setEndDate', datePicked);
        });

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
        $('.navbar-nav li.merchant').addClass('active');
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
                        var tenantId = $("#TenantId").val();
                        var url = $('#product-modal').data('upload-url');

                        $.post(url, {
                            Base64Logo: resp,
                            FileName: fileName,
                            ProductId: productId,
                            TenantId: tenantId
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
                    ${!data.isPrimary ?
                        `<a href="/Agent/Product/SetPrimary/${data.tenantId}/${data.id}" class="btn btn-primary btn-set-primary"><i class="fa fa-check mr-1"></i> Set Primary</a>` :
                        ''
                    }
                    <a href="/Agent/Product/DeleteImage/${data.tenantId}/${data.id}" class="btn btn-danger btn-remove-product-image"><i class="fa fa-trash mr-1"></i> Remove</a>
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

    initializeAgencyCategoryMultipleSelect() {
        var self = this;
        var url = $('form', self.element).data("search-agency-category-url");
        self.initializeMultipleSelect($("#MultipleAgencyCategory", self.element),
            url,
            'Pilih agency kategori',
            function (query) {
                var values = $("#MultipleAgencyCategory", self.element).val();
                if (values.length !== 0) {
                    var filters = [];
                    $.each(values, function (i, v) {
                        filters.push(`Id != ${v}`)
                    });
                    query.filters = filters.join(" and ")
                }

                return query;
            }
        )
    }

    initializeStoreMultipleSelect() {
        var self = this;
        var url = $('form', self.element).data("search-store-url");
        self.initializeMultipleSelect($("#MultipleStore", self.element), url, 'Pilih store',
            function (query) {
                var values = $("#MultipleStore", self.element).val();
                query.filters = `tenantId=${$("#TenantId", self.element).val()}`;
                if (values.length !== 0) {
                    var filters = [];
                    $.each(values, function (i, v) {
                        filters.push(`Id != ${v}`)
                    });

                    query.filters = `${query.filters} and ${filters.join(" and ")}`;
                }

                return query;
            })
    }

    initializeMultipleSelect(element, url, placeholder, setQueryFilter) {
        var self = this;

        element.select2({
            placeholder: placeholder,
            allowClear: true,
            ajax: {
                url: url,
                type: "POST",
                dataType: 'json',
                delay: 250,
                data: function (params) {

                    var query = {
                        current: params.page || 1,
                        rowCount: 10
                    };

                    if (typeof params.term !== 'undefined') {
                        query.searchPhrase = params.term; // search term
                    }

                    if (typeof setQueryFilter == 'function') {
                        query = setQueryFilter(query);
                    }

                    return query;
                },
                processResults: function (response, params) {
                    var results = response.rows.map((item) => { return { id: item.id, text: item.name } });

                    return {
                        results: results,
                        pagination: {
                            more: (params.page * 10) < response.total
                        }
                    };
                },
                cache: true
            }
        });
    }
}

export default AgentVoucherEdit;