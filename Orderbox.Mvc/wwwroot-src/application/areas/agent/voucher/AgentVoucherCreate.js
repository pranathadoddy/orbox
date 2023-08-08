import $ from 'jquery';
import '../../../../lib/bootstrap-datepicker/bootstrap-datepicker.css';
import '../../../../lib/bootstrap-datepicker/bootstrap-datepicker-custom.css';
import '../../../../lib/bootstrap-datepicker/bootstrap-datepicker.js';
import moment from 'moment';
import CrudHelper from '../../../component/CrudHelper';
import '../../../../lib/select2/select2.min.css';
import '../../../../lib/select2/select2.min.js';

const crudHelper = new CrudHelper();

class AgentVoucherCreate {
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
        crudHelper.submitFormWithFile(
            $('#btn-save', self.element),
            undefined,
            function (data) {
                // not implemented
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

    initializeDatePicker(element, onDateChanged) {
        element.datepicker({
            changeMonth: true,
            changeYear: true,
            format: "yyyy-mm-dd",
            language: "tr"
        }).on('changeDate', function (ev) {
            onDateChanged(ev.date);
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

export default AgentVoucherCreate;