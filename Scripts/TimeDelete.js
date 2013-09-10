$(document).ready(function () {


    var AllowedDays = $.parseJSON($('#hUnapprovedDates')[0].innerHTML);
    var StartDate = new Date(AllowedDays[0]);
    var EndDate = new Date(AllowedDays[AllowedDays.length - 1]);

    function MustbeInList(CalDate) {

        for (var c = 0; c < AllowedDays.length; c++) {
            var xxx = CalDate.getMonth();
            var CalDayStr = '%02d/%02d/%04d'.sprintf(
                        CalDate.getMonth() + 1,
                        CalDate.getDate(),
                        CalDate.getFullYear());

            if (AllowedDays[c] == CalDayStr)
                return [true];
        }

        return [false];
    };

    function DateChange(dateText, inst) {
        GetHoursByDay();
    }


    function ShowHours(Idx) {

        //////////////////////////
        // Clear Record Details //
        //////////////////////////
        $('#lblDeptSelection')[0].innerHTML = "INAVLID";
        $('#lblJobNoSelection')[0].innerHTML = "INVALID";
        $('#lblTaskCodeSelection')[0].innerHTML = "INVALID";
        $('#lblOhAcctSelection')[0].innerHTML = "INVALID";
        $('#lblStatusSelection')[0].innerHTML = "INVALID";
        $('#lblST')[0].innerHTML = "";
        $('#lblOT')[0].innerHTML = "";
        $('#lblDT')[0].innerHTML = "";
        $('#lblAB')[0].innerHTML = "";
        $('#lblHT')[0].innerHTML = "";

        var xxx = $('#hlblEntryNo');
        $('#hlblEntryNo')[0].value = "INVALID";

        ///////////////////////////////////////
        // Don't Proceed If There Is No Data //
        ///////////////////////////////////////
        if (Idx > DailyHoursDetails.length)
            return;
        if (DailyHoursDetails.length == 0)
            return;

        ////////////////////////////
        // Display Record Details //
        ////////////////////////////
        $('#lblDeptSelection')[0].innerHTML = DailyHoursDetails[Idx].Shortcut_Dimension_1_Code;
        $('#lblJobNoSelection')[0].innerHTML = DailyHoursDetails[Idx].Job_No_;
        $('#lblTaskCodeSelection')[0].innerHTML = DailyHoursDetails[Idx].Task_Code;
        $('#lblOhAcctSelection')[0].innerHTML = DailyHoursDetails[Idx].Pay_Posting_Group;

        $('#lblStatusSelection')[0].innerHTML = StatusToText(DailyHoursDetails[Idx].Status);

        $('#lblST')[0].innerHTML = DailyHoursDetails[Idx].Straight_Time;
        $('#lblOT')[0].innerHTML = DailyHoursDetails[Idx].Over_Time;
        $('#lblDT')[0].innerHTML = DailyHoursDetails[Idx].Double_Time;
        $('#lblAB')[0].innerHTML = DailyHoursDetails[Idx].Absence_Time;
        $('#lblHT')[0].innerHTML = DailyHoursDetails[Idx].Holiday_Time;
        $('#hlblEntryNo')[0].value = DailyHoursDetails[Idx].Entry_No_;

        //////////////////////////////////////////
        // Show This Rcd And Total Record Count //
        //////////////////////////////////////////
        $('#lblThisRcd')[0].innerHTML = Idx + 1;
        $('#lblTotRcds')[0].innerHTML = DailyHoursDetails.length;

        ///////////////////////////////////////////////
        // Hide Next Button If We Are At Last Record //
        ///////////////////////////////////////////////
        if (DailyHoursDetails.length <= Idx + 1)
            $('#btnNext').hide();
        else
            $('#btnNext').show();

        ///////////////////////////////////////////////
        // Hide Prev Button If We Are At First Record //
        ///////////////////////////////////////////////
        if (Idx == 0)
            $('#btnPrev').hide();
        else
            $('#btnPrev').show();

    }


    $("#btnNext").click(function () {
        var Idx = parseInt($('#lblThisRcd')[0].innerHTML);
        ShowHours(Idx);
    });

    $("#btnPrev").click(function () {
        var Idx = $('#lblThisRcd')[0].innerHTML;
        ShowHours(Idx - 2);
    });

    function StatusToText(StatusValue) {
        switch (StatusValue) {
            case 0:
                return "New";
                break;
            case 1:
                return "Pending Employee Approval";
                break;
            case 2:
                return "Employee Approved";
                break;
            case 3:
                return "Pending Verification";
                break;
            case 4:
                return "Being Reviewed by Verifier";
                break;
            case 5:
                return "Pending Manager Approval";
                break;
            case 6:
                return "Manager Approved";
                break;
            case 7:
                return "Rejected";
                break;
            case 8:
                return "Posted";
                break;
            default:
                return "INVALID !!!!!";
                break;
        }
    }


    $('#datepicker').datepicker({
        minDate: StartDate,
        maxDate: EndDate,
        //dateFormat: 'DD, MM, d, yy',
        beforeShowDay: MustbeInList,
        constrainInput: true,
        onSelect: DateChange
    });


    var DailyHoursDetails;
    function GetTimeUnapproved_success(data) {
        DailyHoursDetails = $.parseJSON(data.d);
        $('#lblTotRcds')[0].innerHTML = DailyHoursDetails.length;
        ShowHours(0);        
    }
    

    function GetHoursByDay() {
        DailyHoursDetails = null;

        var GetTimeUnapproved_Ajax = new AsyncServerMethod();
        GetTimeUnapproved_Ajax.add('EmpID',     $('#hlblEID')[0].innerHTML );
        GetTimeUnapproved_Ajax.add('Date',      $('#datepicker')[0].value);
        GetTimeUnapproved_Ajax.exec("/SIU_DAO.asmx/GetTimeUnapproved", GetTimeUnapproved_success);
    }
    GetHoursByDay();
});