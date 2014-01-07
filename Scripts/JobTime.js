$(document).ready(function () {
    
    ////////////////////////////////////////
    // Event Management For Job Selection //
    ////////////////////////////////////////
    var listOfJobs = [];
    function GetTimeJobs_success(data) {

        listOfJobs = data.d.split("\r");

        $("#acJobNo").autocomplete({ source: listOfJobs },
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
                    $(this).val(DataPieces[0].replace(/\n/g, ""));
                    GetJobDetails();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $(this).val(DataPieces[0].replace(/\n/g, ""));
                        GetJobDetails();
                        $("#acJobNo").autocomplete("close");
                    }

                    return ui;
                }
            });
    }

    function GetTimeJobs() {
        var GetTimeJobsCall = new AsyncServerMethod();
        GetTimeJobsCall.exec("/SIU_DAO.asmx/Affe31", GetTimeJobs_success);
    }
    GetTimeJobs();








    //////////////////////////////////////////////////////
    // Make Sure Text Entered For Jobs Was In Jobs List //
    //////////////////////////////////////////////////////
    $("#acJobNo").blur(function () {
        if (!listOfJobs.containsCaseInsensitive(this.value)) {
            $(this).val("");
        }
    });









    function GetJobDetails() {
        var GetTimeJobCall = new AsyncServerMethod();
        GetTimeJobCall.add('jobNo', $('#acJobNo').val());
        GetTimeJobCall.exec("/SIU_DAO.asmx/Gffeop1", GetTimeJob_success);
    }


    function GetTimeJob_success(data) {
        var JobDetails = $.parseJSON(data.d);
        $('#hlblJobNoSelection')[0].innerHTML = JobDetails.JobNo;

        $('#lblJobNoSelection')[0].innerHTML = "<b>Job No:</b> " + JobDetails.JobNo + "<br/>";
        $('#lblJobSiteSelection')[0].innerHTML = "<b>Customer:</b> " + JobDetails.JobCust + "<br/>";
        $('#lblJobDescSelection')[0].innerHTML = "<b>Job:</b> " + JobDetails.JobDesc + "<br/>";
    }
    

});