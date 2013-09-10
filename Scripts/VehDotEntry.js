$(document).ready(function () {

    $('#jTableContainer').jtable({
        title: '<b>Unresolved Hazards Report --- Select Line To Update</b>',
        edit: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'SubmitTimeStamp ASC',
        loadingAnimationDelay: 0,

        actions: {

            listAction: '/SIU_DAO.asmx/GetOpenDOT'
        },
        fields: {
            SubmitTimeStamp: {
                title: 'Date',
                sorting: true,
                width: '2%',
                listClass: 'jTableTD'
            },
            Vehicle: {
                title: 'Veh',
                width: '1%',
                list: true,
                sorting: true
            },
            Hazard: {
                title: 'Hazard',
                sorting: false,
                width: '3%',
                listClass: 'jTableTD',
                display: function (data) { return showHazard(data.record.Hazard, data.record.Correction); }
            },
            SubmitEmpID: { list: false },
            CorrectionTimeStamp: { list: false },
            CorrectionEmpID: { list: false },
            RefID: { list: false, key: true },
            Make: { list: false },
            Model: { list: false },
            PLate: { list: false },
            SubmitEmpName: { list: false }
        },
        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableContainer').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    $('#txtVehicleNo').val(record.Vehicle);
                    $('#txtHazard').val(record.Hazard);
                    $('#txtCorrection').val('');

                    $('#txtEntryDate')[0].innerHTML = record.SubmitTimeStamp;
                    $('#hlblRefID')[0].innerHTML = record.RefID;
                    $('#txtHazard').attr("readonly", true);
                    validate();
                    $('#txtCorrection').focus();
                });
            } else {
                //No rows selected
                $('#txtVehicleNo').val('');
                $('#txtHazard').val('');
                $('#txtCorrection').val('');
                $('#hlblRefID')[0].innerHTML = 0;
                $('#txtHazard').attr("readonly", false);
                validate();
            }
        }
    });



    function showHazard(hazard, correction) {
        if (correction == null) {
            return '<span style="color: red; font-weight: bold;">' + hazard + '</span>';
            //return '<span style="color: red; font-weight: bold;">' + Hazard.substring(0, 20) + '</span>';
        }


        if (correction.length == 0) {
            return '<span style="color: red; font-weight: bold;">' + hazard.substring(0, 20) + '</span>';
        } else {
            return hazard.substring(0, 20);
        }
    }


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
        showDowColor();
    });


    $('.nextarrow').click(function () {
        var idx = parseInt($('#hlblWeekIdx')[0].innerHTML);
        if (idx < 0)
            idx = idx + 1;

        $('#hlblWeekIdx')[0].innerHTML = idx;
        showDowColor();
    });

    function showDowColor() {
        var idx = parseInt($('#hlblWeekIdx')[0].innerHTML);

        if (idx == 0)
            $(".DowBtnCSS").css("background-color", "green");

        if (idx == -1)
            $(".DowBtnCSS").css("background-color", "red");

        if (idx == 1)
            $(".DowBtnCSS").css("background-color", "orange");
    }



    ////////////////////////////////////////////////////////////////
    // Catch Click Event For DOW Buttons And Update DOW Text Box  //
    ////////////////////////////////////////////////////////////////
    $(document).on('click', '.DowBtnCSS', function () {
        var idx = parseInt($('#hlblWeekIdx')[0].innerHTML);

        if (idx == 0)
            $('#txtEntryDate')[0].innerHTML = this.getAttribute('DateForThis');

        if (idx == -1)
            $('#txtEntryDate')[0].innerHTML = this.getAttribute('DateForPrev');

        if (idx == 1)
            $('#txtEntryDate').val(this.getAttribute('DateForNext'));

        $('.DowBtnCSS').removeClass('DowBtnSelectedCss');
        $(this).addClass('DowBtnSelectedCss');
    });



    $('#txtHazard').focus(function () {
        $('#txtHazard').css('height', '80px');
        $('#txtCorrection').css('height', '20px');

        $('#txtHazardDiv').css('height', '80px');
        $('#txtCorrectionDiv').css('height', '20px');
    });



    $('#txtCorrection').focus(function () {
        $('#txtHazard').css('height', '20px');
        $('#txtCorrection').css('height', '80px');

        $('#txtHazardDiv').css('height', '20px');
        $('#txtCorrectionDiv').css('height', '80px');
    });


    $('#txtHazard').keyup(function () {
        validate();
    });

    $('#txtCorrection').keyup(function () {
        validate();
    });

    $('#txtVehicleNo').keyup(function () {
        validate();
    });


    function resetTextBoxSize() {
        $('#txtHazard').css('height', '20px');
        $('#txtCorrection').css('height', '20px');
        $('#txtHazardDiv').css('height', '20px');
        $('#txtCorrectionDiv').css('height', '20px');
    }



    var listOfVehs = [];
    function getVehilceMileageRptAvailSuccess(data) {
        var d = data.d.replace(/\"/g, "").replace(/\[/g, "").replace(/\}/g, "");
        listOfVehs = d.split(",");

        $("#txtVehicleNo").autocomplete({ source: listOfVehs },
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
        getVehilceMileageRptAvailAjax.add('WeekEnding', weekEnding );
        getVehilceMileageRptAvailAjax.exec("/SIU_DAO.asmx/GetVehilceMileageRptAvail", getVehilceMileageRptAvailSuccess);
    }


    function getPersonalVehListSuccess(data) {
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
        validate();
        $('#txtHazard').focus();
    }
    function getPersonalVehList() {
        var weekEndingParts = $('#txtEntryDate')[0].innerHTML.split(" ");
        var weekEnding = weekEndingParts[weekEndingParts.length - 1];
        var getVehicleAssignedAjax = new AsyncServerMethod();

        getVehicleAssignedAjax.add('empNo', $('#hlblEID')[0].innerHTML );
        getVehicleAssignedAjax.add('WeekEnding', weekEnding );
        getVehicleAssignedAjax.exec("/SIU_DAO.asmx/GetVehicleAssigned", getPersonalVehListSuccess);
    }


    $('.AssignedVehicleSelected').click( function () {
        $('#txtVehicleNo')[0].value = $(this)[0].title;
        $('#txtHazard').focus();
    });



    function validate() {
        $('#btnSubmit').show();
        $('#btnNoHaz').show();
        
        $('#txtHazard').removeClass('ValidationError');
        $('#txtVehicleNo').removeClass('ValidationError');
        $('#txtHazard').removeClass('ValidationSuccess');
        $('#txtVehicleNo').removeClass('ValidationSuccess');


        if ($('#txtVehicleNo').val().length == 0) {
            $('#txtVehicleNo').addClass('ValidationError');
            $('#btnSubmit').hide();
            $('#btnNoHaz').hide();
        } else {
            $('#txtVehicleNo').addClass('ValidationSuccess');
        }

        if ($('#txtHazard').val().length == 0) {
            $('#txtHazard').addClass('ValidationError');
            $('#btnSubmit').hide();
        } else {
            $('#txtHazard').addClass('ValidationSuccess');
        }
    }

    $('#btnNoHaz').click(function () {
        $('#txtHazard').val('No Hazard Found');
        $('#txtCorrection').val('No Action Required');
        validate();
        //$('#btnSubmit').click();
    });


    ////////////
    // Submit //
    ////////////
    function recordDotSuccess() {
        //alert("Thank you for submitting your inspection. Click on the Report link above to see all of your reports.");
        clear();
    }
    
    $('#btnSubmit').click(function () {
        var refId = $('#hlblRefID')[0].innerHTML;
        if (refId.length == 0)
            refId = 0;

        var recordDotAjax = new AsyncServerMethod();
        recordDotAjax.add('RefID',             refId );
        recordDotAjax.add('EmpID',             $('#hlblEID')[0].innerHTML );
        recordDotAjax.add('InspDate',          $('#txtEntryDate')[0].innerHTML );
        recordDotAjax.add('VehNo',             $('#txtVehicleNo').val() );
        recordDotAjax.add('Hazard',            $('#txtHazard').val() );
        recordDotAjax.add('CorrectiveAction',  $('#txtCorrection').val() );
        recordDotAjax.exec("/SIU_DAO.asmx/RecordDot", recordDotSuccess);
    });

    $('#btnClear').click(function () {
        clear();
    });

    function clear() {
        resetTextBoxSize();
        $('#hlblRefID')[0].innerHTML = '';
        $('#txtCorrection')[0].innerHTML = '';
        $('#txtCorrection').val('');
        $('#txtHazard').val('');
        $('#txtHazard').attr("readonly", false);

        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });

        validate();

        if ($('#txtVehicleNo').val().length == 0) {
            $('#txtVehicleNo').focus();
        } else {
            $('#txtHazard').focus();
        }
    }

    getAllVehList();
    getPersonalVehList();
    clear();


});