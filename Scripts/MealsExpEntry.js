$(document).ready(function () {

    /////////////////////////////////////////
    // Hide Addendum Data Collection Panes //
    /////////////////////////////////////////
    $('.JobAndOh').hide();
    $('#ovrAmount').hide();
    $('.MilesAndMeals').hide();


    ///////////////////////////
    // Disable Submit Button //
    ///////////////////////////
    $('#btnSubmit').attr('disabled', true);



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
        mileExpSubmitCall.add('empNo', $('#hlblEID')[0].innerHTML);
        mileExpSubmitCall.add('workDate', $('#txtWorkDate').val());
        mileExpSubmitCall.add('JobNo', $('#hlblJobNo')[0].innerHTML);
        mileExpSubmitCall.add('OhAcct', $('#hlblOhAcct')[0].innerHTML);
        mileExpSubmitCall.add('Miles', $('#hlblMiles')[0].innerHTML);
        mileExpSubmitCall.add('Meals', $('#hlblMealsIdx')[0].innerHTML);
        mileExpSubmitCall.add('Amount', $('#hlblAmount')[0].innerHTML);
        mileExpSubmitCall.exec("/SIU_DAO.asmx/MileExpSubmit", mileExpSubmitSuccess);
    });


    //////////////////
    // Clear Button //
    //////////////////
    $('#btnClear').click(function () {
        clear();
    });


    function clear() {
        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });

        $('.JobAndOh').hide();
        $('#ovrAmount').hide();

        $('.MilesAndMeals').hide();
        $('#btnSubmit').attr('disabled', true);
        $('#DateDiv').show('slow');

        $('#txtWorkDate').val('');
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

    getTimeJobs();

    function showJobDetails() {
        var job = $('#ddJobNo').val();

        $('#hlblJobNo')[0].innerHTML = job;
        $('#lblJobNo')[0].innerHTML = "<b>Job No:</b> " + job + "<br/>";

        $('.MilesAndMeals').show('slow');
        $('.JobAndOh').hide();
    }






    ///////////////////////////////
    // Load List Of O/H Accounts //
    ///////////////////////////////
    var listOfAccounts = [];
    function GetExpenseOHAcctsSuccess(data) {
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
                showOhDetails();
                $('#txtMiles').focus();
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var dataPieces = ui.content[0].value.split(' ');
                    $(this).val(dataPieces[0]);
                    showOhDetails();
                    $("#ddOhAcct").autocomplete("close");
                    $('#txtMiles').focus();
                }

                return ui;
            }


        });
    }

    function GetExpenseOHAccts() {
        var GetExpenseOHAcctsAjax = new AsyncServerMethod();
        GetExpenseOHAcctsAjax.exec("/SIU_DAO.asmx/GetExpenseOHAccts", GetExpenseOHAcctsSuccess);
    }
    GetExpenseOHAccts();

    function showOhDetails() {
        var ohAcct = $('#ddOhAcct').val();

        $('#hlblOhAcct')[0].innerHTML = ohAcct;
        $('#lblOhAcct')[0].innerHTML = "<b>OH Acct:</b> " + ohAcct + "<br/>";

        $('.MilesAndMeals').show('slow');
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


            $('.MilesAndMeals').hide();
            $('#btnSubmit').attr('disabled', false);
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

            $('.MilesAndMeals').hide();
            $('#ovrAmount').show();
            $('#btnSubmit').attr('disabled', false);
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
    getMealRates();




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





    ///////////////////////////////////////
    // Setup Report Of Unposted Expenses //
    ///////////////////////////////////////
    $('#jTableContainer').jtable({
        title: 'Unposted Expenses',
        defaultSorting: 'JobNo ASC',
        edit: true,
        actions: {
            listAction: '/SIU_DAO.asmx/GetMyExpenses',
            deleteAction: '/SIU_DAO.asmx/DeleteMyExpense'
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

    var timestamp = new Date();
    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });




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
                    clear();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = dataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);

                        clear();
                    }

                    return ui;
                }
            });
    }


    // Load Emps AutoComplete List
    //if ($("#SuprArea").length > 0) {
    //    var getEmpsCall = new AsyncServerMethod();
    //    getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);
    //}
});