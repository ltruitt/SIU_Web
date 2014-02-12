$(document).ready(function () {

    var timestamp = new Date();


    $('#StartDate').datepicker({
        constrainInput: true,
        onSelect: function () {
            lookUp();
        }
    });

    $('#EndDate').datepicker({
        constrainInput: true,
        onSelect: function () {
            lookUp();
        }
    });




    function defaultQuarter() {
        var d = new Date();
        var thisQ = d.getMonth();
        var thisY = d.getFullYear();

        if (thisQ < 3)
            thisY--;

        var quarter = Math.floor((d.getMonth() / 3));

        var firstDate = new Date(d.getFullYear(), quarter * 3, 1);
        $("#StartDate").datepicker("setDate", firstDate);
        $("#EndDate").datepicker("setDate", new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0));
    }
    defaultQuarter();




    function setQuarter(quarter) {
        var d = new Date();
        var thisQ = d.getMonth();
        var thisY = d.getFullYear();

        if (thisQ < 1)
            if (quarter == 4)
                thisY--;

        quarter = quarter - 1;

        var firstDate = new Date(thisY, quarter * 3, 1);
        $("#StartDate").datepicker("setDate", firstDate);
        $("#EndDate").datepicker("setDate", new Date(thisY, firstDate.getMonth() + 3, 0));
    }

    $('#btnQ1').click(function () {
        setQuarter(1);
        lookUp();
    });

    $('#btnQ2').click(function () {
        setQuarter(2);
        lookUp();
    });

    $('#btnQ3').click(function () {
        setQuarter(3);
        lookUp();
    });

    $('#btnQ4').click(function () {
        setQuarter(4);
        lookUp();
    });




    $('.MonBtn').click(function () {

        switch (this.value.toLowerCase()) {

            case 'jan':
                setMonth(1);
                break;

            case 'feb':
                setMonth(2);
                break;

            case 'mar':
                setMonth(3);
                break;

            case 'apr':
                setMonth(4);
                break;

            case 'may':
                setMonth(5);
                break;

            case 'jun':
                setMonth(6);
                break;

            case 'jul':
                setMonth(7);
                break;

            case 'aug':
                setMonth(8);
                break;

            case 'sep':
                setMonth(9);
                break;

            case 'oct':
                setMonth(10);
                break;
            case 'nov':
                setMonth(11);
                break;
            case 'dec':
                setMonth(12);
                break;
        }

    });


    function setMonth(mon) {

        mon = mon - 1;

        var d = new Date();
        var firstDate = new Date(d.getFullYear(), mon, 1);
        var lastDate = new Date(d.getFullYear(), mon + 1, 0);

        $("#StartDate").datepicker("setDate", firstDate);
        $("#EndDate").datepicker("setDate", lastDate);
        lookUp();
        return;
    }

    function lookUp() {

        if ($('#StartDate').val().length == 0)
            return;

        if ($('#EndDate').val().length == 0)
            return;

        var postdata = $("#ExpTbl").jqGrid('getGridParam', 'postData');
        postdata.startDate = $('#StartDate').val();
        postdata.endDate = $('#EndDate').val();
        postdata.T = timestamp.getTime();
        jQuery("#ExpTbl").trigger("reloadGrid");
    }


});