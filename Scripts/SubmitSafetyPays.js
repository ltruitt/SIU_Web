$(document).ready(function () {

    ////////////////////////////////////////////////////////
    // Hide Specific Incident Type Data Collection Fields //
    // Default Box Is Safety Suggestion That Does Not     //
    // Collect This Data                                  //
    ////////////////////////////////////////////////////////
    $('#SafeActFields').hide();
    $('#MeetingTypeFields').hide();

    /////////////////////////////////////////////////////
    // Make Report Type Check Boxes Mutually Exclusive //
    /////////////////////////////////////////////////////
    mutex('chkbox');

    ///////////////////////////
    // Setup Calendar Fields //
    ///////////////////////////
    $('#SafetyMeetingDate').datepicker({
        constrainInput: true,
        onSelect: validate()
    });


    $('#IncidentDate').datepicker({
        constrainInput: true,
        onSelect: validate()
    });




    ////////////////////////////////////////
    // Event Management For Job Selection //
    ////////////////////////////////////////
    var listOfJobs = [];
    function getTimeJobsSuccess(data) {

        listOfJobs = data.d.split("\r");

        $("#JobNo").autocomplete({ source: listOfJobs },
        {
            matchContains: false,
            minChars: 1,
            autoFill: false,
            mustMatch: false,
            cacheLength: 20,
            max: 20,
            delay: 10,
            select: function (event, ui) {
                var DataPieces = ui.item.value.split(' ');
                $(this).val(DataPieces[0].replace(/\n/g, ""));
                getJobDetails();
                validate();
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var DataPieces = ui.content[0].value.split(' ');
                    $(this).val(DataPieces[0].replace(/\n/g, ""));
                    getJobDetails();
                    $("#JobNo").autocomplete("close");
                    validate();
                }
                else {
                    $('#lblJobSiteSelection')[0].innerHTML = '';
                    $('#lblJobDescSelection')[0].innerHTML = '';
                    $('#lblDeptSelection')[0].innerHTML = '';
                }
                

                return ui;
            }
        });
    }

    function getTimeJobs() {
        var GetTimeJobsCall = new AsyncServerMethod();
        GetTimeJobsCall.exec("/SIU_DAO.asmx/GetTimeJobs", getTimeJobsSuccess);
    }
    getTimeJobs();


    //////////////////////////////////////////////////////
    // Make Sure Text Entered For Jobs Was In Jobs List //
    //////////////////////////////////////////////////////
    $("#JobNo").blur(function () {
        if (!listOfJobs.containsCaseInsensitive(this.value)) {
            $(this).val("");
        }
    });


    function getTimeJobSuccess(data) {
        var jobDetails = $.parseJSON(data.d);

        $('#lblJobSiteSelection')[0].innerHTML = "<b>Customer:</b> " + jobDetails.JobCust + "<br/>";
        $('#lblJobDescSelection')[0].innerHTML = "<b>Job:</b> " + jobDetails.JobDesc + "<br/>";
        $('#lblDeptSelection')[0].innerHTML = "<b>Div/Dept:</b> " + jobDetails.JobDept; // +"<br/>"
   
    }
    function getJobDetails() {

        $('#hlblJobNo')[0].innerHTML = $('#JobNo').val();
        var getTimeJobCall = new AsyncServerMethod();
        getTimeJobCall.add('jobNo', $('#JobNo').val());
        getTimeJobCall.exec("/SIU_DAO.asmx/GetTimeJob", getTimeJobSuccess);
    }





    /////////////////////////////////////////////////////
    // Load List Of Employees into Observed Data Field //
    /////////////////////////////////////////////////////
    var listOfEmps = [];
    function getEmpsSuccess(data) {
        listOfEmps = data.d.split("\r");

        $("#ObservedEmpID").autocomplete({ source: listOfEmps },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: false,
                cacheLength: 20,
                max: 20,
                delay: 2,
                select: function (event, ui) {
                    var dataPieces = ui.item.value.split(' ');
                    $('#hlblObsEID')[0].innerHTML = dataPieces[0];
                    $("#ObservedEmpID").autocomplete("close");
                    $("#ObservedEmpID").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                    validate();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $('#hlblObsEID')[0].innerHTML = dataPieces[0];
                        $("#ObservedEmpID").autocomplete("close");
                        $("#ObservedEmpID").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                        validate();
                    }

                    return ui;
                }
            });
        
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
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var dataPieces = ui.content[0].value.split(' ');
                    $('#hlblEID')[0].innerHTML = dataPieces[0];
                    $("#ddEmpIds").autocomplete("close");
                    $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                }

                return ui;
            }
        });
    }


    // Load Emps AutoComplete List
    var getEmpsCall = new AsyncServerMethod();
    getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);










    
    //////////////////////////////////////////
    // Catch CheckBox Change Events         //
    // Show and Hide Data Collection Panels //
    // Based On Incident Type Checkboxes    //
    //////////////////////////////////////////
    $('input:checkbox').change(function () {

        // Hide Popup Data Collection Boxed
        $('#SafeActFields').hide();
        $('#MeetingTypeFields').hide();

        // Which Button Changed
        var chkboxName = this.name;
        var chkboxText = this.value;

        switch (chkboxText) {

            case 'I Saw Something Safe':
                $('#InitialResponseDiv').hide();       // Corrective Action Text Box
                $('#ObservedEmpIDDiv').show();         // Observed Employee
                $('#SafeActFields').show('slow');   // Safe Act Block
                $('#CommentLbl')[0].innerHTML = 'Safe Act Observed';
                break;

            case 'Unsafe Act':
                $('#InitialResponseDiv').show();       // Corrective Action Text Box
                $('#ObservedEmpIDDiv').hide();         // Observed Employee
                $('#SafeActFields').show('slow');   // Safe Act Block
                $('#CommentLbl')[0].innerHTML = 'Unsafe Act Observed';
                break;

            case 'Safety Suggestion':
                $('#CommentLbl')[0].innerHTML = 'Suggestion';
                break;


            case 'EHS Topic':
                $('#CommentLbl')[0].innerHTML = 'Topic';
                $('#SafetyMeetingDateDiv').hide();      // Meeting Date Text Box 
                $('#MeetingTypeFields').show('slow');   // Meeting Type Block
                break;

            case 'EHS Summary':
                $('#CommentLbl')[0].innerHTML = 'Summary';
                $('#SafetyMeetingDateDiv').show();      // Meeting Date Text Box
                $('#MeetingTypeFields').show('slow');   // Meeting Type Block
                break;

            default:
        }
    });


    ///////////////////////////////////////////////////////////////////
    // Expand / Contract Task Definition Field On Focus Enter / Exit //
    ///////////////////////////////////////////////////////////////////
    var refTaskDefinition = $("#TaskDefinition");

    refTaskDefinition.blur(function () {
        refTaskDefinition.height(18);
    });

    refTaskDefinition.focus(function () {
        refTaskDefinition.height(200);
    });
    
    NumericInput('RcdId');
    $('#RcdId').keyup(function () { validate(); });
    
    ///////////////////////////////////
    // CR Check For Job Number Field //
    ///////////////////////////////////
    $("#RcdId").keyup(function (e) {
        var keyId = (window.event) ? event.keyCode : e.keyCode;

        if (keyId == 13) {
            $("#btnSubmit").click();
        }
    });
    
    ////////////////////////////////////////////////////////
    // Check Form To See If All Data Fields Are Filled In //
    ////////////////////////////////////////////////////////
    function validate() {

        /////////////////////////////////////////////////////
        // If 
        /////////////////////////////////////////////////////
        if ($('#RcdId').val()) {
            if ($('#RcdId').val().length > 0) {
                $('#btnSubmit').show();
                return;
            }
        }
        
        /////////////////////////////////////////////////////
        // If 
        /////////////////////////////////////////////////////
        if ($('#JobSite')[0].value.length == 0) {
            $('#btnSubmit').hide();
            return;
        }
        
        if ($('#hlblObsEID').html() == $('#hlblEID').html() ) {
            alert('You can not observe yourself in a Safety Pays Report');
            $('#hlblObsEID').html('');
            $("#ObservedEmpID").val('');
            $('#btnSubmit').hide();
            return;
        }
            

        //////////////////////////////
        // Comments Must Be Present //
        //////////////////////////////
        //if ($('#Comments')[0].value.length == 0) {
        //    $('#btnSubmit').hide();
        //    return;
        //}
        
        ///////////////////////////////////////////////////
        // Make Sure Comments Has More Than Just A Space //
        ///////////////////////////////////////////////////
        var splessComments = $('#Comments')[0].value.replace(/\n/g, "").replace(/ /g, "");
        if (splessComments.length == 0) {
            $('#btnSubmit').hide();
            return;
        }
        
        //////////////////////////////////////////////
        // Get The Text Off the Checked Report Type //
        //////////////////////////////////////////////
        var chkboxText = $('input:checkbox[class=chkbox]:checked')[0].value;

        switch (chkboxText) {

            case 'I Saw Something Safe':
                if ($('#IncidentDate')[0].value.length == 0) {
                    $('#btnSubmit').hide();
                    return;
                }
                //if ($('#ObservedEmpID')[0].value.length == 0) {
                //    $('#btnSubmit').hide();
                //    return;
                //}
                break;

            case 'Unsafe Act':
                if ($('#IncidentDate')[0].value.length == 0) {
                    $('#btnSubmit').hide();
                    return;
                }
                break;

            case 'EHS Summary':
                if ($('#SafetyMeetingDate')[0].value.length == 0) {
                    $('#btnSubmit').hide();
                    return;
                }
                break;

            default:
        }
        

        $('#btnSubmit').show();
    }


    ///////////////////////////
    // Setup Validate Events //
    ///////////////////////////
    $('input:checkbox').change(function () {
        validate();
    });

    $('#JobSite').keyup(function () {
        validate();
    });

    $('#Comments').keyup(function () {
        validate();
    });

    $('#InitialResponse').keyup(function () {
        validate();
    });

    $('#SafetyMeetingDate').blur(function () {
        validate();
    });

    $('#IncidentDate').blur(function () {
        validate();
    });




    $("#btnSubmit").click(function () {

        //$('#btnSubmit').prop('disabled', true);
        $('#btnSubmit').hide();

        if ($('#RcdId').val()) {
            if ($('#RcdId').val().length > 0) {
                Clear();
                rcdLookup();
                $('#RcdId').val('');
                return;
            }
        }
        
        var safetyPaysSubmitCall = new AsyncServerMethod();
        safetyPaysSubmitCall.add('IncidentNo', $('#hlblIncidentNo').html());
        safetyPaysSubmitCall.add('EID', $('#hlblEID').html());
        safetyPaysSubmitCall.add('JobNo', $('#hlblJobNo').html());

        safetyPaysSubmitCall.add('IncTypeSafeFlag', $('#IncTypeSafeFlag')[0].checked);
        safetyPaysSubmitCall.add('IncTypeUnsafeFlag', $('#IncTypeUnsafeFlag')[0].checked);
        safetyPaysSubmitCall.add('IncTypeSuggFlag', $('#IncTypeSuggFlag')[0].checked);
        safetyPaysSubmitCall.add('IncTypeTopicFlag', $('#IncTypeTopicFlag')[0].checked);
        safetyPaysSubmitCall.add('IncTypeSumFlag', $('#IncTypeSumFlag')[0].checked);

        safetyPaysSubmitCall.add('IncidentDate', $('#IncidentDate').val());
        safetyPaysSubmitCall.add('ObservedEmpID', $('#hlblObsEID')[0].innerHTML);
        safetyPaysSubmitCall.add('InitialResponse',  encodeURIComponent( $('#InitialResponse').val())  );

        safetyPaysSubmitCall.add('SafetyMeetingType', $('#SafetyMeetingType').val());
        safetyPaysSubmitCall.add('SafetyMeetingDate', $('#SafetyMeetingDate').val());

        safetyPaysSubmitCall.add('Comments', encodeURIComponent( $('#Comments').val()) );

        safetyPaysSubmitCall.add('JobSite', $('#JobSite')[0].value);
        safetyPaysSubmitCall.add('IncTypeText', $('input:checkbox[class=chkbox]:checked')[0].value);
        
        safetyPaysSubmitCall.add('QomID', '');
        
        safetyPaysSubmitCall.exec("/SIU_DAO.asmx/RecordSafetyPays", safetyPaysSubmitCallSuccess);
    });


    function safetyPaysSubmitCallSuccess(data) {
        if (data.d.substring(0, 5) == "Error") {
            alert(data.d);
        } else {
            if (isMobile.any()) {
                if (!isMobile.iPad()) {
                    window.location = window.location.protocol + "//" + window.location.hostname + "/phone/homepage.aspx";
                    return;
                }
            }

            window.location = window.location.protocol + '//' + window.location.hostname + "/Safety/SafetyHome.aspx";
        }
    }

    function rcdLookup() {
        $('#hlblIncidentNo').html('0');
        var safetyPaysGetCall = new AsyncServerMethod();
        safetyPaysGetCall.add('RptID', $('#RcdId').val());
        safetyPaysGetCall.exec("/SIU_DAO.asmx/GetSafetyPays", safetyPaysGetCallSuccess);
    };
    
    function safetyPaysGetCallSuccess(data) {
        data = JSON.parse(data.d);
        if ( typeof data.JobNo === "undefined" )
            return;
        
        $('#hlblIncidentNo').html(data.IncidentNo);
        
        if ( data.JobNo.length > 0 )
            $("#JobNo").autocomplete("search", data.JobNo);

        $("#ddEmpIds").autocomplete("search", data.EmpID);
        
        $('#IncTypeSafeFlag')[0].checked = data.IncTypeSafeFlag;
        if ($('#IncTypeSafeFlag')[0].checked)
            $('#IncTypeSafeFlag').change();
        
        $('#IncTypeUnsafeFlag')[0].checked = data.IncTypeUnsafeFlag;
        if ($('#IncTypeUnsafeFlag')[0].checked)
            $('#IncTypeUnsafeFlag').change();
        
        $('#IncTypeSuggFlag')[0].checked = data.IncTypeSuggFlag;
        if ($('#IncTypeSuggFlag')[0].checked)
            $('#IncTypeSuggFlag').change();

        $('#IncTypeTopicFlag')[0].checked = data.IncTypeTopicFlag;
        if ($('#IncTypeTopicFlag')[0].checked)
            $('#IncTypeTopicFlag').change();

        $('#IncTypeSumFlag')[0].checked = data.IncTypeSumFlag;
        if ($('#IncTypeSumFlag')[0].checked)
            $('#IncTypeSumFlag').change();
        
        $('#IncidentDate').val('');
        if (data.IncidentDate)
            $('#IncidentDate').datepicker("setDate", $.datepicker.parseDate('mm-dd-yy', $.fn.dateTest(data.IncidentDate)));

        $('#InitialResponse').val(data.InitialResponse);

        if (data.ObservedEmpID.length > 0)
            $("#ObservedEmpID").autocomplete("search", data.ObservedEmpID);

        $('#SafetyMeetingType').val(data.SafetyMeetingType);
        $('#SafetyMeetingDate').val('');
        if (data.SafetyMeetingDate)
            $('#SafetyMeetingDate').datepicker("setDate", $.datepicker.parseDate('mm-dd-yy', $.fn.dateTest(data.SafetyMeetingDate)));
        
        $('#JobSite').val(data.JobSite);
        $('#Comments').val(data.Comments);
        validate();
    }
    
    $("#btnClear").click(function () {
        Clear();
    });

    function Clear() {

        //$('#btnSubmit').prop('disabled', true);
        $('#btnSubmit').hide();

        $('#SafeActFields').hide();
        $('#MeetingTypeFields').hide();

        $('#hlblIncidentNo').html('0');
        $('#hlblJobNo')[0].innerHTML = "";
        $('#hlblObsEID')[0].innerHTML = "";
        $('#IncTypeSuggFlag')[0].checked = true;
        $('#CommentLbl')[0].innerHTML = 'Suggestion';

        $('#IncTypeSafeFlag')[0].checked = false;
        $('#IncTypeUnsafeFlag')[0].checked = false;
        $('#IncTypeTopicFlag')[0].checked = false;
        $('#IncTypeSumFlag')[0].checked = false;


        $('#JobNo').val('');
        $('#JobSite').val('');
        
        $('#lblJobSiteSelection')[0].innerHTML = '';
        $('#lblJobDescSelection')[0].innerHTML = '';
        $('#lblDeptSelection')[0].innerHTML = '';
        
        $('#IncidentDate').val('');
        $('#ObservedEmpID').val('');
        $('#InitialResponse')[0].innerHTML = '';

        $('#SafetyMeetingType').val('Monday');
        $('#SafetyMeetingDate').val('');
        $('#Comments').val('');
    }
    

    $('#hlblIncidentNo').html('0');
    validate();
    

});
