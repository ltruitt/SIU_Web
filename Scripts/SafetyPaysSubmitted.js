$(document).ready(function () {

    var deletedCount = 0;

    
    // When clicking on the button close or the mask layer the popup closed
    //$(document).on('click', 'a.close, #btnPopupReject, #btnPopupAccept' , function () {
    //    $('#mask , .popup').fadeOut(300, function () {
    //        $('#mask').remove();
    //    });
    //    return false;
    //});
    


    $('#btnAccept').click(function () {
        var ProcessSafetyPaysCall = new AsyncServerMethod();

        ProcessSafetyPaysCall.add('RcdID', $('#hlblKey')[0].innerHTML);
        ProcessSafetyPaysCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        ProcessSafetyPaysCall.add('Points', $('#txtPts').val());
        ProcessSafetyPaysCall.add('ehsRepsonse', $('#ehsRepsonse').val());

        ProcessSafetyPaysCall.exec("/SIU_DAO.asmx/RecordSafetyPaysStatusAcceptClosed", removeSelectedRow);
        

        //$('#btnPopupReject').hide();
        //$('#btnPopupAccept').show();
        

        ////Get The Popup Window Container
        //var loginBox = $(this).attr('href');
        //loginBox = $('#popup-box');

        //// Fade in the Popup
        //$(loginBox).fadeIn(300);

        ////Set the center alignment padding + border see css style
        //var popMargTop = ($(loginBox).height() + 24) / 2;
        //var popMargLeft = ($(loginBox).width() + 24) / 2;

        //$(loginBox).css({
        //    'margin-top': -popMargTop,
        //    'margin-left': -popMargLeft
        //});

        //// Add the mask to body
        //$('body').append('<div id="mask"></div>');
        //$('#mask').fadeIn(300);

        //return false;
    });


    $('#btnPopupAccept').click(function () {
        var ProcessSafetyPaysCall = new AsyncServerMethod();
        
        ProcessSafetyPaysCall.add('RcdID', $('#hlblKey')[0].innerHTML);
        ProcessSafetyPaysCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        ProcessSafetyPaysCall.add('Points', $('#txtPts').val());
        ProcessSafetyPaysCall.add('ehsRepsonse', $('#ehsRepsonse').val());

        ProcessSafetyPaysCall.exec("/SIU_DAO.asmx/RecordSafetyPaysStatusAcceptClosed", removeSelectedRow);
    });






    $('#btnWork').click(function () {
        var processSafetyPaysCall = new AsyncServerMethod();

        processSafetyPaysCall.add('RcdID', $('#hlblKey')[0].innerHTML);
        processSafetyPaysCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        processSafetyPaysCall.add('Points', $('#txtPts').val());
        processSafetyPaysCall.add('ehsRepsonse', $('#ehsRepsonse').val());

        processSafetyPaysCall.exec("/SIU_DAO.asmx/RecordSafetyPaysStatusWork", removeSelectedRow);
    });



    $('#btnReject').click(function () {

        var processSafetyPaysCall = new AsyncServerMethod();

        processSafetyPaysCall.add('RcdID', $('#hlblKey')[0].innerHTML);
        processSafetyPaysCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        processSafetyPaysCall.add('ehsRepsonse', $('#ehsRepsonse').val());

        processSafetyPaysCall.exec("/SIU_DAO.asmx/RecordSafetyPaysStatusReject", removeSelectedRow);
        
        //$('#btnPopupReject').show();
        //$('#btnPopupAccept').hide();
        //$('#ehsRepsonse').val('');

        ////Get The Popup Window Container
        //var loginBox = $(this).attr('href');
        //loginBox = $('#popup-box');

        //// Fade in the Popup
        //$(loginBox).fadeIn(300);

        ////Set the center alignment padding + border see css style
        //var popMargTop = ($(loginBox).height() + 24) / 2;
        //var popMargLeft = ($(loginBox).width() + 24) / 2;

        //$(loginBox).css({
        //    'margin-top': -popMargTop,
        //    'margin-left': -popMargLeft
        //});

        //// Add the mask to body
        //$('body').append('<div id="mask"></div>');
        //$('#mask').fadeIn(300);

        //return false;
    });

    //$('#btnPopupReject').click(function () {
        //var ProcessSafetyPaysCall = new AsyncServerMethod();

        //ProcessSafetyPaysCall.add('RcdID', $('#hlblKey')[0].innerHTML);
        //ProcessSafetyPaysCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        //ProcessSafetyPaysCall.add('ehsRepsonse', $('#ehsRepsonse').val());

        //ProcessSafetyPaysCall.exec("/SIU_DAO.asmx/RecordSafetyPaysStatusReject", RemoveSelectedRow);
    //});





    function removeSelectedRow() {

        if ($('#hlblKey')[0].innerHTML != '') {
            $('#jTableContainer').jtable('deleteRecord', {
                key: $('#hlblKey')[0].innerHTML,
                clientOnly: true,
                animationsEnabled: true
            });
            deletedCount++;



            if (deletedCount > 5) {
                reloadTable();
            }
        }

        ClearDtl();
    }

    function reloadTable() {
        var timestamp = new Date();
        $('#jTableContainer').jtable('load', { T: timestamp.getTime() });
        deletedCount = 0;
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
        sorting: false,
        multiselect: false,
        selectingCheckboxes: false,
        actions: {
            listAction: '/SIU_DAO.asmx/GetSafetyPaysRptData'
        },
        fields: {
            IncidentNo: {
                key: true,
                create: false,
                edit: false,
                list: false
            },

            IncidentDate: {
                title: 'Incident Date',
                width: '10%',
                type: 'date',
                displayFormat: 'mm-dd-yy',
                listClass: 'jTableTD',
                list: true,
                display: function (data) { return dateTest(data.record.IncidentDate); }
            },
            IncOpenTimestamp: {
                title: 'Open Date',
                width: '10%',
                type: 'date',
                displayFormat: 'mm-dd-yy',
                listClass: 'jTableTD',
                list: true
            },
            IncTypeTxt: {
                title: 'Inc. Type',
                width: '20%',
                listClass: 'jTableTD'
            },
            defaultPoints: {
                title: 'Sug. Pts',
                width: '5%',
                listClass: 'center',
                list: true
            },
            ReportedByEmpName: {
                title: 'Reporter',
                width: '20%',
                listClass: 'jTableTD',
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

        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableContainer').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    var OpenDate = new Date(parseInt(record.IncOpenTimestamp.substr(6)));
                    var IncDate = new Date(parseInt(record.IncidentDate.substr(6)));
                    var MtgDate = new Date(parseInt(record.SafetyMeetingDate.substr(6)));

                    $('#hlblKey')[0].innerHTML = record.IncidentNo;

                    $('#lblIncTypeTxt')[0].innerHTML = record.IncTypeTxt;
                    $('#lblIncOpenTimestamp')[0].innerHTML = $.datepicker.formatDate('mm-dd-yy', OpenDate);
                    $('#lblIncidentDate')[0].innerHTML = $.datepicker.formatDate('mm-dd-yy', IncDate);

                    $('#lblJobSite')[0].innerHTML = record.JobSite;
                    $('#lblReportedByEmpName')[0].innerHTML = record.ReportedByEmpName;
                    if (record.ObservedEmpName != null) {
                        $('#lblObservedEmpName')[0].innerHTML = record.ObservedEmpName;
                    }

                    if (record.SafetyMeetingType != null) {
                        $('#lblSafetyMeetingDate')[0].innerHTML = $.datepicker.formatDate('mm-dd-yy', MtgDate);
                        $('#lblSafetyMeetingType')[0].innerHTML = record.SafetyMeetingType;
                    }

                    $('#lblComments')[0].innerHTML = record.Comments;
                    if (record.InitialResponse != null) {
                        $('#lblInitialResponse')[0].innerHTML = record.InitialResponse;
                    }

                    $('#txtPts').val(record.defaultPoints);
                    $('#ehsRepsonse').val('');

                    $('#cmdDiv').show('');
                });
            } else {
                //No rows selected
                ClearDtl();
            }


        }
    });


    function ClearDtl() {
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

        $('#txtPts').val('');
    }

    $('#cmdDiv').hide();
    var timestamp = new Date();
    $('#jTableContainer').jtable('load', { DataFilter: 'New', isA: '1', T: timestamp.getTime() });






});