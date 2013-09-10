$(document).ready(function () {

    $('#jTableContainer').jtable({
        title: 'These Jobs Need A Report',
        defaultSorting: 'JobNo ASC',
        edit: true,
        actions: {
            listAction: '/SIU_DAO.asmx/GetMyPastDueJobRptsSum'
        },
        fields: {
            JobNo: {
                title: 'Job No',
                width: '2%',
                listClass: 'jTableTD'
            },
            JobCust: {
                title: 'Cust ',
                width: '4%',
                listClass: 'jTableTD'
            },
            CostingDate: {
                title: 'Cost Date',
                width: '4%',
                listClass: 'jTableTD',
                list: false
            },
            LastLaborDate: {
                title: 'Last Worked',
                width: '4%',
                listClass: 'jTableTD'
            },
            DaysLate: {
                title: 'Late',
                width: '2%',
                listClass: 'jTableTD'
            },
            JobNo2: {
                title: '',
                width: '1%',
                sorting: false,
                edit: true,
                create: false,
                display: function (JobData) {

                    // Create an image that will be used to open child table
                    var $img = $('<a href="/Reporting/SubmitJobReport.aspx?Job=' + JobData.record.JobNo + '"><img style="width:30px; height: 30px;" src="/Images/menuRtnTransparent.png" title="Open Form" /></a>');
                    return $img;
                }
            }
        }
    });


    $('#DetailDiv').hide();
    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });


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
                    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = DataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);

                        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });
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