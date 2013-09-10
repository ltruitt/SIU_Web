$(document).ready(function () {

    var timestamp = new Date();

    jQuery("#ExpTbl").jqGrid({

        url: '/SIU_DAO.asmx/GetMyYtdExpenses',
        mtype: 'POST',
        ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },
        datatype: 'json',
        postData: { EmpID: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() },
        serializeGridData: function (postData) { return JSON.stringify(postData); },
        autowidth: false,
        autoheight: true,

        pgbuttons: false,
        pginput: false,
        rowNum: 10000,
        sortname: 'WorkDate',
        colNames: ['Month', 'Date', 'Job', 'O/H', 'Miles', 'Meals', 'Amt'],
        colModel: [
            { name: 'WorkMonth', index: 'WorkMonth', width: 000, sorttype: "date", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'F'} },
            { name: 'WorkDate', index: 'WorkDate', width: 140, sorttype: "date", align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm-d'} },
            { name: 'JobNo', index: 'JobNo ', width: 100, sorttype: "float", align: "center" },
            { name: 'OH_AccountNo', index: 'OH_AccountNo', width: 100, sorttype: "float", align: "center" },
            { name: 'Miles', index: 'Miles', width: 100, sortable: false, align: "center", formatter: 'number' },
            { name: 'Meals', index: 'Meals', width: 100, sortable: false, align: "center", sorttype: "float", formatter: 'select', editoptions: { value: "1:4-10;2:10-12;3:12-24"} },
            { name: 'Amount', index: 'Amount', width: 100, sortable: false, formatter: 'currency', summaryType: 'sum', summaryTpl: '<b>{0}</b>' }
        ],
        jsonReader: {
            repeatitems: false,

            root: function (obj) { return obj.d; },
            page: function (obj) { return 1; },
            total: function (obj) { return 1; },
            records: function (obj) { return obj.d.length; }
        },
        grouping: true,
        groupingView: {
            groupField: ['WorkMonth'],
            groupSummary: [true],
            groupColumnShow: [false],
            groupText: ['<b>{0}</b>'],
            groupCollapse: true,
            groupOrder: ['asc']
        }

    });

    jQuery("#ExpTbl").jqGrid('navGrid', { edit: false, add: false, del: false });
    jQuery(window).bind('resize', function () {


        var width = jQuery("#jqwrapper").width();
        var height = jQuery("#jqwrapper").height();
        var footer = jQuery("#footer").height();

        width = width - 2; // Fudge factor to prevent horizontal scrollbars
        height = height - footer - 2; // Fudge factor to prevent horizontal scrollbars
        if (width > 0 &&
        // Only resize if new width exceeds a minimal threshold
        // Fixes IE issue with in-place resizing when mousing-over frame bars
            Math.abs(width - jQuery("#ExpTbl").width()) > 5) {
            jQuery("#ExpTbl").setGridWidth(width);
        }

        if (height > 0 &&
            Math.abs(height - jQuery("#ExpTbl").height()) > 5) {
            jQuery("#ExpTbl").setGridHeight(height);
        }

        var height = $(window).height();
        $('.ui-jqgrid-bdiv').height(height - footer);

    }).trigger('resize');


    ///////////////////////////////////////////////////////////////
    // Load List Of Employees So Supr Can Change Viewed Employee //
    ///////////////////////////////////////////////////////////////
    var listOfEmps = [];
    function GetEmps_success(data) {
        listOfEmps = data.d.split("\r");
        $("#ddEmpIds").autocomplete({ source: listOfEmps },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: false,
                cacheLength: 20,
                max: 20,
                delay: 0,
                select: function (event, ui) {
                    var DataPieces = ui.item.value.split(' ');
                    $('#hlblEID')[0].innerHTML = DataPieces[0];
                    $("#ddEmpIds").autocomplete("close");
                    $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);

                    $("#ExpTbl").setGridParam({ postData: { EmpID: DataPieces[0], T: timestamp.getTime()} }).trigger("reloadGrid", [{ page: 1}]);
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = DataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);

                        var xxx = $("#ExpTbl");
                        $("#ExpTbl").setGridParam({ postData: { EmpID: DataPieces[0], T: timestamp.getTime()} }).trigger("reloadGrid", [{ page: 1}]);
                    }

                    return ui;
                }
            });
    }


    // Load Emps AutoComplete List
    if ($("#SuprArea").length > 0) {
        var GetEmpsCall = new AsyncServerMethod();
        GetEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", GetEmps_success);
    }
});