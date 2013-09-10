$(document).ready(function () {


    ///////////////////////////////////////////
    // Get Jason Array Of Total Hours By Day //
    ///////////////////////////////////////////
    var SummaryCounts;
    function GetMySiSum_success(data) {
        SummaryCounts = $.parseJSON(data.d);
        $('#WaitImg').hide();
        showSummary();
    }
    function GetMySiSum() {
        hideAll();
        DailyHoursDetails = null;

        var GetMySiSum_Ajax = new AsyncServerMethod();
        GetMySiSum_Ajax.add('EmpID', $('#hlblEID')[0].innerHTML);
        GetMySiSum_Ajax.exec("/SIU_DAO.asmx/GetMySiSummaryCounts", GetMySiSum_success);
    }
    GetMySiSum();



    function showSummary() {

        $('#liTimeStudy').show();
        $('#liBandC').show();
        $('#liMyYtdExp').show();
        

        $('#SpMyOpen')[0].innerHTML = SummaryCounts.SpMyOpen;
        $('#SpMyAssigned')[0].innerHTML = SummaryCounts.SpMyAssigned;
        $('#SpMyLateTask')[0].innerHTML = SummaryCounts.SpMyLateTask;
        $('#SpMyLateStatus')[0].innerHTML = SummaryCounts.SpMyLateStatus;
        $('#LiSafetyPays').show();
        

        
        
        


        if (SummaryCounts.VehIssues == 1)
            $('#VehIssues')[0].innerHTML = SummaryCounts.VehIssues;
        else
            $('#VehIssues')[0].innerHTML = SummaryCounts.VehIssues;

        if (SummaryCounts.VehIssues > 0)
            $('#liVehIssues').show('slow');




        if (SummaryCounts.JobRptsInProgress == 1)
            $('#JobRptsInProgress')[0].innerHTML = SummaryCounts.JobRptsInProgress + " Job Report ";
        else
            $('#JobRptsInProgress')[0].innerHTML = SummaryCounts.JobRptsInProgress + " Job Reports ";

        if (SummaryCounts.JobRptsInProgress > 0)
            $('#liJobRptsInProgress').show('slow');



        if (SummaryCounts.JobRptsPastDue == 1)
            $('#JobRptsPastDue')[0].innerHTML = SummaryCounts.JobRptsPastDue + " Job Report";
        else
            $('#JobRptsPastDue')[0].innerHTML = SummaryCounts.JobRptsPastDue + " Job Reports";

        if (SummaryCounts.JobRptsPastDue > 0)
            $('#liJobRptsPastDue').show('slow');



        if (SummaryCounts.RejectedTime == 1)
            $('#RejectedTime')[0].innerHTML = SummaryCounts.RejectedTime + " Rejected Time Record";
        else
            $('#RejectedTime')[0].innerHTML = SummaryCounts.RejectedTime + " Rejected Time Records";

        if (SummaryCounts.RejectedTime > 0)
            $('#liRejectedTime').show('slow');



        if (SummaryCounts.VehicleMileageReported == 1)
            $('#VehicleMileageReported')[0].innerHTML = " DID";
        else
            $('#VehicleMileageReported')[0].innerHTML = " DID NOT";

        if (SummaryCounts.VehicleMileageReported == 0)
            $('#liVehicleMileageReported').show('slow');


        if (SummaryCounts.ExpiringBandC == 1)
            $('#ExpiringBandC')[0].innerHTML = "1 Badge or Certificate ";
        else
            $('#ExpiringBandC')[0].innerHTML = SummaryCounts.ExpiringBandC + " Badges and/or Certificates ";

        if (SummaryCounts.ExpiringBandC > 0)
            $('#liExpiringBandC').show('slow');



        $('#MissedMeetings')[0].innerHTML = SummaryCounts.MissedMeetings;
        if (SummaryCounts.SummaryCounts > 0)
            $('#liMissedMeetings').show('slow');



        if (SummaryCounts.OpenHwReq == 1)
            $('#OpenHwReq')[0].innerHTML = "1 Hardware Request ";
        else
            $('#OpenHwReq')[0].innerHTML = SummaryCounts.OpenHwReq + " Hardware Requests ";

        if (SummaryCounts.OpenHwReq > 0)
            $('#liOpenHwReq').show('slow');



        //if (SummaryCounts.OpenBugReq == 1)
        //    $('#OpenBugReq')[0].innerHTML = "1 Bug Report ";
        //else
        //    $('#OpenBugReq')[0].innerHTML = SummaryCounts.OpenBugReq + " Bug Reports ";

        //if (SummaryCounts.OpenBugReq > 0)
        //    $('#liOpenBugReq').show('slow');



        if (SummaryCounts.HoursThisWeek == 1)
            $('#HoursThisWeek')[0].innerHTML = "1 Hour";
        else
            $('#HoursThisWeek')[0].innerHTML = SummaryCounts.HoursThisWeek + " Hours";

        if (SummaryCounts.HoursToday == 0)
            $('#HoursToday')[0].innerHTML = "Please turn in todays hours.";
        $('#liHoursThisWeek_HoursToday').show('slow');



        $('#PHoliday')[0].innerHTML = SummaryCounts.PHoliday;
        $('#Vacation')[0].innerHTML = SummaryCounts.Vacation;
        $('#Sick')[0].innerHTML = SummaryCounts.Sick;
        $('#l1AccruedTime').show('slow');


        $('#ExpCnt')[0].innerHTML = SummaryCounts.ExpCnt;
        $('#ExpAmt')[0].innerHTML = '$' + SummaryCounts.ExpAmt;

        if ( SummaryCounts.ExpCnt > 0 )
            $('#liExpCntExpAmt').show('slow');

        //$('#liMyYtdExp').show('slow');
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
        $('#liMissedMeetings').hide();
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
        $('#liMissedMeetings').show('slow');
        $('#liExpCntExpAmt').show('slow');
        $('#liVehIssues').show('slow');

        $('#btnShowAll').hide();
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
                    GetMySiSum();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = DataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);

                        GetMySiSum();
                    }

                    return ui;
                }
            });
    }


    // Load Emps AutoComplete List
    if ($("#SuprArea").length > 0) {
        var GetEmpsCall = new AsyncServerMethod();
        GetEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", GetEmps_success);
    }
});