$(document).ready(function () {

    var timestamp = new Date();
    
    $('#jTableContainer').jtable({
        title: 'Expiring Badges and Certifications',
        title: '',
        actions: {
            listAction: '/SIU_DAO.asmx/GetMyExpiringBandC'
        },
        fields: {
            Description: {
                title: 'Qualification',
                width: '4%',
                listClass: 'jTableTDlj'
            },
            From_Date: {
                title: 'Issued',
                width: '2%',
                type: 'date',
                listClass: 'jTableTDcj'
            },
            Expiration_Date: {
                title: 'Expires',
                width: '2%',
                type: 'date',
                listClass: 'jTableTDcj'
            }
        }
    });

    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });
    
    ///////////////////////////////////////////////////////////////
    // Load List Of Employees So Supr Can Change Viewed Employee //
    ///////////////////////////////////////////////////////////////
    var listOfEmps = [];
    function getEmpsSuccess(data) {
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
                    var dataPieces = ui.item.value.split(' ');
                    $('#hlblEID')[0].innerHTML = dataPieces[0];
                    $("#ddEmpIds").autocomplete("close");
                    $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = dataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);

                        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });
                    }

                    return ui;
                }
            });
    }

    /////////////////////////////////
    // Load Emps AutoComplete List //
    /////////////////////////////////
    if ($("#SuprArea").length > 0) {
        var getEmpsCall = new AsyncServerMethod();
        getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);
    }

});