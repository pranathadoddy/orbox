import CommonDataTable from '../../../component/CommonDataTable';

var commonDataTable = new CommonDataTable();

class CategoryIndex {
    constructor(element) {
        this.element = element;
    }

    register() {
        var self = this;
        self.initialize();

        var grid = $('table[grid]', self.element);
        commonDataTable.init(grid, null, {
            labels: {
                search: "Cari kategori"
            }
        });
    }

    initialize() {
        // Activate menu
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.category').addClass('active');
    }
}

export default CategoryIndex;