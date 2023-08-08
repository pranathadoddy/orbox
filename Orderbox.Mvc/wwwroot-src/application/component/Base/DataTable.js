import '../../../lib/bootgrid/jquery.bootgrid.css';
import '../../../lib/bootgrid/jquery.bootgrid-custom.css';

import $ from 'jquery';
import '../../../lib/bootgrid/jquery.bootgrid';
import '../../../lib/bootgrid/jquery.bootgrid.fa';

class DataTable {
    constructor(element, options) {
        this.element = element;
        this.url = options.url;
        this.columns = options.columns || {};
        this.onDrawCallback = options.onDrawCallback;
        this.onAddItemClicked = options.onAddItemClicked;
        this.addItemButtonVisible =
            options.addItemButtonVisible === undefined ?
                true : options.addItemButtonVisible;
        this.addItemButtonTitle = options.addItemButtonTitle || '';
        this.onReloadMaintenancePane = options.onReloadMaintenancePane;
        this.data = options.data || {};
        this.onInitComplete = options.onInitComplete;
        this.rowCount = options.rowCount || [10, 25, 50];
        this.labels = options.labels || {};
        this.templates = options.templates || {};
        this.requestHandler = options.requestHandler || function (r) { return r; };
    }

    register()
    {
        var self = this;

        var dataTableToolbar =
            '<tfoot>' +
            '<tr>' +
            '<th colspan="5">' +
            '<button type="button" class="btn btn-xs btn-default command-add" title="' + self.addItemButtonTitle + '">' +
            '<span class="fa fa-plus"></span>' +
            '</button>' +
            '</th>' +
            '</tr>' +
            '</tfoot>';

        $(self.element)
            .on("initialized.rs.jquery.bootgrid",
                function () {
                    if (self.addItemButtonVisible) {
                        $(self.element).append(dataTableToolbar);
                        $(".command-add", self.element).on("click", self.onAddItemClicked);
                    }

                    if (typeof self.onInitComplete === "function") {
                        self.onInitComplete();
                    }
                })
            .on("loaded.rs.jquery.bootgrid",
                function () {
                    self.onDrawCallback();
                })
            .bootgrid({
                templates: self.templates,
                ajax: true,
                post: self.data,
                requestHandler: self.requestHandler,
                labels: self.labels,
                url: self.url,
                formatters: self.columns,
                rowCount: self.rowCount
            });
    }

    reload()
    {
        var self = this;
        $(self.element).bootgrid('refresh');
    }

    reloadMaintenancePane(data, callback)
    {
        this.onReloadMaintenancePane(data, callback);
    }

    getRowCount()
    {
        var self = this;
        return $(self.element).bootgrid("getTotalRowCount");
    }
}

export default DataTable;