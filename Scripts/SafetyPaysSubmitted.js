$(document).ready(function () {

    $('#btnAccept').click(function () {
        var processSafetyPaysCall = new AsyncServerMethod();

        processSafetyPaysCall.add('RcdID', $('#hlblKey')[0].innerHTML);
        processSafetyPaysCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        processSafetyPaysCall.add('Points', $('#txtPts').val());
        processSafetyPaysCall.add('ehsRepsonse', $('#ehsRepsonse').val());
        processSafetyPaysCall.add('SuprID', $('#hlblSID')[0].innerHTML);

        processSafetyPaysCall.exec("/SIU_DAO.asmx/RecordSafetyPaysStatusAcceptClosed", removeSelectedRow);
        
    });



    $('#btnWork').click(function () {
        var processSafetyPaysCall = new AsyncServerMethod();

        processSafetyPaysCall.add('RcdID', $('#hlblKey')[0].innerHTML);
        processSafetyPaysCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        processSafetyPaysCall.add('Points', $('#txtPts').val());
        processSafetyPaysCall.add('ehsRepsonse', $('#ehsRepsonse').val());
        processSafetyPaysCall.add('SuprID', $('#hlblSID')[0].innerHTML);
        
        processSafetyPaysCall.exec("/SIU_DAO.asmx/RecordSafetyPaysStatusWork", removeSelectedRow);
    });



    $('#btnReject').click(function () {

        var processSafetyPaysCall = new AsyncServerMethod();

        processSafetyPaysCall.add('RcdID', $('#hlblKey')[0].innerHTML);
        processSafetyPaysCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        processSafetyPaysCall.add('ehsRepsonse', $('#ehsRepsonse').val());
        processSafetyPaysCall.add('SuprID', $('#hlblSID')[0].innerHTML);
        
        processSafetyPaysCall.exec("/SIU_DAO.asmx/RecordSafetyPaysStatusReject", removeSelectedRow);
    });

    function removeSelectedRow() {

        if ($('#hlblKey')[0].innerHTML != '') {
            $('#jTableContainer').jtable('deleteRecord', {
                key: $('#hlblKey')[0].innerHTML,
                clientOnly: true,
                animationsEnabled: true
            });
        }

        clearDtl();
    }



    function dateTest(datefield) {
        var i = -1;
        if (datefield != null)
            i = parseInt(datefield.substr(6));

        if (i > 0)
            return $.datepicker.formatDate('mm-dd-yy', new Date(i));
        else
            return '';
    }
    
    ///////////////////////////////////////
    // Setup Report Of Unposted Expenses //
    ///////////////////////////////////////
    $('#jTableContainer').jtable({
        title: 'New Safety Pays Submissions',
        defaultSorting: 'IncidentNo ASC',
        edit: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        paging: true,
        pageSize: 50,
        actions: {
            listAction: '/SIU_DAO.asmx/GetSafetyPaysRptDataSorted',
            deleteAction: '/SIU_DAO.asmx/RemoveSafetyPaysRpt'
        },
        fields: {
            IncidentNo: {
                key: true,
                create: false,
                edit: false,
                list: true,
                width: '1%',
                sorting: true
            },

            IncidentDate: {
                title: 'Incident Date',
                width: '10%',
                type: 'date',
                displayFormat: 'mm-dd-yy',
                listClass: 'jTableTD',
                list: true,
                sorting: true,
                display: function (data) { return dateTest(data.record.IncidentDate); }
            },
            IncOpenTimestamp: {
                title: 'Open Date',
                width: '10%',
                type: 'date',
                displayFormat: 'mm-dd-yy',
                listClass: 'jTableTD',
                sorting: true,
                list: true
            },
            IncTypeTxt: {
                title: 'Inc. Type',
                width: '20%',
                sorting: true,
                listClass: 'jTableTD'
            },
            defaultPoints: {
                title: 'Sug. Pts',
                width: '5%',
                listClass: 'center',
                sorting: false,
                list: true
            },
            ReportedByEmpName: {
                title: 'Reporter',
                width: '20%',
                listClass: 'jTableTD',
                sorting: false,
                list: true
            },


            IncStatus: {
                title: 'Status',
                width: '0%',
                listClass: 'jTableTD',
                list: false
            },
            JobSite: {
                title: 'Site',
                width: '0%',
                listClass: 'jTableTD',
                list: false
            },
            Comments: {
                title: 'Comments',
                width: '8%',
                listClass: 'jTableTD',
                list: false
            },
            SafetyMeetingType: {
                title: 'Mtg. Type',
                width: '8%',
                listClass: 'jTableTD',
                list: false
            },


            IncCloseTimestamp: {
                title: 'Close Date',
                width: '3%',
                type: 'date',
                displayFormat: 'mm-dd',
                listClass: 'jTableTD',
                list: false
            },
            PointsAssignedTimeStamp: {
                title: 'Points Date',
                width: '3%',
                type: 'date',
                displayFormat: 'mm-dd',
                listClass: 'jTableTD',
                list: false
            },
            SafetyMeetingDate: {
                title: 'Mtg Date',
                width: '3%',
                type: 'date',
                displayFormat: 'mm-dd',
                listClass: 'jTableTD',
                list: false
            },
            PointsAssigned: {
                title: 'Pts',
                list: false
            },
            ehsResponse: {
                title: 'Initial EHS Response',
                list: false
            },
            InitialResponse: {
                title: 'Initial Response',
                list: false
            },
            ObservedEmpName: {
                title: 'Observed',
                width: '0%',
                listClass: 'jTableTD',
                list: false
            }
        },
        recordsLoaded: function () {
            $('#jTableContainer').jtable('selectRows', $('.jtable-data-row').first());
        },
        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableContainer').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    var openDate = new Date(parseInt(record.IncOpenTimestamp.substr(6)));
                    var incDate = new Date(parseInt(record.IncidentDate.substr(6)));
                    var mtgDate = new Date(parseInt(record.SafetyMeetingDate.substr(6)));

                    $('#hlblKey')[0].innerHTML = record.IncidentNo;

                    $('#lblIncTypeTxt')[0].innerHTML = record.IncTypeTxt;
                    $('#lblIncOpenTimestamp')[0].innerHTML = $.datepicker.formatDate('mm-dd-yy', openDate);
                    




                    $('#lblIncidentDate')[0].innerHTML = '';
                    $('#lbllblIncidentDate').hide();
                    var incd = $.datepicker.formatDate('mm-dd-yy', incDate);
                    if (incd != '01-01-1') {
                        $('#lbllblIncidentDate').show();
                        $('#lblIncidentDate')[0].innerHTML = incd;
                    };



                    $('#lblJobSite')[0].innerHTML = '';
                    $('#lbllblJobSite').hide();
                    if (record.JobSite != null && record.JobSite.length > 0 && record.JobSite != '-') {
                        $('#lblJobSite')[0].innerHTML = record.JobSite;
                        $('#lbllblJobSite').show();
                    }
                    



                    $('#lblReportedByEmpName')[0].innerHTML = record.ReportedByEmpName;
                    




                    $('#lbllblObservedEmpName').hide();
                    $('#lblObservedEmpName')[0].innerHTML = '';
                    if (record.ObservedEmpName != 'Unknown') {
                        $('#lblObservedEmpName')[0].innerHTML = record.ObservedEmpName;
                        $('#lbllblObservedEmpName').show();
                    }




                    $('#lbllblSafetyMeetingDate').hide();
                    $('#lbllblSafetyMeetingType').hide();
                    $('#lblSafetyMeetingDate')[0].innerHTML = '';
                    $('#lblSafetyMeetingType')[0].innerHTML = '';
                    var md = $.datepicker.formatDate('mm-dd-yy', mtgDate);
                    if (md != '01-01-1') {
                        $('#lbllblSafetyMeetingDate').show();
                        $('#lbllblSafetyMeetingType').show();
                        $('#lblSafetyMeetingDate')[0].innerHTML = md;
                        $('#lblSafetyMeetingType')[0].innerHTML = record.SafetyMeetingType;
                    }






                    $('#lblComments')[0].innerHTML = record.Comments;
                    





                    $('#lbllblInitialResponse').hide();
                    $('#lblInitialResponse')[0].innerHTML = '';
                    if (record.InitialResponse != null && record.InitialResponse.length > 0) {
                        $('#lbllblInitialResponse').show();
                        $('#lblInitialResponse')[0].innerHTML = record.InitialResponse;
                    }

                    $('#txtPts').val(record.defaultPoints);
                    $('#ehsRepsonse').val('');

                    $('#cmdDiv').show('');
                });
            } else {
                //No rows selected
                clearDtl();
            }


        }
    });


    function clearDtl() {
        $('#cmdDiv').hide();
        $('#hlblKey')[0].innerHTML = '';

        $('#lblIncTypeTxt')[0].innerHTML = '';
        $('#lblIncOpenTimestamp')[0].innerHTML = '';
        $('#lblIncidentDate')[0].innerHTML = '';

        $('#lblJobSite')[0].innerHTML = '';
        $('#lblReportedByEmpName')[0].innerHTML = '';
        $('#lblObservedEmpName')[0].innerHTML = '';

        $('#lblSafetyMeetingDate')[0].innerHTML = '';
        $('#lblSafetyMeetingType')[0].innerHTML = '';

        $('#lblComments')[0].innerHTML = '';
        $('#lblInitialResponse')[0].innerHTML = '';
        
        $('#hlblSID')[0].innerHTML = '';
        $("#ddEmpIds").val('');

        $('#txtPts').val('');
    }

    $('#cmdDiv').hide();
    var timestamp = new Date();
    $('#jTableContainer').jtable('option', 'pageSize', 20);
    $('#jTableContainer').jtable('load', { DataFilter: 'New', isA: '1', T: timestamp.getTime() });



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
                    $('#hlblSID')[0].innerHTML = dataPieces[0];
                    $("#ddEmpIds").autocomplete("close");
                    $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $('#hlblSID')[0].innerHTML = dataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                    }

                    return ui;
                }
            });
    }

    /////////////////////////////////
    // Load Emps AutoComplete List //
    /////////////////////////////////
    var getEmpsCall = new AsyncServerMethod();
    getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);


});