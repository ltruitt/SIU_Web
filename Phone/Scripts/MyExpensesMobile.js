$(document).bind('pagecreate', function () {

    var timestamp = new Date();

    jQuery("#ExpTbl").jqGrid({

        url: '/SIU_DAO.asmx/adf1a',
        mtype: 'POST',
        ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },
        datatype: 'json',
        postData: { EmpID: $('#hlblEID').html(), LY: $('#chkLY').checked, T: timestamp.getTime() },
        serializeGridData: function (postData) { return JSON.stringify(postData); },
        autowidth: false,
        autoheight: true,

        pgbuttons: false,
        pginput: false,
        rowNum: 10000,
        sortname: 'WorkDate',
        colNames: ['Month', 'Date', 'Job', 'O/H', 'Miles', 'Meals', 'Amt'],
        colModel: [
            { name: 'WorkMonth', index: 'WorkMonth', width: 000, sorttype: "date", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'F' } },
            { name: 'WorkDate', index: 'WorkDate', width: 140, sorttype: "date", align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm-d' } },
            { name: 'JobNo', index: 'JobNo ', width: 100, sorttype: "float", align: "center" },
            { name: 'OH_AccountNo', index: 'OH_AccountNo', width: 100, sorttype: "float", align: "center" },
            { name: 'Miles', index: 'Miles', width: 100, sortable: false, align: "center", formatter: 'number' },
            { name: 'Meals', index: 'Meals', width: 100, sortable: false, align: "center", sorttype: "float", formatter: 'select', editoptions: { value: "1:4-10;2:10-12;3:12-24" } },
            { name: 'Amount', index: 'Amount', width: 100, sortable: false, formatter: 'currency', summaryType: 'sum', summaryTpl: '<b>{0}</b>' }
        ],
        jsonReader: {
            repeatitems: false,

            root: function (obj) { return obj.d; },
            page: function (obj) { return 1; },
            total: function (obj) { return 1; },
            records: function (obj) { return obj.d.length; }
        }


    });


    //    ,
    //grouping: true,
    //groupingView: {
    //    groupField: ['WorkMonth'],
    //    groupSummary: [true],
    //    groupColumnShow: [false],
    //    groupText: ['<b>{0}</b>'],
    //    groupCollapse: true,
    //    groupOrder: ['asc']
    //}
    

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

});