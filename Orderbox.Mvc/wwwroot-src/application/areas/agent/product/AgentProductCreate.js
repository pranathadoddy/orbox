import $ from 'jquery';
import CrudHelper from '../../../component/CrudHelper';
import '../../../../lib/select2/select2.min.css';
import '../../../../lib/select2/select2.min.js';

const crudHelper = new CrudHelper();

class AgentProductCreate {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;

        self.initialize();
        self.initializeAgencyCategoryFilter();
        self.registerValidation();
        self.registerButtonSave();
        self.registerEvents();
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

    initializeAgencyCategoryFilter() {
        var self = this;

        var url = $('form', self.element).data("search-agency-category-url");

        $("#MultipleAgencyCategory", self.element).select2({
            placeholder: 'Pilih agency kategori',
            allowClear: true,
            ajax: {
                url: url,
                type: "POST",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var values = $("#MultipleAgencyCategory", self.element).val();

                    var query = {
                        current: params.page || 1,
                        rowCount: 10
                    };

                    if (typeof params.term !== 'undefined') {
                        query.searchPhrase = params.term; // search term
                    }
                    
                    if (values.length !== 0) {
                        var filters = [];
                        $.each(values, function (i, v) {
                            filters.push(`Id != ${v}`)
                        });
                        query.filters= filters.join(" and ")
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

export default AgentProductCreate;