import $ from 'jquery';
import CommonDataTable from '../../../component/CommonDataTable';
import CrudHelper from '../../../component/CrudHelper';

var commonDataTable = new CommonDataTable();
const crudHelper = new CrudHelper();

class StoreEdit {
    constructor(element) {
        this.element = element;
        this.gridElement = $('table[grid]', self.element);
    }

    register() {
        var self = this;

        self.initialize();
        self.registerValidation();
        self.registerButtonSave();
        crudHelper.buttonDeleteClicked($('#btn-delete', self.element));
    }

    initializeGrid() {
        var grid = $('table[grid]', self.element);
        commonDataTable.init(grid, null, {
            labels: {
                search: "Cari store"
            }
        });
    }

    registerValidation() {
        var self = this;
        crudHelper.initializeValidation($('form', self.element));
    }

    registerButtonSave() {
        var self = this;
        crudHelper.buttonSaveClicked($('#btn-save', self.element));
    }

    initialize() {
        // Activate menu
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.store').addClass('active');
    }
}

export default StoreEdit;