import $ from 'jquery';
import CommonDataTable from '../../../component/CommonDataTable';
import UiHelper from '../../../component/UiHelper';

var commonDataTable = new CommonDataTable();
var uiHelper = new UiHelper();

class AgentMerchantIndex {
    constructor(element) {
        this.element = element;
        this.gridElement = $('table[grid]', self.element);
    }

    register() {
        var self = this;

        self.initialize();
        self.initializeGrid();
        self.initializeButtonCopy();
    }

    initialize() {
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.merchant').addClass('active');
    }

    initializeGrid() {
        var self = this;

        commonDataTable.init(self.gridElement,
            null,
            {
                columns: {
                    Name: function (column, row) {
                        return "<a href=\"#\" class=\"primaryLink text-inherit\" data-id=\"" + row.id + "\">" + row.name + "</a>";
                    },                    
                    Edit: function (column, row) {
                        var icon = row.isEditable === false ? 'fa-eye' : 'fa-edit';
                        var title = row.isEditable === false ? 'View' : 'Edit';
                        return "<a href=\"#\" class=\"btn btn-primary btn-sm btn-block primaryLink\" data-id=\"" + row.id + "\"><i class=\"fa " + icon + "\"></i> " + title + "</a>";
                    },
                    Active: function (column, row) {
                        return row.enableShop ? `<span class="text-success">Active</span>` : `<span class="text-secondary">Inactive</span>`;
                    }
                },
                labels: {
                    search: "Cari merchant"
                }
            });
    }

    initializeButtonCopy() {
        $('#link-copy').click(function (e) {
            e.preventDefault();
            navigator.clipboard.writeText($(this).attr('href'));
            uiHelper.notyf.success("Copied to clipboard.")
        })
    }
}

export default AgentMerchantIndex;