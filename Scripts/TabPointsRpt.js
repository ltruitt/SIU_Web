$(document).ready(function () {

    var timestamp = new Date();


    $('#StartDate').datepicker({
        constrainInput: true,
        onSelect: LookUp()
    });

    $('#EndDate').datepicker({
        constrainInput: true,
        onSelect: LookUp()
    });


    function DefaultQuarter() {
        var d = new Date();
        var quarter = Math.floor((d.getMonth() / 3));
        var firstDate = new Date(d.getFullYear(), quarter * 3, 1);
        $("#StartDate").datepicker("setDate", firstDate);
        $("#EndDate").datepicker("setDate", new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0));
    }
    DefaultQuarter();




    function SetQuarter(quarter) {
        var d = new Date();
        quarter = quarter - 1;

        var firstDate = new Date(d.getFullYear(), quarter * 3, 1);
        $("#StartDate").datepicker("setDate", firstDate);
        $("#EndDate").datepicker("setDate", new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0));
    }

    $('#btnQ1').click(function () {
        SetQuarter(1);
        LookUp();
    });

    $('#btnQ2').click(function () {
        SetQuarter(2);
        LookUp();
    });

    $('#btnQ3').click(function () {
        SetQuarter(3);
        LookUp();
    });

    $('#btnQ4').click(function () {
        SetQuarter(4);
        LookUp();
    });











    $('.MonBtn').click(function () {

        switch (this.value.toLowerCase()) {

            case 'jan':
                SetMonth(1);
                break;

            case 'feb':
                SetMonth(2);
                break;

            case 'mar':
                SetMonth(3);
                break;

            case 'apr':
                SetMonth(4);
                break;

            case 'may':
                SetMonth(5);
                break;

            case 'jun':
                SetMonth(6);
                break;

            case 'jul':
                SetMonth(7);
                break;

            case 'aug':
                SetMonth(8);
                break;

            case 'sep':
                SetMonth(9);
                break;

            case 'oct':
                SetMonth(10);
                break

            case 'nov':
                SetMonth(11);
                break

            case 'dec':
                SetMonth(12);
                break
        }

    });


    function SetMonth(mon) {

        mon = mon - 1;

        if ($('#StartDate').val().length > 0 && $('#EndDate').val().length > 0) {
            $('#StartDate').val('');
            $('#EndDate').val('');
        }

        var d = new Date();
        var firstDate = new Date(d.getFullYear(), mon, 1);
        var lastDate = new Date(d.getFullYear(), mon + 1, 0);

        if ( $('#StartDate').val().length == 0 ) {
            $("#StartDate").datepicker("setDate", firstDate);
            LookUp()
            return;
        }

        if ($('#EndDate').val().length == 0) {
            $("#EndDate").datepicker("setDate", lastDate);
            LookUp()
            return;
        }

    }

    $('.ui-icon-plus').on("click", function (event) {
        var top = event.screenY;
        var left = event.screenX;
    });


    function LookUp() {

        if ($('#StartDate').val().length == 0)
            return;

        if ($('#EndDate').val().length == 0)
            return;

        var postdata = $("#ExpTbl").jqGrid('getGridParam', 'postData');
        postdata.startDate = $('#StartDate').val();
        postdata.endDate = $('#EndDate').val();
        postdata.T = timestamp.getTime();
        jQuery("#ExpTbl").trigger("reloadGrid");
    }





    var h1 = 0;
    var h2 = 0;
    var h3 = 0;

    var t2id = '';
    jQuery("#ExpTbl").jqGrid({

        url: '/SIU_DAO.asmx/GetAdminPointsRptDepts',
        datatype: 'json',
        mtype: 'POST',

        ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },
        postData: { startDate: $('#StartDate').val(), endDate: $('#EndDate').val(), T: timestamp.getTime() },
        serializeGridData: function (postData) {
            return JSON.stringify(postData);
        },
        jsonReader: {
            repeatitems: false,

            root: function (obj) { return obj.d.rows; },
            page: function (obj) { return obj.d.page; },
            total: function (obj) { return obj.d.total; },
            records: function (obj) { return obj.d.records; }
        },

        autowidth: true,
        autoheight: false,
        pgbuttons: false,
        pginput: true,
        excel: true,
        height: 300,

        rowNum: 100,

        sortname: 'EmpDept',
        colNames: ['Department', 'Points', ''],
        colModel: [

            { name: 'EmpDept', hidden: false, key: true, index: 'EmpDept', width: 20, sortable: false, sorttype: "number", align: "left", formatter: 'text' },
            { name: 'Points', hidden: false, index: 'Points', width: 50, sortable: false, sorttype: "number", align: "center", formatter: 'number' },
            { name: 'Comments', hidden: false,  width: 300}
        ],

        grouping: false,

        groupingView: {
            groupField: ['EmpDept'],
            groupDataSorted: true,
            groupOrder: ['asc'],
            groupSummary: [false],
            groupColumnShow: [true],
            groupText: ['<b>Department {0}</b>', '<b>{0}</b>'],
            groupCollapse: true

        },

        gridComplete: function () {
            h1 = jQuery("#ExpTbl").height();

            var iWindowHeight = $(window).height();
            var iDocHeight = $(document).height();
            var divPos = $('.ui-jqgrid-bdiv').position();
            //var top = $(".ui-jqgrid-bdiv").offset().top + window.screenY;

            h1 = iDocHeight - 385;

            jQuery("#ExpTbl").setGridHeight(h1);
            $('.ui-jqgrid-bdiv').height(h1 + 6);

        },

// Start Of Emp SUmmary Table //
        subGrid: true,

        subGridRowExpanded: function(subgrid_id, row_id) {
            // If we want to pass additional parameters to the url we can use
            // the method getRowData(row_id) - which returns associative array in type name-value
            var subgrid_table_id;

            subgrid_table_id = subgrid_id + "_t";
            jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");

            jQuery("#"+subgrid_table_id).jqGrid({

                url: '/SIU_DAO.asmx/GetAdminPointsRptEmps',
                datatype: 'json',
                mtype: 'POST',

                ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },
        
                postData: { startDate: $('#StartDate').val(), endDate: $('#EndDate').val(), dept: row_id, T: timestamp.getTime() },
                serializeGridData: function (postData) {
                    return JSON.stringify(postData);
                },
                jsonReader: {
                    repeatitems: false,

                    root: function (obj) { return obj.d.rows; },
                    page: function (obj) { return obj.d.page; },
                    total: function (obj) { return obj.d.total; },
                    records: function (obj) { return obj.d.records; }
                },

                autowidth: false,
                autoheight: true,
                width: 600,
                pgbuttons: false,
                pginput: false,
                pager: '#xpager',
                excel: false,

                rowNum: 100,

                sortname: 'EmpDept',
                colNames: ['Dept', 'No', 'Name', 'Pts', ''],
                colModel: [

                    { name: 'EmpDept',  hidden: true, index: 'EmpDept ' },
                    { name: 'Emp_No',   key: true,  hidden: true, index: 'Emp_No' },
                    { name: 'EmpName',  index: 'Emp_No', width: 150, sortable: false, sorttype: "text", align: "center", formatter: 'text' },
                    { name: 'Points',   index: 'Points',   width: 50, sortable: false, sorttype: "number", align: "center", formatter: 'number'},
                    { name: 'Comments', hidden: false, width: 300 }
                    
                ],

                grouping: false,

                groupingView: {
                    groupField: ['Emp_No'],
                    groupDataSorted: true,
                    groupOrder: ['asc'],
                    groupSummary: [false],
                    groupColumnShow: [true],
                    groupText: ['<b>Employee {0}</b>', '<b>{0}</b>'],
                    groupCollapse: true

                },


                gridComplete: function () {
                    t2id = "#" + subgrid_table_id;
                    h2 = jQuery(t2id).height();
                    jQuery(t2id).setGridHeight(h2 + 6);
                },

// Start Of Emp Detail View
                subGrid: true,
                subGridRowExpanded: function(subgrid_id, row_id) {
                    var subgrid_table_id;

                    subgrid_table_id = subgrid_id + "_t";
                    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");

                    jQuery("#"+subgrid_table_id).jqGrid({

                        url: '/SIU_DAO.asmx/GetAdminPointsRptEmpPoints',
                        datatype: 'json',
                        mtype: 'POST',

                        ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },

                        postData: { startDate: $('#StartDate').val(), endDate: $('#EndDate').val(), empNo: row_id, T: timestamp.getTime() },
                        serializeGridData: function (postData) {
                            return JSON.stringify(postData);
                        },
                        jsonReader: {
                            repeatitems: false,

                            root: function (obj) { return obj.d.rows; },
                            page: function (obj) { return obj.d.page; },
                            total: function (obj) { return obj.d.total; },
                            records: function (obj) { return obj.d.records; }
                        },

                        width: 400,
                        height: 100,
                        pgbuttons: false,
                        pginput: false,
                        excel: false,

                        rowNum: 100,

                        gridComplete: function () {
                            h3 = jQuery("#" + subgrid_table_id).height();
                            jQuery(t2id).setGridHeight(h2 + h3 + 40);
                            jQuery("#" + subgrid_table_id).setGridHeight(h3 + 6);
                        },

                        sortname: 'EventDate',
                        colNames: ['Date', 'Recorded', 'Type', /*'Rec By',*/ 'Comments', 'Pts', 'UID'],
                        colModel: [

                            { name: 'EventDate', index: 'EventDate', width: 30, sortable: false, sorttype: "date", align: "center", formatter: 'date' },
                            { name: 'DatePointsGiven', index: 'DatePointsGiven', width: 30, sortable: false, sorttype: "date", align: "center", formatter: 'date' },
                            { name: 'PointsType', index: 'ReasonForPoints', width: 50, sortable: false, sorttype: "number", align: "center", formatter: 'text' },
                            //{ name: 'PointsGivenByName', index: 'PointsGivenBy ', width: 50, sortable: false, sorttype: "number", align: "center", formatter: 'text' },
                            { name: 'Comments', index: 'Comments', width: 100, sortable: false, align: "center" },
                            { name: 'Points', index: 'Points', width: 50, sortable: false, sorttype: "number", align: "center", formatter: 'number', summaryType: 'sum', summaryTpl: '<b>{0}</b>' },
                            { name: 'UID', hidden: true, key: true }
                        ],

                        grouping: false,
                    });

                }


            });

        }



    });







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

                    $("#ExpTbl").setGridParam({ postData: { EmpID: DataPieces[0], T: timestamp.getTime() } }).trigger("reloadGrid", [{ page: 1 }]);
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = DataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);

                        $("#ExpTbl").setGridParam({ postData: { EmpID: DataPieces[0], T: timestamp.getTime() } }).trigger("reloadGrid", [{ page: 1 }]);
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