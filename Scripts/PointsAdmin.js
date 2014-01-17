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
                    $('#ddTypes').focus();

                    var timestamp = new Date();
                    $('#EmpPointsRpt').jtable('load', { EmpID: dataPieces[0], T: timestamp.getTime() });
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = dataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                        $('#ddTypes').focus();

                        var timestamp = new Date();
                        $('#EmpPointsRpt').jtable('load', { EmpID: dataPieces[0], T: timestamp.getTime() });
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
                    var dataPieces = ui.item.value.split(' ');
                    $("#txtNumPts").val(listOfPoints[dataPieces[0] - 1]);

                    dataPieces = ui.item.value.split('-');
                    $('#hlblPointsType')[0].innerHTML = dataPieces[0];
                    $("#ddTypes").autocomplete("close");
                    ui.item.value = dataPieces[1].replace(' ', '');
                    $('#txtNumPts').focus();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split('-');
                        $('#hlblPointsType')[0].innerHTML = dataPieces[0];
                        $("#ddTypes").autocomplete("close");
                        $("#ddTypes").val(dataPieces[1].replace(' ', ''));

                        dataPieces = ui.content[0].value.split(' ');
                        $("#txtNumPts").val(listOfPoints[dataPieces[0] - 1]);
                        $('#txtNumPts').focus();
                    }
                    else {
                        $('#hlblPointsType')[0].innerHTML = '';
                    }

                    return ui;
                }
            });
    }

    $("#datacollection").delegate("*", "focus blur", function () {
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

        var pointsSubmitCall = new AsyncServerMethod();
        pointsSubmitCall.add('EmpID', $('#hlblEID')[0].innerHTML);
        pointsSubmitCall.add('ReasonCode', $('#hlblPointsType')[0].innerHTML);
        pointsSubmitCall.add('Points', $('#txtNumPts').val() );
        pointsSubmitCall.add('Comment', '');
        pointsSubmitCall.add('DateOfEvent', $('#txtEntryDate').val());
        pointsSubmitCall.add('UID', $('#hlblUID').html());
        
        pointsSubmitCall.exec("/SIU_DAO.asmx/Xxx5554S", submitSuccess);
    });


    ///////////////////////////
    // Submit Button Handler //
    ///////////////////////////
    function submitSuccess(data) {
        var errorMsg = data.d;

        if (errorMsg.length > 6 && errorMsg.substring(0, 7) == "Success") {

            ////////////////////
            // Reset The Form //
            ////////////////////
            $('#hlblPointsType')[0].innerHTML = '';
            $('#txtNumPts').val('');
            $('#ddTypes').val('');
            $('#hlblUID').html('');
            $('#txtEntryDate').val('');

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

        /////////////////
        // Clear Table //
        /////////////////
        $('#EmpPointsRpt').jtable('selectRows', $('*'));
        var $selectedRows = $('#EmpPointsRpt').jtable('selectedRows');
        $selectedRows.each(function () {
            var record = $(this).data('record');
            $('#EmpPointsRpt').jtable('deleteRecord', {
                key: record.UID,
                clientOnly: true,
                animationsEnabled: true
            });
        });
        $('#EmpPointsRpt').jtable('selectRows', $('xxx'));

        ////////////////////////////////////////
        // Clear Data Confirmation Containers //
        ////////////////////////////////////////        
        $('#hlblEID')[0].innerHTML = '';
        $('#hlblUID').html('');
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
            listAction: '/SIU_DAO.asmx/k00PY34',
            deleteAction: '/SIU_DAO.asmx/GhjjI0E'
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
                title: 'Event Date',
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
        },
        selectionChanged: function () {
            $('#hlblUID').html('');
            //Get all selected rows
            var $selectedRows = $('#EmpPointsRpt').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows

                $selectedRows.each(function () {
                    var record = $(this).data('record');

                    $('#ddTypes').val(function () { return listOfTypes[record.ReasonForPoints - 1]; });
                    $('#hlblPointsType').html(record.ReasonForPoints);
                    $('#hlblUID').html(record.UID);
                    $('#txtNumPts').val(record.Points);
                    $('#txtEntryDate').datepicker("setDate", $.fn.parseJsonDate(record.EventDate));
                    validate();
                });
            }
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
        //else
        //    $('#ddEmpIds').focus();
        return false;
    });


    $('#txtNumPts').keyup(function () {
        validate();
    });


    /////////////////////////////////
    // Load Emps AutoComplete List //
    /////////////////////////////////
    var getEmpsCall = new AsyncServerMethod();
    getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);
    
    ////////////////////////////////////////
    // Load Points Type AutoComplete List //
    ////////////////////////////////////////
    var getTypesCall = new AsyncServerMethod();
    getTypesCall.exec("/SIU_DAO.asmx/GetAutoCompletePointTypes", getTypesSuccess);


    //var timestamp = new Date();
    //$('#txtEntryDate').val(timestamp.getMonth() + 1 + "/" + timestamp.getDate() + "/" + timestamp.getFullYear() );
    validate();

    $('#ddEmpIds').focus();
});