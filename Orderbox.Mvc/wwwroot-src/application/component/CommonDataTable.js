import $ from 'jquery';
import DataTable from './Base/DataTable';
import UiHelper from './UiHelper';

var uiHelper = new UiHelper();

class CommonDataTable
{
    init(gridElement, parameter, additionalOptions)
    {
        var self = this;

        var options = {
            url: gridElement.data('url'),
            data: parameter || {},
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
            onDrawCallback: function () {
                $('.primaryLink', gridElement).on("click",
                    function (e) {
                        e.preventDefault();
                        uiHelper.loadingOverlay('show', { image: '', custom: $(uiHelper.spinner.getSpinnerHtml()) });

                        var url = `${gridElement.data('edit-url')}/${$(this).data('id')}`;

                        uiHelper.loadingOverlay('hide');
                        window.location = url;
                    });

                $('.popup', gridElement).on('click',
                    function (e) {
                        e.preventDefault();
                        var url = $(this).attr('href');
                        var param =
                            'scrollbars=no,resizable=no,status=no,location=no,toolbar=no,menubar=no, width=960,height=600';
                        window.open(url, '_blank', param);
                    });
            },
            addItemButtonVisible: false
        };

        Object.assign(options, additionalOptions);

        var dataTable = new DataTable(gridElement, options);

        dataTable.register();

        return dataTable;
    }
}

export default CommonDataTable;