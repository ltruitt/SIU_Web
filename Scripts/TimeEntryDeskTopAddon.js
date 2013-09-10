$(document).ready(function () {

    var monthDailyChart;
    var monthDailyOptions = {
        chart: {
            renderTo: 'MonthDaily',
            type: 'column',
            marginRight: 80,
            marginBottom: 35,
            plotBackgroundColor: 'rgb(90, 90, 90)'

        },

        title: { text: 'Reporting Period Hours', x: -20 },
        
        yAxis: { title: { text: 'Hours' }, max: 20, tickInterval: 8, id: 'y-axis', plotLines: [{ color: '#FF0000', width: .5, value: 8 }] },
        tooltip: { formatter: function () { return this.y; } },
        legend: { layout: 'vertical', align: 'right', verticalAlign: 'top', x: -10, y: 20, borderWidth: 0 },
        plotOptions: { column: { stacking: 'normal'} },
        series: [{ name: 'HT' }, { name: 'AB' }, { name: 'DT' }, { name: 'OT' }, { name: 'ST'}],
        credits: { enabled: false }
    };


    ////////////////////////////////
    // Show Sum Of Hours For Week //
    ////////////////////////////////
    var weeklyHours = [];

    function getMoHours() {
        var getMoHoursAjax = new AsyncServerMethod();
        getMoHoursAjax.add('EmpID', $('#hlblEID')[0].innerHTML);
        getMoHoursAjax.add('SD', $('#hlblSD')[0].innerHTML);
        getMoHoursAjax.exec("/SIU_DAO.asmx/GetEloRptPeriodHours", getMoHoursSuccess);
    };

    function getMoHoursSuccess(data) {

        ////////////////////////////
        // Clean Up The Data Some //
        ////////////////////////////
        weeklyHours = data.d.replace('{', '').replace('}', '');
        weeklyHours = weeklyHours.split(/],/g);

        /////////////////////
        // Load Hours Data //
        // Idx 0 = HT      //
        // Idx 1 = AB      //
        // Idx 2 = DT      //
        // Idx 3 = OB      //
        // Idx 4 = ST      //
        // Idx 5 = Sum     //
        // Idx 7 = DOM     //
        /////////////////////
        var hoursIdx;
        for (var loadIdx = 0; loadIdx < 5; loadIdx++) {
            var hoursArr = weeklyHours[loadIdx].split(',');
            var hoursData = [];

            /////////////////////////////////////////////////
            // Assume 32 Days In A Month and Day 0 Invalid //
            /////////////////////////////////////////////////
            for (hoursIdx = 1; hoursIdx < hoursArr.length; hoursIdx++) {
                hoursData.push(parseFloat((hoursArr[hoursIdx])));
            }

            //////////////////////////////////
            // Load The Graph With The Data //
            //////////////////////////////////
            monthDailyOptions.series[loadIdx].data = hoursData;
        }
        var domCat = weeklyHours[7].split(',');

        ///////////////////////
        // Produce The Chart //
        ///////////////////////
        monthDailyChart = new Highcharts.Chart(monthDailyOptions);

        ///////////////////////////
        // Product X Axis Labels //
        ///////////////////////////
        var xAxisLabels = [];
        for (hoursIdx = 1; hoursIdx < hoursArr.length; hoursIdx++) {
            xAxisLabels.push(domCat[hoursIdx].replace(/"/g, '').replace(']', '')  );
        }
        monthDailyChart.xAxis[0].setCategories(xAxisLabels);
    };


    Highcharts.AppendHours = function (day, type, hours) {
        //////////////////////////////////////////////
        // Figure Out Which Series Array To Look At //
        //////////////////////////////////////////////
        var hoursTypeIdx = 0;
        switch (type) {
            case "ST": hoursTypeIdx = 4; break;
            case "OT": hoursTypeIdx = 3; break;
            case "DT": hoursTypeIdx = 2; break;
            case "AT": hoursTypeIdx = 1; break;
            case "HT": hoursTypeIdx = 0; break;
        }

        //////////////////////////
        // Get The Series Array //
        //////////////////////////
        var currentData = monthDailyChart.series[hoursTypeIdx].data;

        ///////////////////////////////////////
        // Find The Series Element (The Day) //
        ///////////////////////////////////////
        for (var dayIdx = 0; dayIdx < 21; dayIdx++) {
            if (currentData[dayIdx].category == day)
                break;
        }

        if (monthDailyChart.series[hoursTypeIdx].data[dayIdx] != undefined) {
            var curhours = monthDailyChart.series[hoursTypeIdx].data[dayIdx].y;
            monthDailyChart.series[hoursTypeIdx].data[dayIdx].update(y = hours + curhours);
        }
    };


    //function AppendHours(Day, Type, Hours) {
    //    //////////////////////////////////////////////
    //    // Figure Out Which Series Array To Look At //
    //    //////////////////////////////////////////////
    //    var HoursTypeIdx = 0;
    //    switch (Type) {
    //        case "ST": HoursTypeIdx = 4; break;
    //        case "OT": HoursTypeIdx = 3; break;
    //        case "DT": HoursTypeIdx = 2; break;
    //        case "AT": HoursTypeIdx = 1; break;
    //        case "HT": HoursTypeIdx = 0; break;
    //    }
    //    //////////////////////////
    //    // Get The Series Array //
    //    //////////////////////////
    //    var CurrentData = MonthDailyChart.series[HoursTypeIdx].data;
    //    ///////////////////////////////////////
    //    // Find The Series Element (The Day) //
    //    ///////////////////////////////////////
    //    for (var DayIdx = 1; DayIdx < 21; DayIdx++) {
    //        if (CurrentData[DayIdx].category == Day)
    //            break;
    //    }
    //    MonthDailyChart.series[HoursTypeIdx].data[DayIdx].update(y = Hours);
    //};

    getMoHours();
});    

