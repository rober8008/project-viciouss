if (google)
    google.load('visualization', '1', { packages: ['corechart', 'line'] });
var spinnerAddWatchlist;
var spinnerDeleteWatchlist;

$(document).ready(function () { 
    var activeTab = $('a.tab-portfolio-selector.active');
    if (activeTab && activeTab.length)
        activeTab = activeTab.attr('href').replace('#', '');
    initializePage(activeTab);
});

function initializePage(activeWatchlistId) {
    $('.tab-portfolio-selector').unbind('click').click(function () {
        var watchlistId = $(this).attr('href').replace('#', '');
        drawCharts(watchlistId);        
    });
    if (activeWatchlistId && activeWatchlistId.length)
        drawCharts(activeWatchlistId);
    
    $('[data-toggle="popover-add"]').popover({
        html: true,
        title: function () {
            return $("#popover-add-head").html();
        },
        content: function () {
            setTimeout(function () {
                $('#submit_add_porfolio').unbind('click').click(function () {
                    spinnerAddWatchlist = new Spinner().spin(document.getElementById('dashboard_add_portfolio'));
                });
            }, 1000);
            return $("#popover-add-content").html();            
        },
    });

    $('[data-toggle="popover-delete"]').popover({
        html: true,
        title: function () {
            return $("#popover-delete-head").html();
        },
        content: function () {
            setTimeout(function () {
                $('#submit_delete_porfolio').unbind('click').click(function () {
                    spinnerDeleteWatchlist = new Spinner().spin(document.getElementById('dashboard_delete_portfolio'));
                });
            }, 1000);
            return $("#popover-delete-content").html();
        },
    });

    $('a.popover-add-symbol-opener').each(function () {
        $(this).popover({
            html: true,
            title: function () {
                var popoverName = $(this).attr('data-toggle');
                return $('#' + popoverName + '-head').html();
            },
            content: function () {
                var watchlistId = $(this).attr('id').replace('popoverOpener_', '');
                setTimeout(function () {
                    setupWatchListAddStockSubmitButton(watchlistId);
                    setupWatchListAddStockAutocomplete(watchlistId);
                }, 1000);
                var popoverName = $(this).attr('data-toggle');
                return $('#' + popoverName + '-content').html();
            }
        });        
    }); 

    $('a.scrollTo[href*="#"]').on('click', function (el) {        
        var scrolltoItemId = $(this).attr('scrollTo');
        var scrolltoItem = $('#container_' + scrolltoItemId);
        var scrolltoItemOffset = $(scrolltoItem).offset();
        console.log(scrolltoItemId);
        console.log(scrolltoItem);
        console.log(scrolltoItemOffset);

        $('html, body').animate({
            scrollTop: scrolltoItemOffset.top
        }, 500, 'linear');

        el.preventDefault();
    });
}

function OnSuccessAddPortfolio(data, status, xhr) {
    window.location.reload(true);
    //$('#watchlists_content').html(data);
    //if (spinnerAddWatchlist) {
    //    spinnerAddWatchlist.spin(false);
    //}    
    //initializePage($('a.tab-portfolio-selector')[0].attributes['href'].nodeValue.replace('#', ''));    
}

function OnSuccessAddSymbolToPortfolio(data, status, xhr) {
    window.location.reload(true);
    //$('#watchlists_content').html(data);    
    //initializePage($('a.tab-portfolio-selector')[0].attributes['href'].nodeValue.replace('#', ''));    
}

function OnSuccessDeletePortfolio(data, status, xhr) {
    window.location.reload(true);
    //$('#watchlists_content').html(data);
    //if (spinnerDeleteWatchlist) {
    //    spinnerDeleteWatchlist.spin(false);
    //} 
    //initializePage($('a.tab-portfolio-selector')[0].attributes['href'].nodeValue.replace('#', ''));    
}

var spinners;
function drawCharts(active_portfolio) {
    spinners = {};
    $('.symbol_id_container').each(function () {
        var actionUrl = '/Dashboard/GetSimplifiedSymbolData';
        var url_data = { 'symbol_id': $(this).attr('symbolid'), 'portfolio_id': $(this).attr('portfolioid'), 'containerid': $(this).attr('id') };
        if (active_portfolio == $(this).attr('portfolioid')) {
            $.getJSON(actionUrl, url_data, drawChartData);

            var target1 = document.getElementById('chart_' + $(this).attr('id'));
            var target2 = document.getElementById('chart_intradiary_' + $(this).attr('id'));

            var spinner1 = new Spinner().spin(target1);
            var spinner2 = new Spinner().spin(target2);

            spinners['chart_' + $(this).attr('id')] = spinner1;
            spinners['chart_intradiary_' + $(this).attr('id')] = spinner2;
        }
    });
}

function drawChartData(response) {
    var data = new google.visualization.DataTable(response.quotesdata);
    var chart_id = 'chart_' + response.containerid;

    var intradiary_data = new google.visualization.DataTable(response.quotesintradiary);
    var chart_intradiary_id = 'chart_intradiary_' + response.containerid;

    var options = {
        legend: 'none',
        crosshair: { trigger: 'both'/*, color: '#000'*/ },
        backgroundColor: '#0d261f',
        colors: ['#147063'],
        'height': 149
    };

    var chart = new google.visualization.LineChart(document.getElementById(chart_id));
    chart.draw(data, options);
    if (spinners[chart_id]) {
        spinners[chart_id].spin(false);
    }

    var intradiary_chart = new google.visualization.LineChart(document.getElementById(chart_intradiary_id));
    intradiary_chart.draw(intradiary_data, options);
    if (spinners[chart_intradiary_id]) {
        spinners[chart_intradiary_id].spin(false);
    }
}

$('#close_error_modal').unbind('click').click(function () {
    $('#errorModal').removeClass('in');
    $('#errorModal').css("display", "none");
});

$('#open_report_error_modal').unbind('click').click(function () {
    $('#reportErrorModal').addClass('in');
    $('#reportErrorModal').css("display", "block");

    $('#close_report_error_modal').unbind('click').click(function () {
        $('#reportErrorModal').removeClass('in');
        $('#reportErrorModal').css("display", "none");
        return false;
    });

    return false;
});

var spinnerAutocomplete;
var target1;
var symbol_selected = false;
function setupWatchListAddStockSubmitButton(watchlistId) {
    $('#portfolioAddStockSubmitBtn_' + watchlistId).unbind('click').click(function () {
        target1 = document.getElementById('spinner_container_' + watchlistId);
        spinnerAutocomplete = new Spinner({ length: 3, radius: 4, left: '57%', top: '48%' }).spin(target1);
    });
    $('#portfolioAddStockSubmitBtn_' + watchlistId).prop('disabled', true);
    symbol_selected = false;
}

function setupWatchListAddStockAutocomplete(watchlistId) {
    //Symbol Autocomplete
    $('#symbol_text_' + watchlistId).autocomplete({
        source: function (request, response) {
            symbol_selected = false;
            $('#portfolioAddStockSubmitBtn_' + watchlistId).prop('disabled', true);
            target1 = document.getElementById('spinner_container_' + watchlistId);
            spinnerAutocomplete = new Spinner({ length: 3, radius: 4, left: '45.5%', top: '48%' }).spin(target1);
            $.ajax({
                url: "/Dashboard/GetSymbolByLabel",
                type: 'POST',
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Symbol_Name + ' (' + item.Market_Name + ')',
                            symbol: item.Symbol_Id
                        };
                    }));
                    spinnerAutocomplete.spin(false);
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            $('#symbol_id_' + watchlistId).val(ui.item.symbol);
            $('#portfolioAddStockSubmitBtn_' + watchlistId).prop('disabled', false);
        }
    });
}