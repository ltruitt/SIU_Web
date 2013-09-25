$(document).ready(function () {

    ///////////////////////////////////////////
    // Get Jason Array Of Total Hours By Day //
    ///////////////////////////////////////////
    var summaryCounts;
    function getSafetyPaysSummaryCountsSuccess(data) {
        summaryCounts = $.parseJSON(data.d);
        showSummary();
    }
    function getMySiSum() {
        var getSafetyPaysSummaryCountsAjax = new AsyncServerMethod();
        getSafetyPaysSummaryCountsAjax.exec("/SIU_DAO.asmx/GetSafetyPaysSummaryCounts", getSafetyPaysSummaryCountsSuccess);
    }
    getMySiSum();



    function showSummary() {

        //SummaryCounts.QomCurrentEnd
        //SummaryCounts.QomCurrentQiD

        //SummaryCounts.QomCurrentStart
        //SummaryCounts.QomNextEnd
        //SummaryCounts.QomNextStart




        $('#SafetyPaysTotalWaiting')[0].innerHTML = summaryCounts.SafetyPaysTotalWaiting + ' New Submissions';
        $('#SafetyPaysTotalNotTask')[0].innerHTML = summaryCounts.SafetyPaysTotalNotTask + ' Need Task';
        $('#SafetyPaysTotalLateTask')[0].innerHTML = summaryCounts.SafetyPaysTotalLateTask + ' Late Tasks';
        $('#SafetyPaysTotalLateStatus')[0].innerHTML = summaryCounts.SafetyPaysTotalLateStatus + ' Late Status';
        $('#SafetyPaysTotalCloseReady')[0].innerHTML = summaryCounts.SafetyPaysTotalCloseReady + ' Ready To Close';

        //$('#SafetyQoMAnsAdminLink')[0].innerHTML = summaryCounts.QomCurrentWaitingResponses + ' Q. O. M. Responses Waiting';






    }





});