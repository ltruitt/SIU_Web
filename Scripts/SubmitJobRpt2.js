$(document).ready(function () {

    var jobForm = "";
        
    $('#jobLoadingDiv').hide();
    
    //////////////////////////////////////////////////
    // Prevent Buttons From Firing Events On Server //
    //////////////////////////////////////////////////
    $("#SiteWrapper").submit(function (e) { e.preventDefault(); });


    /////////////////////////////////////////////
    // Setup Mutual Exclusion Data Check Boxes //
    /////////////////////////////////////////////
    mutex('chkRts');
    mutex('chkCt');
    mutex('chkPd');
    mutex('chkOs');
    mutex('chkOsLtr');
    mutex('chkRptDisp');
    mutex('chkRptFmt');
    mutex('chkDrBox');
    mutex('chkIR');
    mutex('chkIrDrBox');
    mutex('chkSales');
    mutex('chkIrData');
    
    /////////////////////////////////
    // Setup Numerical Input Masks //
    /////////////////////////////////
    $('#esd2DfsMailCnt').autoNumeric('init',    { vMin: '0', vMax: '11', mDec: '0' });
    $('#esd2DfsCdCnt').autoNumeric('init', { vMin: '0', vMax: '11', mDec: '0' });
    //$('#esd3DfsCnt').autoNumeric('init', { vMin: '0', vMax: '11', mDec: '0' });
    //$('#esd3CdCnt').autoNumeric('init', { vMin: '0', vMax: '11', mDec: '0' });
    $('#msd3HardCnt').autoNumeric('init', { vMin: '0', vMax: '11', mDec: '0' });
    $('#msd1HardCnt').autoNumeric('init', { vMin: '0', vMax: '11', mDec: '0' });
    


    ///////////////////
    // Setup Toggles //
    ///////////////////
    (function () {
        window.switcher = new AcidJs.ToggleSwitch();

        window.switcher.render({
            type: "checkbox",               // (String) "checkbox"|"radio" [optional] default: "checkbox"
            name: "Esd2Switches",              // (String) [required]
            on: "Yes",                      // (String) [optional] default: "on"
            off: "No",                      // (String) [optional] default: "off"
            defaultCheckedNode: [2,2],      // (Number for type "radio"|Array for type "checkbox") [optional] default: 0
            boundingBox: $("#DfsEmailSwitch"),
            
            items:
            [
                {
                    cssClasses: ["ToggleLi"],
                    value: "DfsEmail",
                    label: "<span id='SwitcherLabel_Esd2'>The final report is on dfs, Corporate Services needs to email it to the customer</span>" // (String) optional
                }
            ]
        });
        

        //window.switcher.render({
        //    type: "checkbox",               // (String) "checkbox"|"radio" [optional] default: "checkbox"
        //    name: "Esd3Switches",              // (String) [required]
        //    on: "Yes",                      // (String) [optional] default: "on"
        //    off: "No",                      // (String) [optional] default: "off"
        //    defaultCheckedNode: 2,      // (Number for type "radio"|Array for type "checkbox") [optional] default: 0
        //    boundingBox: $("#SendFinalSwitch"),

        //    items:
        //    [
        //        {
        //            cssClasses: ["ToggleLi"],
        //            value: "DfsEmail",
        //            label: "<span id='SwitcherLabel_Esd3'>Corporate Services needs to send final report to the customer</span>" // (String) optional
        //        }
        //    ]
        //});
        
        window.switcher.render({
            type: "checkbox",               // (String) "checkbox"|"radio" [optional] default: "checkbox"
            name: "Esd3Switches",              // (String) [required]
            on: "Yes",                      // (String) [optional] default: "on"
            off: "No",                      // (String) [optional] default: "off"
            defaultCheckedNode: 2,      // (Number for type "radio"|Array for type "checkbox") [optional] default: 0
            boundingBox: $("#Esd3Switches"),

            items:
            [
                {
                    cssClasses: ["ToggleLi"],
                    value: "EsdStdDrv",
                    label: "<span id='SwitcherLabel_Esd3'>Standard Drives & Automation Report Complete</span>" // (String) optional
                }
            ]
        });
        


        window.switcher.render({
            type: "checkbox",               // (String) "checkbox"|"radio" [optional] default: "checkbox"
            name: "Msd1Switches",              // (String) [required]
            on: "Yes",                      // (String) [optional] default: "on"
            off: "No",                      // (String) [optional] default: "off"
            defaultCheckedNode: 2,      // (Number for type "radio"|Array for type "checkbox") [optional] default: 0
            boundingBox: $("#Msd1Switches"),

            items:
            [
                {
                    cssClasses: ["ToggleLi"],
                    value: "MsdEmail",
                    label: "<span id='SwitcherLabel_Msd1'>Corporate Services to email the final report to the customer. The final report is in Navision</span>" // (String) optional
                }
            ]
        });
        
        window.switcher.render({
            type: "checkbox",               // (String) "checkbox"|"radio" [optional] default: "checkbox"
            name: "Msd2Switches",              // (String) [required]
            on: "Yes",                      // (String) [optional] default: "on"
            off: "No",                      // (String) [optional] default: "off"
            defaultCheckedNode: 2,      // (Number for type "radio"|Array for type "checkbox") [optional] default: 0
            boundingBox: $("#Msd2Switches"),

            items:
            [
                {
                    cssClasses: ["hide"],
                    value: "MsdInitial",
                    label: "<span id='SwitcherLabel_Msd2'>Initial Inspection email will be sent to unknown </span>" // (String) optional
                },
                {
                    cssClasses: ["ToggleLi"],
                    value: "MsdComplete",
                    label: "<span id='SwitcherLabel_Msd21'>Complete: All data is in PowerDB.  Corporate Services to send.</span>" // (String) optional
                }
            ]
        });
        
        window.switcher.render({
            type: "checkbox",               // (String) "checkbox"|"radio" [optional] default: "checkbox"
            name: "Msd3Switches",              // (String) [required]
            on: "Yes",                      // (String) [optional] default: "on"
            off: "No",                      // (String) [optional] default: "off"
            defaultCheckedNode: 2,      // (Number for type "radio"|Array for type "checkbox") [optional] default: 0
            boundingBox: $("#Msd3Switches"),

            items:
            [
                {
                    cssClasses: ["ToggleLi"],
                    value: "MsdFolderEmail",
                    label: "<span id='SwitcherLabel_Msd31'>Complete.  The report is ready to be emailed to the customer.  It is located in the job folder.</span>" // (String) optional
                }
            ]
        });
        
        window.switcher.render({
            type: "checkbox",               // (String) "checkbox"|"radio" [optional] default: "checkbox"
            name: "Msd4Switches",              // (String) [required]
            on: "Yes",                      // (String) [optional] default: "on"
            off: "No",                      // (String) [optional] default: "off"
            defaultCheckedNode: 2,      // (Number for type "radio"|Array for type "checkbox") [optional] default: 0
            boundingBox: $("#Msd4Switches"),

            items:
            [
                {
                    cssClasses: ["ToggleLi"],
                    value: "MsdPdb",
                    label: "<span id='SwitcherLabel_Msd41'>PowerDB data is synched.  Ready for final report.</span>" // (String) optional
                },
                {
                    cssClasses: ["ToggleLi"],
                    value: "MsdJobFolder",
                    label: "<span id='SwitcherLabel_Msd42'>Data is saved in the job folder</span>" // (String) optional
                }
            ]
        });
        
    })();
    
    ///////////////////////
    // Setup Date Fields //
    ///////////////////////
    $('#dfsDelivDate').datepicker({
        constrainInput: true
    });
    
    $('#msd4date').datepicker({
        constrainInput: true
    });
    
    $('#msd3date').datepicker({
        constrainInput: true
    });
    

    ///////////////////////////////////
    // Catch CheckBox Change Events  //
    // For Toggle Switches           //
    ///////////////////////////////////
    $('.ToggleLi').change(function () {
        validate();
    });

    $('.CntFld').keyup(function () {
        validate();
    });

    ///////////////////////////////////
    // Catch CheckBox Change Events  //
    // For Rpt Disposition           //
    ///////////////////////////////////
    $('.chkRptDisp').change(function () {
        $('#lblRptDisp')[0].innerHTML = "<span class='FixedLabel'>Rpt Disp:</span>" + this.value + "<br/><br/>";
    });

    
    ////////////////////////////////////////
    // Disable Form If No-Report-Required //
    ////////////////////////////////////////
    $("#txtNoRpt").blur(function () {

        if ($('#txtNoRpt').val().length == 0) {
            //$('input:checkbox').attr('disabled', false);
            //$('#lblRptDisp')[0].innerHTML = "";

            ////////////////////////////////////////////////////////
            // Check Box Group CbOsLtr Only Allows N/A by Default //
            ////////////////////////////////////////////////////////
            $('#CbOsLtrY').attr('disabled', true);
            $('#CbOsLtrN').attr('disabled', true);
            $('.chkOsLtr').attr('checked', false);
            $('#CbOsLtrNA')[0].checked = true;

            return;
        }

        //$('#lblRptDisp')[0].innerHTML = "<span class='FixedLabel'>Rpt Disp:</span>" + "No Report" + "<br/>";

        $('input:checkbox').attr('checked', false);
        //$('input:checkbox').attr('disabled', true);


    });

    //////////////////////////////////////////////////
    // Disable All Form Fields If No-Report Entered //
    //////////////////////////////////////////////////
    $(".NoRpt").keyup(function () {
        if (this.value.length == 0) {
            $('input:checkbox').attr('disabled', false);
            $('input[type=text], textarea').attr('disabled', false);
        } else {
            $('input:checkbox').attr('disabled', true);
            $('input[type=text], textarea').attr('disabled', true);
            $('#lblRptDisp')[0].innerHTML = "<span class='FixedLabel'>Rpt Disp:</span>" + "No Report" + "<br/>";
            $(this).attr('disabled', false);
            $('#txtComments').attr('disabled', false);
            $(this).focus();
        }
    });


    ///////////////////////////////////
    // Catch CheckBox Change Events  //
    // For List Of Submitted Reports //
    ///////////////////////////////////
    $('.chkOs').change(function () {
        if ($('#CbOsY')[0].checked == true) {
            $('#CbOsLtrY').attr('disabled', false);
            $('#CbOsLtrN').attr('disabled', false);
        } else {
            ////////////////////////////////////////////////////////
            // Check Box Group CbOsLtr Only Allows N/A by Default //
            ////////////////////////////////////////////////////////
            $('#CbOsLtrY').attr('disabled', true);
            $('#CbOsLtrN').attr('disabled', true);
            $('.chkOsLtr').attr('checked', false);
            $('#CbOsLtrNA')[0].checked = true;
        }
    });


    ///////////////////////////////////
    // Catch CheckBox Change Events  //
    // For No Testing Was Done       //
    ///////////////////////////////////
    $('#CbTestNone').change(function () {
        if ($('#CbTestNone')[0].checked == true) {
            $('.ChkTest').attr('disabled', true);
            $('.ChkTest').attr('checked', false);
        }
        else {
            $('.ChkTest').attr('disabled', false);
        }
    });


    ////////////////////////////////////////////////////////////////////////
    // Bind To The OtherData Field And Enable / Disable The Submit Button //
    ////////////////////////////////////////////////////////////////////////
   
    $('.NoRpt').bind('hastext', function () {
        validate();
    });
    $('.NoRpt').bind('notext', function () {
        validate();
    });


    //////////////////////////////////////////////////////////////////////
    // Leaving Job No Field, Fire A Query To Check If Job Number Exists //
    //////////////////////////////////////////////////////////////////////
    $("#ddJobNo").blur(function () {
        if ($('#ddJobNo').val() != "") {
            $('#JobDiv').hide();
            getJobDetails();
        }
    });

    ///////////////////////////////////
    // CR Check For Job Number Field //
    ///////////////////////////////////
    $("#ddJobNo").keyup(function (e) {
        var keyId = (window.event) ? event.keyCode : e.keyCode;

        if (keyId == 13) {
            $("#ddJobNo").blur();
        }
    });

    ////////////////////////////////////
    // Lookup Previously Entered Data //
    ////////////////////////////////////
    function getJobDetails() {
        var job = $('#ddJobNo').val();
        $('#JobNoLookup').html(job);
        $('#jobLoadingDiv').show();
        
        if (job == "") return;

        var getTimeJobAjax = new AsyncServerMethod();
        getTimeJobAjax.add('jobNo', job);
        getTimeJobAjax.exec("/SIU_DAO.asmx/Gffeop1", getJobSuccess, getJobFailure);


    }

    ////////////////////////////////////////////
    // If the Request For A Job Number Failed //
    ////////////////////////////////////////////
    function getJobFailure() {
        $('#jobLoadingDiv').hide();

        $('#JobDiv').val("");
        $('#JobDiv').show('slow');
        $("#ddJobNo").val("");
        $("#ddJobNo").focus();
    }


    /////////////////////////////////////////
    // Get Details Of A Job And Job Report //
    /////////////////////////////////////////
    var dateTimeReviver;

    function getJobSuccess(data) {

        var jobDetails = JSON.parse(data.d, dateTimeReviver);

        if (jobDetails == null) {
            clearForm();
            $('#NoJobError').show();
            $('#jobLoadingDiv').hide();
            return;
        }
        
        var getSubmitJobReportByNoAjax = new AsyncServerMethod();
        getSubmitJobReportByNoAjax.add('jobNo', jobDetails.JobNo);
        getSubmitJobReportByNoAjax.exec("/SIU_DAO.asmx/Gffeop2", getSubmitJobReportByNoSuccess, getSubmitJobReportByNoFail);


        $('#hlblJobNo')[0].innerHTML = jobDetails.JobNo;

        $('#lblJobDesc')[0].innerHTML = "<span class='FixedLabel'>Description:</span><span class='FixedData'>" + jobDetails.JobDesc + "</span><br/>";
        $('#lblJobNo')[0].innerHTML = "<span class='FixedLabel'>Job No:</span>" + jobDetails.JobNo + "<br/>";
        $('#lblJobStatus')[0].innerHTML = "<span class='FixedLabel'>Status:</span>" + jobDetails.JobStatus + "<br/>";

        $('#lblJobSite')[0].innerHTML = "<span class='FixedLabel'>Customer:</span>" + jobDetails.JobCust + "<br/>";
        $('#lblDept')[0].innerHTML = "<span class='FixedLabel'>Div/Dept:</span>" + jobDetails.JobDept + "<br/>";
        $('#lblTech')[0].innerHTML = "<span class='FixedLabel'>Lead Tech:</span>" + jobDetails.LeadTech + "<br/>";

        $('#lblCost')[0].innerHTML = "<span class='FixedLabel'>Cost Date</span>" + jobDetails.JobCostDate + '<br/>';

        $('#Comments').show('slow');

        validate();
    }


    //////////////////////////////////////////////
    // DateTime Formatter passed To Json Parser //
    //////////////////////////////////////////////
    dateTimeReviver = function (key, value) {
        var a;
        if (typeof value === 'string') {
            a = /\/Date\((\d*)\)\//.exec(value);
            if (a) {
                return dateFormat(new Date(+a[1]), 'mm/dd/yy');
            }
            a = /\/Date/.exec(value);
            if (a != null) {
                return '--/--/--';
            }
        }
        return value;
    };


    function getSubmitJobReportByNoFail() { }

    //////////////////////////////////////////////////////////////////
    // Response To Request For Previuosly Submitted Job Report Data //
    //////////////////////////////////////////////////////////////////
    function getSubmitJobReportByNoSuccess(data) {
        
        $('#jobLoadingDiv').hide();
        
        var jobResponse = JSON.parse(data.d, dateTimeReviver);
        var jobDetails = jobResponse.Rpt;
        var jobExtData = jobResponse.ExtData;
        jobForm = jobResponse.Form;
        

        if ($('#hlblJobNo').html().length == 0) return;
        
        
        if (jobForm != null) {

            switch (jobForm) {
                case 'ESD1':
                    esd1Data(jobDetails, jobExtData);
                    $('#tabsESD1').show('slow');
                    break;

                case 'ESD2':
                    esd2Data(jobDetails, jobExtData);
                    $('#tabsESD2').show('slow');
                    break;

                case 'ESD3':
                    esd3Data(jobDetails, jobExtData);
                    $('#tabsESD3').show('slow');
                    break;

                case 'MSD1':
                    msd1Data(jobDetails, jobExtData);
                    $('#tabsMSD1').show('slow');
                    break;

                case 'MSD2':
                    msd2Data(jobDetails, jobExtData);
                    $('#tabsMSD2').show('slow');
                    break;

                case 'MSD3':
                    msd3Data(jobDetails, jobExtData);
                    $('#tabsMSD3').show('slow');                   
                    break;

                case 'MSD4':
                    msd4Data(jobDetails, jobExtData);
                    $('#tabsMSD4').show('slow');
                    break;

                default:
                    esd1Data(jobDetails, jobExtData);
                    $('#tabsESD1').show('slow');
            }
        }
        

        sidebarData(jobDetails);

    }
    
    ///////////////////////////////////////
    // Functions For Re-Hydrating Forms  //
    // This would have better been a SPA //
    ///////////////////////////////////////
    function sidebarData(jobDetails) {
        
        if (jobDetails == null)
            return;
        
        /////////////////////////////
        // Sidebar Data Population //
        /////////////////////////////
        // http://blog.stevenlevithan.com/archives/date-time-format
        $('#lblSubmitDate')[0].innerHTML = "<span class='FixedLabel'>Submit</span>" + jobDetails.Turned_in_by_Tech_Date + '<br/>';
        $('#lblJhaDate')[0].innerHTML = "<span class='FixedLabel'>JHA Submit</span>" + jobDetails.JHA_Submitted_Date + '<br/>';
        $('#lblIrSubmitDate')[0].innerHTML = "<span class='FixedLabel'>IR Submit</span>" + jobDetails.IR_Received_From_Tech + '<br/>';
        $('#lblIrCompleteDate')[0].innerHTML = "<span class='FixedLabel'>IR Complete</span>" + jobDetails.IR_Complete_and_Delivered + '<br/>';
        $('#lblLoginDate')[0].innerHTML = "<span class='FixedLabel'>Logged Date</span>" + jobDetails.Logged_and_Scanned + '<br/>';
        $('#lblDataEntryDate')[0].innerHTML = "<span class='FixedLabel'>Data Entry</span>" + jobDetails.Start_Data_Sheet_Entry + '<br/>';
        $('#lblProofDate')[0].innerHTML = "<span class='FixedLabel'>Proof</span>" + jobDetails.Start_Proofread + '<br/>';
        $('#lblCorrectDate')[0].innerHTML = "<span class='FixedLabel'>Corrected</span>" + jobDetails.Received_for_Corrections + '<br/>';
        $('#lblReviewDate')[0].innerHTML = "<span class='FixedLabel'>Review</span>" + jobDetails.Tech_Review_Completed + '<br/>';
        $('#lblReadyDate')[0].innerHTML = "<span class='FixedLabel'>Delivered</span>" + jobDetails.Complete_and_Delivered + '<br/>';


        $("#SiteWrapper").data("changed", false);
        validate();
    }
    function esd1Data(jobDetails, jobExtData) {
        $('#saleNotesDiv').hide();

        ///////////////////////////////////
        // Catch CheckBox Change Events  //
        // For Rpt Disposition           //
        ///////////////////////////////////
        $('.chkSales').change(function () {
            if ($('#chkSalesY')[0].checked)
                $('#saleNotesDiv').show('slow');
            else
                $('#saleNotesDiv').hide();
        });

        if ($('#chkIrDataY')[0].checked) {
            $('#irDataDiv').show('slow');
            $('#irQ').hide();
        }
        else {
            $('#irDataDiv').hide();
            $('#irQ').show('slow');
        }

        ///////////////////////////////////
        // Catch CheckBox Change Events  //
        // For Rpt Disposition           //
        ///////////////////////////////////
        $('.chkIrData').change(function () {
            if ($('#chkIrDataY')[0].checked) {
                $('#irDataDiv').show('slow');
                $('#irQ').hide();
            }
            else {
                $('#irDataDiv').hide();
                $('#irQ').show('slow');
            }
        });
        
        if (jobDetails == null) 
            return;

        /////////////////////////////////
        // Comments Box Reconstruction //
        /////////////////////////////////
        $('#txtComments')[0].value = jobDetails.Comment;

        /////////////////////////////////////
        // Disposition Page Reconstruction //
        /////////////////////////////////////
        $('#txtNoRpt')[0].value = jobDetails.No_Report_Required_Reason;
        if ($('#txtNoRpt')[0].value.length > 0) {
            $('#lblRptDisp')[0].innerHTML = "<span class='FixedLabel'>Rpt Disp:</span>" + "No Report" + "<br/>";

            $('input:checkbox').attr('checked', false);
            $('input:checkbox').attr('disabled', true);
        }


        $('#chkComplete')[0].checked = jobDetails.TmpComplete;
        $('#chkPartial')[0].checked = jobDetails.TmpPartial;
        $('#chkPe')[0].checked = jobDetails.TmpPE;

        if (jobDetails.Turned_in_by_Tech_Date != '--/--/--') {
            if (jobDetails.General_Dropbox == 1)
                $('#RptDrBoxY')[0].checked = true;
            if (jobDetails.General_Dropbox_No == 1)
                $('#RptDrBoxN')[0].checked = true;
        }

        ////////////////////////////////
        // Format Page Reconstruction //
        ////////////////////////////////
        switch (jobDetails.Report_Data_Format) {
            case 0:
                // No Selection
                break;
            case 1:
                $('#chkNoData')[0].checked = true;
                break;
            case 2:
                $('#chkPowerDB')[0].checked = true;
                break;
            case 3:
                $('#chkScanned')[0].checked = true;
                break;
            case 4:
                $('#chkPdbMaster')[0].checked = true;
                break;
            case 5:
                $('#chkOtherData')[0].checked = true;
                break;
        }

        ////////////////////////////////////
        // Other Data Page Reconstruction //
        ////////////////////////////////////
        switch (jobDetails.RTS_Relay_Data) {
            case 1:
                $('#CbRtsNA')[0].checked = false;
                $('#CbRtsY')[0].checked = true;
                break;
            case 2:
                $('#CbRtsNA')[0].checked = false;
                $('#CbRtsN')[0].checked = true;
                break;
            case 3:
                $('#CbRtsNA')[0].checked = true;
                break;
        }

        switch (jobDetails.CT_Data_Saved) {
            case 1:
                $('#CbCtNA')[0].checked = false;
                $('#CbCtY')[0].checked = true;
                break;
            case 2:
                $('#CbCtNA')[0].checked = false;
                $('#CbCtN')[0].checked = true;
                break;
            case 3:
                $('#CbCtNA')[0].checked = true;
                break;
        }

        switch (jobDetails.Partial_Discharge) {
            case 1:
                $('#CbPdNA')[0].checked = false;
                $('#CbPdY')[0].checked = true;
                break;
            case 2:
                $('#CbPdNA')[0].checked = false;
                $('#CbPdN')[0].checked = true;
                break;
            case 3:
                $('#CbPdNA')[0].checked = true;
                break;
        }

        switch (jobDetails.Oil_Sample) {
            case 1:
                $('#CbOsNA')[0].checked = false;
                $('#CbOsY')[0].checked = true;
                break;
            case 2:
                $('#CbOsNA')[0].checked = false;
                $('#CbOsN')[0].checked = true;
                break;
            case 3:
                $('#CbOsNA')[0].checked = true;
                break;
        }


        switch (jobDetails.Oil_Sample_Follow_UP) {
            case 1:
                $('#CbOsLtrNA')[0].checked = false;
                $('#CbOsLtrY')[0].checked = true;
                break;
            case 2:
                $('#CbOsLtrNA')[0].checked = false;
                $('#CbOsLtrN')[0].checked = true;
                break;
            case 3:
                $('#CbOsLtrNA')[0].checked = true;
                break;
        }

        $('#OtherData')[0].value = jobDetails.TmpOtherText;

        if ($('#CbOsY')[0].checked == true) {
            $('#CbOsLtrY').attr('disabled', false);
            $('#CbOsLtrN').attr('disabled', false);
            $('#CbOsLtrNA')[0].checked = false;
        } else {
            ////////////////////////////////////////////////////////
            // Check Box Group CbOsLtr Only Allows N/A by Default //
            ////////////////////////////////////////////////////////
            $('#CbOsLtrY').attr('disabled', true);
            $('#CbOsLtrN').attr('disabled', true);
            $('.chkOsLtr').attr('checked', false);
            $('#CbOsLtrNA')[0].checked = true;
        }

        ///////////////////////////////////////////
        // Test Descriptions Page Reconstruction //
        ///////////////////////////////////////////
        $('#CbTestSonic')[0].checked = jobDetails.Ultrasonic_Testing;
        $('#CbTestTTR')[0].checked = jobDetails.TTR;
        $('#CbTestThermo')[0].checked = jobDetails.Thermograpic_IR_;
        $('#CbTestRelay')[0].checked = jobDetails.Protective_Relays;
        $('#CbTestPCB')[0].checked = jobDetails.PCB_Info;
        $('#CbTestPD')[0].checked = jobDetails.Partial_Discharge_B;
        $('#CbTestOil')[0].checked = jobDetails.Oil_Tests;
        $('#CbTestNFPA')[0].checked = jobDetails.NFPA - 99;
        $('#CbTestInslResit')[0].checked = jobDetails.Insulation_Resistance;
        $('#CbTestGrdEltrode')[0].checked = jobDetails.Grounding___Ground_Electrode;
        $('#CbTestGrdFlt')[0].checked = jobDetails.Ground_Fault_Systems;
        $('#CbTestDoble')[0].checked = jobDetails.Doble;
        $('#CbTestDLRO')[0].checked = jobDetails.DLRO;
        $('#CbTestDecal')[0].checked = jobDetails.Decal_Color_Codes;
        $('#CbTestHiPot')[0].checked = jobDetails.DC_Hipot;
        $('#CbTestBBT')[0].checked = jobDetails.Bus_Bolt_Torque;
        $('#CbTestNone')[0].checked = jobDetails.No_Testing_Done;


        ////////////////////////////
        // IR Page Reconstruction //
        ////////////////////////////
        if (jobDetails.Turned_in_by_Tech_Date != '--/--/--') {
            if (jobDetails.TmpIRData == 1) {
                $('#IrDrpBoxY')[0].checked = true;
                $('#chkIrDataY')[0].checked = true;
            }

            if (jobDetails.TmpIRData_No == 1) {
                $('#IrDrpBoxN')[0].checked = true;
                $('#chkIrDataY')[0].checked = true;
            }
        }

        $('#chkIrOnly')[0].checked = jobDetails.IROnly;
        $('#chkIrPort')[0].checked = jobDetails.IRonFinalReport;
        if ($('#chkIrOnly')[0].checked)
            $('#chkIrDataY')[0].checked = true;
        if ($('#chkIrPort')[0].checked)
            $('#chkIrDataY')[0].checked = true;

        $('#txtIrHardCnt').val(jobDetails.No__of_Copies);
        $('#txtAddEmail').val(jobDetails.Email);

        ///////////////////////////////
        // Sales Page Reconstruction //
        ///////////////////////////////
        if (jobDetails.SalesFollowUp == 1) {
            $('#chkSalesY')[0].checked = true;
            $('#saleNotesDiv').show();
        }
        else {
            $('#chkSalesN')[0].checked = true;
            $('#saleNotesDiv').hide();
        }

        $('#txtSalesNotes')[0].value = jobDetails.SalesFollowUp_Comment;
    }
    function esd2Data(jobDetails, jobExtData) {
        $('#saleNotesDiv2').hide();
        
        ///////////////////////////////////
        // Catch CheckBox Change Events  //
        // For Rpt Disposition           //
        ///////////////////////////////////
        $('.chkSales').change(function () {
            if ($('#chkSalesY2')[0].checked)
                $('#saleNotesDiv2').show('slow');
            else
                $('#saleNotesDiv2').hide();
        });
        
        if (jobDetails != null) {
            ///////////////////////////////
            // Sales Page Reconstruction //
            ///////////////////////////////
            if (jobDetails.SalesFollowUp == 1) {
                $('#chkSalesY2')[0].checked = true;
                $('#saleNotesDiv2').show();
            }
            else {
                $('#chkSalesN2')[0].checked = true;
                $('#saleNotesDiv2').hide();
            }

            $('#txtSalesNotes2')[0].value = jobDetails.SalesFollowUp_Comment;

            /////////////////////////////////
            // Comments Box Reconstruction //
            /////////////////////////////////
            $('#txtComments').val(jobDetails.Comment);
        }
        

        if (jobExtData == null)
            return;
        
        if (jobExtData.RD_GaveToCustDate != null) {
            $('#dfsDelivDate').datepicker("setDate", jobExtData.RD_GaveToCustDate);
        }
        
        if (jobExtData.RD_RptInDFS == 1) {
            $('input:checkbox[value=DfsEmail]')[0].checked = true;
        }
        
        if (jobExtData.RD_PlsMailCnt > 0) {
            $('#esd2DfsMailCnt').val(jobExtData.RD_PlsMailCnt);
        }
        
        if (jobExtData.RD_PlsMailCdCnt > 0) {
            $('#esd2DfsCdCnt').val(jobExtData.RD_PlsMailCdCnt);
        }
        
        $('#esd2Other').val(jobExtData.RD_GaveOther);
        $('#esd2NoRpt').val(jobDetails.No_Report_Required_Reason);


    }
    function esd3Data(jobDetails, jobExtData) {

        if (jobDetails != null) {
            /////////////////////////////////
            // Comments Box Reconstruction //
            /////////////////////////////////
            $('#txtComments')[0].value = jobDetails.Comment;
        }

        if (jobExtData == null)
            return;
        
        if (jobExtData.RD_StdDrvAutoRptDone == 1) {
            $('input:checkbox[value=EsdStdDrv]')[0].checked = true;
        }

    }
    function msd1Data(jobDetails, jobExtData) {

        if (jobDetails != null) {
            /////////////////////////////////
            // Comments Box Reconstruction //
            /////////////////////////////////
            $('#txtComments')[0].value = jobDetails.Comment;
        }

        if (jobExtData == null)
            return ;

        if (jobExtData.RD_PlsEmail == 1) {
            $('input:checkbox[value=MsdEmail]')[0].checked = true;
        }

        if (jobExtData.RD_PlsMailCnt > 0) {
            $('#msd1HardCnt').val(jobExtData.RD_PlsMailCnt);
        }
    }
    function msd2Data(jobDetails, jobExtData) {

        $('#saleNotesDiv5').hide();
        
        ///////////////////////////////////
        // Catch CheckBox Change Events  //
        // For Rpt Disposition           //
        ///////////////////////////////////
        $('.chkSales').change(function () {
            if ($('#chkSalesY5')[0].checked)
                $('#saleNotesDiv5').show('slow');
            else
                $('#saleNotesDiv5').hide();
        });
        
        if (jobDetails != null) {
            ///////////////////////////////
            // Sales Page Reconstruction //
            ///////////////////////////////
            if (jobDetails.SalesFollowUp == 1) {
                $('#chkSalesY5')[0].checked = true;
                $('#saleNotesDiv5').show();
            }
            else {
                $('#chkSalesN5')[0].checked = true;
                $('#saleNotesDiv5').hide();
            }

            $('#txtSalesNotes5')[0].value = jobDetails.SalesFollowUp_Comment;

            /////////////////////////////////
            // Comments Box Reconstruction //
            /////////////////////////////////
            $('#txtComments').val(jobDetails.Comment);
        }
        

        if (jobExtData == null)
            return;
        
        if (jobExtData.RD_InitInsp == 1) {
            $('input:checkbox[value=MsdInitial]')[0].checked = true;
        }

        if (jobExtData.RD_RptInPdb == 1) {
            $('input:checkbox[value=MsdComplete]')[0].checked = true;
        }

        $('#msd2NoRpt').val(jobDetails.No_Report_Required_Reason);
        

    }
    function msd3Data(jobDetails, jobExtData) {

        if (jobDetails != null) {
            /////////////////////////////////
            // Comments Box Reconstruction //
            /////////////////////////////////
            $('#txtComments')[0].value = jobDetails.Comment;
        }

        if (jobExtData == null)
            return;
        
        if (jobExtData.RD_RptInJobFolder == 1) {
            $('input:checkbox[value=MsdFolderEmail]')[0].checked = true;
        }


        if (jobExtData.RD_PrioritySendByDate != null) {
            $('#msd3date').datepicker("setDate", jobExtData.RD_PrioritySendByDate);
        }

        if (jobExtData.RD_PlsMailCnt > 0) {
            $('#msd3HardCnt').val(jobExtData.RD_PlsMailCnt);
        }

        $('#msd3NoRpt').val(jobDetails.No_Report_Required_Reason);
    }
    function msd4Data(jobDetails, jobExtData) {

        if (jobDetails != null) {
            /////////////////////////////////
            // Comments Box Reconstruction //
            /////////////////////////////////
            $('#txtComments')[0].value = jobDetails.Comment;
        }

        if (jobExtData == null)
            return;
        
        if (jobExtData.RD_RptInPdb == 1) {
            $('input:checkbox[value=MsdPdb]')[0].checked = true;
        }
        
        if (jobExtData.RD_RptInJobFolder == 1) {
            $('input:checkbox[value=MsdJobFolder]')[0].checked = true;
        }
        
        if (jobExtData.RD_PrioritySendByDate != null) {
            $('#msd4date').datepicker("setDate", jobExtData.RD_PrioritySendByDate);
        }
        
        $('#msd4NoRpt').val(jobDetails.No_Report_Required_Reason);
    }



    /////////////////////////////////
    // Catch Page Exit And Confirm //
    /////////////////////////////////
    $("#SiteWrapper :input").change(function () {
        $("#SiteWrapper").data("changed", true);
        window.onbeforeunload = confirmPageExit;
        validate();
    });
    var confirmPageExit = function (e) {

        if ($("#SiteWrapper").isChanged()) {
            // If event not passed in, get it
            e = e || window.event;

            var message = 'You have unsaved changes.  Do you want to abondon these changes?';

            return message;
        }

    };

    ///////////////////////////////////////////
    // Setup Validation Event On Sales Notes //
    ///////////////////////////////////////////
    $('#txtSalesNotes').keyup(function () {
        validate();
    });

    ///////////////////////////
    // Data Validation Rules //
    ///////////////////////////
    function validate() {

        $('#NotReady').html('( Not Ready To Submit )');

        if ($("#SiteWrapper").isChanged()) {
            $('#NotReady').html('( Changes Not Ready To Submit )');
        }

        //1
        ////////////////////////////////////////////
        // If Email Address Given -- Check Format //
        ////////////////////////////////////////////
        if ($('#txtAddEmail')[0].value.length > 0) {

            if (!isValidEmailAddress($('#txtAddEmail').val())) {
                $('#txtAddEmail').css('background-color', 'red');
                $('#btnSubmit').hide();
                return;
            } else {
                $('#txtAddEmail').css('background-color', 'rgb(204,204,221)');
            }
        } else {
            $('#txtAddEmail').css('background-color', 'rgb(204,204,221)');
        }

        //2
        /////////////////////////////////////////////////////
        // If No Report Is Checked, Thats All Thats Needed //
        /////////////////////////////////////////////////////
        if ($('#txtNoRpt')[0].value.length > 0) {
            $('#btnSubmit').show();
            return;
        }
        

        //2.5A
        ////////////////////////////////////////////////
        // IF Any Of THe No Report Required Textboxes //
        // are filled in, were good to go             //
        ////////////////////////////////////////////////
        if ($('#msd4NoRpt')[0].value.length > 0) {
            $('#btnSubmit').show();
            return;
        }

        if ($('#msd3NoRpt')[0].value.length > 0) {
            $('#btnSubmit').show();
            return;
        }
        if ($('#msd2NoRpt')[0].value.length > 0) {
            $('#btnSubmit').show();
            return;
        }
        
        if ($('#esd2NoRpt')[0].value.length > 0) {
            $('#btnSubmit').show();
            return;
        }
        
        //2.5B
        /////////////////////////////////////////
        // IF The ESD2 Report Sent "Other" Way //
        // is filled in, were good to go       //
        /////////////////////////////////////////
        if ($('#esd2Other')[0].value.length > 0) {
            $('#btnSubmit').show();
            return;
        }
        
        //2.5C
        //////////////////////////////////////////////////
        // IF Any Of THe Checkboxes on any of the Depts //
        // (except ESD1) are checked, were good to go   //
        //////////////////////////////////////////////////
        if ($('input:checkbox[name*=Switch]:checked').length > 0) {
            $('#btnSubmit').show();
            return;
        }
        
        //2.5D
        ///////////////////////////////////////////////////
        // IF Any Of THe Date Fields on any of the Depts //
        // (except ESD1) are checked, were good to go    //
        ///////////////////////////////////////////////////
        var passDate = false;
        $('.hasDatepicker').each(function () {
            if (this.value.length > 0) {
                passDate = true;
                return false;
            }
        });
        if (passDate) {
            $('#btnSubmit').show();
            return;
        }

        //2.5E
        ////////////////////////////////////////////////////
        // IF Any Of The Count Fields on any of the Depts //
        // (except ESD1) are checked, were good to go     //
        ////////////////////////////////////////////////////
        var passCnt = false;
        $('.CntFld').each(function () {
            if (this.value.length > 0) {
                passCnt = true;
                return false;
            }
        });
        if (passCnt) {
            $('#btnSubmit').show();
            return;
        }
        

        //3
        ///////////////////////////////////////////////////////////////////
        // If Sales Contact Request Checked, Ensure Sales Notes Provided //
        ///////////////////////////////////////////////////////////////////
        if ($('#chkSalesY:checked').length > 0) {
            if (!isAlpha(($('#txtSalesNotes').val()))) {
                $('#btnSubmit').hide();
                return;
            }
        }

        //4
        ////////////////////
        // Check I/R Data //
        ////////////////////
        if ($('input:checkbox[class=chkIrData]:checked').length == 0) {
            if ($('#txtNoRpt')[0].value.length == 0) {
                $('#btnSubmit').hide();
                return;
            }
        }

        //5
        ////////////////////
        // Check I/R Data //
        ////////////////////
        if ($('input:checkbox[class=chkSales]:checked').length == 0) {
            if ($('#txtNoRpt')[0].value.length == 0) {
                $('#btnSubmit').hide();
                return;
            }
        }


        if ($('#chkIrDataY:checked').length > 0) {

            // 6A
            ///////////////////////////
            // Check I/R In Drop Box //
            ///////////////////////////
            if ($('input:checkbox[class=chkIrDrBox]:checked').length == 0) {
                if ($('#txtNoRpt')[0].value.length == 0) {
                    $('#btnSubmit').hide();
                    return;
                }
            }

            //6B
            ////////////////////////
            // Check I/R Type Set //
            ////////////////////////
            if ($('input:checkbox[class=chkIR]:checked').length == 0) {
                if ($('#txtNoRpt')[0].value.length == 0) {
                    $('#btnSubmit').hide();
                    return;
                }
            }

            //6C
            /////////////////////////////////////////////////
            // If IR Only Checked, Thats All That's Needed //
            /////////////////////////////////////////////////
            if ($('#chkIrPort:checked').length > 0) {
                $('#btnSubmit').show();
                return;
            }

            //6D
            /////////////////////////////////////////////////
            // If IR Only Checked, Thats All That's Needed //
            /////////////////////////////////////////////////
            if ($('#chkIrOnly:checked').length > 0) {
                $('#btnSubmit').show();
                return;
            }

        }

        //7
        ///////////////////////////////////
        // Check Report Disposition Page //
        ///////////////////////////////////
        if ($('input:checkbox[class=chkRptDisp]:checked').length == 0) {
            if ($('#txtNoRpt')[0].value.length == 0) {
                $('#btnSubmit').hide();
                return;
            }
        }

        //8
        //////////////////////////////
        // Check Report Format Page //
        //////////////////////////////
        if ($('input:checkbox[class=chkRptFmt]:checked').length == 0) {
            $('#btnSubmit').hide();
            return;
        }

        //9 
        if ($('input:checkbox[class=chkDrBox]:checked').length == 0) {
            $('#btnSubmit').hide();
            return;
        }

        //10
        //////////////////////
        // Check Other Page //
        //////////////////////
        if ($('input:checkbox[class=chkRts]:checked').length == 0) {
            $('#btnSubmit').hide();
            return;
        }

        //11
        if ($('input:checkbox[class=chkCt]:checked').length == 0) {
            $('#btnSubmit').hide();
            return;
        }

        //12
        if ($('input:checkbox[class=chkPd]:checked').length == 0) {
            $('#btnSubmit').hide();
            return;
        }

        //13
        if ($('input:checkbox[class=chkOs]:checked').length == 0) {
            $('#btnSubmit').hide();
            return;
        }


        //////////////////////////////////
        // Check Test Descriptions Page //
        //////////////////////////////////
        if ($('input:checkbox[class=ChkTest]:checked').length == 0) {
            if ($('input:checkbox[class=chkNoTest]:checked').length == 0) {
                $('#btnSubmit').hide();
                return;
            }
        }




        $('#btnSubmit').show();
        $('#NotReady').html('( Ready To Submit )');

        if ($("#SiteWrapper").isChanged()) {
            $('#NotReady').html('( Changes Ready To Submit )');
        }
    };
    function isValidEmailAddress(emailAddress) {
            var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
            return pattern.test(emailAddress);
        };




    ////////////////////////////
    // Submit Data Processing //
    ////////////////////////////
    $("#btnSubmit").on("click", function () {
        $("#SiteWrapper").data("changed", false);

        $('#NoJobError').hide();
        var submitJobRptAjax = new AsyncServerMethod();

        submitJobRptAjax.add('UserEmpID', $('#hlblEID')[0].innerHTML);
        submitJobRptAjax.add('jobNo', $('#hlblJobNo')[0].innerHTML);

        submitJobRptAjax.add('Complete', $('#chkComplete')[0].checked);
        submitJobRptAjax.add('Partial', $('#chkPartial')[0].checked);
        submitJobRptAjax.add('PE', $('#chkPe')[0].checked);
        submitJobRptAjax.add('No_Report_Reason', encodeURIComponent($('#txtNoRpt').val() + $('#msd2NoRpt').val() + $('#msd3NoRpt').val() + $('#msd4NoRpt').val() + $('#esd2NoRpt').val()));
        submitJobRptAjax.add('comments', encodeURIComponent($('#txtComments').val()));

        
        submitJobRptAjax.add('chkNoData', $('#chkNoData')[0].checked);
        submitJobRptAjax.add('chkPowerDB', $('#chkPowerDB')[0].checked);
        submitJobRptAjax.add('chkScanned', $('#chkScanned')[0].checked);
        submitJobRptAjax.add('chkPdbMaster', $('#chkPdbMaster')[0].checked);
        submitJobRptAjax.add('chkOtherData', $('#chkOtherData')[0].checked);



        if ($('.chkRts:checked').length > 0)
            submitJobRptAjax.add('RTS_Relay_Data', $('.chkRts:checked')[0].value);
        else
            submitJobRptAjax.add('RTS_Relay_Data', '0');

        if ($('.chkCt:checked').length > 0)
            submitJobRptAjax.add('CT_Data_Saved', $('.chkCt:checked')[0].value);
        else
            submitJobRptAjax.add('CT_Data_Saved', '0');

        if ($('.chkPd:checked').length > 0)
            submitJobRptAjax.add('Partial_Discharge', $('.chkPd:checked')[0].value);
        else
            submitJobRptAjax.add('Partial_Discharge', '0');

        if ($('.chkOs:checked').length > 0)
            submitJobRptAjax.add('Oil_Sample', $('.chkOs:checked')[0].value);
        else
            submitJobRptAjax.add('Oil_Sample', '0');

        if ($('.chkOsLtr:checked').length > 0)
            submitJobRptAjax.add('Oil_Sample_Follow_UP', $('.chkOsLtr:checked')[0].value);
        else
            submitJobRptAjax.add('Oil_Sample_Follow_UP', '0');

        submitJobRptAjax.add('OtherText', encodeURIComponent($('#OtherData').val()));




        submitJobRptAjax.add('Sonic', $('#CbTestSonic')[0].checked);
        submitJobRptAjax.add('TTR', $('#CbTestTTR')[0].checked);
        submitJobRptAjax.add('Thermo', $('#CbTestThermo')[0].checked);
        submitJobRptAjax.add('Relay', $('#CbTestRelay')[0].checked);
        submitJobRptAjax.add('PCB', $('#CbTestPCB')[0].checked);
        submitJobRptAjax.add('PD', $('#CbTestPD')[0].checked);
        submitJobRptAjax.add('Oil', $('#CbTestOil')[0].checked);
        submitJobRptAjax.add('NFPA', $('#CbTestNFPA')[0].checked);
        submitJobRptAjax.add('InslResit', $('#CbTestInslResit')[0].checked);
        submitJobRptAjax.add('GrdEltrode', $('#CbTestGrdEltrode')[0].checked);
        submitJobRptAjax.add('GrdFlt', $('#CbTestGrdFlt')[0].checked);
        submitJobRptAjax.add('Doble', $('#CbTestDoble')[0].checked);
        submitJobRptAjax.add('DLRO', $('#CbTestDLRO')[0].checked);
        submitJobRptAjax.add('Decal', $('#CbTestDecal')[0].checked);
        submitJobRptAjax.add('HiPot', $('#CbTestHiPot')[0].checked);
        submitJobRptAjax.add('BBT', $('#CbTestBBT')[0].checked);
        submitJobRptAjax.add('None', $('#CbTestNone')[0].checked);

        submitJobRptAjax.add('IrData', 'false');

        submitJobRptAjax.add('chkIrDrpBox', $('#IrDrpBoxY')[0].checked);
        submitJobRptAjax.add('chkIrOnly', $('#chkIrOnly')[0].checked);
        submitJobRptAjax.add('chkIrPort', $('#chkIrPort')[0].checked);
        submitJobRptAjax.add('txtIrHardCnt', $('#txtIrHardCnt')[0].value);
        submitJobRptAjax.add('txtAddEmail', encodeURIComponent($('#txtAddEmail').val()));
        submitJobRptAjax.add('chkRptDrBox', $('#RptDrBoxY')[0].checked);

        submitJobRptAjax.add('chkRptDrBoxNo', $('#RptDrBoxN')[0].checked);
        submitJobRptAjax.add('chkIrDrpBoxNo', $('#IrDrpBoxN')[0].checked);


        if ($('#chkSalesY')[0].checked || $('#chkSalesY2')[0].checked || $('#chkSalesY5')[0].checked)
            submitJobRptAjax.add('SalesFollowUp', true);
        
        submitJobRptAjax.add('SalesNotes', encodeURIComponent($('#txtSalesNotes').val() + $('#txtSalesNotes2').val() + $('#txtSalesNotes5').val()));
        
        var isOnDfs = $('input:checkbox[value=DfsEmail]').is(':checked');
        var isOnNv = false;
        var isOnPdb =  $('input:checkbox[value=MsdPdb]').is(':checked');
        var isInFolder = $('input:checkbox[value=MsdFolderEmail]').is(':checked');
        
        if ($('#dfsDelivDate').val().length > 0)
            isOnDfs = true;
        
        var plsEmail = false;
        if ($('#esd2DfsMailCnt').val().length > 0) {
            plsEmail = true;
            isOnDfs = true;
        }
        
        if ($('#esd2DfsCdCnt').val().length > 0) {
            plsEmail = true;
            isOnDfs = true;
        }
        
        if ($('#msd1HardCnt').val().length > 0) {
            plsEmail = true;
            isOnNv = true;
        }
        
        if ($('input:checkbox[value=MsdEmail]').is(':checked')) {
            //plsEmail = true;
            isOnNv = true;
        }
        
        if ( $('input:checkbox[value=MsdComplete]').is(':checked')) {
            isOnPdb = true;
        }
        
        if ( $('input:checkbox[value=MsdJobFolder]').is(':checked') ) {
            isInFolder = true;
        }
        
        var mailCnt = $('#esd2DfsMailCnt').val() + $('#msd1HardCnt').val() + $('#msd3HardCnt').val();
        var cdCnt = $('#esd2DfsCdCnt').val();
        
        submitJobRptAjax.add('GaveToCustDate', $('#dfsDelivDate').val() );                              // 1
        submitJobRptAjax.add('PlsEmail', plsEmail);                                                     // 2
        submitJobRptAjax.add('PlsMailCnt', mailCnt);                                                    // 3
        submitJobRptAjax.add('PlsMailCdCnt', cdCnt);                                                    // 4
        submitJobRptAjax.add('GaveOther', encodeURIComponent($('#esd2Other').val()));                   // 5
        submitJobRptAjax.add('StdDrvAutoRptDone', $('input:checkbox[value=EsdStdDrv]').is(':checked')); // 6
        submitJobRptAjax.add('RptInDfs', isOnDfs);                                                      // 7
        submitJobRptAjax.add('RptInNv', isOnNv);                                                        // 8
        submitJobRptAjax.add('RptInPdb', isOnPdb);                                                      // 9
        submitJobRptAjax.add('InitInsp', $('input:checkbox[value=MsdInitial]').is(':checked'));         // 10
        submitJobRptAjax.add('RptInFolder',isInFolder );                                                // 11
        submitJobRptAjax.add('SendByDate', $('#msd3date').val() + $('#msd4date').val() );               // 12
        submitJobRptAjax.add('FinalReady', $('input:checkbox[value=MsdPdb]').is(':checked'));           // 13
        
        submitJobRptAjax.exec("/SIU_DAO.asmx/q00p00ssa", submitJobRptSuccess);
    });

    ////////////////////////////////////////////////////
    // Submit Button Success Redirects To Parent Menu //
    ////////////////////////////////////////////////////
    function submitJobRptSuccess() {
        window.location.href = "http://" + window.location.hostname + "/elo/mainmenu.aspx";
    }




    ////////////////////////
    // Clear / Reset Form //
    ////////////////////////
    $("#btnClear").click(function () {
        var ans = confirm('Are you sure you want to clear this data');
        if (ans == true)
            clearForm();
    });
    function clearForm() {

        $('#NotReady').html('');

        ////////////////////////////////////////
        // Clear Data Confirmation Containers //
        ////////////////////////////////////////   
        $('#hlblJobNo')[0].innerHTML = "";

        $('#lblJobNo')[0].innerHTML = "";
        $('#lblJobSite')[0].innerHTML = "";
        $('#lblJobDesc')[0].innerHTML = "";
        $('#lblDept')[0].innerHTML = "";
        $('#lblTech')[0].innerHTML = "";
        $('#lblRptDisp')[0].innerHTML = "";

        $('#OtherData')[0].value = "";
        $('#txtComments')[0].value = "";
        $('#txtSalesNotes')[0].value = "";
        $('#txtSalesNotes2')[0].value = "";
        $('#txtSalesNotes5')[0].value = "";
        
       
        $('#txtNoRpt')[0].value = "";
        $('#ddJobNo')[0].value = "";

        $('#lblJobStatus')[0].innerHTML = "";
        $('#lblSubmitDate')[0].innerHTML = "";
        $('#lblJhaDate')[0].innerHTML = "";
        $('#lblIrSubmitDate')[0].innerHTML = "";
        $('#lblIrCompleteDate')[0].innerHTML = "";
        $('#lblCost')[0].innerHTML = "";
        $('#lblLoginDate')[0].innerHTML = "";
        $('#lblDataEntryDate')[0].innerHTML = "";
        $('#lblProofDate')[0].innerHTML = "";
        $('#lblCorrectDate')[0].innerHTML = "";
        $('#lblReviewDate')[0].innerHTML = "";
        $('#lblReadyDate')[0].innerHTML = "";


        $('#msd1HardCnt').val('');
        
        $('#msd2NoRpt').val('');
        
        $('#msd3date').val('');
        $('#msd3HardCnt').val('');
        $('#msd3NoRpt').val('');
        
        $('#msd4date').val('');
        $('#msd4NoRpt').val('');
        
        
        $('#dfsDelivDate').val('');
        $('#esd2DfsMailCnt').val('');
        $('#esd2DfsCdCnt').val('');
        $('#esd2NoRpt').val('');
        
        


        //////////////////////////////
        // Clear Any Previous Error //
        //////////////////////////////
        $('#lblErr')[0].innerHTML = '';
        $('#lblErrServer')[0].innerHTML = '';
        $('#NoJobError').hide();
        
        $('#saleNotesDiv').hide();
        $('#saleNotesDiv2').hide();
        $('#saleNotesDiv5').hide();
        

        ///////////////////////////
        // Disable Submit Button //
        ///////////////////////////
        $('#btnSubmit').hide();

        ////////////////////////////////////
        // Clear All CheckBoxes           //
        // Enable IsAllDefined CheckBoxes //
        // Default ST Checkbox            //
        ////////////////////////////////////
        $('input:checkbox').attr('checked', false);
        $('input:checkbox').attr('disabled', false);


        /////////////////////////////////////////
        // Hide Addendum Data Collection Panes //
        /////////////////////////////////////////
        $('#Comments').hide();
        $('#tabsESD1').hide();
        $('#tabsESD2').hide();
        $('#tabsESD3').hide();
        $('#tabsMSD1').hide();
        $('#tabsMSD2').hide();
        $('#tabsMSD3').hide();
        $('#tabsMSD4').hide();
        $('#JobDiv').show();

        //////////////////////////////////////
        // Send Focus To Job No Entry Field //
        //////////////////////////////////////
        $("#ddJobNo").focus();
    };






    /////////////////
    // Format Tabs //
    /////////////////
    $("#tabsESD1").tabs();
    $("#tabsESD2").tabs();
    $("#tabsESD3").tabs();
    $('#tabsMSD1').tabs();
    $('#tabsMSD2').tabs();
    $('#tabsMSD3').tabs();
    $('#tabsMSD4').tabs();
    

    /////////////////////////////////////////
    // Hide Addendum Data Collection Panes //
    /////////////////////////////////////////
    $('#tabsESD1').hide();
    $('#tabsESD2').hide();
    $('#tabsESD3').hide();
    $('#tabsMSD1').hide();
    $('#tabsMSD2').hide();
    $('#tabsMSD3').hide();
    $('#tabsMSD4').hide();
    $('#Comments').hide();

    ///////////////////////////
    // Disable Submit Button //
    ///////////////////////////
    $('#btnSubmit').hide();

    ////////////////////////////////////////////////////////
    // Check Box Group CbOsLtr Only Allows N/A by Default //
    ////////////////////////////////////////////////////////
    $('#CbOsLtrY').attr('disabled', true);
    $('#CbOsLtrN').attr('disabled', true);
    $('.chkOsLtr').attr('checked', false);
    $('#CbOsLtrNA')[0].checked = true;

    ///////////////////////////////
    // Set Focus On Job No Field //
    ///////////////////////////////
    $('#ddJobNo').focus();


    ///////////////////////////////////////
    // Past Due Job Reports Notification //
    ///////////////////////////////////////
    function showPastDue() {
        $('#PastDueCnt').hide();
        var getDueCnt = new AsyncServerMethod();
        getDueCnt.add('EmpID', $('#hlblEID')[0].innerHTML);
        getDueCnt.exec("/SIU_DAO.asmx/GetMyPastDueJobRptsSum", pastDueSuccess);
    }
    showPastDue();

    var listOfPastDue = [];
    function pastDueSuccess(data) {
        listOfPastDue = data.d.split("JobNo");
        var l = listOfPastDue.length - 1;
        if (l > 0) {
            $('#PastDueCnt').show('slow');
            $('#PastDueCnt')[0].innerHTML = 'Click Here To See ' + l + ' Past Due Job Reports';
        }
    }


    //////////////////////////////////////////
    // If A Job No Was Passed in, Go Get It //
    // i.e. Called From MySi Or An Email    //
    //////////////////////////////////////////
    var jobNo = $.fn.getURLParameter('Job');
    if (jobNo != null && jobNo.length > 0) {
        $('#ddJobNo').val(jobNo);
        $('#JobDiv').hide();
        getJobDetails();
    }


});