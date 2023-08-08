import '../../../../lib/bootstrap-datepicker/bootstrap-datepicker.css';
import '../../../../lib/bootstrap-datepicker/bootstrap-datepicker-custom.css';
import '../../../../lib/bootstrap-datepicker/bootstrap-datepicker.js';
import * as c3 from 'c3';
import moment from 'moment';

import Spinner from '../../../component/Base/Spinner';

class ReportIndex {
    constructor(element) {
        this.element = element;
        this.selectedDate = moment(new Date());
        this.chartColorPalette = [
            '#81D2C7',
            '#B5BAD0',
            '#7389AE',
            '#416788',
            '#E0E0E2',
            '#1D84B5'
        ];
    }

    register () {
        var self = this;
        self.initialize();
        self._LoadSummary();
        self._LoadDoughnutChart();
        self._LoadBarChart();
        self._LoadItemDetailList();

        self._InitializeMonthPicker(
            $('[monthyearpicker] input', self.Element),
            function (datepicked) {
                self.selectedDate = moment(datepicked);
                self._LoadSummary();
                self._LoadDoughnutChart();
                self._LoadBarChart();
                self._LoadItemDetailList();
            }
        );
    }

    initialize() {
        $('.navbar-nav li').removeClass('active');
        $('.navbar-nav li.report').addClass('active');
    }

    _InitializeMonthPicker(element, onMonthChanged) {
        element.datepicker({
            autoclose: true,
            format: 'MM yyyy',
            startView: 'months',
            minViewMode: 'months',
            orientation: 'bottom'
        }).on('changeMonth', function (e) {
            onMonthChanged(e.date);
        }).on('show', function () {
            var datepickerDropDown = $('.datepicker');
            datepickerDropDown.width(element.parent().width());
        });
        element.datepicker('setDate', new Date());
    }

    _LoadSummary() {
        var self = this;
        var $summary = $('[summary]', self.Element);

        $summary.each(function (i, v) {
            $('[summary-container]', $(v)).html('');
            var spinner = new Spinner($('[spinnerpane]', $(v)));
            spinner.show();

            $.get($(v).data('url'), { date: self.selectedDate.format('YYYY-MM-DD') }, function (resp) {
                spinner.hide();

                var $html = '';

                if (resp.value.type === 'SUM_INT') {
                    $html = resp.value.result;
                }
                else if (resp.value.type === 'SUM_MON') {
                    $html = resp.value.result.toLocaleString('id-ID');
                }

                $('[summary-container]', $(v)).html($html);
            });
        });
    }

    _FetchItemListData(container, url, request, callback) {
        var self = this;
        $.post(url, request, function (resp) {
            
            if (resp.isSuccess) {
                if (typeof callback === "function") {
                    callback();
                }

                var $html = '';
                $.each(resp.value.result, function (index, value) {
                    $html += '<li class="list-separated-item">';
                    $html += '<div class="row align-items-center">';
                    $html += '<div class="col-auto p-0 p-md-3"><span class="avatar avatar-md d-block" style="background-image: url(' + value.primaryImageFileName + ')"></span></div>';
                    $html += '<div class="col"><span class="text-inherit">' + value.productName + '</span></div>';
                    $html += '<div class="col"><span class="text-inherit">' + value.unitPrice.toLocaleString('id-ID') + ' x ' + value.quantity + ' ' + value.unit + '</span></div>';
                    $html += '<div class="col"><span class="text-inherit">' + value.currency + ' '  + value.totalSales.toLocaleString('id-ID') + '</span></div>';
                    $html += '</div></li>';
                });

                $('[list-container]', container).html($html);
            }

        });
    }

    _LoadItemDetailList() {
        var self = this;
        var $listItem = $('[infinite-list]', self.Element);

        $listItem.each(function (i, v) {
            $('[list-container]', $(v)).html('');
            var spinner = new Spinner($('[spinnerpane]', $(v)));
            spinner.show();

            var request = {
                date: self.selectedDate.format('YYYY-MM-DD'),
                categoryId: 0,
                keyword: '',
                pageIndex: 0
            };
            self._FetchItemListData(v, $(v).data('url'), request, function () {
                spinner.hide();
            });
        });
    }

    _LoadDoughnutChart() {
        var self = this;

        var $chart = $('[doughnut-chart]', self.Element);

        $chart.each(function (i, v) {
            $('[chart-container]', $(v)).html('');
            var spinner = new Spinner($('[spinnerpane]', $(v)));
            spinner.show();

            $.get($(v).data('url'), { date: self.selectedDate.format('YYYY-MM-DD') }, function (resp) {
                spinner.hide();

                var $html = '<div chart-display></div>';
                $html += '<table chart-legend class="table card-table"></table>';

                $('[chart-container]', $(v)).html($html);

                var columns = [];
                var colors = {};
                var chartLegends = [];
                resp.value.forEach(function (rv, i) {
                    var column = [];

                    var display = rv.display;
                    if ($(v).data('value-type') === 'money') {
                        display = rv.value.toLocaleString('id-ID');
                    }

                    column.push(rv.key);
                    column.push(rv.value);
                    columns.push(column);
                    colors[rv.key] = self.chartColorPalette[i];
                    chartLegends.push(
                        `<tr><td class="w-50 pl-0"><span class="badge" style="background-color:${self.chartColorPalette[i]};">${rv.key}</span></td><td class="text-right pr-0">${display}</span></td></tr>`
                    );
                });

                c3.generate({
                    bindto: $('[chart-display]', $(v))[0],
                    data: {
                        columns,
                        type: 'donut',
                        colors
                    },
                    donut: {
                        title: ""
                    },
                    legend: {
                        hide: true
                    }
                });

                chartLegends.forEach(function (l) {
                    $('[chart-legend]', $(v)).append(l);
                });
            });
        });
    }

    _LoadBarChart() {
        var self = this;

        var $chart = $('[bar-chart]', self.Element);

        $chart.each(function (i, v) {
            $('[chart-container]', $(v)).html('');
            var spinner = new Spinner($('[spinnerpane]', $(v)));
            spinner.show();

            $.get($(v).data('url'), { date: self.selectedDate.format('YYYY-MM-DD') }, function (resp) {
                spinner.hide();
                var $html = '<div chart-display></div>';

                $('[chart-container]', $(v)).html($html);

                var columns = [];
                var xAxisLabel = [];
                xAxisLabel.push('x');
                var values = [];
                values.push($(v).data('label'));
                var displays = [];
                var colors = {}
                colors[$(v).data('label')] = self.chartColorPalette[0];
                $.each(resp.value, function (i, rv) {
                    xAxisLabel.push(rv.key);
                    values.push(rv.value);
                    displays.push(rv.display);
                });
                columns.push(xAxisLabel);
                columns.push(values);

                c3.generate({
                    bindto: $('[chart-display]', $(v))[0],
                    data: {
                        x: 'x',
                        columns,
                        type: 'bar',
                        colors
                    },
                    legend: {
                        show: false
                    },
                    tooltip: {
                        format: {
                            title: function (d) { return d + ' ' + self.selectedDate.format('MMM') + ' ' + self.selectedDate.format('YYYY'); },
                            value: function (value, ratio, id, index) { return displays[index]; }
                        }
                    }
                });
            });
        });
    }

}

export default ReportIndex;