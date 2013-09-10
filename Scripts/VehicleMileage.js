$(document).ready(function () {
    
    $('#btnRemove').hide();
    $('#btnSubmit').hide();
    $('#btnClear').show();

    //////////////////////////////
    // Next and Previous Arrows //
    //////////////////////////////
    $(".prevarrow").click(function () {
        $('#txtEntryDate').removeClass('ThisWeekCss');
        $('#txtEntryDate').removeClass('NextWeekCss');
        $('#txtEntryDate').removeClass('PrevWeekCss');

        if ($("#txtEntryDate")[0].innerHTML == $("#hlblNextWeek")[0].innerHTML) {
            $("#txtEntryDate")[0].innerHTML = $("#hlblThisWeek")[0].innerHTML;
            $('#txtEntryDate').addClass('ThisWeekCss');
        } else {
            $("#txtEntryDate")[0].innerHTML = $("#hlblPrevWeek")[0].innerHTML;
            $('#txtEntryDate').addClass('PrevWeekCss');
        }

        getReportedMileage();
    });

    $(".nextarrow").click(function () {
        $('#txtEntryDate').removeClass('ThisWeekCss');
        $('#txtEntryDate').removeClass('NextWeekCss');
        $('#txtEntryDate').removeClass('PrevWeekCss');

        if ($("#txtEntryDate")[0].innerHTML == $("#hlblPrevWeek")[0].innerHTML) {
            $("#txtEntryDate")[0].innerHTML = $("#hlblThisWeek")[0].innerHTML;
            $('#txtEntryDate').addClass('ThisWeekCss');
        } else {
            $("#txtEntryDate")[0].innerHTML = $("#hlblNextWeek")[0].innerHTML;
            $('#txtEntryDate').addClass('NextWeekCss');
        }

        getReportedMileage();

    });


    $("#txtEntryDate")[0].innerHTML = $("#hlblThisWeek")[0].innerHTML;
    $('#txtEntryDate').addClass('ThisWeekCss');


    /////////////////////////////////////////
    // Handle Assigned Vehicles List Click //
    /////////////////////////////////////////
    $('.AssignedVehicleSelected').click( function () {
        $('#txtVehicleNo')[0].value = $(this)[0].title;
    });



    /////////////////////
    // Get Report Data //
    /////////////////////
    var listOfVehs = [];
    function getVehilceMileageRptAvailSuccess(data) {
        var d = data.d.replace(/\"/g, "").replace(/\[/g, "").replace(/\}/g, "");
        listOfVehs = d.split(",");

        $("#txtVehicleNo").autocomplete(
            { source: listOfVehs },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: true,
                cacheLength: 20,
                max: 20,
                select: function (event, ui) {
                    $('#txtVehicleNo').val(ui.item.value);
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        $('#txtVehicleNo').val(ui.content[0].value);
                        $("#ddDept").autocomplete("close");
                    }

                    return ui;
                },
                close: function () {
                    $('#txtVehicleMileage').focus();
                }
            });
    }
    function getAllVehList() {
        var weekEndingParts = $('#txtEntryDate')[0].innerHTML.split(" ");
        var weekEnding = weekEndingParts[weekEndingParts.length - 1];

        var getVehilceMileageRptAvailAjax = new AsyncServerMethod();
        getVehilceMileageRptAvailAjax.add('empNo', $('#hlblEID')[0].innerHTML );
        getVehilceMileageRptAvailAjax.add('WeekEnding', weekEnding);
        getVehilceMileageRptAvailAjax.exec("/SIU_DAO.asmx/GetVehilceMileageRptAvail", getVehilceMileageRptAvailSuccess);
    }



    ///////////////////////////////////
    // Get List Of Personal Vehicles //
    ///////////////////////////////////
    function getVehicleAssignedSuccess(data) {
        var vehList = $.parseJSON(data.d);
        var span = "";

        for (var c = 0; c < vehList.length; c++) {
            var vehModel = vehList[c].VehModel;
            var vehNo = vehList[c].VehNo;

            span += "<a href='#' class='AssignedVehicleSelected' title='" + vehNo + "';    >" + vehModel + "</a>";

            if (c == 0) {
                $('#txtVehicleNo').val(vehNo);
            }
        }

        $('#AssignedVehicleList')[0].innerHTML = span;        
    }
    function getPersonalVehList() {
        
        var weekEndingParts = $('#txtEntryDate')[0].innerHTML.split(" ");
        var weekEnding = weekEndingParts[weekEndingParts.length - 1];
        var getVehicleAssignedAjax = new AsyncServerMethod();
        
        getVehicleAssignedAjax.add('empNo', $('#hlblEID')[0].innerHTML);
        getVehicleAssignedAjax.add('WeekEnding', weekEnding );
        getVehicleAssignedAjax.exec("/SIU_DAO.asmx/GetVehicleAssigned", getVehicleAssignedSuccess);
    }






    /////////////////////////////
    // Form Validation Section //
    /////////////////////////////
    $('#txtVehicleNo').keyup(function () {
        validate();
    });
    $('#txtVehicleMileage').keyup(function () {
        validate();
    });
    function validate() {

        $('#btnSubmit').hide();
        $('#txtVehicleNo').removeClass('ValidationError');
        $('#txtVehicleNo').removeClass('ValidationSuccess');
        $('#txtVehicleMileage').removeClass('ValidationError');
        $('#txtVehicleMileage').removeClass('ValidationSuccess');
        $('#lblErr')[0].innerHTML = '';


        var vehMiles = $('#txtVehicleMileage').val();
        //var vehNo = $('#txtVehicleNo').val();

        ///////////////////////////////
        // Vehicle No Must Have Data //
        ///////////////////////////////
        if ($('#txtVehicleNo').val().length == 0) {
            $('#txtVehicleNo').addClass('ValidationError');
            return;
        }
        
        ////////////////////////////////////
        // Vehicle Number Must Be Numeric //
        ////////////////////////////////////
        if (!isNumber($('#txtVehicleNo').val())) {
            $('#txtVehicleNo').addClass('ValidationError');
            return;
        }

        //////////////////////////////////
        // Vehicle Number Must Positive //
        //////////////////////////////////
        if ( $('#txtVehicleNo').val() < 0 ) {
            $('#txtVehicleNo').addClass('ValidationError');
            return;
        }

        //////////////////////////////////////
        // Vehicle Number Passed Validation //
        //////////////////////////////////////
        $('#txtVehicleNo').addClass('ValidationSuccess');



        //////////////////////////////////
        // Vehicle Miles Must Have Data //
        //////////////////////////////////
        if ($('#txtVehicleMileage').val().length == 0) {
            $('#txtVehicleMileage').addClass('ValidationError');
            return;
        }

        ///////////////////////////////////
        // Vehicle Miles Must Be Numeric //
        ///////////////////////////////////
        if (!isNumber($('#txtVehicleMileage').val())) {
            $('#txtVehicleMileage').addClass('ValidationError');
            return;
        }

        //////////////////////////////////////
        // Vehicle Miles Must Be Reasonable //
        //////////////////////////////////////
        if (vehMiles < 0 || vehMiles > 3000 ) {
            $('#txtVehicleMileage').addClass('ValidationError');
            return;
        }

        /////////////////////////////////////
        // Vehicle Miles Passed Validation //
        /////////////////////////////////////
        $('#txtVehicleMileage').addClass('ValidationSuccess');

        //////////////////////////////////////////
        // All Validation Passed, Enable Submit //
        //////////////////////////////////////////
        $('#btnSubmit').show();
    }


    //////////////////
    // Clear Button //
    //////////////////
    $("#btnClear").click(function () {
        clear();
    });



    ///////////////////////////
    // Submit Button Handler //
    ///////////////////////////
    function submitVehicleMileageSuccess(data) {
        var errorMsg = data.d;

        if (errorMsg.length < 5) {
            ////////////////////
            // Reset The Form //
            ////////////////////
            $("#btnClear").trigger('click');
        } else {
            $('#lblErrServer')[0].innerHTML = "Failed submitting data";
        }
    }
    
    $("#btnSubmit").click(function () {
        var weekEndingParts = $('#txtEntryDate')[0].innerHTML.split(" ");
        var weekEnding = weekEndingParts[weekEndingParts.length - 1];
        var submitVehicleMileageAjax = new AsyncServerMethod();
        
        submitVehicleMileageAjax.add('empNo', $('#hlblEID')[0].innerHTML );
        submitVehicleMileageAjax.add('VehNo', $('#txtVehicleNo').val() );
        submitVehicleMileageAjax.add('Mileage', $('#txtVehicleMileage').val() );
        submitVehicleMileageAjax.add('WeekEnding', weekEnding);
        
        submitVehicleMileageAjax.exec("/SIU_DAO.asmx/SubmitVehicleMileage", submitVehicleMileageSuccess);
    });






    ///////////////////////////
    // Submit Remove Handler //
    ///////////////////////////
    function removeVehicleMileageSuccess(data) {
        var errorMsg = data.d;

        if (errorMsg.length < 5) {
            ////////////////////
            // Reset The Form //
            ////////////////////
            $("#btnClear").trigger('click');
        } else {
            $('#lblErrServer')[0].innerHTML = errorMsg;
        }        
    }
    
    $("#btnRemove").click(function () {
        var weekEndingParts = $('#txtEntryDate')[0].innerHTML.split(" ");
        var weekEnding = weekEndingParts[weekEndingParts.length - 1];
        var removeVehicleMileageAjax = new AsyncServerMethod();

        removeVehicleMileageAjax.add('empNo',      $('#hlblEID')[0].innerHTML );
        removeVehicleMileageAjax.add('VehNo',      $('#txtVehicleNo').val()   );
        removeVehicleMileageAjax.add('Mileage',    $('#txtVehicleMileage').val()  );
        removeVehicleMileageAjax.add('WeekEnding', weekEnding );
        
        removeVehicleMileageAjax.exec("/SIU_DAO.asmx/RemoveVehicleMileage", removeVehicleMileageSuccess);
    });











    function clear() {

        /////////////////////////////
        // Clear Data Entry Fields //
        /////////////////////////////
        $('#txtVehicleNo').val("");
        $('#txtVehicleMileage').val("");
        $('#AssignedVehicleList')[0].innerHTML = "";

        ///////////////////////////////
        // Disable The Submit Button //
        ///////////////////////////////
        $('#btnSubmit').hide();

        /////////////////////////
        // Clear Error Message //
        /////////////////////////
        $('#lblErrServer')[0].innerHTML = '';

        ////////////////////////////////////////////////////
        // Check If There Is ALready Data For This Date   //
        // If THere Is No Data, Data ENtry Fields Enabled //
        ////////////////////////////////////////////////////
        getReportedMileage();

        //////////////////////////////////////////////////
        // Prevent Buttons From Firing Events On Server //
        //////////////////////////////////////////////////
        $("#aspnetForm").submit(function (e) { e.preventDefault(); });
    }



    function getVehicleMileageRecordSuccess(data) {
        if (data.d.length > 5) {

            ////////////////////////////////////
            // Data Already Exists -- Show It //
            ////////////////////////////////////
            var vehList = $.parseJSON(data.d);

            /////////////////////////////
            // Clear Data Entry Fields //
            /////////////////////////////
            $('#txtVehicleNo').val(vehList.Assigned_Vehicle_No_);
            $('#txtVehicleMileage').val(vehList.Weekly_Personal_Mileage);


            ////////////////////////
            // Disable Data Entry //
            ////////////////////////
            $('#txtVehicleNo').attr('class', 'DataInputDisabledCss');
            $('#txtVehicleMileage').attr('class', 'DataInputDisabledCss');
            $('#btnRemove').show();
            $('#btnSubmit').hide();
            $('#btnClear').hide();
        } else {

            ///////////////////////////////////
            // No Data Yet, Allow Data Entry //
            ///////////////////////////////////
            $('#txtVehicleNo').attr('class', 'DataInputCss');
            $('#txtVehicleMileage').attr('class', 'DataInputCss');
            $('#btnRemove').hide();
            $('#btnSubmit').hide();
            $('#btnClear').show();

            getAllVehList();
            getPersonalVehList();
        }        
    }

    function getReportedMileage() {

        /////////////////////////////
        // Clear Data Entry Fields //
        /////////////////////////////
        $('#txtVehicleNo').val("");
        $('#txtVehicleMileage').val("");
        $('#AssignedVehicleList')[0].innerHTML = "";

        /////////////////////////
        // Clear Error Message //
        /////////////////////////
        $('#lblErrServer')[0].innerHTML = '';
        
        ///////////////////////////////
        // Disable The Submit Button //
        ///////////////////////////////
        $('#btnSubmit').hide();

        var weekEndingParts = $('#txtEntryDate')[0].innerHTML.split(" ");
        var weekEnding = weekEndingParts[weekEndingParts.length - 1];
        var getVehicleMileageRecordAjax = new AsyncServerMethod();
        
        getVehicleMileageRecordAjax.add('empNo', $('#hlblEID')[0].innerHTML);
        getVehicleMileageRecordAjax.add('WeekEnding', weekEnding);
        
        getVehicleMileageRecordAjax.exec("/SIU_DAO.asmx/GetVehicleMileageRecord", getVehicleMileageRecordSuccess);
    }


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
    if ($("#SuprArea").length > 0) {
        var getEmpsCall = new AsyncServerMethod();
        getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);
    }
    

    /////////////////
    // Start It Up //
    /////////////////
    clear();
});