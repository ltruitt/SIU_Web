$(document).ready(function () {
    
    var d = $.fn.getURLParameter('d');
    
    ////////////////////////////////////////
    // Load Points Type AutoComplete List //
    ////////////////////////////////////////
    function getFleetRpt(dept) {
        var getTypesCall = new AsyncServerMethod();
        getTypesCall.add('Dept', dept);
        getTypesCall.exec("/SIU_DAO.asmx/GenVehInspRpt", genVehInspRptSuccess);
    }
    
    function genVehInspRptSuccess(data) {
        var record = $.parseJSON(data.d);
        $('#PlsWait').hide('slow');
        if (record) {
            $('#VehInspRpt').html(record);
        }
    }

    if (d == null) { d = ''; }
    if (d.length == 0) { d = ''; }
    getFleetRpt(d);
});
