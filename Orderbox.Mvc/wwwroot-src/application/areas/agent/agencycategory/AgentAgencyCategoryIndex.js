import $ from 'jquery';
import CommonDataTable from '../../../component/CommonDataTable';
import UiHelper from '../../../component/UiHelper';

var commonDataTable = new CommonDataTable();
var uiHelper = new UiHelper();

class AgentAgencyCategoryIndex {
    constructor(element) {
        this.element = element;
        this.gridElement = $('table[grid]', self.element);
    }

    register() {
        var self = this;

        self.initialize();
        self.initializeGrid();
    }


    initializeGrid() {
        var grid = $('table[grid]', self.element);
        commonDataTable.init(grid,
            {
                filters: grid.data('filters')
            },
            {
                labels: {
                    search: "Cari kategori"
                }
            });
    }

    initialize() {
        // Activate menu
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.agencycategory').addClass('active');
    }
}

export default AgentAgencyCategoryIndex;