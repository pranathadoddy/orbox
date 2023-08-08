import $ from 'jquery';
import CommonDataTable from '../../../component/CommonDataTable';

var commonDataTable = new CommonDataTable();

class AgencyIndex {
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
        // Activate menu
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.dealsagency').addClass('active');
    }

    initializeGrid() {
        var self = this;

        var dataTable = commonDataTable.init(self.gridElement,
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
                    }
                },
                labels: {
                    search: "Cari agency"
                }
            });
    }

}

export default AgencyIndex;