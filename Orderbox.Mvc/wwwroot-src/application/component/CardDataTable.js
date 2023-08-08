import './CardDataTable.css';
import DataTable from './Base/DataTable';

class CardDataTable {
    constructor() {
        this.dataTable = null;
    }

    init(gridElement, options) {
        this.dataTable = new DataTable(
            gridElement, {
                url: gridElement.data('url'),
                data: options.parameter || {},
                columns: {
                    Card: options.card
                },
                onDrawCallback: options.onDrawCallback,
                addItemButtonVisible: false,
                rowCount: [4]
            });

        this.dataTable.register();
    }

    reload() {
        this.dataTable.reload();
    }
}

export default CardDataTable;