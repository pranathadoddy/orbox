import $ from 'jquery';
import CommonDataTable from '../../../component/CommonDataTable';
import CrudHelper from '../../../component/CrudHelper';

const crudHelper = new CrudHelper();
var commonDataTable = new CommonDataTable();

class AgentMerchantEdit {
    constructor(element) {
        this.element = element;
        this.gridElement = $('table[grid]', self.element);
    }

    register() {
        var self = this;

        self.initialize();
        self.initializeGrid();
        self.registerValidation();
        self.registerButtonSave();
        self.registerButtonDelete();
    }

    registerValidation() {
        var self = this;
        crudHelper.initializeValidation($('form', self.element));
    }

    registerButtonSave() {
        var self = this;
        crudHelper.buttonSaveClicked($('#btn-save', self.element));
    }
    registerButtonDelete() {
        var self = this;
        crudHelper.buttonDeleteClicked($('#btn-delete', self.element));
    }
    initialize() {
        // Activate menu
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.merchant').addClass('active');
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
                    search: "Cari merchant"
                }
            });
    }
}

export default AgentMerchantEdit;