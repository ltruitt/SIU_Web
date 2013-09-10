
$(document).ready(function () {
    ////////////////////////////////////////
    // Event Management For Job Selection //
    ////////////////////////////////////////
    var listOfCerts = [];
    function GetCerts_success(data) {
        listOfCerts = data.d.split("\r");

        $("#acCertList").autocomplete(
            { source: listOfCerts },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: false,
                cacheLength: 20,
                max: 20,
                select: function (event, ui) {
                    var DataPieces = ui.item.value.split(' ');
                    $(this).val(DataPieces[0].replace(/\n/g, ""));
                    $("#acCertList").val(DataPieces[0]);
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $(this).val(DataPieces[0].replace(/\n/g, ""));
                        $("#acCertList").autocomplete("close");
                        $("#acCertList").val(DataPieces[0]);
                    }

                    return ui;
                }
            });
    }

    function GetCerts() {
        var GetCerts_Ajax = new AsyncServerMethod();
        GetCerts_Ajax.exec("/SIU_DAO.asmx/GetCerts", GetCerts_success);
    }



    

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
                    $("#ddEmpIds").autocomplete("close");
                    $("#ddEmpIds").val(DataPieces[0]);
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(DataPieces[0]);
                    }

                    return ui;
                }
            });
    }

    // Look Up List Of Valid Cert Codes
    GetCerts();

    // Load Emps AutoComplete List
    var GetEmpsCall = new AsyncServerMethod();
    GetEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", GetEmps_success);

});