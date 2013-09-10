$(document).ready(function () {

    ////////////////////
    // Setup Calendar //
    ////////////////////
    $('#txtEntryDate').datepicker({
        constrainInput: false,
        onSelect: function() { $('#ddTypes').focus(); }
    });

    
        



    ///////////////////////////////////////////////////////////////
    // Load List Of Employees So Supr Can Change Viewed Employee //
    ///////////////////////////////////////////////////////////////
    var listOfEmps = [];
    function GetEmps_success(data) {
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
                    var DataPieces = ui.item.value.split(' ');
                    $('#hlblEID')[0].innerHTML = DataPieces[0];
                    $("#ddEmpIds").autocomplete("close");
                    $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);
                    $('#ddTypes').focus();

                    var timestamp = new Date();
                    $('#EmpPointsRpt').jtable('load', { EmpID: DataPieces[0], T: timestamp.getTime() });
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = DataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);
                        $('#ddTypes').focus();

                        var timestamp = new Date();
                        $('#EmpPointsRpt').jtable('load', { EmpID: DataPieces[0], T: timestamp.getTime() });
                    }
                    else {
                        $('#hlblEID')[0].innerHTML = '';
                    }

                    return ui;
                }
            });
    }


    ///////////////////////////////////////////////////////////////
    // Load List Of Employees So Supr Can Change Viewed Employee //
    ///////////////////////////////////////////////////////////////
    var listOfTypes = [];
    var listOfPoints = [];
    function getTypesSuccess(data) {
        listOfTypes = data.d.split("\r");

        for (var c = 0; c < listOfTypes.length; c++) {
            var strParts = listOfTypes[c].split(",");
            listOfPoints[c] = strParts[1];
            listOfTypes[c] = strParts[0];
        }


        $("#ddTypes").autocomplete({ source: listOfTypes },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: false,
                cacheLength: 20,
                max: 20,
                delay: 0,
                select: function (event, ui) {
                    var DataPieces = ui.item.value.split(' ');
                    $("#txtNumPts").val(listOfPoints[DataPieces[0] - 1]);

                    DataPieces = ui.item.value.split('-');
                    $('#hlblPointsType')[0].innerHTML = DataPieces[0];
                    $("#ddTypes").autocomplete("close");
                    ui.item.value = DataPieces[1].replace(' ', '');
                    $('#txtNumPts').focus();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split('-');
                        $('#hlblPointsType')[0].innerHTML = DataPieces[0];
                        $("#ddTypes").autocomplete("close");
                        $("#ddTypes").val(DataPieces[1].replace(' ', ''));

                        DataPieces = ui.content[0].value.split(' ');
                        $("#txtNumPts").val(listOfPoints[DataPieces[0] - 1]);
                        $('#txtNumPts').focus();
                    }
                    else {
                        $('#hlblPointsType')[0].innerHTML = '';
                    }

                    return ui;
                }
            });
    }

    $("#datacollection").delegate("*", "focus blur", function (event) {
        var elem = $(this);
        setTimeout(function () {
            elem.toggleClass("focused", elem.is(":focus"));
        }, 0);
    });



    //////////////////////////////////////
    // Validate Data Fields.            //
    // Highlight Bad Fields.            //
    // Enable or Disable Submit Button. //
    //////////////////////////////////////
    function validate() {

        var vFail = false;
        $('#btnSubmit').prop('disabled', false);
        $('#btnSubmit').css('color', "green");

        $('#lblErr')[0].innerHTML = '';
        $('#ddEmpIds').removeClass('ValidationError');
        $('#ddEmpIds').removeClass('ValidationSuccess');
        $('#ddTypes').removeClass('ValidationError');
        $('#ddTypes').removeClass('ValidationSuccess');
        $('#txtNumPts').removeClass('ValidationError');
        $('#txtNumPts').removeClass('ValidationSuccess');
        $('#txtEntryDate').removeClass('ValidationSuccess');

        /////////////////////////
        // Eid must be present //
        /////////////////////////
        if ($('#hlblEID')[0].innerHTML.length == 0 || $('#ddEmpIds').val().length == 0) {
            $('#hlblEID')[0].innerHTML = '';
            $('#ddEmpIds').addClass('ValidationError');
            $('#btnSubmit').prop('disabled', true);
            $('#btnSubmit').css('color', "red");
            vFail = true;
        }
        else {
            $('#ddEmpIds').addClass('ValidationSuccess');
        }

        ////////////////////////////////////////
        // Date Points Earned Must Be Present //
        ////////////////////////////////////////
        if ($('#txtEntryDate').val().length == 0) {
            $('#txtEntryDate').addClass('ValidationError');
            $('#btnSubmit').prop('disabled', true);
            $('#btnSubmit').css('color', "red");
            vFail = true;
        }
        else {
            $('#txtEntryDate').addClass('ValidationSuccess');
        }


        /////////////////////////
        // Type must be present //
        /////////////////////////
        if ($('#hlblPointsType')[0].innerHTML.length == 0 || $('#ddTypes').val().length == 0) {
            $('#hlblPointsType')[0].innerHTML = '';
            $('#ddTypes').addClass('ValidationError');
            $('#btnSubmit').prop('disabled', true);
            $('#btnSubmit').css('color', "red");
            vFail = true;
        }
        else {
            $('#ddTypes').addClass('ValidationSuccess');
        }


        //////////////////////////////////
        // Points Value Must Be Present //
        //////////////////////////////////
        if ($('#txtNumPts').val().length == 0) {
            $('#txtNumPts').addClass('ValidationError');
            $('#btnSubmit').prop('disabled', true);
            $('#btnSubmit').css('color', "red");
            vFail = true;
            return;
        }

        //////////////////////////
        // Time Must Be Numeric //
        //////////////////////////
        if (!isNumber($('#txtNumPts').val())) {
            $('#txtNumPts').addClass('ValidationError');
            vFail = true;
            return;
        }

        //////////////////////////////
        // Time Must In Valid Range //
        //////////////////////////////
        if ($('#txtNumPts').val() < 0 || $('#txtNumPts').val() > 10) {
            $('#txtNumPts').addClass('ValidationError');
            $('#btnSubmit').prop('disabled', true);
            $('#btnSubmit').css('color', "red");
            vFail = true;
            return;
        }
        $('#txtNumPts').addClass('ValidationSuccess');

        ////////////////////////
        // Validation Success //
        ////////////////////////
        if (vFail == true) {
            $('#btnSubmit').prop('disabled', true);
            $('#btnSubmit').css('color', "red");
        }

    }



    $("#btnSubmit").click(function () {
        $('#btnSubmit').prop('disabled', true);
        $('#btnSubmit').css('color', "red");

        var PointsSubmitCall = new AsyncServerMethod();
        PointsSubmitCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        PointsSubmitCall.add('ReasonCode', $('#hlblPointsType')[0].innerHTML);
        PointsSubmitCall.add('Points', $('#txtNumPts').val() );
        PointsSubmitCall.add('Comment', '');
        PointsSubmitCall.add('DateOfEvent', $('#txtEntryDate').val() );
        

        PointsSubmitCall.exec("/SIU_DAO.asmx/RecordAdminPoints", TimeSubmit_success);

    });


    ///////////////////////////
    // Submit Button Handler //
    ///////////////////////////
    function TimeSubmit_success(data) {
        var ErrorMsg = data.d;

        if (ErrorMsg.length > 6 && ErrorMsg.substring(0, 7) == "Success") {

            ////////////////////
            // Reset The Form //
            ////////////////////
            $('#hlblPointsType')[0].innerHTML = '';
            $('#txtNumPts').val('');
            $('#ddTypes').val('');

            //////////////////////////////
            // Clear Any Previous Error //
            //////////////////////////////
            $('#lblErr')[0].innerHTML = '';
            $('#lblErrServer')[0].innerHTML = '';

            var timestamp = new Date();
            $('#EmpPointsRpt').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });
        }
    };



    ////////////////////////
    // Clear / Reset Form //
    ////////////////////////
    $("#btnClear").click(function () {

        ////////////////////////////////////////
        // Clear Data Confirmation Containers //
        ////////////////////////////////////////        
        $('#hlblEID')[0].innerHTML = '';
        $('#hlblPointsType')[0].innerHTML = '';
        $('#txtNumPts').val('');
        $('#ddTypes').val('');
        $('#ddEmpIds').val('');
        $('#txtEntryDate').val('');

        //////////////////////////////
        // Clear Any Previous Error //
        //////////////////////////////
        $('#lblErr')[0].innerHTML = '';
        $('#lblErrServer')[0].innerHTML = '';

        //////////////////////////////////////////////
        // Set Focus To First Data Collection Field //
        //////////////////////////////////////////////
        validate();
        $("#ddEmpIds").focus();
    });


    Date.prototype.getWeek = function () {
        var onejan = new Date(this.getFullYear(), 0, 1);
        return Math.ceil((((this - onejan) / 86400000) + onejan.getDay() + 1) / 7);
    };

    $('#EmpPointsRpt').jtable({
        title: 'Previously Recorded Points',
        edit: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'DatePointsGiven ASC',

        actions: {
            listAction: '/SIU_DAO.asmx/GetEmpIdPoints',
            deleteAction: '/SIU_DAO.asmx/RemoveEmpIdPoints'
        },
        fields: {
            UID: {
                title: 'No',
                width: '3%',
                key: true,
                create: false,
                edit: false,
                list: false
            },
            EventDate: {
                title: 'Date',
                type: 'date',
                sorting: true,
                width: '1.5%',
                listClass: 'jTableTD'
            },
            Points: {
                title: 'Pts',
                sorting: true,
                width: '.5%',
                listClass: 'jTableTD'
            },
            ReasonForPoints: {
                title: 'Points For',
                sorting: true,
                width: '3%',
                listClass: 'jTableTD',
                display: function (data) { return listOfTypes[data.record.ReasonForPoints - 1]; }
            },
            Emp_No: { list: false },
            PointsGivenBy: { list: false },
            Comments: { list: false },
            DatePointsGiven: { list: false }
        }
    });



    /////////////////////////////
    // Form Validation Section //
    // Emp Id Blur = Validate  //
    /////////////////////////////
    $('#ddEmpIds').blur(function () {
        validate();
        if ($('#hlblEID')[0].innerHTML.length > 0 || $('#ddEmpIds').val().length > 0) {
            if ($('#txtEntryDate').val().length == 0)
                $('#txtEntryDate').focus();
            else
                $('#ddTypes').focus();
        }
    });

    /////////////////////////////
    // Form Validation Section //
    // Catch Tab Key           //
    /////////////////////////////
    $('#ddEmpIds').keydown(function (e) {
        var code = e.keyCode || e.which;
        if (code == '9') {
            if ($('#txtEntryDate').val().length == 0) 
                $('#txtEntryDate').focus();
            else
                $('#ddTypes').focus();
            return false;
        }
    });


    //////////////////////////////////
    // Form Validation Section      //
    // Earned Date Blur = Validate  //
    //////////////////////////////////
    $('#txtEntryDate').blur(function () {
        validate();
        $('#ddTypes').focus();
    });

    //////////////////////////////
    // Form Validation Section  //
    // Pts Type Blur = Validate //
    //////////////////////////////
    $('#ddTypes').blur(function () {
        validate();
        if ($('#hlblPointsType')[0].innerHTML.length > 0 || $('#ddTypes').val().length > 0)
            $('#txtNumPts').focus();
    });
        

    /////////////////////////////
    // Form Validation Section //
    // No Pts Blur = Validate  //
    /////////////////////////////
    $('#txtNumPts').blur(function () {
        validate();
        if ( $('#btnSubmit').prop('disabled') == false )
            $("#btnSubmit").focus();
        else
            $('#ddEmpIds').focus();
        return false;
    });


    $('#txtNumPts').keyup(function () {
        validate();
    });


    /////////////////////////////////
    // Load Emps AutoComplete List //
    /////////////////////////////////
    var GetEmpsCall = new AsyncServerMethod();
    GetEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", GetEmps_success);
    
    ////////////////////////////////////////
    // Load Points Type AutoComplete List //
    ////////////////////////////////////////
    var GetTypesCall = new AsyncServerMethod();
    GetTypesCall.exec("/SIU_DAO.asmx/GetAutoCompletePointTypes", getTypesSuccess);


    var timestamp = new Date();
    $('#txtEntryDate').val(timestamp.getMonth() + 1 + "/" + timestamp.getDate() + "/" + timestamp.getFullYear() );
    validate();

    $('#ddEmpIds').focus();
});