$(document).ready(function () {

    $('#jTableContainer').jtable({
        title: 'Vehicle Mileage',
        defaultSorting: 'Assigned_Vehicle_No_ ASC',
        edit: true,
        actions: {
            listAction: '/SIU_DAO.asmx/GetMyVehicleMileageRpt',
            deleteAction: '/SIU_DAO.asmx/DeleteMyVehicleMileageRecord'
        },
        fields: {
            Assigned_Vehicle_No_: {
                title: 'Vehicle',
                width: '2%',
                listClass: 'jTableTD'
            },
            Week_Ending: {
                title: 'Week',
                key: true,
                width: '4%',
                type: 'date',
                listClass: 'jTableTD'
            },
            Weekly_Personal_Mileage: {
                title: 'Miles',
                width: '3%',
                listClass: 'jTableTD'
            },
            Emp_No: { title: 'Emp No', key: true, list: false },
            timestamp: { title: 'ts', list: false }
        }
    });

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

