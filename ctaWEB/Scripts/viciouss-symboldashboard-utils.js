function OnSuccessAddIndicator(data, status, xhr) {
    $('#symbol_dashboard_content').html(data);
    getChartInfo(null, currentMainChartType, currentChartRange, currentCandelRange);
}
function OnSuccessAddShape(data, status, xhr) {
    $('#symbol_dashboard_content').html(data);
    getChartInfo(null, currentMainChartType, currentChartRange, currentCandelRange);
}

function OnSuccessDeleteShape() {
    getChartInfo(null, currentMainChartType, currentChartRange, currentCandelRange);
}

function OnSuccessDeleteIndicator() {
    getChartInfo(null, currentMainChartType, currentChartRange, currentCandelRange);
}

$(document).ready(function () {
    initializePage();
});

function initializePage() {
    $('#btn_candel_chart').unbind('click').click(function () {
        $('#btn_candel_chart').removeClass('btn-default');
        $('#btn_candel_chart').addClass('btn-success');
        $('#btn_line_chart').removeClass('btn-success');
        $('#btn_line_chart').addClass('btn-default');

        currentMainChartType = 'candel';
        getChartInfo(null, currentMainChartType, currentChartRange, currentCandelRange);
    });
    $('#btn_line_chart').unbind('click').click(function () {
        $('#btn_line_chart').removeClass('btn-default');
        $('#btn_line_chart').addClass('btn-success');
        $('#btn_candel_chart').removeClass('btn-success');
        $('#btn_candel_chart').addClass('btn-default');

        currentMainChartType = 'line';
        getChartInfo(null, currentMainChartType);
    });
    $('#down_arrow').unbind('click').click(function () {
        if (dataCurrentMaxVal - 10 >= dataMaxVal) {
            dataCurrentMaxVal -= 10;
            comboChart.setOption('vAxis.viewWindow.max', dataCurrentMaxVal, currentChartRange, currentCandelRange);
            comboChart.draw();
        }
    });
    $('#up_arrow').unbind('click').click(function () {
        dataCurrentMaxVal += 10;
        comboChart.setOption('vAxis.viewWindow.max', dataCurrentMaxVal);
        comboChart.draw();
    });
    $('#right_arrow').unbind('click').click(function () {
        datarangeCurrentMaxVal += hAxisUnitsByMonth();        

        for (var i = 0; i < all_charts.length; i++) {
            all_charts[i].setOption('hAxis.viewWindow.max', datarangeCurrentMaxVal);
            all_charts[i].draw();
        }

        var auxmin = dateSlider.getState().range.start;
        dateSlider.setState({ range: { start: auxmin, end: datarangeCurrentMaxVal } });
        dateSlider.draw();
    });
    $('#left_arrow').unbind('click').click(function () {
        if (datarangeMaxVal <= datarangeCurrentMaxVal - hAxisUnitsByMonth()) {
            datarangeCurrentMaxVal -= hAxisUnitsByMonth();

            for (var i = 0; i < all_charts.length; i++) {
                all_charts[i].setOption('hAxis.viewWindow.max', datarangeCurrentMaxVal);
                all_charts[i].draw();
            }

            var auxmin = dateSlider.getState().range.start;
            dateSlider.setState({ range: { start: auxmin, end: datarangeCurrentMaxVal } });
            dateSlider.draw();
        }        
    });
    $('#double_left_arrow').unbind('click').click(function () {
        var current_range_size = dateSlider.getState().range.end - dateSlider.getState().range.start;

        var current_start_range = dateSlider.getState().range.start;
        current_start_range -= (hAxisUnitsByMonth() * 12);
        current_start_range = (current_start_range > datarangeMinVal) ? current_start_range : datarangeMinVal;

        var current_end_range = current_start_range + current_range_size;

        dateSlider.setState({ range: { start: current_start_range, end: current_end_range } });
        dateSlider.draw();
    });
    $('#single_left_arrow').unbind('click').click(function () {
        var current_range_size = dateSlider.getState().range.end - dateSlider.getState().range.start;

        var current_start_range = dateSlider.getState().range.start;
        current_start_range -= hAxisUnitsByMonth();
        current_start_range = (current_start_range > datarangeMinVal) ? current_start_range : datarangeMinVal;

        var current_end_range = current_start_range + current_range_size;

        dateSlider.setState({ range: { start: current_start_range, end: current_end_range } });
        dateSlider.draw();
    });
    $('#single_right_arrow').unbind('click').click(function () {
        var current_range_size = dateSlider.getState().range.end - dateSlider.getState().range.start;

        var current_end_range = dateSlider.getState().range.end;
        current_end_range += hAxisUnitsByMonth();
        current_end_range = (current_end_range < datarangeMaxVal) ? current_end_range : datarangeMaxVal;

        var current_start_range = current_end_range - current_range_size;

        dateSlider.setState({ range: { start: current_start_range, end: current_end_range } });
        dateSlider.draw();
    });
    $('#double_right_arrow').unbind('click').click(function () {
        var current_range_size = dateSlider.getState().range.end - dateSlider.getState().range.start;

        var current_end_range = dateSlider.getState().range.end;
        current_end_range += (hAxisUnitsByMonth() * 12);
        current_end_range = (current_end_range < datarangeMaxVal) ? current_end_range : datarangeMaxVal;

        var current_start_range = current_end_range - current_range_size;

        dateSlider.setState({ range: { start: current_start_range, end: current_end_range } });
        dateSlider.draw();
    });
    $('.chartRangeSelector').unbind('click').click(function () {
        $('.chartRangeSelector').removeClass('btn-success');
        $('.chartRangeSelector').addClass('btn-default');
        $(this).addClass('btn-success');
        $('#btn_line_chart').removeClass('btn-success');
        $('#btn_candel_chart').removeClass('btn-success');
        currentChartRange = $(this).attr('range');
    });
    $('.candelRangeSelector').unbind('click').click(function () {
        $('.candelRangeSelector').removeClass('btn-success');
        $('.candelRangeSelector').addClass('btn-default');
        $(this).addClass('btn-success');
        $('#btn_line_chart').removeClass('btn-success');
        $('#btn_candel_chart').removeClass('btn-success');
        currentCandelRange = $(this).attr('range');
    });

    $('#dropdownIndicators').unbind('click').click(function () {
        $('.delete-indicator').unbind('click').click(function () {
            var indicator_id = $(this).attr('indicator-id');

            var actionUrl = '/Dashboard/DeleteIndicator';
            var url_data = { indicator_id: indicator_id, username: getUrlParameter('username') };
            $.getJSON(actionUrl, url_data, OnSuccessDeleteIndicator);

            $('#indicator_listitem_' + indicator_id).hide();
        })
    });

    $('#dropdownShapes').unbind('click').click(function () {
        $('.delete-shape').unbind('click').click(function () {
            var shape_id = $(this).attr('shape-id');

            var actionUrl = '/Dashboard/DeleteShape';
            var url_data = { shape_id: shape_id, username: getUrlParameter('username') };
            $.getJSON(actionUrl, url_data, OnSuccessDeleteShape);

            $('#shape_listitem_' + shape_id).hide();
        })
    });

    $('[data-toggle="popover-new-indicator"]').popover({
        html: true,
        title: function () {
            return $("#popover-new-indicator-head").html();
        },
        content: function () {
            setTimeout(function () {
                $('#indicator_color1').unbind('click').click(function () {
                    $('#indicator_color1_picker').click();
                });
                $('#cp1').colorpicker();
                $('#indicator_color2').unbind('click').click(function () {
                    $('#indicator_color2_picker').click();
                });
                $('#cp2').colorpicker();
                $('#indicator_color3').unbind('click').click(function () {
                    $('#indicator_color3_picker').click();
                });
                $('#cp3').colorpicker();
            }, 1000);
            return $("#popover-new-indicator-content").html();
        },
    });

    $('[data-toggle="popover-new-shape"]').popover({
        html: true,
        title: function () {
            return $("#popover-new-shape-head").html();
        },
        content: function () {
            setTimeout(function () {
                $('#rect_color').unbind('click').click(function () {
                    $('#rect_color_picker').click();
                });
                $('#cp4').colorpicker();
            }, 1000);
            return $("#popover-new-shape-content").html();
        },
    });

    $('#close_error_modal').unbind('click').click(function () {
        $('#errorModal').removeClass('in');
        $('#errorModal').css("display", "none");
        return false;
    });
}

function hAxisUnitsByMonth() {//base on CandelRange
    var candelRange = $('.candelRangeSelector.btn-success').attr('range');
    if(candelRange == "1")
        return 22;
    if(candelRange == "2")
        return 4.4;
    if (candelRange == "3")
        return 1;
}

function getDateByDataIndex(index) {
    if (index < data.getNumberOfRows()) {
        return data.getValue(index, data.getNumberOfColumns() - 1);
    }
    var dataMinDateVal = new Date(data.getColumnRange(data.getNumberOfColumns() - 1).min.getTime());
    var months = Math.round(index / hAxisUnitsByMonth());
    return new Date(dataMinDateVal.setMonth(dataMinDateVal.getMonth() + months));
}

function IndicatorSelectorChange(elmt) {
    $('#closeX1').show();
    var value_selected = $(elmt).attr('value');
    $('#indicator_selector_text').text($(elmt).text());
    var indicator_id = $(elmt).attr('indicator_id');
    var param1_label = $(elmt).attr('param1_label');
    var color1_label = $(elmt).attr('color1_label');
    var param2_label = $(elmt).attr('param2_label');
    var color2_label = $(elmt).attr('color2_label');
    var param3_label = $(elmt).attr('param3_label');
    var color3_label = $(elmt).attr('color3_label');
    updateFormContent(indicator_id, param1_label, color1_label, param2_label, color2_label, param3_label, color3_label);
}

function ShapeSelectorChange(elmt) {
    $('#add_shape_form').show();
    $('#closeX2').show();
    shape_date1 = shape_date2 = shape_value1 = shape_value2 = null;
    var value_selected = $(elmt).attr('value');
    if (value_selected != '0') {
        current_shape_drawing = $(elmt).attr('value_key');
        $('#shape_selector_text').text($(elmt).text());
        $('#shape_id').val(value_selected);
    }
    else {
        current_shape_drawing = '';
        $('#shape_id').val(null);
    }

    $('#shape_date1').val(null);
    $('#shape_date2').val(null);
    $('#shape_value1').val(null);
    $('#shape_value2').val(null);
    pointF = null;
    pointS = null;
}

var currentMainChartType;
var currentChartRange;
var currentCandelRange;
var dashboard;
var data;
var dateSlider;
var comboChart;
var volumeChart;
var lineChart;
var datarangeMinVal;
var datarangeMaxVal;
var datarangeCurrentMaxVal;
var dataMinVal;
var dataMaxVal;
var dataCurrentMaxVal;
var pointF;
var pointS;
var newRectStartID = 1;
var rect_name;
var rect_color;
var all_charts;
var spinner1;
var server_response;

// Load the Visualization API and the controls package.
// Packages for all the other charts you need will be loaded
// automatically by the system.
google.load('visualization', '1.0', { packages: ['controls', 'corechart'] });
google.setOnLoadCallback(getChartInfo);


function getChartInfo(evt, main_chart_type, chartRange, candelRange) {
    $('.chart').empty();
    if (!main_chart_type) {
        main_chart_type = 'candel';
        $('#btn_candel_chart').addClass('btn-success');
    }
    if (!chartRange) {
        chartRange = '2';
        $('#btn_threemonths').addClass('btn-success');
    }
    if (!candelRange) {
        candelRange = '1';
        $('#btn_daily').addClass('btn-success');
    }

    var target1 = document.getElementById('dashboard');
    if (!spinner1) spinner1 = new Spinner();
    spinner1.spin(target1);

    var actionUrl = '/Dashboard/GetSymbolContent';
    var url_data = { portfolio_id: getUrlParameter('portfolio_id'), symbol_id: getUrlParameter('symbol_id'), main_chart_type: main_chart_type, chartRange: chartRange, candelRange: candelRange };
    $.getJSON(actionUrl, url_data, displayData);

    initializePage();
}

function ProcessSeriesFromServer(data) {
    var result = {};

    for (var i = 0; i < data.length; i++) {
        result[data[i].Key] = data[i].Value;
    }

    return result;
}

function displayData(response) {
    server_response = response;
    var all_charts_data = response.data;
    var main_chart_series_from_server = response.series_main_chart;
    var main_chart_view = response.view_main_chart;
    var chart_indicators = response.indicators;
    var main_chart_series = ProcessSeriesFromServer(main_chart_series_from_server);
    var date_range_selector_view = response.date_range_selector_view;
    //var max_data_date = response.max_data_date;
    var volume_chart_view = response.volume_chart_view;

    all_charts = [];
    dashboard = new google.visualization.Dashboard(document.getElementById('dashboard'));

    data = new google.visualization.DataTable(all_charts_data);

    //var ticks = [];
    //for (var i = 0; i < data.getNumberOfRows(); i++) {
    //    ticks.push({ v: data.getValue(i, 0), f: data.getFormattedValue(i, data.getNumberOfColumns() - 1) });
    //}
    
    var dataRange = data.getColumnRange(0);
    datarangeMinVal = dataRange.min;
    datarangeMaxVal = dataRange.max;//new Date(dataRange.max.getTime());
    datarangeCurrentMaxVal = dataRange.max;
    var dataValueRange;
    if (data.getColumnLabel(3) == "Cierre")
        dataValueRange = data.getColumnRange(3);
    else
        dataValueRange = data.getColumnRange(5);
    dataMinVal = dataValueRange.min;
    dataMaxVal = dataValueRange.max;
    dataCurrentMaxVal = dataValueRange.max;    

    dateSlider = new google.visualization.ControlWrapper({
        controlType: 'ChartRangeFilter',
        containerId: 'controls',
        options: {
            filterColumnIndex: 0,
            ui: {
                chartOptions: {
                    height: 50,
                    chartArea: {
                        width: '90%'
                    },
                    chartType: 'ComboChart',
                    vAxes: {
                        0: { logScale: false },
                        1: { logScale: false }
                    },
                    series: {
                        0: { type: 'line', color: '#7a0088', targetAxisIndex: 0 },
                        1: { type: 'bars', color: '#FF00ff', targetAxisIndex: 1 }
                    },
                    hAxis: {
                        textPosition: 'none',
                        gridlines: { 'color': 'none' }
                    },
                    vAxis: {
                        textPosition: 'none',
                        gridlines: { 'color': 'none' }
                    },
                    backgroundColor: '#0d261f'
                },
                chartView: {
                    columns: date_range_selector_view
                }
            }
        },
        state: { range: { start: datarangeCurrentMaxVal - (hAxisUnitsByMonth() * 6), end: datarangeCurrentMaxVal } }
    });

    // Create a combo chart, passing some options
    comboChart = new google.visualization.ChartWrapper({
        chartType: 'ComboChart',
        containerId: 'combo_chart',
        options: {
            series: main_chart_series,
            height: 500,
            chartArea: {
                top: '10',
                width: '90%',
                height: '85%'                
            },
            vAxis: {
                gridlines: { count: 5 },
                viewWindow: {
                    max: dataMaxVal + dataMaxVal / 10,
                    min: dataMinVal - dataMinVal / 10
                }                
            },
            hAxis: {
                textPosition: 'none',
                //gridlines: { 'color': 'none' },
                //ticks: ticks
            },
            candlestick: {
                fallingColor: { strokeWidth: 1, fill: '#a52714', stroke: '#a52714' },
                risingColor: { strokeWidth: 1, fill: '#0f9d58', stroke: '#0f9d58' }
            },
            bar: { groupWidth: '95%' },
            //crosshair: { trigger: 'both' },
            backgroundColor: '#0d261f',
            tooltip: { isHtml: true },
            focusTarget: 'category'
        },
        view: { columns: main_chart_view }
    });

    all_charts.push(comboChart);

    for (var key in main_chart_series) {
        newRectStartID = parseInt(key);
    }
    newRectStartID++;


    // Create a volume chart, passing some options
    volumeChart = new google.visualization.ChartWrapper({
        chartType: 'ComboChart',
        containerId: 'volume_chart',
        options: {
            series: { 0: { type: 'bars', color: '#0000ff', visibleInLegend: false } },
            height: 100,
            chartArea: {
                width: '90%'
            },
            vAxis: {
                gridlines: { count: 0 }
            },
            bar: { groupWidth: "100%" },
            backgroundColor: '#0d261f'
        },
        view: { columns: volume_chart_view }
    });
    //all_charts.push(volumeChart);

    var new_chart, containerid, dataserie, dataview;
    for (var j = 0; j < chart_indicators.length; j++) {
        containerid = chart_indicators[j].id;
        dataserie = ProcessSeriesFromServer(chart_indicators[j].series);
        dataview = chart_indicators[j].view;
        // Create a line chart, passing some options
        new_chart = new google.visualization.ChartWrapper({
            chartType: 'ComboChart',
            containerId: containerid,
            options: {
                series: dataserie,
                height: 100,
                chartArea: {
                    width: '90%'
                },
                backgroundColor: '#0d261f',
                tooltip: { isHtml: true },
                focusTarget: 'category'
            },
            view: { columns: dataview }
        });
        all_charts.push(new_chart);
    }

    // Every time the table fires the "select" event, it should call your
    // selectHandler() function.
    google.visualization.events.addOneTimeListener(comboChart, 'ready', function () {
        google.visualization.events.addListener(comboChart.getChart(), 'click', clickComboHandler);
        google.visualization.events.addListener(comboChart.getChart(), 'select', selectComboHandler);
    });

    // Many-to-one binding where 'ageSelector' and 'salarySelector' concurrently
    // participate in selecting which data 'ageVsSalaryScatterPlot' visualizes.
    //dashboard.bind([agePicker, salaryPicker], ageVsSalaryScatterPlot);

    // bind() chaining where each control drives its own chart.
    //dashboard.bind(agePicker, ageBarChart).bind(salaryRangePicker, salaryPieChart);

    //bind controls to other controls to establish chains of dependencies
    //dashboard.bind(countryPicker, regionPicker).bind(regionPicker, cityPicker);

    // One-to-many binding where 'dateSlider' drives two charts.
    dashboard.bind(dateSlider, all_charts);

    dashboard.draw(data);   
    spinner1.spin(false);
}

function selectComboHandler() {
    /*var selection = comboChart.getChart().getSelection();
    var message = '';
    for (var i = 0; i < selection.length; i++) {
        var item = selection[i];
        if (item.row != null && item.column != null) {
            var str = data.getFormattedValue(item.row, item.column);
            message += '{row:' + item.row + ',column:' + item.column + '} = ' + str + '\n';
        } else if (item.row != null) {
            var str = data.getFormattedValue(item.row, 0);
            message += '{row:' + item.row + ', column:none}; value (col 0) = ' + str + '\n';
        } else if (item.column != null) {
            var str = data.getFormattedValue(0, item.column);
            message += '{row:none, column:' + item.column + '}; value (row 0) = ' + str + '\n';
        }
    }
    if (message == '') {
        message = 'nothing';
    }
    alert('selected: ' + message);*/
}

var date;
function clickComboHandler(e) {
    if (current_shape_drawing != '' && !pointF) {
        pointF = {};
        pointF['X'] = Math.round(comboChart.getChart().getChartLayoutInterface().getHAxisValue(e.x));
        pointF['Y'] = comboChart.getChart().getChartLayoutInterface().getVAxisValue(e.y);
        
        date = getDateByDataIndex(pointF['X']);
        $('#shape_date1').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear() + ' 12:00:00 AM');
        $('#shape_value1').val(comboChart.getChart().getChartLayoutInterface().getVAxisValue(e.y));
    }
    else if (current_shape_drawing != '' && pointF) {
        pointS = {};
        pointS['X'] = Math.round(comboChart.getChart().getChartLayoutInterface().getHAxisValue(e.x));
        pointS['Y'] = comboChart.getChart().getChartLayoutInterface().getVAxisValue(e.y);
        date = getDateByDataIndex(pointS['X']);
        $('#shape_date2').val((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear() + ' 12:00:00 AM');
        $('#shape_value2').val(comboChart.getChart().getChartLayoutInterface().getVAxisValue(e.y));
        rect_color = $('#rect_color').val();
        $('#shape_color').val(rect_color);
        rect_name = $('#rect_name').val();

        var startDate = pointF.X;
        var endDate = pointS.X;
        var startValue = pointF.Y;
        var endValue = pointS.Y;
        if (pointF.X > pointS.X) {  //Date of first point is greater than second point
            startDate = pointS.X;
            startValue = pointS.Y;
            endDate = pointF.X;
            endValue = pointF.Y;
        }

        $('#save_shape').click();
        if (drawing[current_shape_drawing])
            drawing[current_shape_drawing](data, comboChart, rect_name, rect_color, startDate, startValue, endDate, endValue);
        pointF = null;
        pointS = null;
    }
}

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}

function updateFormContent(indicator_id, param1_label, color1_label, param2_label, color2_label, param3_label, color3_label) {
    $('#add_indicator_form').show();
    $('#indicator_id').val(indicator_id);

    $('#param1_container').hide();
    $('#color1_container').hide();
    $('#param2_container').hide();
    $('#color2_container').hide();
    $('#param3_container').hide();
    $('#color3_container').hide();

    if (param1_label.indexOf(':') != -1) {
        $('#param1_container').show();
        $('#param1_label').text(param1_label.split(':')[0]);
        $('#param1').val(param1_label.split(':')[1]);
    }
    if (color1_label.indexOf(':') != -1) {
        $('#color1_container').show();
        $('#indicator_color1').val(color1_label.split(':')[1]);
    }
    if (param2_label.indexOf(':') != -1) {
        $('#param2_container').show();
        $('#param2_label').text(param2_label.split(':')[0]);
        $('#param2').val(param2_label.split(':')[1]);
    }
    if (color2_label.indexOf(':') != -1) {
        $('#color2_container').show();
        $('#indicator_color2').val(color2_label.split(':')[1]);
    }
    if (param3_label.indexOf(':') != -1) {
        $('#param3_container').show();
        $('#param3_label').text(param3_label.split(':')[0]);
        $('#param3').val(param3_label.split(':')[1]);
    }
    if (color3_label.indexOf(':') != -1) {
        $('#color3_container').show()
        $('#indicator_color3').val(color3_label.split(':')[1]);
    }
} 


var current_shape_drawing = '';
var drawing = {};
var shape_date1, shape_date2, shape_value1, shape_value2;

drawing['line'] = DrawLine;
drawing['text'] = DrawText;
drawing['fiblinelow'] = DrawFibonacciLinesLow;
drawing['fiblinehigh'] = DrawFibonacciLinesHigh;
drawing['fibarclow'] = DrawFibonacciArcsLow;
drawing['fibarchigh'] = DrawFibonacciArcsHigh;
drawing['poligone'] = DrawPoligone;

function DrawLine(data, comboChart, rect_name, rect_color, startDate, startValue, endDate, endValue) {
    //data total rows
    var rowsCount = data.getNumberOfRows();
    //data total columns
    var columnsCount = data.getNumberOfColumns();

    //add new data column to views
    var view_columns = comboChart.getView();
    view_columns.columns.push(columnsCount);
    comboChart.setView(view_columns);


    //setup new serie of data
    var series_data = comboChart.getOption('series');
    series_data[newRectStartID] = { type: "line", lineDashStyle: [4, 0], color: rect_color };
    comboChart.setOption('series', series_data);

    //trendlines: { 0: {} }
    /*var trendlines = comboChart.getOption('trendlines');
    trendlines[newRectStartID] = {
        opacity: 1,
        pointsVisible: false
    };
    comboChart.setOption('trendlines', trendlines);*/

    newRectStartID++;

    //add new colunm to data
    data.addColumn('number', rect_name);

    //sort data by date
    //data.sort(0);

    //Calculate terms of the rect formula
    //var m = CalculateSlop(pointF, pointS);
    //var b = CalculateIndependentTerm(m, pointF);

    //building RECT


    var dataRange = data.getColumnRange(0);
    var currentPointDate;
    var currentValue;

    var dataStartPointIndex;
    var dataEndPointIndex;

    /*for (i = 0 ; i < rowsCount; i ++) {
        currentPointDate = data.getValue(i, 0);
        if (currentPointDate.getTime() == startDate) {
            dataStartPointIndex = i;
            data.setValue(i, columnsCount, startValue);
        }
        if (currentPointDate.getTime() == endDate) {
            dataEndPointIndex = i;
            data.setValue(newRowIndex, columnsCount, endValue);
        }
        /*if (currentPointDate.getTime() >= startDate && currentPointDate.getTime() <= endDate) {
            currentValue = (m * currentPointDate.getTime()) + b;
            data.setValue(i, columnsCount, currentValue);
        }
    }*/

    //if (!dataStartPointIndex) {
    var newRowIndex = data.addRow();
    data.setValue(newRowIndex, 0, new Date(startDate));
    data.setValue(newRowIndex, columnsCount, startValue);
    //}
    //if (!dataEndPointIndex) {
    var newRowIndex = data.addRow();
    data.setValue(newRowIndex, 0, new Date(endDate));
    data.setValue(newRowIndex, columnsCount, endValue);
    //}

    dashboard.draw(data);
}

function DrawText() { }

function DrawFibonacciLinesLow() { }

function DrawFibonacciLinesHigh() { }

function DrawFibonacciArcsLow() { }

function DrawFibonacciArcsHigh() { }

function DrawPoligone() { }

function CalculateSlop(pointA, pointB) {
    return (pointB.Y - pointA.Y) / (pointB.X - pointA.X);
}

function CalculateIndependentTerm(slope, point) {
    return point.Y - (slope * point.X);
}

function getUrlParameter(param) {
    var pageUrl = window.location.search.substring(1);
    var urlParameters = pageUrl.split('&');
    for (var i = 0; i < urlParameters.length; i++) {
        var parameter = urlParameters[i].split('=');
        if (parameter[0] == param) {
            return parameter[1];
        }
    }
    return '';
}