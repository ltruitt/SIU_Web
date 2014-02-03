$(document).ready(function () {
 

    ////////////////////////////////////////
    // Event Management For Job Selection //
    ////////////////////////////////////////
    var listOfJobs = [];
    function getJobsSuccess(data) {

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
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var dataPieces = ui.content[0].value.split(' ');
                    $(this).val(dataPieces[0].replace(/\n/g, ""));
                    getJobDetails();
                    $("#ddJobNo").autocomplete("close");
                }

                return ui;
            }
        });
        
        $('#ddJobNo').show();
        $('#ddJobNoWait').hide();
    }


    function getJobs() {
        var getTimeJobsCall = new AsyncServerMethod();
        getTimeJobsCall.exec("/SIU_DAO.asmx/Affe31", getJobsSuccess);
    }



    //////////////////////////////////////////////////////
    // Make Sure Text Entered For Jobs Was In Jobs List //
    //////////////////////////////////////////////////////
    //$("#ddJobNo").blur(function () {
    //    if (!listOfJobs.containsCaseInsensitive(this.value)) {
    //        $(this).val("");
    //    }
    //});
    function getJobSuccess(data) {
        var jobDetails = $.parseJSON(data.d);

        $('#lblJobSiteSelection').html ( "<b>Customer:</b> " + jobDetails.JobCust + "<br/>" );
        $('#lblJobDescSelection').html ( "<b>Job:</b> " + jobDetails.JobDesc + "<br/>" );
        $('#lblDeptSelection').html ( "<b>Div/Dept:</b> " + jobDetails.JobDept + "<br/>" );
    }
    function getJobCompletionSuccess(data) {
        $('#ddJobNo').show();
        $('#ddJobNoWait').hide();
        var jobDetails = $.parseJSON(data.d);
        
        $('#basicBillingDiv').show('slow');
        $('#DetailBillingDiv').hide();
        
        if (!jobDetails)
            return;
        
        $('#txtNumMgrs').val(jobDetails.NumMgrs);
        $('#txtAddMaterial').val(jobDetails.AddMaterial);
        $('#txtAddTravel').val(jobDetails.AddTravel);
        $('#txtAddLodge').val(jobDetails.AddLodge);
        $('#txtAddOther').val(jobDetails.AddOther);
        $('#txtTotHours').val(jobDetails.TotHours);
        
        $("input:checkbox[value='Complete']").prop("checked", jobDetails.JobComplete);
        $("input:checkbox[value='Extra Billing']").prop("checked", jobDetails.OrigionalScope);
        
        if (jobDetails.OrigionalScope)
            $('#DetailBillingDiv').show('slow');
        
        validate();
    }
    
    function getJobDetails() {

        var getJobCall = new AsyncServerMethod();
        getJobCall.add('jobNo', $('#ddJobNo').val());
        getJobCall.exec("/SIU_DAO.asmx/Gffeop1", getJobSuccess);
        
        var getJobCompletionCall = new AsyncServerMethod();
        getJobCompletionCall.add('jobNo', $('#ddJobNo').val());
        getJobCompletionCall.exec("/SIU_DAO.asmx/a886ppo9", getJobCompletionSuccess, getJobFail);
        $('#ddJobNo').hide();
        $('#ddJobNoWait').show();
    }
    
    function getJobFail() {
        $('#basicBillingDiv').hide();
        $('#DetailBillingDiv').hide();
        clear();
    }
    
    $('#btnClear').click(function () {
        clear();
    });
    
    ////////////////
    // Form Reset //
    ////////////////
    function clear() {
        $('#ddJobNo').val('');
        $('#ddJobNo').show();
        $('#ddJobNoWait').hide();
        
        $('#txtNumMgrs').val('');
        $('#txtAddMaterial').val('');
        $('#txtAddTravel').val('');
        $('#txtAddLodge').val('');
        $('#txtAddOther').val('');
        $('#txtTotHours').val('');
        
        $('#lblJobSiteSelection').html('');
        $('#lblJobDescSelection').html('');
        $('#lblDeptSelection').html('');
        
        $("input:checkbox[value='Complete']").prop("checked", false);
        $("input:checkbox[value='Extra Billing']").prop("checked", false);
        
        $('#btnSubmit').hide();
        $('#basicBillingDiv').hide();
        $('#DetailBillingDiv').hide();
    }
    
    /////////////////////
    // Form Validation //
    /////////////////////
    function validate() {
        
        $('#btnSubmit').hide();

        ///////////////////////////////////////////////////////////////////////
        // If Job Is Not Complete And Extra Billing Is No, Nothing To Submit //
        ///////////////////////////////////////////////////////////////////////
        if ($("input:checkbox[value='Complete']").prop("checked") == false)
            if ($("input:checkbox[value='Extra Billing']").prop("checked") == false) {
                return;
            }
        

        //////////////////////////////////////////////////////////////
        // If Job Is Complete And Extra Billing Is No, Allow Submit //
        //////////////////////////////////////////////////////////////
        if ( $("input:checkbox[value='Complete']").prop("checked") == true )
            if ( $("input:checkbox[value='Extra Billing']").prop("checked") == false) {
                $('#btnSubmit').show();
                return;
            }
        
        ///////////////////////////////////////
        // If Extra Billing Is Requested.... //
        ///////////////////////////////////////
        if ($("input:checkbox[value='Extra Billing']").prop("checked") == true) {

            /////////////////////////////
            // No Managers Is Required //
            /////////////////////////////
            if ($('#txtNumMgrs').val() == '')
                return;
            
            /////////////////////////////
            // Total Hours Is Required //
            /////////////////////////////
            if ($('#txtTotHours').val() == '')
                return;
            
            ///////////////////////////////
            // Material Cost Is Required //
            ///////////////////////////////
            if ($('#txtAddMaterial').val() == '')
                return;

            ///////////////////////////////
            // Travel Cost Is Required //
            ///////////////////////////////
            if ($('#txtAddTravel').val() == '')
                return;

            ///////////////////////////////
            // Lodgeing Cost Is Required //
            ///////////////////////////////
            if ($('#txtAddLodge').val() == '')
                return;

            ///////////////////////////////
            // Pther Cost Is Required //
            ///////////////////////////////
            if ($('#txtAddOther').val() == '')
                return;

        }
        
        $('#btnSubmit').show();


        //$('#lblJobSiteSelection').html('');
        //$('#lblJobDescSelection').html('');
        //$('#lblDeptSelection').html('');

        //$("input:checkbox[value='Complete']").prop("checked", false);
        //$("input:checkbox[value='Extra Billing']").prop("checked", false);
    }
    $(':input').blur(function () {
        validate();
    });

    ////////////////////////////////////////
    // Show We Are Looking Up Job Numbers //
    ////////////////////////////////////////
    $('#ddJobNo').hide();
    $('#ddJobNoWait').show();
    
    /////////////////////////////////
    // Setup Numerical Input Masks //
    /////////////////////////////////
    $('#txtNumMgrs').autoNumeric('init', { vMin: '0', vMax: '11', mDec: '0' });
    $('#txtAddMaterial').autoNumeric('init', { vMin: '0', vMax: '99999.00', mDec: '2' });
    $('#txtAddTravel').autoNumeric('init', { vMin: '0', vMax: '99999.00', mDec: '2' });
    $('#txtAddLodge').autoNumeric('init', { vMin: '0', vMax: '99999.00', mDec: '2' });
    $('#txtAddOther').autoNumeric('init', { vMin: '0', vMax: '99999.00', mDec: '2' });
    $('#txtTotHours').autoNumeric('init', { vMin: '0', vMax: '9999.00', mDec: '1' });
    
    ///////////////////
    // Setup Toggles //
    ///////////////////
    (function () {
        window.switcher = new AcidJs.ToggleSwitch();

        window.switcher.render({
            type: "checkbox",               // (String) "checkbox"|"radio" [optional] default: "checkbox"
            name: "basicInfo",              // (String) [required]
            on: "Yes",                      // (String) [optional] default: "on"
            off: "No",                      // (String) [optional] default: "off"
            defaultCheckedNode: [2,2],      // (Number for type "radio"|Array for type "checkbox") [optional] default: 0
            boundingBox: $("#SwitcherBilling"),
            
            items:
            [
                {
                    cssClasses: ["ToggleLi"],
                    value: "Complete",
                    label: "<span id='SwitcherLabel_1'>Job Is Complete</span>" // (String) optional
                },
                
                {
                    value: "Extra Billing",
                    label: "<span id='SwitcherLabel_2'>Extra Billing</span>"
                }
            ]
        });
        
        window.switcher.render({
            type: "checkbox",
            name: "salesInfo",
            on: "Yes",
            off: "No",
            defaultCheckedNode: [2, 2],
            boundingBox: $("#SwitcherSales"),

            items:
            [
                {
                    cssClasses: ["ToggleLi"],
                    value: "Contact",
                    label: "<span id='SwitcherLabel_3'>Sales Should Contact Customer</span>" // (String) optional
                },

                {
                    value: "Quote",
                    label: "<span id='SwitcherLabel_4'>Produce a New Quote</span>"
                }
            ]
        });
        

        $("#SwitcherBilling").on("acidjs-toggle-switch", function (e, data) {
            window.console.log(e.type, data);
            
            if ( data.value == "Extra Billing" )
                if ( data.checked == true) {
                    $('#DetailBillingDiv').show('slow');
                } else {
                    $('#DetailBillingDiv').hide();
                }
            
            validate();
        });
        


    })();

    
    $("#btnSubmit").click(function () {
        $('#btnSubmit').hide();
        $('#ddJobNo').hide();
        $('#ddJobNoWait').show();
        $('#basicBillingDiv').hide();
        $('#DetailBillingDiv').hide();
        
        //var hoursType = $('input:checkbox:checked')[0].value;
        var SubmitCall = new AsyncServerMethod();
        
        SubmitCall.add('JobNo', $('#ddJobNo').val() );
        SubmitCall.add('JobComplete', $("input:checkbox[value='Complete']").prop("checked")  );
        SubmitCall.add('OrigionalScope', $("input:checkbox[value='Extra Billing']").prop("checked") );
        SubmitCall.add('NumMgrs', $('#txtNumMgrs').val()  );
        SubmitCall.add('AddMaterial', $('#txtAddMaterial').val()  );
        SubmitCall.add('AddTravel', $('#txtAddTravel').val()  );
        SubmitCall.add('AddLodge', $('#txtAddLodge').val()  );
        SubmitCall.add('AddOther', $('#txtAddOther').val()  );
        SubmitCall.add('TotHours', $('#txtTotHours').val()  );
        SubmitCall.add('ApprovalName', $('#AppName').val());
        SubmitCall.add('SalesCall', $("input:checkbox[value='Contact']").prop("checked"));
        SubmitCall.add('SalesQuote', $("input:checkbox[value='Quote']").prop("checked") );

        SubmitCall.exec("/SIU_DAO.asmx/a886ppo8", clear);
    });
    
    

    clear();
    getJobs();
})