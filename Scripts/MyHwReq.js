$(document).ready(function () {


    $('#jTableContainer').jtable({
        title: 'Open Hardware Requests',
        actions: {
            listAction: '/SIU_DAO.asmx/GetMyOpenHwReq'
        },
        fields: {
            Hardware: {
                title: 'Hardware',
                width: '3%',
                listClass: 'jTableTD'
            },
            Price: {
                title: 'Cost',
                width: '2%',
                listClass: 'jTableTD'
            },
            Timestamp: {
                title: 'Submitted',
                width: '2%',
                type: 'date',
                listClass: 'jTableTD'
            }
        }
    });

    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });

});