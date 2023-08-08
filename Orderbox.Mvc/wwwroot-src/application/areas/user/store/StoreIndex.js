import $ from 'jquery';
import CommonDataTable from '../../../component/CommonDataTable';

var commonDataTable = new CommonDataTable();

class StoreIndex {
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
        $('.navbar-nav li.store').addClass('active');
    }

    initializeGrid() {
        var grid = $('table[grid]', self.element);
        commonDataTable.init(grid, null, {
            labels: {
                search: "Cari store"
            }
        });
    }
}

export default StoreIndex;