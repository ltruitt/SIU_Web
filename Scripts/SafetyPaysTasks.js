$(document).ready(function () {


    ///////////////////////////////////////
    // Setup Report Of Unposted Expenses //
    ///////////////////////////////////////
    $('#jTableTasks').jtable({
        title: 'Safety Pays Report Tasks',
        defaultSorting: 'IncidentNo ASC',
        edit: true,
        selecting: true,
        sorting: false,
        multiselect: false,
        selectingCheckboxes: false,
        actions: {
            createAction: '/SIU_DAO.asmx/RecordSafetyPaysRptTask',
            listAction: '/SIU_DAO.asmx/GetSafetyPaysRptTasks'
            
        },
        formSubmitting: function (event, data) {
            data.form.find('input[name="IncidentNo"]').val($('#hlblKey')[0].innerHTML);

            var eid = data.form.find('input[name="AssignedEmpId"]').val().split(' ')[0];
            data.form.find('input[name="AssignedEmpId"]').val(eid);
            
        },
        messages: {
            addNewRecord: 'Add new Task'
        },

        fields: {
            IncidentNo: {
                key: true,
                create: true,
                edit: false,
                list: false,
                type: 'hidden'
            },
            TaskNo: {
                key: true,
                create: false,
                edit: false,
                list: false
            },

            TaskDefinition: {
                title: 'Task Description',
                width: '50%',
                listClass: 'jTableTD',
                list: true,
                inputClass: 'DataInputCss jTableInput'
            },

            AssignedEmpId: {
                title: 'Assign To',
                list: false,
                inputClass: 'DataInputCss jTableAutoCompleteInput'
            },

            AssignedEmpName: {
                create: false,
                title: 'Assigned',
                width: '15%',
                listClass: 'jTableTD',
                list: true
            },
            
            DueDate: {
                title: 'Due',
                width: '7%',
                type: 'date',
                displayFormat: 'mm-dd-yy',
                listClass: 'jTableTD',
                list: true,
                inputClass: 'DataInputCss datepicker'
            },

            CompletedDate: {
                create: false,
                title: 'Completion',
                width: '7%',
                type: 'date',
                displayFormat: 'mm-dd-yy',
                listClass: 'jTableTD',
                list: true,
                display: function (data) { return compDate(data.record); }
            }


        },

        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableTasks').jtable('selectedRows');

            if ($selectedRows.length == 1) {

                $selectedRows.each(function () {
                    $('#jTableStatusDiv').show('slow');
                    var record = $(this).data('record');
                    $('#hlblTaskNo')[0].innerHTML = record.TaskNo;

                    var timestamp = new Date();
                    $('#jTableStatus').jtable('load', { RptID: record.IncidentNo, TaskNo: record.TaskNo, T: timestamp.getTime() });
                    

                });
            }
            else
                $('#jTableStatusDiv').hide('slow');
        },

        formCreated: function (event, data) {
            data.form.find('[name=AssignedEmpId]').autocomplete({ source: listOfEmps },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: false,
                cacheLength: 20,
                max: 20,
                delay: 0,
                select: function (event, ui) {
                    var empid = data.form.find('[name=AssignedEmpId]');
                    empid.autocomplete("close");
                    validate();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var empid = data.form.find('[name=AssignedEmpId]');
                        empid.val(ui.content[0].value);
                        empid.autocomplete("close");
                        validate();
                    }

                    return ui;
                }
            });


            var task = data.form.find('input[name="TaskDefinition"]');
            var eid = data.form.find('input[name="AssignedEmpId"]');
            var date = data.form.find('input[name="DueDate"]');


            function isDate(txtDate) 
            { 
                var currVal = txtDate; 
                if(currVal == '') 
                        return false; 

                //Declare Regex   
                var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;  
                var dtArray = currVal.match(rxDatePattern); // is format OK? 

                if (dtArray == null) 
                        return false; 

                //Checks for mm/dd/yyyy format. 
                var dtMonth = dtArray[1]; 
                var dtDay = dtArray[3]; 
                var dtYear = dtArray[5]; 

                if (dtMonth < 1 || dtMonth > 12) 
                        return false; 

                else if (dtDay < 1 || dtDay> 31) 
                        return false; 

                else if ((dtMonth==4 || dtMonth==6 || dtMonth==9 || dtMonth==11) && dtDay ==31) 
                        return false; 

                else if (dtMonth == 2) 
                    { 
                            var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0)); 
                            if (dtDay> 29 || (dtDay ==29 && !isleap)) 
                                    return false; 
                    } 
                return true; 
            } 
                

            function validate()
            {
                task.removeClass("validateError");
                eid.removeClass("validateError");
                date.removeClass("validateError");
                $('#AddRecordDialogSaveButton').show();

                if (task.val().length == 0) {
                    task.addClass("validateError");
                    $('#AddRecordDialogSaveButton').hide();
                }

                if (eid.val().length == 0) {
                    eid.addClass("validateError");
                    $('#AddRecordDialogSaveButton').hide();
                }

                if (! isDate(date.val())) {
                    date.addClass("validateError");
                    $('#AddRecordDialogSaveButton').hide();
                }
            }

            task.focus(function () {
                validate();
            });
            
            task.keyup(function () {
                validate();
            });

            eid.blur(function () {
                validate();
            });


            date.keyup(function () {
                validate();
            });

            validate();

        }

    });


    ///////////////////////////////////////
    // Setup Report Of Unposted Expenses //
    ///////////////////////////////////////
    $('#jTableStatus').jtable({
        title: 'Safety Pays Report Task Status Reports',
        defaultSorting: 'IncidentNo ASC',
        edit: false,
        selecting: true,
        sorting: false,
        multiselect: false,
        selectingCheckboxes: false,
        actions: {
            listAction: '/SIU_DAO.asmx/GetSafetyPaysRptTaskStatus',
            createAction: '/SIU_DAO.asmx/RecordSafetyPaysRptTaskStatus',      
            updateAction: { url: '/SIU_DAO.asmx/RecordSafetyPaysRptTaskStatus', 
                    enabled: function(record) {
                        return record.Response == null;
                    }
            }            

        },
        messages: {
            addNewRecord: 'Add new Status'
        },

        recordUpdated: function(event, data) {
            var $row = data.row;            
            var rowCnt = ($row)[0].cells.length;
            ($row)[0].cells[rowCnt - 1].outerHTML = "<td></td>";
        },


        formSubmitting: function (event, data) {
            data.form.find('input[name="IncidentNo"]').val($('#hlblKey')[0].innerHTML);
            data.form.find('input[name="TaskNo"]').val($('#hlblTaskNo')[0].innerHTML);
            data.form.find('input[name="UID"]').val(0);
        },

        fields: {
            UID: {
                title: 'UID',
                key: true,
                create: true,
                type: 'hidden',
                edit: true,
                list: false
            },
            IncidentNo: {
                title: 'Inc',
                create: true,
                type: 'hidden',
                edit: false,
                list: false
            },
            TaskNo: {
                title: 'Task',
                create: true,
                type: 'hidden',
                edit: false,
                list: false
            },

            Response: {
                title: 'Progress Report',
                edit: true,
                width: '70%',
                listClass: 'jTableTD',
                list: true,
                type: 'textarea',
                inputClass: 'DataInputCss ResponseCss',
                display: function (data) { return statusText(data); }
            },

            LastReportCnt: {
                title: 'Req #',
                edit: false,
                width: '1%',
                listClass: 'jTableTD',
                list: true,
                display: function (data) { return pickReqCnt(data); },
                create: false
            },

            LastReportDate: {
                title: 'Last Req',
                edit: false,
                width: '7%',
                listClass: 'jTableTD',
                list: true,
                display: function (data) { return pickDate(data); },
                create: false
            },

            ResponseDate: {
                title: 'Rpt Date',
                edit: false,
                width: '10%',
                type: 'date',
                displayFormat: 'mm-dd-yy',
                listClass: 'jTableTD',
                list: true,
                create: false
            }
        },

        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableTasks').jtable('selectedRows');

            if ($selectedRows.length == 1) {

                $selectedRows.each(function () {
                    var record = $(this).data('record');

                    var timestamp = new Date();
                    $('#jTableStatus').jtable('load', { RptID: record.IncidentNo, TaskNo: record.TaskNo, T: timestamp.getTime() });
                });
            }
        }
    });


    function statusText(data) {
        // Create an image that will be used to open child table
        var $img = $('<span class="PrgRptCss" >Please Submit A Progress Report</span>');

        if (data.record.Response == null)
            return $img;

        return data.record.Response;
    }




    function pickReqCnt(data) {
        if (data.record.ReqDate3 != null)
            return 3;

        if (data.record.ReqDate2 != null)
            return 2;

        return 1;

    }


    function pickDate(data) {
        if (data.record.ReqDate3 != null)
            return $.datepicker.formatDate('mm-dd-yy', new Date(parseInt(data.record.ReqDate3.substr(6))));

        if (data.record.ReqDate2 != null)
            return $.datepicker.formatDate('mm-dd-yy', new Date(parseInt(data.record.ReqDate2.substr(6))));

        if (data.record.ReqDate1 != null)
            return $.datepicker.formatDate('mm-dd-yy', new Date(parseInt(data.record.ReqDate1.substr(6))));

        return 'unsolicited';
    }




    //////////////////////////////////////////////////////////////////////////
    // When clicking on the button close or the mask layer the popup closed //
    //////////////////////////////////////////////////////////////////////////
    $(document).on('click', 'a.close, #btnPopupSubmit', function () {
        $('#mask , .popup').fadeOut(300, function () {
            $('#mask').remove();
        });
        
        if (this.className == "close")
            return false;
        
        function closeSuccess(data) {
            $('#jTableTasks').jtable('updateRecord', { record: data.d, clientOnly: true, animationsEnabled: true });
        }
        
        function closeFail(data) {
            alert(data);
        }
        
        var closeSafetyPaysRpt = new AsyncServerMethod();
        closeSafetyPaysRpt.add('RptID', $('#hlblKey')[0].innerHTML);
        closeSafetyPaysRpt.add('TaskNo', $('#hlblTaskNo')[0].innerHTML);
        closeSafetyPaysRpt.add('CloseNotes', $('#Report').val());
        closeSafetyPaysRpt.exec("/SIU_DAO.asmx/RecordSafetyPaysTaskComplete", closeSuccess);
        
        return false;
    });
    



    function compDate(record) {
        var i = -1;
        if ( record.CompletedDate != null )
            i = parseInt(record.CompletedDate.substr(6));

        var $img = $('<a id=' + "ClsTsk" + record.IncidentNo + ' href="CloseTask"><img class="ClsTskCss" src="/Images/TaskComplete1.png"/></a>').click(function (e) {
                e.preventDefault();
                e.stopPropagation();
            
                $('#hlblTaskNo')[0].innerHTML = record.TaskNo;

                //////////////////////////////////
                // Open The Popup Dialog Window //
                //////////////////////////////////
                $('#Report').val('');

                ////////////////////////////////////
                // Get The Popup Window Container //
                ////////////////////////////////////
                var loginBox = $(this).attr('href');
                loginBox = $('#popup-box');

                ///////////////////////
                // Fade in the Popup //
                ///////////////////////
                $(loginBox).fadeIn(300);

                ///////////////////////////////////////////
                // Set Center Alignment Padding + Border //
                ///////////////////////////////////////////
                var popMargTop = ($(loginBox).height() + 24) / 2;
                var popMargLeft = ($(loginBox).width() + 24) / 2;

                $(loginBox).css({
                    'margin-top': -popMargTop,
                    'margin-left': -popMargLeft
                });

                //////////////////////
                // Add Mask To Body //
                //////////////////////
                $('body').append('<div id="mask"></div>');
                $('#mask').fadeIn(300);

                return false;
            





        });



        //////////////////////////////////////////////
        // If The Tsk Is Closed -- Show Closed Date //
        //////////////////////////////////////////////
        if (i > 0)
            return $.datepicker.formatDate('mm-dd-yy', new Date(i));



        ///////////////////////////////
        // For Open Tasks:           //
        // Is This User Not An Admin //
        ///////////////////////////////
        if ($('#hlblAdmin')[0].innerHTML == '0') {
            
            //////////////////////////////////////////////
            // If This User Is Not The Assigned To User //
            // And Task Not Closed -- Just Show "OPEN"  //
            //////////////////////////////////////////////
            if (record.AssignedEmpID != $('#hlblEID')[0].innerHTML) {
                if (i < 0)
                    return 'Open';
            }
            
            ////////////////////////////////////////
            // This Is The User Task Assigned To  //
            // Task Still Open -- Show Close Task //
            ////////////////////////////////////////
            return $img;
            
        } else {                
            ////////////////////////////////////////
            // Task Still Open -- Show Close Task //
            ////////////////////////////////////////
            return $img;
        }
    }




    function loadTasks() {
        var timestamp = new Date();
        $('#jTableTasks').jtable('load', { RptID: $('#hlblKey')[0].innerHTML, T: timestamp.getTime() });
    }


    function showRptDtl(data) {
        var record = JSON.parse(data.d).Records;

        if (record == null)
            return;

        var openDate = new Date(parseInt(record.IncOpenTimestamp.substr(6)));
        var incDate = new Date(parseInt(record.IncidentDate.substr(6)));
        
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
        
        if (record.EhsRepsonse != null) {
            $('#lblEhsResponse')[0].innerHTML = record.EhsRepsonse;
        }

    }

    ///////////////////////////////////
    // Load Safety Report To Display //
    ///////////////////////////////////
    $('#hlblKey')[0].innerHTML = $.fn.getURLParameter('RptID');

    //////////////////////////////////////////
    // Or If There Is No Report Id Provided //
    // Switch To List Screen                //
    //////////////////////////////////////////
    if ($.fn.getURLParameter('RptID') == null) {
        window.location = window.location.protocol + '//' + window.location.hostname + '/' + "Safety/SafetyPays/SafetyPaysWorking.aspx";
        return;
    }

    ///////////////////////////////////////////////////////////////
    // Load List Of Employees For New Task Form Lookup Assign To //
    ///////////////////////////////////////////////////////////////
    var listOfEmps = [];
    function getEmpsSuccess(data) {
        listOfEmps = data.d.split("\r");
    }
    var getEmpsCall = new AsyncServerMethod();
    getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);

    /////////////////////////
    // Test For Admin User //
    /////////////////////////
    switch ($.fn.getURLParameter('isA')) {
        case '0':   $('#hlblAdmin')[0].innerHTML = "0";
            break;
        case '1': $('#hlblAdmin')[0].innerHTML = "1";
            break;
    }
    
    ///////////////////////////////////////////////
    // Preserve Admin Setting Is They Click Back //
    ///////////////////////////////////////////////
    $('#hrefBack')[0].href += $('#hlblAdmin')[0].innerHTML;

    /////////////////////////////////////////
    // Hide Add Task Toolbar For Non-Admin //
    // + Add new Task                      //
    /////////////////////////////////////////
    if ($('#hlblAdmin')[0].innerHTML == '1') {
        $('#jTableTasks .jtable-toolbar').show();
    } else {
        $('#jTableTasks .jtable-toolbar').hide();
    }
    
    ////////////////////
    // Load Task List //
    ////////////////////
    var loadSafetyPaysRptCall = new AsyncServerMethod();
    loadSafetyPaysRptCall.add('DataFilter', $('#hlblKey')[0].innerHTML);
    loadSafetyPaysRptCall.add('isA', $('#hlblAdmin')[0].innerHTML);
    loadSafetyPaysRptCall.add('jtStartIndex', 0);
    loadSafetyPaysRptCall.add('jtPageSize', 0);
    loadSafetyPaysRptCall.exec("/SIU_DAO.asmx/GetSafetyPaysRptData", showRptDtl);
   
    loadTasks();
    
});