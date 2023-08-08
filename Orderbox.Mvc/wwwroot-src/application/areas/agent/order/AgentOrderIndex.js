﻿import $ from 'jquery';
import CommonDataTable from '../../../component/CommonDataTable';

var commonDataTable = new CommonDataTable();

class AgentOrderIndex {
    constructor(element, status) {
        this.element = element;
        this.gridElement = $('table[grid]', self.element);
        this.status = status;
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

        commonDataTable.init(self.gridElement, {
            filters: self.gridElement.data('filters')
        }, {
            requestHandler: function (r) {
                if (self.status !== null && self.status !== "") {
                    r.filters = `${r.filters} and status="${self.status}"`;
                }

                return r;
            },
            templates: {
                header: `<div id="{{ctx.id}}" class="{{css.header}}">
                            <div class="row"><div class="col-sm-12 actionBar">
                            <label class="align-middle mb-0 mr-1">
                            <nobr>
                                <div class="input-group">
                                    <div class="wrapping-search">
                                        <select id="search-order-field" class="search-order-field form-control form-control">
                                        </select>
                                    </div>
                                </div>
                            </nobr>
                            </label>
                            <p class="{{css.search}}"></p><p class="{{css.actions}}"></p></div></div>
                         </div>`
            },
            labels: {
                search: "Cari order"
            },
            columns: {
                BuyerName: function (column, row) {
                    if (row.status !== 'FIN') {
                        const buyerName = row.status === 'NEW' ? `<strong>${row.buyerName}</strong>` : row.buyerName;
                        return `${buyerName}<span class="badge ${row.statusBadgeColor} ml-1">${row.statusDescription}</span>`;
                    }

                    return row.buyerName;
                },
                Edit: function (column, row) {
                    return `<a href="#" class="primaryLink" data-id="${row.id}">${row.orderNumber}</a>`;
                }
            }
        });
        this.initializeOrderFilter();
    }

    initializeOrderFilter() {
        var self = this;
        var statusData = self.gridElement.data("status-list");
        let tenantId = self.gridElement.data('tenant-id');

        var select2 = $("#search-order-field", $(".bootgrid-header")).select2({
            placeholder: 'Filter berdasarkan status',
            allowClear: true,
            data: statusData
        });

        select2.val(self.status).trigger('change');

        $("#search-order-field", $(".bootgrid-header")).on("change", function (e) {
            self.status = $(this).val();
            window.location.href = "/Agent/Order/Index/"+tenantId+"/"+self.status;
        });
    }
}

export default AgentOrderIndex;