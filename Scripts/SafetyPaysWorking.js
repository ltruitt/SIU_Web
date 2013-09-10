$(document).ready(function () {

    $('#btnMngTasks').click(function () {
        window.location =   window.location.protocol +
                            '//' + window.location.hostname +
                            '/' + 'Safety/SafetyPays/SafetyPaysTasks.aspx?RptID=' +
                            $('#hlblKey')[0].innerHTML +
                            '&isA=' +
                            $('#hlblAdmin')[0].innerHTML;
    });
    
    $('#btnCloseRpt').click( function() {
        var closeRptAjax = new AsyncServerMethod();

        closeRptAjax.add('RcdID', $('#hlblKey')[0].innerHTML);
        closeRptAjax.add('EmpID', $('#hlblEID')[0].innerHTML);
        closeRptAjax.exec("/SIU_DAO.asmx/RecordSafetyPaysStatusComplete", removeSelectedRow);
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
        title: 'Select A Record for Detail or to Manage',
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
                title: 'Incident',
                width: '7%',
                type: 'date',
                displayFormat: 'mm-dd-yy',
                listClass: 'jTableTD',
                list: true,
                display: function (data) { return dateTest(data.record.IncidentDate); }
            },
            IncOpenTimestamp: {
                title: 'Opened',
                width: '7%',
                type: 'date',
                displayFormat: 'mm-dd-yy',
                listClass: 'jTableTD',
                list: true
            },
            PointsAssignedTimeStamp: {
                title: 'Accepted',
                width: '7%',
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
            ReportedByEmpName: {
                title: 'Reporter',
                width: '20%',
                listClass: 'jTableTD',
                list: true
            },
            TotalTasks: {
                title: 'Tasks',
                width: '3%',
                listClass: 'center',
                list: true
            },
            OpenTasks: {
                title: 'Open',
                width: '3%',
                listClass: 'center',
                list: true
            },
            LateTasks: {
                title: 'Late Comp.',
                width: '7%',
                listClass: 'center',
                list: true
            },
            LateStatus: {
                title: 'Late Stat.',
                width: '7%',
                listClass: 'center',
                list: true
            },




            defaultPoints: {
                title: 'Sug. Pts',
                width: '5%',
                listClass: 'center',
                list: false
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
            RejectReason: {
                title: 'Reject Reason',
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
                    var openDate = new Date(parseInt(record.IncOpenTimestamp.substr(6)));
                    var incDate = new Date(parseInt(record.IncidentDate.substr(6)));

                    $('#hlblKey')[0].innerHTML = record.IncidentNo;

                    $('#lblIncTypeTxt')[0].innerHTML = record.IncTypeTxt;
                    $('#lblIncOpenTimestamp')[0].innerHTML = $.datepicker.formatDate('mm-dd-yy', openDate);
                    $('#lblIncidentDate')[0].innerHTML = $.datepicker.formatDate('mm-dd-yy', incDate);

                    $('#lblJobSite')[0].innerHTML = record.JobSite;
                    $('#lblReportedByEmpName')[0].innerHTML = record.ReportedByEmpName;
                    if (record.ObservedEmpName != null) {
                        $('#lblObservedEmpName')[0].innerHTML = record.ObservedEmpName;
                    }

                    $('#lblComments')[0].innerHTML = record.Comments;
                    if (record.InitialResponse != null) {
                        $('#lblInitialResponse')[0].innerHTML = record.InitialResponse;
                    }

                    $('#txtPts').val(record.defaultPoints);

                    if (record.TotalTasks > 0 && record.OpenTasks == 0 && record.LateTasks == 0)
                        $('#btnCloseRpt').show();
                    else
                        $('#btnCloseRpt').hide();

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

        $('#lblComments')[0].innerHTML = '';
        //$('#lblInitialResponse')[0].innerHTML = '';

        $('#txtPts').val('');
    }



    function loadNoTask() {
        //$('#cmdDiv').hide();
        var timestamp = new Date();
        $('#jTableContainer').jtable('load', { DataFilter: 'NoTasks', isA: $('#hlblAdmin')[0].innerHTML, T: timestamp.getTime() });
    }

    function loadLateTask() {
        //$('#cmdDiv').hide();
        var timestamp = new Date();
        $('#jTableContainer').jtable('load', { DataFilter: 'LateTask', isA: $('#hlblAdmin')[0].innerHTML, T: timestamp.getTime() });
    }

    function loadLateStatus() {
        //$('#cmdDiv').hide();
        var timestamp = new Date();
        $('#jTableContainer').jtable('load', { DataFilter: 'LateStaus', isA: $('#hlblAdmin')[0].innerHTML, T: timestamp.getTime() });
    }

    function loadCloseReady() {
        //$('#cmdDiv').hide();
        var timestamp = new Date();
        $('#jTableContainer').jtable('load', { DataFilter: 'CloseReady', isA: $('#hlblAdmin')[0].innerHTML, T: timestamp.getTime() });
    }

    function loadCurrent() {
        //$('#cmdDiv').hide();
        var timestamp = new Date();
        $('#jTableContainer').jtable('load', { DataFilter: 'Current', isA: $('#hlblAdmin')[0].innerHTML, T: timestamp.getTime() });
    }

    function loadAssigned() {
        //$('#cmdDiv').hide();
        var timestamp = new Date();
        $('#jTableContainer').jtable('load', { DataFilter: 'Assigned', isA: $('#hlblAdmin')[0].innerHTML, T: timestamp.getTime() });
    }
    
    function loadAll() {
        //$('#cmdDiv').hide();
        var timestamp = new Date();
        $('#jTableContainer').jtable('load', { DataFilter: 'All', isA: $('#hlblAdmin')[0].innerHTML, T: timestamp.getTime() });
    }

    $('#btnNoTaks').click( function() {
        loadNoTask();
    });

    $('#btnLateTasks').click(function () {
        loadLateTask();
    });

    $('#btnLateSta').click(function () {
        loadLateStatus();
    });

    $('#btnRdyToCloseTasks').click(function () {
        loadCloseReady();
    });

    $('#btnAllCurrenttasks').click(function () {
        loadCurrent();
    });
    
    $('#btnAllTasks').click(function () {
        loadAll();
    });
    
    $('#btnAssigned').click(function () {
        loadAssigned();
    });
    
    switch ($.fn.getURLParameter('isA')) {
        case '0':   $('#hlblAdmin')[0].innerHTML = "0";
                    break;
        case '1':   $('#hlblAdmin')[0].innerHTML = "1";
                    break;
    }
        

    if ($('#hlblAdmin')[0].innerHTML == '0') {
        $('#btnAllCurrenttasks').hide();
        $('#btnRdyToCloseTasks').hide();
        $('#btnNoTaks').hide();
        $('#Userheader').show();
    } else {
        $('#btnAssigned').hide();
        $('#AdminHeader').show();
    }

    switch ($.fn.getURLParameter('s')) {
        case 'non': loadNoTask(); break;
        case 'tsk': loadLateTask(); break;
        case 'sta': loadLateStatus(); break;
        case 'clo': loadCloseReady(); break;
        case 'ass': loadAssigned(); break;
        default: loadAll();
    }


});