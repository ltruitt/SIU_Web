
$(document).ready(function () {

    var timestamp = new Date();

    jQuery("#Missed_Tbl").jqGrid({

        url: '/SIU_DAO.asmx/GetMyMissedSafetyClasses',
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
        sortname: 'Date',
        colNames: ['Meeting Type', 'Topic', 'Meeting Date'],
        colModel: [
            { name: 'MeetingType', index: 'Meeting_Type', width: 30, sortable: true, sorttype: "string", align: "center", formatter: 'string' },
            { name: 'Topic', index: 'Topic', width: 200, sortable: true, sorttype: "string", align: "left", formatter: 'string' },
            { name: 'Date', index: 'Date', width: 50, sortable: true, sorttype: "date", align: "center", formatter: 'date' }
        ],
        jsonReader: {
            repeatitems: false,

            root: function (obj) { return obj.d; },
            page: function (obj) { return 1; },
            total: function (obj) { return 1; },
            records: function (obj) {
                 return obj.d.length;
            }
        },
        grouping: false,
        gridComplete: function () {
            jQuery("#Missed_Tbl").jqGrid('setGridHeight', $("#Missed_Tbl").height());
        }
    });

    jQuery("#Missed_Tbl").jqGrid('navGrid', { edit: false, add: false, del: false });

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

                    var postdata = $("#Missed_Tbl").jqGrid('getGridParam', 'postData');
                    postdata.EmpID = $('#hlblEID')[0].innerHTML;
                    postdata.T = timestamp.getTime();
                    jQuery("#Missed_Tbl").trigger("reloadGrid");

                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = DataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);

                        var postdata = $("#Missed_Tbl").jqGrid('getGridParam', 'postData');
                        postdata.EmpID = $('#hlblEID')[0].innerHTML;
                        postdata.T = timestamp.getTime();
                        jQuery("#Missed_Tbl").trigger("reloadGrid");
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