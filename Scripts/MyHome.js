$(document).ready(function () {


    ///////////////////////////////////////////
    // Get Jason Array Of Total Hours By Day //
    ///////////////////////////////////////////
    var summaryCounts;
    function getMySiSumSuccess(data) {
        summaryCounts = $.parseJSON(data.d);
        $('#WaitImg').hide();
        showSummary();
    }
    function getMySiSum() {
        hideAll();

        var getMySiSumAjax = new AsyncServerMethod();
        getMySiSumAjax.add('EmpID', $('#hlblEID')[0].innerHTML);
        getMySiSumAjax.exec("/SIU_DAO.asmx/GetMySiSummaryCounts", getMySiSumSuccess);
    }
    getMySiSum();



    function showSummary() {

        $('#liTimeStudy').show();
        $('#liBandC').show();
        $('#liMyYtdExp').show();
        


        $('#SpMyOpen')[0].innerHTML = summaryCounts.SpMyOpen;
        $('#SpMyAssigned')[0].innerHTML = summaryCounts.SpMyAssigned;
        $('#SpMyLateTask')[0].innerHTML = summaryCounts.SpMyLateTask;
        $('#SpMyLateStatus')[0].innerHTML = summaryCounts.SpMyLateStatus;
        if (summaryCounts.SpMyOpen == 0)
            $('#spOpen').hide();
        if (summaryCounts.SpMyAssigned == 0)
            $('#spAssigned').hide();
        if (summaryCounts.SpMyLateTask == 0)
            $('#spLateT').hide();
        if (summaryCounts.SpMyLateStatus == 0)
            $('#spLateS').hide();

        $('#QomOpen')[0].innerHTML = summaryCounts.QomOpen;
        $('#QomPend')[0].innerHTML = summaryCounts.QomPend;
        $('#QomAccept')[0].innerHTML = summaryCounts.QomAccept;
        
        


        if (summaryCounts.VehIssues == 1)
            $('#VehIssues')[0].innerHTML = summaryCounts.VehIssues;
        else
            $('#VehIssues')[0].innerHTML = summaryCounts.VehIssues;

        if (summaryCounts.VehIssues > 0)
            $('#liVehIssues').show('slow');




        if (summaryCounts.JobRptsInProgress == 1)
            $('#JobRptsInProgress')[0].innerHTML = summaryCounts.JobRptsInProgress + " Job Report ";
        else
            $('#JobRptsInProgress')[0].innerHTML = summaryCounts.JobRptsInProgress + " Job Reports ";

        if (summaryCounts.JobRptsInProgress > 0)
            $('#liJobRptsInProgress').show('slow');



        if (summaryCounts.JobRptsPastDue == 1)
            $('#JobRptsPastDue')[0].innerHTML = summaryCounts.JobRptsPastDue + " Job Report";
        else
            $('#JobRptsPastDue')[0].innerHTML = summaryCounts.JobRptsPastDue + " Job Reports";

        if (summaryCounts.JobRptsPastDue > 0)
            $('#liJobRptsPastDue').show('slow');



        if (summaryCounts.RejectedTime == 1)
            $('#RejectedTime')[0].innerHTML = summaryCounts.RejectedTime + " Rejected Time Record";
        else
            $('#RejectedTime')[0].innerHTML = summaryCounts.RejectedTime + " Rejected Time Records";

        if (summaryCounts.RejectedTime > 0)
            $('#liRejectedTime').show('slow');



        if (summaryCounts.VehicleMileageReported == 1)
            $('#VehicleMileageReported')[0].innerHTML = " DID";
        else
            $('#VehicleMileageReported')[0].innerHTML = " DID NOT";

        if (summaryCounts.VehicleMileageReported == 0)
            $('#liVehicleMileageReported').show('slow');


        if (summaryCounts.ExpiringBandC == 1)
            $('#ExpiringBandC')[0].innerHTML = "1 Badge or Certificate ";
        else
            $('#ExpiringBandC')[0].innerHTML = summaryCounts.ExpiringBandC + " Badges and/or Certificates ";

        if (summaryCounts.ExpiringBandC > 0)
            $('#liExpiringBandC').show('slow');



        //$('#MissedMeetings')[0].innerHTML = summaryCounts.MissedMeetings;
        //if (summaryCounts.SummaryCounts > 0)
        //    $('#liMissedMeetings').show('slow');



        if (summaryCounts.OpenHwReq == 1)
            $('#OpenHwReq')[0].innerHTML = "1 Hardware Request ";
        else
            $('#OpenHwReq')[0].innerHTML = summaryCounts.OpenHwReq + " Hardware Requests ";

        if (summaryCounts.OpenHwReq > 0)
            $('#liOpenHwReq').show('slow');



        //if (SummaryCounts.OpenBugReq == 1)
        //    $('#OpenBugReq')[0].innerHTML = "1 Bug Report ";
        //else
        //    $('#OpenBugReq')[0].innerHTML = SummaryCounts.OpenBugReq + " Bug Reports ";

        //if (SummaryCounts.OpenBugReq > 0)
        //    $('#liOpenBugReq').show('slow');



        if (summaryCounts.HoursThisWeek == 1)
            $('#HoursThisWeek')[0].innerHTML = "1 Hour";
        else
            $('#HoursThisWeek')[0].innerHTML = summaryCounts.HoursThisWeek + " Hours";

        if (summaryCounts.HoursToday == 0)
            $('#HoursToday')[0].innerHTML = "Please turn in todays hours.";
        $('#liHoursThisWeek_HoursToday').show('slow');



        $('#PHoliday')[0].innerHTML = summaryCounts.PHoliday;
        $('#Vacation')[0].innerHTML = summaryCounts.Vacation;
        $('#Sick')[0].innerHTML = summaryCounts.Sick;


        $('#ExpCnt').html(summaryCounts.ExpCnt);
        $('#ExpAmt').html('$' + summaryCounts.ExpAmt);

        if ( summaryCounts.ExpCnt > 0 )
            $('#liExpCntExpAmt').show('slow');
    }

    function hideAll() {
        $('#WaitImg').show();
        $('#LiSafetyPays').hide();
        $('#liHoursThisWeek_HoursToday').hide();
        $('#l1AccruedTime').hide();
        $('#liExpCntExpAmt').hide();
        //$('#liMyYtdExp').hide();
        $('#liJobRptsInProgress').hide();
        $('#liJobRptsPastDue').hide();
        $('#liRejectedTime').hide();
        $('#liVehicleMileageReported').hide();
        $('#liVehicleMileageReport').hide();
        $('#liExpiringBandC').hide();
        $('#liOpenHwReq').hide();
        $('#liOpenBugReq').hide();
        $('#liExpCnt_ExpAmt').hide();
        //$('#liMissedMeetings').hide();
        $('#liVehIssues').hide();
        
        $('#liTimeStudy').hide();
        $('#liBandC').hide();
        $('#liMyYtdExp').hide();
    }
    hideAll();


    $('#btnShowAll').click(function () {
        $('#liJobRptsInProgress').show('slow');
        $('#liJobRptsPastDue').show('slow');
        $('#liRejectedTime').show('slow');
        $('#liVehicleMileageReported').show('slow');
        $('#liExpiringBandC').show('slow');
        $('#liOpenHwReq').show('slow');
        //$('#liOpenBugReq').show('slow');
        $('#liExpCnt_ExpAmt').show('slow');
        //$('#liMissedMeetings').show('slow');
        $('#liExpCntExpAmt').show('slow');
        $('#liVehIssues').show('slow');

        $('#btnShowAll').hide();
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
                    getMySiSum();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = dataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);

                        getMySiSum();
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
});