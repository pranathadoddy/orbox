import $ from 'jquery';
import CommonDataTable from '../../../component/CommonDataTable';
import '../../../../lib/select2/select2.min.css';
import '../../../../lib/select2/select2.min.js';

var commonDataTable = new CommonDataTable();

class AgentVoucherIndex {
    constructor(element) {
        this.element = element;
        this.gridElement = $('table[grid]', self.element);
    }

    register() {
        var self = this;

        self.initialize();
        self.initializeGrid();
    }

    initialize() {
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.merchant').addClass('active');
    }

    initializeGrid() {
        var self = this;

        var dataTable = commonDataTable.init(self.gridElement,
            {
                filters: self.gridElement.data('filters')
            },
            {
                requestHandler: function (r) {
                    var categoryId = $("#search-category-field", self.element).val();

                    if (categoryId !== null) {
                        r.filters = `${self.gridElement.data('filters')} and CategoryId=${categoryId}`;
                    }

                    return r;
                },
                templates: {
                    header: `
                        <div id="{{ctx.id}}" class="{{css.header}}">
                            <div class="row"><div class="col-sm-12 actionBar">
                                <label class="align-middle mb-0 mr-1">
                                    <nobr>
                                        <div class="input-group">
                                            <div class="wrapping-search">
                                                <select id="search-category-field" class="search-category-field form-control form-control"></select>
                                            </div>
                                        </div>
                                    </nobr>
                                </label>
                                <p class="{{css.search}}"></p>
                                <p class="{{css.actions}}"></p>
                            </div>
                        </div>`
                },
                columns: {
                    Name: function (column, row) {
                        return `<a href="#" class="primaryLink text-inherit" data-id="${row.id}">${row.name}</a>`;
                    },
                    Price: function (column, row) {
                        if (row.discount > 0) {
                            let newPrice = row.price - (row.discount * row.price / 100);
                            return `<span class="text-linethrough">${row.price.toLocaleString('id-ID')}</span> ${newPrice.toLocaleString('id-ID')}`;
                        }
                        return row.price.toLocaleString('id-ID');
                    },
                    Edit: function (column, row) {
                        var icon = row.isEditable === false ? 'fa-eye' : 'fa-edit';
                        var title = row.isEditable === false ? 'View' : 'Edit';
                        return `<a href="#" class="btn btn-primary btn-sm btn-block primaryLink" data-id="${row.id}"><i class="fa ${icon}"></i> ${title}</a>`;
                    }
                },
                labels: {
                    search: "Cari produk"
                }
            });

        this.initializeCategoryFilter(dataTable);
    }

    initializeCategoryFilter(dataTable) {
        var self = this;
        var url = self.gridElement.data("search-category-url");

        $("#search-category-field", $(".bootgrid-header")).select2({
            placeholder: 'Pilih berdasarkan kategori',
            allowClear: true,
            ajax: {
                url: url,
                type: "POST",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        current: params.page || 1,
                        rowCount: 10,
                        filters: self.gridElement.data("filters")
                    };

                    if (typeof params.term !== 'undefined') {
                        query.searchPhrase = params.term; // search term
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

        $("#search-category-field", $(".bootgrid-header")).on("change", function (e) {
            dataTable.reload();
        });
    }
}

export default AgentVoucherIndex;