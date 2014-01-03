$(document).ready(function () {
    ////////////////////////////////////////
    // Load Points Type AutoComplete List //
    ////////////////////////////////////////
    var GetTypesCall = new AsyncServerMethod();
    GetTypesCall.add('Dept', '');
    GetTypesCall.exec("/SIU_DAO.asmx/GenVehInspRpt", GenVehInspRptSuccess);
    
    function GenVehInspRptSuccess(data) {
        var record = $.parseJSON(data.d);
        $('#PlsWait').hide('slow');
        if (record) {
            $('#VehInspRpt').html(record);
        }



    }
});
