$(document).ready(function () {

    ///////////////////////////////////////////
    // Get Jason Array Of Total Hours By Day //
    ///////////////////////////////////////////
    var SummaryCounts;
    function GetSafetyPaysSummaryCounts_success(data) {
        SummaryCounts = $.parseJSON(data.d);
        showSummary();
    }
    function GetMySiSum() {
        var GetSafetyPaysSummaryCounts_Ajax = new AsyncServerMethod();
        GetSafetyPaysSummaryCounts_Ajax.exec("/SIU_DAO.asmx/GetSafetyPaysSummaryCounts", GetSafetyPaysSummaryCounts_success);
    }
    GetMySiSum();



    function showSummary() {

        //SummaryCounts.QomCurrentEnd
        //SummaryCounts.QomCurrentQiD

        //SummaryCounts.QomCurrentStart
        //SummaryCounts.QomNextEnd
        //SummaryCounts.QomNextStart




        $('#SafetyPaysTotalWaiting')[0].innerHTML = SummaryCounts.SafetyPaysTotalWaiting + ' New Submissions';
        $('#SafetyPaysTotalNotTask')[0].innerHTML = SummaryCounts.SafetyPaysTotalNotTask + ' Need Task';
        $('#SafetyPaysTotalLateTask')[0].innerHTML = SummaryCounts.SafetyPaysTotalLateTask + ' Late Tasks';
        $('#SafetyPaysTotalLateStatus')[0].innerHTML = SummaryCounts.SafetyPaysTotalLateStatus + ' Late Status';
        $('#SafetyPaysTotalCloseReady')[0].innerHTML = SummaryCounts.SafetyPaysTotalCloseReady + ' Ready To Close';

        $('#SafetyQoMAnsAdminLink')[0].innerHTML = SummaryCounts.QomCurrentWaitingResponses + ' Q. O. M. Responses Waiting';






    }





});