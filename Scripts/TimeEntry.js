$(document).ready(function () {

    ///////////////////////////
    // Disable Submit Button //
    ///////////////////////////
    //$('#btnSubmit').prop('disabled', true);
    $('#btnSubmit').hide();

    //////////////////////////////////////////////////
    // Prevent Buttons From Firing Events On Server //
    //////////////////////////////////////////////////
    $("#aspnetForm").submit(function (e) { e.preventDefault(); });
    
    /////////////////////////////
    // Set Week To "This" Week //
    /////////////////////////////
    $('#hlblWeekIdx')[0].innerHTML = 0;
    $('.prevarrow').click(function () {
        var idx = parseInt($('#hlblWeekIdx')[0].innerHTML);

        if (idx > -1)
            idx = idx - 1;

        $('#hlblWeekIdx')[0].innerHTML = idx;
        $('#txtEntryDate').val('');
        $('#txtEntryDate').addClass('ValidationError');
        showDowColor();
        $('#lblHoursThisDay')[0].innerHTML = '<b>--</b>';
        $('.DowBtnCSS').removeClass('DowBtnSelectedCss');

        showWeekSum(idx);
    });
    $('.nextarrow').click(function () {
        var idx = parseInt($('#hlblWeekIdx')[0].innerHTML);
        if (idx < 1)
            idx = idx + 1;

        $('#hlblWeekIdx')[0].innerHTML = idx;
        $('#txtEntryDate').val('');
        $('#txtEntryDate').addClass('ValidationError');
        showDowColor();
        $('#lblHoursThisDay')[0].innerHTML = '<b>--</b>';
        $('.DowBtnCSS').removeClass('DowBtnSelectedCss');

        showWeekSum(idx);
    });


    ////////////////////////////////
    // Show Sum Of Hours For Week //
    ////////////////////////////////
    var weeklyHours = Array(0, 0, 0);
    function getWeekSumHours() {
        $('#lblHoursThisWeek')[0].innerHTML = '<b>--</b>';
                
        var getWeekSumHoursAjax = new AsyncServerMethod();
        getWeekSumHoursAjax.add('EmpID', $('#hlblEID')[0].innerHTML);
        getWeekSumHoursAjax.add('StartDate', $('#hlblSD')[0].innerHTML);
        getWeekSumHoursAjax.exec("/SIU_DAO.asmx/GetTimeSheet_Sum", getWeekSumHoursSuccess, getWeekSumHoursFail);
        $('#WeekSumDiv').hide();
    }
    function getWeekSumHoursSuccess(data) {
        $('#WeekSumDiv').show();
        weeklyHours = data.d;
        var idx = parseInt($('#hlblWeekIdx')[0].innerHTML);
        showWeekSum(idx);
        
    }
    function getWeekSumHoursFail() {
        $('#WeekSumDiv').hide();
    }
    
    function showWeekSum(idx) {
        $('#lblHoursThisWeek')[0].innerHTML = '<b>' + weeklyHours[idx + 1] + '</b>';
    }
    getWeekSumHours();



    /////////////////////////////////////////////////////////
    // Change Color For DOW Buttons Based On Selected Week //
    /////////////////////////////////////////////////////////
    function showDowColor() {
        var idx = parseInt($('#hlblWeekIdx')[0].innerHTML);

        if (idx == 0)
            $(".DowBtnCSS").css("background-color", "green");

        if (idx == -1)
            $(".DowBtnCSS").css("background-color", "red");

        if (idx == 1)
            $(".DowBtnCSS").css("background-color", "orange");
    }


    ///////////////////////////////////////////
    // Get Jason Array Of Total Hours By Day //
    ///////////////////////////////////////////
    var dailyHoursDetails;
    function getSumHoursByDaySuccess(data) {
        dailyHoursDetails = null;
        dailyHoursDetails = $.parseJSON(data.d);
        showHoursForDate();
    }
    function getSumHoursByDayFail() {
        dailyHoursDetails = null;
        showHoursForDate();
    }
    function getSumHoursByDay() {
        var getSumHoursByDayCall = new AsyncServerMethod();
        getSumHoursByDayCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        getSumHoursByDayCall.add('StartDate', $('#hlblSD')[0].innerHTML);
        getSumHoursByDayCall.add('EndDate', $('#hlblEndD')[0].innerHTML);
        getSumHoursByDayCall.exec("/SIU_DAO.asmx/GetTimeTotHoursByDay", getSumHoursByDaySuccess, getSumHoursByDayFail);
    }
    getSumHoursByDay();


    ////////////////////
    // Setup Calendar //
    ////////////////////
    var startDate = new Date($('#hlblSD')[0].innerHTML);
    var endDate = new Date($('#hlblEndD')[0].innerHTML);

    $('#txtEntryDate').datepicker({
        minDate: startDate,
        maxDate: endDate,
        constrainInput: true,
        onSelect: showHoursForDate
    });
    $('#txtEntryDate').blur(function () {
        if (dailyHoursDetails != null)
            showHoursForDate();
    });


    /////////////////////////////////////////
    // Show Hours Entered For Date Showing //
    /////////////////////////////////////////
    function showHoursForDate() {
        validate();
        $('#lblHoursThisDay')[0].innerHTML = '<b>0</b>';
        if (dailyHoursDetails == undefined)
            return;
        for (var c = 0; c < dailyHoursDetails.length; c++) {
            if (new Date(dailyHoursDetails[c].workDate).getDate() == new Date($('#txtEntryDate').val()).getDate()) {
                $('#lblHoursThisDay')[0].innerHTML = '<b>' + dailyHoursDetails[c].Hours + '</b>';
            }
        }
    }


    //////////////////////////////////////////////////////////
    // Get Jason Array Of Avail Sick, Vac, and Holiday Time //
    //////////////////////////////////////////////////////////
    var availHours;
    function getTimeOpenBalanceSuccess(data) {
        availHours = $.parseJSON(data.d);
        $('#lblSick')[0].innerHTML = availHours.SICK;
        $('#lblVac')[0].innerHTML = availHours.VACATION;
        $('#lblHol')[0].innerHTML = availHours.PHOLIDAY;
        $('#AccruDiv').show();
    }
    
    function getTimeOpenBalanceFail() {
        $('#AccruDiv').hide();
    }
    
    function getTimeOpenBalance() {
        $('#AccruDiv').hide();
        availHours = "";

        var getTimeOpenBalanceCall = new AsyncServerMethod();
        getTimeOpenBalanceCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        getTimeOpenBalanceCall.exec("/SIU_DAO.asmx/GetTimeOpenBalance", getTimeOpenBalanceSuccess, getTimeOpenBalanceFail);
    }
    getTimeOpenBalance();


    ///////////////////////////////////////////////////////
    // Event Management For Overhead Account Departments //
    ///////////////////////////////////////////////////////
    var listOfDepts = [];
    function getTimeDeptsSuccess(data) {
        listOfDepts = data.d.split("\r");
        $("#ddDept").autocomplete({ source: listOfDepts },
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
                    var lblval = '<b>Div/Dept:</b> ' + dataPieces[0];
                    $('#lblDeptSelection')[0].innerHTML = lblval;
                    $('#hlblDeptSelection')[0].innerHTML = dataPieces[0];
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        var lblval = '<b>Div/Dept:</b> ' + dataPieces[0];
                        $('#lblDeptSelection')[0].innerHTML = lblval;
                        $('#hlblDeptSelection')[0].innerHTML = dataPieces[0];
                        $("#ddDept").autocomplete("close");
                    }

                    return ui;
                }
            });
    }
    function showDept() {

        // Hide Job and O/H Account Input Boxes
        $('#OhAcctDiv').hide();
        $('#JobDiv').hide();

        // Show Dept Input Box
        $('#DepDiv').show('slow');

        // Load Department AutoComplete List
        var getTimeDeptsCall = new AsyncServerMethod();
        getTimeDeptsCall.exec("/SIU_DAO.asmx/GetTimeDepts", getTimeDeptsSuccess);

        // Add Lost Focus Event That Will Update Dept Labels
        // If the Code Eneterd was 6020 (Not On LIst)
        $("#ddDept").blur(function () {
            if ($("#ddDept").val() == '6020') {
                var lblval = '<b>Div/Dept:</b> 6020';
                $('#lblDeptSelection')[0].innerHTML = lblval;
                $('#hlblDeptSelection')[0].innerHTML = '6020';
            }
            $(this).val("");
        });
    };


    /////////////////////////////////////////////
    // Event Management For Job Task Selection //
    /////////////////////////////////////////////
    var listOfTasks = [];
    function getTimeTasksSuccess(data) {
        listOfTasks = data.d.split("\r");

        $("#ddTaskNo").autocomplete({ source: listOfTasks },
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
                $('#ddTaskNo').val(dataPieces);
                var lblval = '<b>Task:</b> ' + ui.item.value;
                $('#lblTaskCodeSelection')[0].innerHTML = lblval;
                $('#hlblTaskCodeSelection')[0].innerHTML = dataPieces[0];
                $("#ddTaskNo").autocomplete("close");
                $('#TimeCheckBoxes').show('slow');
                $('#TaskDiv').hide();
                $('#txtTime').focus();
                $('#txtTime').val('');
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var dataPieces = ui.content[0].value.split(' ');
                    $('#ddTaskNo').val(ui.content[0].value);
                    var lblval = '<b>Task:</b> ' + ui.content[0].value;
                    $('#lblTaskCodeSelection')[0].innerHTML = lblval;
                    $('#hlblTaskCodeSelection')[0].innerHTML = dataPieces[0];
                    $("#ddTaskNo").autocomplete("close");
                    $('#TimeCheckBoxes').show('slow');
                    $('#TaskDiv').hide();
                    $('#txtTime').focus();
                    $('#txtTime').val('');
                }

                return ui;
            }
        });
    }
    function showTask() {

        // Hide Job and O/H Account Input Boxes
        $('#OhAcctDiv').hide();
        $('#JobDiv').hide();

        // Show Dept Input Box
        $('#TaskDiv').show('slow');

        var getTimeTasksCall = new AsyncServerMethod();
        getTimeTasksCall.add('JobNo', $('#ddJobNo').val());
        getTimeTasksCall.exec("/SIU_DAO.asmx/GetTimeTasks", getTimeTasksSuccess);
    };


    //////////////////////////////////////////////////
    // Make Sure Text Entered For OH Was In OH List //
    //////////////////////////////////////////////////
    $("#ddTaskNo").blur(function () {
        if (!listOfTasks.containsCaseInsensitive(this.value)) {
            $(this).val("");
        }
    });


    //////////////////////////////////////////////////
    // Open Data Collection Boxes For Class Details //
    //////////////////////////////////////////////////
    function showClasses() {
        // Hide Job and O/H Account Input Boxes
        $('#OhAcctDiv').hide();
        $('#JobDiv').hide();
        $("#ClassDataCollectionDiv").show('slow');

        $('#chkST').prop('checked', true);

        $('#chkST').prop('disabled', true);
        $('#chkDT').prop('disabled', true);
        $('#chkOT').prop('disabled', true);
        $('#chkAB').prop('disabled', true);
        $('#chkHT').prop('disabled', true);
    }

    ////////////////////////////////////
    // Class Details Completed Button //
    ////////////////////////////////////
    $("#btnClassComplete").click(function () {

        $('#lblClassTimeSelection')[0].innerHTML = '<br/><b>Class Time:</b> ' + $('#txtClassTime').val() + '<br/>';
        $('#hlblClassTimeSelection')[0].innerHTML = $('#txtClassTime').val();

        $('#lblClassLocSelection')[0].innerHTML = '<b>Class Loc:</b> ' + $('#txtClassLoc').val() + '<br/>';
        $('#hlblClassLocSelection')[0].innerHTML = $('#txtClassLoc').val();

        $('#lblClassInstrSelection')[0].innerHTML = '<b>Teacher:</b> ' + $('#TxtClassInstr').val() + '<br/>';
        $('#hlblClassInstrSelection')[0].innerHTML = $('#TxtClassInstr').val();

        $('#lblClassDescSelection')[0].innerHTML = '<b>Desc:</b> ' + $('#txtClassDesc').val() + '<br/>';
        $('#hlblClassDescSelection')[0].innerHTML = $('#txtClassDesc').val();


        $("#ClassDataCollectionDiv").hide();
        $('#TimeCheckBoxes').show('slow');
        $('#txtTime').focus();
        $('#txtTime').val('');
    });


    ////////////////////////
    // Advance The Focus / /
    ////////////////////////    
    $('#txtTime').blur(function () {
        $("#ddJobNo").focus();
    });
    $('#btnClear').blur(function () {
        $("#txtTime").focus();
    });




    ///////////////////////////
    // Submit Button Handler //
    ///////////////////////////
    function timeSubmitSuccess(data) {
        
        var errorMsg = data.d;

        if (errorMsg.length > 7 && errorMsg.substring(0, 7) == "Success") {

            var hoursType = $('input:checkbox:checked')[0].value;
            var hours = parseInt($('#txtTime').val());
            var day = new Date($('#txtEntryDate').val()).getDate();
            
            $('#TimeAck').show();
            $('#TimeAck').html(hours + ' Hours Submitted');

            ///////////////////////////////////////////////////////////////////////////
            // If This Is The Desktop Version, Then a HighCharts Object Should Exist //
            // Update It.  The Appended Hours Is Added To highCharts By The Desktop  //
            // Script                                                                //
            ///////////////////////////////////////////////////////////////////////////
            if (typeof Highcharts != "undefined") {
                if (typeof MonthDaily != "undefined")
                    Highcharts.AppendHours(day, hoursType, hours);
            }

            ////////////////////
            // Reset The Form //
            ////////////////////
            $("#btnClear").trigger('click');

            /////////////////////////
            // Update Hours By Day //
            /////////////////////////
            getSumHoursByDay();

            /////////////////////////
            // Update Accrued Time //
            /////////////////////////
            $('#lblSick')[0].innerHTML = '0';
            $('#lblVac')[0].innerHTML = '0';
            $('#lblHol')[0].innerHTML = '0';

            getTimeOpenBalance();

            ////////////////////////////////
            // Update Weekly Hour Summary //
            ////////////////////////////////
            getWeekSumHours();

        } else {
            if (errorMsg.length == 0) {
                errorMsg = 'Unknown Server Error Occured';
            }
            $('#lblErrServer')[0].innerHTML = errorMsg;
        }
    };
    $("#btnSubmit").click(function () {
        $('#btnSubmit').hide();
        $('#TimeAck').html('');
        $('#TimeAck').hide();
        var hoursType = $('input:checkbox:checked')[0].value;

        var timeSubmitCall = new AsyncServerMethod();
        timeSubmitCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        timeSubmitCall.add('JobNo', $('#hlblJobNoSelection')[0].innerHTML);
        timeSubmitCall.add('OhAcct', $('#hlblOhAcctSelection')[0].innerHTML);
        timeSubmitCall.add('Dept', $('#hlblDeptSelection')[0].innerHTML);
        timeSubmitCall.add('Task', $('#hlblTaskCodeSelection')[0].innerHTML);
        timeSubmitCall.add('ClassTime', $('#hlblClassTimeSelection')[0].innerHTML);
        timeSubmitCall.add('ClassDesc', $('#hlblClassDescSelection')[0].innerHTML);
        timeSubmitCall.add('ClassLoc', $('#hlblClassLocSelection')[0].innerHTML);
        timeSubmitCall.add('ClassInstr', $('#hlblClassInstrSelection')[0].innerHTML);
        timeSubmitCall.add('HoursType', hoursType);
        timeSubmitCall.add('Hours', $('#txtTime').val());
        timeSubmitCall.add('workDate', $('#txtEntryDate').val());

        timeSubmitCall.exec("/SIU_DAO.asmx/TimeSubmit", timeSubmitSuccess);

    });







    ////////////////////////////////////////
    // Event Management For Job Selection //
    ////////////////////////////////////////
    var listOfJobs = [];
    function getTimeJobsSuccess(data) {

        listOfJobs = data.d.split("\r");

        $("#ddJobNo").autocomplete({ source: listOfJobs },
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
                $(this).val(dataPieces[0].replace(/\n/g, ""));
                getJobDetails();
                showTask();
                $('#ddTaskNo').focus();
                $('#ddTaskNo').val('');
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var dataPieces = ui.content[0].value.split(' ');
                    $(this).val(dataPieces[0].replace(/\n/g, ""));
                    getJobDetails();
                    $("#ddJobNo").autocomplete("close");
                    showTask();
                    $('#ddTaskNo').focus();
                    $('#ddTaskNo').val('');
                }

                return ui;
            }
        });
    }
    

    
    function getTimeJobs() {
        var getTimeJobsCall = new AsyncServerMethod();
        getTimeJobsCall.exec("/SIU_DAO.asmx/Affe31", getTimeJobsSuccess);
    }
    getTimeJobs();


    //////////////////////////////////////////////////////
    // Make Sure Text Entered For Jobs Was In Jobs List //
    //////////////////////////////////////////////////////
    $("#ddJobNo").blur(function () {
        if (!listOfJobs.containsCaseInsensitive(this.value)) {
            $(this).val("");
        }
    });
    function getTimeJobSuccess(data) {
        var jobDetails = $.parseJSON(data.d);
        $('#hlblJobNoSelection')[0].innerHTML = jobDetails.JobNo;
        $('#hlblDeptSelection')[0].innerHTML = jobDetails.JobDept;

        $('#lblJobNoSelection')[0].innerHTML = "<b>Job No:</b> " + jobDetails.JobNo + "<br/>";
        $('#lblJobSiteSelection')[0].innerHTML = "<b>Customer:</b> " + jobDetails.JobCust + "<br/>";
        $('#lblJobDescSelection')[0].innerHTML = "<b>Job:</b> " + jobDetails.JobDesc + "<br/>";
        $('#lblDeptSelection')[0].innerHTML = "<b>Div/Dept:</b> " + jobDetails.JobDept; // +"<br/>"
        showDept();
    }
    function getJobDetails() {
        $('#hlblJobNoSelection')[0].innerHTML = $('#ddJobNo').val();
        
        var getTimeJobCall = new AsyncServerMethod();
        getTimeJobCall.add('jobNo', $('#ddJobNo').val());
        getTimeJobCall.exec("/SIU_DAO.asmx/Gffeop1", getTimeJobSuccess);
    }







    /////////////////////////////////////////////////////
    // Event Management For Overhead Account Selection //
    /////////////////////////////////////////////////////
    var listOfAccounts = [];
    function getTimeOhAcctsSuccess(data) {
        listOfAccounts = data.d.split("\r");

        $("#ddOhAcct").autocomplete({ source: listOfAccounts },
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
                    
                    /////////////////////////////////////////
                    // Store Display Value For O/H Account //
                    /////////////////////////////////////////
                    $('#hldlOhAcct')[0].innerHTML = ui.item.value;
                    
                    //////////////////////////////////////////////////////
                    // Get And Store O/H Account Number Only For Upload //
                    //////////////////////////////////////////////////////
                    $(this).val(dataPieces[0]);
                    $('#hlblOhAcctSelection')[0].innerHTML = ui.item.value;
                    
                    getOhDetails();

                    // If The O/H Account Is For A Class 
                    if (dataPieces[0] == "4137") {
                        showClasses();
                    } else {
                        showDept();
                        $('#TimeCheckBoxes').show('slow');
                        $('#txtTime').focus();
                        $('#txtTime').val('');
                    }


                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        
                        /////////////////////////////////////////
                        // Store Display Value For O/H Account //
                        /////////////////////////////////////////
                        $('#hldlOhAcct')[0].innerHTML = ui.content[0].value;
                        
                        //////////////////////////////////////////////////////
                        // Get And Store O/H Account Number Only For Upload //
                        //////////////////////////////////////////////////////
                        $(this).val(dataPieces[0]);
                        $('#hlblOhAcctSelection')[0].innerHTML = ui.content[0].value;
                        
                        getOhDetails();
                        $("#ddOhAcct").autocomplete("close");

                        // If The O/H Account Is For A Class 
                        if (dataPieces[0] == "4137") {
                            showClasses();
                        } else {
                            showDept();
                            $('#TimeCheckBoxes').show('slow');
                            $('#txtTime').focus();
                            $('#txtTime').val('');
                        }
                    }

                    return ui;
                }
            });
    }
    function getTimeOhAccts() {
        var getTimeOhAcctsCall = new AsyncServerMethod();
        getTimeOhAcctsCall.exec("/SIU_DAO.asmx/GetTimeOHAccts", getTimeOhAcctsSuccess);
    }
    getTimeOhAccts();




    //////////////////////////////////////////////////
    // Make Sure Text Entered For OH Was In OH List //
    //////////////////////////////////////////////////
    $("#ddOhAcct").blur(function () {
        if (!listOfAccounts.containsCaseInsensitive(this.value)) {
            $(this).val("");
        }
    });


    /////////////////////////////////////////////
    // Show Selected Overhead Account and Dept //
    // Enable / Disable Time Type Checkboxes   //
    /////////////////////////////////////////////
    function getOhDetails() {

        var acct = $('#ddOhAcct').val();
        var dept = $('#hlblDeptSelection')[0].innerHTML;

        ////////////////////////////////////////////////////////
        // Save and Display OH Account and Default Department //
        ////////////////////////////////////////////////////////
        $('#hlblOhAcctSelection')[0].innerHTML = acct;
        $('#lblOhAcctSelection')[0].innerHTML = "<b>O/H Acct:</b> " + $('#hldlOhAcct')[0].innerHTML + "<br/>";
        $('#lblDeptSelection')[0].innerHTML = "<b>Div/Dept:</b> " + dept; // +"<br/>";

        ///////////////////////////////////////////////////////////
        // Absence And Holiday Time Not Allowed With O/H Account //
        ///////////////////////////////////////////////////////////
        $('#chkAB').prop('disabled', true);
        $('#chkHT').prop('disabled', true);

        ////////////////////////////////////////////////////
        // Force Absence Time Reporting For Sick Hours    //
        // Force Absence Time Reporting For Vacation      //
        ////////////////////////////////////////////////////
        if (acct == "SICK" || acct == "VACATION" || acct == "OTHER") {
            $('#chkST').prop('checked', false);
            $('#chkAB').prop('checked', true);

            $('#chkST').prop('disabled', true);
            $('#chkDT').prop('disabled', true);
            $('#chkOT').prop('disabled', true);
        }
        
        ////////////////////////////////////////////////////
        // Force Holiday Time Reporting For Holiday Hours //
        ////////////////////////////////////////////////////
        if (acct == "HOLIDAY") {
            $('#chkST').prop('checked', false);
            $('#chkHT').prop('checked', true);

            $('#chkST').prop('disabled', true);
            $('#chkDT').prop('disabled', true);
            $('#chkOT').prop('disabled', true);
        }

        ///////////////////////////////////////////////////////
        // Force Holiday Time Reporting For Personal Holiday //
        ///////////////////////////////////////////////////////
        if (acct == "PHOLIDAY") {
            $('#chkST').prop('checked', false);
            $('#chkAB').prop('checked', true);

            $('#chkST').prop('disabled', true);
            $('#chkDT').prop('disabled', true);
            $('#chkOT').prop('disabled', true);
        }


    }


    ////////////////////////////////////////////////////////////////
    // Catch Click Event For DOW Buttons And Update DOW Text Box  //
    ////////////////////////////////////////////////////////////////
    $('.DowBtnCSS').click(function () {

        var idx = parseInt($('#hlblWeekIdx')[0].innerHTML);
        
        if (idx == 0)
            $('#txtEntryDate').val(this.getAttribute('DateForThis'));

        if (idx == -1)
            $('#txtEntryDate').val(this.getAttribute('DateForPrev'));

        if (idx == 1)
            $('#txtEntryDate').val(this.getAttribute('DateForNext'));

        showHoursForDate();
        $('.DowBtnCSS').removeClass('DowBtnSelectedCss');
        $(this).addClass('DowBtnSelectedCss');
    });

    mutex('chkbox');
    ///////////////////////////////////
    // Catch CheckBox Change Events  //
    // Test For At Least One Checked //
    // Check For MutEx               //
    ///////////////////////////////////
    $('input:checkbox').change(function () {
        validate();
    });


    /////////////////////////////
    // Form Validation Section //
    /////////////////////////////
    $('#txtTime').keyup(function () {
        validate();
    });


    //////////////////////////////////////
    // Validate Data Fields.            //
    // Highlight Bad Fields.            //
    // Enable or Disable Submit Button. //
    //////////////////////////////////////
    function validate() {

        $('#btnSubmit').show();
        $('#txtTime').removeClass('ValidationError');
        $('#txtTime').removeClass('ValidationSuccess');
        $('#lblErr')[0].innerHTML = '';
        $('#txtEntryDate').removeClass('ValidationError');
        
        var hours = $('#txtTime').val();


        if ($('#hlblJobNoSelection')[0].innerHTML.length == 0 && $('#hlblOhAcctSelection')[0].innerHTML.length == 0) {
            $('#btnSubmit').hide();
            return;
        }
        

        ////////////////////////////////
        // Date Value Must Be Present //
        ////////////////////////////////
        if ($('#txtEntryDate').val().length == 0) {
            $('#txtEntryDate').addClass('ValidationError');
            $('#btnSubmit').hide();
            return;
        }
        
        ////////////////////////////////
        // Time Value Must Be Present //
        ////////////////////////////////
        if ($('#txtTime').val().length == 0) {
            $('#txtTime').addClass('ValidationError');
            $('#btnSubmit').hide();
            return;
        }

        //////////////////////////
        // Time Must Be Numeric //
        //////////////////////////
        if (!isNumber($('#txtTime').val())) {
            $('#txtTime').addClass('ValidationError');
            return;
        }

        //////////////////////////////
        // Time Must In Valid Range //
        //////////////////////////////
        if (hours < 0 || hours > 24) {
            $('#txtTime').addClass('ValidationError');
            $('#btnSubmit').hide();
            $('#lblErr')[0].innerHTML = 'Invalid Hours Amount.';
            return;
        }


        //////////////////////////////////////
        // Can Not Exceed Accrued Sick Days //
        //////////////////////////////////////
        if ($('#hlblOhAcctSelection')[0].innerHTML == "SICK" && $('#lblSick')[0].innerHTML - hours < 0) {
            $('#txtTime').addClass('ValidationError');
            $('#btnSubmit').hide();
            $('#lblErr')[0].innerHTML = 'Sick Days Exceeded.';
            return;
        }

        //////////////////////////////////////////
        // Can Not Exceed Accrued PHOLIDAY Days //
        //////////////////////////////////////////
        if ($('#hlblOhAcctSelection')[0].innerHTML == "PHOLIDAY" && $('#lblHol')[0].innerHTML - hours < 0) {
            $('#txtTime').addClass('ValidationError');
            $('#btnSubmit').hide();
            $('#lblErr')[0].innerHTML = 'Personal Holidays Exceeded.';
            return;
        }

        /////////////////////////////////////////
        // Can Not Exceed Accrued HOLIDAY Days //
        /////////////////////////////////////////
        if ($('#hlblOhAcctSelection')[0].innerHTML == "HOLIDAY" && hours > 8) {
            $('#txtTime').addClass('ValidationError');
            $('#btnSubmit').hide();
            $('#lblErr')[0].innerHTML = 'Not Permitted More Than 8 Hours Holiday A Day.';
            return;
        }

        //////////////////////////////////////////
        // Can Not Exceed Accrued VACATION Days //
        //////////////////////////////////////////
        if ($('#hlblOhAcctSelection')[0].innerHTML == "VACATION" && $('#lblVac')[0].innerHTML - hours < 0) {
            $('#txtTime').addClass('ValidationError');
            $('#btnSubmit').hide();
            $('#lblErr')[0].innerHTML = 'Vacation Hours Exceeded.';
            return;
        }


        ///////////////////////////////////////////////////
        // Not Permitted More Than 8 Hours Absence A Day //
        ///////////////////////////////////////////////////
        if ($('#chkAB')[0].checked == true && hours > 8) {
            $('#txtTime').addClass('ValidationError');
            $('#btnSubmit').hide();
            $('#lblErr')[0].innerHTML = 'Not Permitted More Than 8 Hours Absence A Day.';
            return;
        }

        ////////////////////////
        // Validation Success //
        ////////////////////////
        $('#txtTime').addClass('ValidationSuccess');
    }


    ////////////////////////
    // Clear / Reset Form //
    ////////////////////////
    $("#btnClear").click(function () {
        
        ////////////////////////////////////////
        // Clear Data Confirmation Containers //
        ////////////////////////////////////////        
        $('#lblJobNoSelection')[0].innerHTML = '';
        $('#lblOhAcctSelection')[0].innerHTML = '';
        $('#lblJobDescSelection')[0].innerHTML = '';
        $('#lblJobSiteSelection')[0].innerHTML = '';
        $('#lblDeptSelection')[0].innerHTML = '';
        $('#lblTaskCodeSelection')[0].innerHTML = '';
        $('#lblClassTimeSelection')[0].innerHTML = '';
        $('#lblClassLocSelection')[0].innerHTML = '';
        $('#lblClassInstrSelection')[0].innerHTML = '';
        $('#lblClassDescSelection')[0].innerHTML = '';

        //////////////////////////////
        // Clear Any Previous Error //
        //////////////////////////////
        $('#lblErr')[0].innerHTML = '';
        $('#lblErrServer')[0].innerHTML = '';

        ////////////////////////////////////////////
        // Clear Hidden Data Reference Containers //
        ////////////////////////////////////////////
        $('#hlblJobNoSelection')[0].innerHTML = '';
        $('#hlblOhAcctSelection')[0].innerHTML = '';
        $('#hlblTaskCodeSelection')[0].innerHTML = '';
        $('#hlblClassTimeSelection')[0].innerHTML = '';
        $('#hlblClassDescSelection')[0].innerHTML = '';
        $('#hlblClassLocSelection')[0].innerHTML = '';
        $('#hlblClassInstrSelection')[0].innerHTML = '';

        $('#hlblDeptSelection')[0].innerHTML = $('#hlblEmpDept')[0].innerHTML;


        /////////////////////////////////
        // Clear Class Data Text Areas //
        /////////////////////////////////
        $('#ddJobNo').val('');
        $('#ddOhAcct').val('');
        $('#ddDept').val('');
        $('#ddTaskNo').val('');

        $('#txtClassTime').val('');
        $('#txtClassLoc').val('');
        $('#TxtClassInstr').val('');
        $('#txtClassDesc').val('');

        ///////////////////////////////
        // Clear And Reset Time Data //
        // Text Areas                //
        ///////////////////////////////
        $('#txtTime').val('');
        $('#txtTime').removeClass("ValidationError").addClass("DataInputCss");

        ///////////////////////////
        // Disable Submit Button //
        ///////////////////////////
        //$('#btnSubmit').prop('disabled', true);
        $('#btnSubmit').hide();

        ////////////////////////////////////
        // Clear All CheckBoxes           //
        // Enable IsAllDefined CheckBoxes //
        // Default ST Checkbox            //
        ////////////////////////////////////
        $('input:checkbox').prop('checked', false);
        $('input:checkbox').prop('disabled', false);
        $('#chkST').prop('checked', true);

        /////////////////////////////////////////
        // Hide Extended Data Collection Areas //
        /////////////////////////////////////////
        $('#DepDiv').hide();
        $('#TaskDiv').hide();
        $('#TimeCheckBoxes').hide();
        $('#ClassDataCollectionDiv').hide();

        /////////////////////////////////////////
        // Reset(Show) O/H and Job Data Fields //
        /////////////////////////////////////////
        $('#OhAcctDiv').show('slow');
        $('#JobDiv').show('slow');



        //////////////////////////////////////
        // Send Focus To Job No Entry Field //
        //////////////////////////////////////
        $('#ddJobNo').focus();
    });


    $('#chkST').prop('checked', true);
    $('#TimeAck').hide();
});