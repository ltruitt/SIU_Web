﻿$(document).ready(function () {
   
    $('#txtExpAmount').autoNumeric('init', { vMin: '0', vMax: '99999.00', mDec: '2' });
    

    
    ///////////////////////////
    // Submit Button Handler //
    ///////////////////////////
    function mileExpSubmitSuccess(data) {
        var errorMsg = data.d;

        if (errorMsg.length == 0) {
            ////////////////////
            // Reset The Form //
            ////////////////////
            $("#btnClear").trigger('click');

        } else {
            $('#lblErrServer')[0].innerHTML = errorMsg;
        }
    }
    $("#btnSubmit").click(function () {
        var mileExpSubmitCall = new AsyncServerMethod();
        mileExpSubmitCall.add('empNo', $('#hlblEID').html());
        mileExpSubmitCall.add('workDate', $('#txtWorkDate').val());
        mileExpSubmitCall.add('JobNo', $('#hlblJobNo').html());
        mileExpSubmitCall.add('OhAcct', $('#hlblOhAcct').html());
        mileExpSubmitCall.add('Miles', $('#hlblMiles').html());
        mileExpSubmitCall.add('Meals', $('#hlblMealsIdx').html()  );
        mileExpSubmitCall.add('Amount', $('#hlblAmount').html()  );
        mileExpSubmitCall.add('Date4291', $('#txt4291Day').val());
        mileExpSubmitCall.exec("/SIU_DAO.asmx/w33", mileExpSubmitSuccess);
        $('#btnSubmit').hide();
    });

    //////////////////
    // Clear Button //
    //////////////////
    $('#btnClear').click(function () {
        clear();
    });
    
    $('#txtFile').click(function () {
        $('#error').hide();
        $('#error2').hide();
        $('#abort').hide();
        $('#warnsize').hide();
        $('#UploadStats').show();
    });

    ///////////////////////////////////////////////
    // Function To Clear The Form And Start Over //
    ///////////////////////////////////////////////
    function clear() {
        var timestamp = new Date();
        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });        

        $('#txtWorkDate').val('');
        $('#txt4291Day').val('');
        $('#txtMiles').val('');
        $('#txtMeals')[0].selectedIndex = 0;
        $("#ovrAmount").val('');
        $('#ddJobNo').val('');
        $('#ddOhAcct').val('');

        $('#hlblJobNo')[0].innerHTML = "";
        $('#hlblOhAcct')[0].innerHTML = "";
        $('#hlblMiles')[0].innerHTML = "";
        $('#hlblMeals')[0].innerHTML = "";
        $('#hlblAmount')[0].innerHTML = "";

        $('#lblDate')[0].innerHTML = "";
        $('#lblJobNo')[0].innerHTML = "";
        $('#lblOhAcct')[0].innerHTML = "";
        $('#lblMiles')[0].innerHTML = "";
        $('#lblMeals')[0].innerHTML = "";
        $('#lblAmount')[0].innerHTML = "";
        
        /////////////////////////////////////////
        // Hide Addendum Data Collection Panes //
        /////////////////////////////////////////
        $('.JobAndOh').hide();
        $('#ovrAmount').hide();
        
        $('#MealsDiv').hide();
        $('#MilesDiv').hide();
        $('#ExpAmountDiv').hide();
        $('#Div4291').hide();
        
        
        
        $('#UploadStats').hide();
        $('#FileUploadDiv').hide();
        $('#btnSubmit').hide();
        
        $('#DateDiv').show('slow');
        
    }

    //////////////////////////////////////////////////
    // Prevent Buttons From Firing Events On Server //
    //////////////////////////////////////////////////
    $("#aspnetForm").submit(function (e) { e.preventDefault(); });


    ////////////////////
    // Setup Calendar //
    ////////////////////
    var startDate = new Date($('#hlblSD')[0].innerHTML);
    var endDate = new Date($('#hlblED')[0].innerHTML);

    $('#txtWorkDate').datepicker({
        minDate: startDate,
        maxDate: endDate,
        constrainInput: true,
        onSelect: showWorkDate
    });
    
    $('#txt4291Day').datepicker({
        minDate: startDate,
        maxDate: endDate,
        constrainInput: true
    });


    /////////////////////////////////////////
    // Show Hours Entered For Date Showing //
    /////////////////////////////////////////
    function showWorkDate() {
        $('#DateDiv').hide();
        $('.JobAndOh').show('slow');

        $('#lblDate')[0].innerHTML = "<b>Work Date:</b> " + $('#txtWorkDate').val() + "<br/>";

    }


    /////////////////////////
    // Setup Job No Lookup //
    /////////////////////////
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
            select: function (event, ui) {
                var dataPieces = ui.item.value.split(' ');
                $(this).val(dataPieces[0]);
                showJobDetails();
                $('#txtMiles').focus();
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var dataPieces = ui.content[0].value.split(' ');
                    $(this).val(dataPieces[0]);
                    showJobDetails();
                    $("#ddJobNo").autocomplete("close");
                    $('#txtMiles').focus();
                }

                return ui;
            }
        });
    }
    function getTimeJobs() {
        var getTimeJobsAjax = new AsyncServerMethod();
        getTimeJobsAjax.exec("/SIU_DAO.asmx/Affe31", getTimeJobsSuccess);
    }

    /////////////////////////////////
    // Process Selected Job Number //
    /////////////////////////////////
    function showJobDetails() {
        var job = $('#ddJobNo').val();

        $('#hlblJobNo')[0].innerHTML = job;
        $('#lblJobNo')[0].innerHTML = "<b>Job No:</b> " + job + "<br/>";

        $('#MealsDiv').show();
        $('#MilesDiv').show();
        
        $('.JobAndOh').hide();
    }



    ///////////////////////////////
    // Load List Of O/H Accounts //
    ///////////////////////////////
    var listOfAccounts = [];
    function getExpenseOhAcctsSuccess(data) {
        listOfAccounts = data.d.split("\r");

        $("#ddOhAcct").autocomplete({ source: listOfAccounts },
        {
            matchContains: false,
            minChars: 1,
            autoFill: false,
            mustMatch: false,
            cacheLength: 20,
            max: 20,
            select: function (event, ui) {
                var dataPieces = ui.item.value.split(' ');
                $(this).val(dataPieces[0]);
                showOhDetails(ui.item.value);
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var dataPieces = ui.content[0].value.split(' ');
                    $(this).val(dataPieces[0]);
                    showOhDetails(ui.content[0].value);
                    $("#ddOhAcct").autocomplete("close");
                }

                return ui;
            }


        });
    }
    function getExpenseOhAccts() {
        var getExpenseOhAcctsAjax = new AsyncServerMethod();
        getExpenseOhAcctsAjax.exec("/SIU_DAO.asmx/bbb66655", getExpenseOhAcctsSuccess);
    }


    
    ///////////////////////////////////////
    // Process Selected Overhead Account //
    ///////////////////////////////////////
    function showOhDetails() {
        var ohAcct = $('#ddOhAcct').val();

        $('#hlblOhAcct')[0].innerHTML = ohAcct;
        $('#lblOhAcct')[0].innerHTML = "<b>OH Acct:</b> " + ohAcct + "<br/>";
        
        switch (ohAcct) {
            case '4291':
                $('#MilesDiv').show('slow');
                $('#txtMiles').focus();
                $('#Div4291').show();
                break;
                
            case '4421':
                $('#MealsDiv').show('slow');
                $('#txtMeals').focus();
                break;
                
            default:
                $('#FileUploadDiv').show('slow');
                $('#UploadHead').show();
                $('#txtFile').focus();
                break;
        }

        $('.JobAndOh').hide();
        
    }

    $("#ddJobNo").blur(function () {
        if (!listOfJobs.containsCaseInsensitive(this.value)) {
            $(this).val("");
        }
    });
    $("#ddOhAcct").blur(function () {
        if (!listOfAccounts.containsCaseInsensitive(this.value)) {
            $(this).val("");
        }
    });
    $("#txtMiles").keyup(function (e) {
        var keyId = (window.event) ? event.keyCode : e.keyCode;

        if (keyId == 13) {
            miles();
        }

    });
    $("#txtMiles").blur(function () {
        miles();
    });

    ///////////////////////////
    // Show Miles and Amount //
    // Show Meals and Amount //
    ///////////////////////////
    function miles() {
        if (!$.isNumeric($('#txtMiles').val())) {
            $('#txtMiles').addClass('ValidationError');
            return;
        }

        $('#lblErr').val('');
        $('#txtMiles').removeClass('ValidationError');

        if ($('#txtMiles').val().length > 0) {

            $('#lblMiles')[0].innerHTML = "<b>Miles:</b> " + $('#txtMiles').val();
            $('#hlblMiles')[0].innerHTML = $('#txtMiles').val();

            $('#hlblAmount')[0].innerHTML = $('#hlblMileRate')[0].innerHTML * $('#txtMiles').val();  // Calc Amount
            $('#hlblAmount')[0].innerHTML = number_format($('#hlblAmount')[0].innerHTML, 2, '.', ''); // Round It
            $('#lblAmount')[0].innerHTML = "<b>Amount:</b> " + $('#hlblAmount')[0].innerHTML; // +"<br/>";

            $('#MealsDiv').hide();
            $('#MilesDiv').hide();
            

            $('#btnSubmit').show();
            $('#btnSubmit').focus();
        }
    }
    function meals() {
        if ($('#txtMeals option:selected').val() > 0) {

            $('#hlblMeals')[0].innerHTML = $('#txtMeals option:selected').val();
            $('#hlblMealsIdx')[0].innerHTML = $('#txtMeals option:selected')[0].index;
            $('#lblMeals')[0].innerHTML = "<b>Meals:</b> " + $('#txtMeals option:selected')[0].text;

            $('#hlblAmount')[0].innerHTML = $('#txtMeals option:selected').val();
            $('#lblAmount')[0].innerHTML = "<b>Amount:</b> " + $('#hlblAmount')[0].innerHTML; // +"<br/>";

            $('#MealsDiv').hide();
            $('#MilesDiv').hide();
            
            $('#ovrAmount').show();
            //$('#btnSubmit').attr('disabled', false);
            $('#btnSubmit').show();
            $('#btnSubmit').focus();
        }
    }



    /////////////////////
    // Get Meals Rates //
    /////////////////////
    var mealRates = [];
    function getMealRatesSuccess(data) {
        mealRates = $.parseJSON(data.d);

        //        for (var c = 0; c < MealRates.length; c++) {
        //            $('#txtMeals').append(new Option(MealRates[c].DisplayString, MealRates[c].Amount));
        //        };

        // IE8 Work Around
        var sel = $("#txtMeals");
        for (var c = 0; c < mealRates.length; c++) {
            var temp = new Option(mealRates[c].DisplayString, mealRates[c].Amount);
            sel[0].options[sel[0].options.length] = temp;
        }



    }
    function getMealRates() {
        var getMealRatesAjax = new AsyncServerMethod();
        getMealRatesAjax.exec("/SIU_DAO.asmx/GetMealRates", getMealRatesSuccess);
    }
    




    /////////////////////////////
    // Form Validation Section //
    /////////////////////////////
    $('#ovrAmount').keyup(function () {
        validateOvrAmt();
    });
    $("#ovrAmount").change(function () {
        if (validateOvrAmt() == true) {
            $('#hlblAmount')[0].innerHTML = $("#ovrAmount").val();
            $('#lblAmount')[0].innerHTML = "<b>Amount:</b> " + $('#hlblAmount')[0].innerHTML;
            $("#ovrAmount").val('');
        }
    });
    function validateOvrAmt() {
        $('#ovrAmount').removeClass('ValidationError');

        if ($('#ovrAmount').val().length == 0) return false;

        ////////////////////////////
        // Amount Must Be Numeric //
        ////////////////////////////
        if (!isNumber($('#ovrAmount').val())) {
            $('#ovrAmount').addClass('ValidationError');
            return false;
        }

        return true;
    };
    $("#txtMeals").change(function () {
        if ($('#txtMeals').val() != null)
            meals();
    });
    
    //////////////////////////
    // Time Must Be Numeric //
    //////////////////////////
    $('#txtExpAmount').keyup(function () {
        if (!isNumber($('#txtExpAmount').val())) {
            $('#txtTime').addClass('ValidationError');
            $('#btnSubmit').hide();
            return;
        } else {
            $('#btnSubmit').show();
        }
    });




    ///////////////////////////////////////
    // Setup Report Of Unposted Expenses //
    ///////////////////////////////////////
    $('#jTableContainer').jtable({
        title: 'Unposted Expenses',
        defaultSorting: 'JobNo ASC',
        edit: true,
        actions: {
            listAction: '/SIU_DAO.asmx/hhtl0',
            deleteAction: '/SIU_DAO.asmx/zz0p'
        },
        fields: {
            Line_No_: {
                key: true,
                create: false,
                edit: false,
                list: false
            },
            Work_Date: {
                title: 'Date',
                width: '4%',
                type: 'date',
                displayFormat: 'mm-dd',
                listClass: 'jTableTD'
            },
            Job_No_: {
                title: 'Job',
                width: '4%',
                listClass: 'jTableTD'
            },
            O_H_Account_No_: {
                title: 'O/H',
                width: '4%',
                listClass: 'jTableTD'
            },
            Mileage: {
                title: 'Miles',
                width: '1%',
                listClass: 'jTableTD'
            },
            Meals: {
                title: 'Meals',
                width: '1%',
                listClass: 'jTableTD',
                options: { '1': '4-10', '2': '10-12', '3': '12-24' }
            },
            Amount: {
                title: 'Amt',
                width: '3%',
                listClass: 'jTableTD',
                display: function (data) { return data.record.Amount.toFixed(2); }
            }
        }
    });



    ////////////////////
    // Setup The Form //
    ////////////////////
    clear();
    getTimeJobs();
    getMealRates();
    getExpenseOhAccts();
});