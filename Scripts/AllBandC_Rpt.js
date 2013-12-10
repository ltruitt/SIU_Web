


$(document).ready(function () {

    var getColumnIndexByName = function (grid, columnName) {
        var cm = grid.jqGrid('getGridParam', 'colModel'), i = 0, l = cm.length;
        for (; i < l; i++) {
            if (cm[i].name === columnName) {
                return i; // return the index
            }
        }
        return -1;
    };

    var timestamp = new Date();

    jQuery("#BandC_Tbl").jqGrid({

        url: '/SIU_DAO.asmx/GetMyAllBandC',
        mtype: 'POST',
        ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },
        datatype: 'json',
        postData: { EmpID: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() },
        serializeGridData: function (postData) { return JSON.stringify(postData); },
        autowidth: true,
        height: 100,
        pgbuttons: false,
        pginput: false,
        rowNum: 10000,
        sortname: 'category',
        colNames: ['category', 'Issue', 'Expire', 'Qualification_Code', 'Description'],
        colModel: [
            { name: 'category', index: 'category', width: 0, sortable: true, sorttype: "string", align: "left", formatter: 'string' },
            { name: 'From_Date', index: 'From Date ', width: 50, sortable: true, sorttype: "date", align: "center", formatter: 'date' },
            { name: 'Expiration_Date', index: 'Expiration Date', width: 50, sortable: true, sorttype: "date", align: "center", formatter: 'date' },
            { name: 'Qualification_Code', index: 'Qualification_Code', width: 50, sortable: true, sorttype: "string", align: "center", formatter: 'string' },
            { name: 'Description', index: 'Description', width: 200, sortable: true, sorttype: "string", align: "center", formatter: 'string' }
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
            groupField: ['category'],
            groupSummary: [false],
            groupColumnShow: [false],
            groupText: ['<b>{0}</b>'],
            groupCollapse: false,
            groupOrder: ['asc']
        },
        gridComplete: function () {
            jQuery("#BandC_Tbl").jqGrid('setGridHeight', $("#BandC_Tbl").height());
        },
        loadComplete: function () {
            var iCol = getColumnIndexByName($(this), 'Expiration_Date'),
            cRows = this.rows.length, iRow, row, className;

            for (iRow = 0; iRow < cRows; iRow++) {
                row = this.rows[iRow];
                className = row.className;
                if ($.inArray('jqgrow', className.split(' ')) > 0) {
                    var x = $(row.cells[iCol]);
                    if (x[0].innerText.length > 0) {
                        var expDate = new Date(x[0].innerText);
                        if (expDate < timestamp ) {
                            row.className = className + ' myAltRowClass';
                        }
                    }
                }
            }
        }

    });

    jQuery("#BandC_Tbl").jqGrid('navGrid', { edit: false, add: false, del: false });

    jQuery(window).bind('resize', function () {

        // Get width of parent container
        var width = jQuery("#jqwrapper").attr('clientWidth');
        if (width == null || width < 1) {
            // For IE, revert to offsetWidth if necessary
            width = jQuery("#jqwrapper").attr('offsetWidth');
        }
        width = width - 2; // Fudge factor to prevent horizontal scrollbars
        if (width > 0 &&
        // Only resize if new width exceeds a minimal threshold
        // Fixes IE issue with in-place resizing when mousing-over frame bars
        Math.abs(width - jQuery("#BandC_Tbl").width()) > 5) {
            jQuery("#BandC_Tbl").setGridWidth(width);
            alert(width);
        }

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

                    var postdata = $("#BandC_Tbl").jqGrid('getGridParam', 'postData');
                    postdata.EmpID = $('#hlblEID')[0].innerHTML;
                    postdata.T = timestamp.getTime();
                    jQuery("#BandC_Tbl").trigger("reloadGrid");

                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = DataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);

                        var postdata = $("#BandC_Tbl").jqGrid('getGridParam', 'postData');
                        postdata.EmpID = $('#hlblEID')[0].innerHTML;
                        postdata.T = timestamp.getTime();
                        jQuery("#BandC_Tbl").trigger("reloadGrid");
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