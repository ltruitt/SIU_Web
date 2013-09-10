$(document).ready(function () {

    $('#jTableContainer').jtable({
        title: 'Open Hardware Requests',
        actions: {
            listAction: '/SIU_DAO.asmx/GetMyOpenBugs'
        },
        fields: {
            Description: {
                title: 'Description',
                width: '3%',
                listClass: 'jTableTD'
            },
            OpenTimeStamp: {
                title: 'Opened ',
                width: '4%',
                type: 'date',
                listClass: 'jTableTD'
            },
            Accepted: {
                title: 'Accept',
                width: '2%',
                type: 'checkbox',
                values: { 'false': '', 'true': '\u2713' },
                defaultValue: 'true',
                listClass: 'jTableTD_ChkBox'
            },
            Working: {
                title: 'Work',
                width: '2%',
                type: 'checkbox',
                values: { 'false': '', 'true': '\u2713' },
                defaultValue: 'true',
                listClass: 'jTableTD_ChkBox'
            },
            Testing: {
                title: 'Test',
                width: '2%',
                type: 'checkbox',
                values: { 'false': '', 'true': '\u2713' },
                defaultValue: 'true',
                listClass: 'jTableTD_ChkBox'
            }            
        }
    });

    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });

});