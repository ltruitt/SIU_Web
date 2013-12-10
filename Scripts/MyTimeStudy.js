
$(document).ready(function () {

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
                    getPrevMoHours();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = dataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                        getPrevMoHours();
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















    var prevMonthDailyChart;
    var prevMonthDailyOptions = {

        chart: {
            renderTo: 'PrevMonthDaily',
            type: 'column',
            marginRight: 80,
            marginBottom: 35,
            plotBackgroundColor: 'rgb(200, 200, 255)'
        },


        title: { text: 'Daily Hours Prev Month', x: -20 },
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
    function getPrevMoHours() {
        var getPrevMoHoursAjax = new AsyncServerMethod();
        getPrevMoHoursAjax.add('EmpID', $('#hlblEID')[0].innerHTML);
        getPrevMoHoursAjax.exec("/SIU_DAO.asmx/GetAllHoursRptPrevMo", getPrevMoHoursSuccess);
    }
    function getPrevMoHoursSuccess(data) {

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
        // Idx 4 = ST     //
        // Idx 5 = Sum     //
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
            prevMonthDailyOptions.series[loadIdx].data = hoursData;
        }

        ///////////////////////
        // Produce The Chart //
        ///////////////////////
        prevMonthDailyChart = new Highcharts.Chart(prevMonthDailyOptions);

        ///////////////////////////
        // Product X Axis Labels //
        ///////////////////////////
        var xAxisLabels = [];
        for (hoursIdx = 1; hoursIdx < hoursArr.length; hoursIdx++) {
            xAxisLabels.push(hoursIdx);
        }
        prevMonthDailyChart.xAxis[0].setCategories(xAxisLabels);




        ////////////////////////////////
        // Build Prev Weeks Pie Chart //
        ////////////////////////////////
        var pieData = [];
        var pieData1 = [];
        var pieData2 = [];

        var sumJobHours = weeklyHours[6].split(',');
        var monthSum = sumJobHours[1];
        var monthJob = sumJobHours[2];

        pieData1.push('Billable', parseFloat(monthJob));
        pieData2.push('OH', parseFloat(monthSum - monthJob));
        pieData.push(pieData1);
        pieData.push(pieData2);
        profitPieChartMonthPrevOptions.series[0].data = pieData;
        profitPieChartMonthPrev = new Highcharts.Chart(profitPieChartMonthPrevOptions);


        getMoHours();

    }
    getPrevMoHours();












    var monthDailyChart;
    var monthDailyOptions = {

        chart: {
            renderTo: 'MonthDaily',
            type: 'column',
            marginRight: 80,
            marginBottom: 35,
            plotBackgroundColor: 'rgb(200, 200, 255)'
        },


        title: { text: 'Daily Hours This Month', x: -20 },
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
    weeklyHours = [];

    function getMoHours() {
        var getMoHoursAjax = new AsyncServerMethod();
        getMoHoursAjax.add('EmpID', $('#hlblEID')[0].innerHTML);
        getMoHoursAjax.exec("/SIU_DAO.asmx/GetAllHoursRptThisMo", getMoHoursSuccess);
    }
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
        // Idx 4 = ST     //
        // Idx 5 = Sum     //
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

        ///////////////////////
        // Produce The Chart //
        ///////////////////////
        monthDailyChart = new Highcharts.Chart(monthDailyOptions);

        ///////////////////////////
        // Product X Axis Labels //
        ///////////////////////////
        var xAxisLabels = [];
        for (hoursIdx = 1; hoursIdx < hoursArr.length; hoursIdx++) {
            xAxisLabels.push(hoursIdx);
        }
        monthDailyChart.xAxis[0].setCategories(xAxisLabels);




        ////////////////////////////////
        // Build Prev Weeks Pie Chart //
        ////////////////////////////////
        var pieData = [];
        var pieData1 = [];
        var pieData2 = [];

        var sumJobHours = weeklyHours[6].split(',');
        var monthSum = sumJobHours[1];
        var monthJob = sumJobHours[2];

        pieData1.push('Billable', parseFloat(monthJob));
        pieData2.push('OH', parseFloat(monthSum - monthJob));
        pieData.push(pieData1);
        pieData.push(pieData2);
        profitPieChartMonthOptions.series[0].data = pieData;
        profitPieChartMonth = new Highcharts.Chart(profitPieChartMonthOptions);


        getHoursByWeek();

    }












    var yearWeeklyChart;
    var yearWeeklyOptions = {
        chart: { renderTo: 'YearWeekly', type: 'column', marginRight: 80, marginBottom: 35, plotBackgroundColor: 'rgb(200, 200, 255)' },
        title: { text: 'Hours By Week of Year', x: -20 },
        yAxis: { title: { text: 'Hours' }, max: 112, tickInterval: 8, id: 'y-axis', plotLines: [{ color: '#FF0000', width: .5, value: 8 }] },
        tooltip: { formatter: function () { return this.y; } },
        legend: { layout: 'vertical', align: 'right', verticalAlign: 'top', x: -10, y: 20, borderWidth: 0 },
        plotOptions: { column: { stacking: 'normal'} },
        series: [{ name: 'HT' }, { name: 'AB' }, { name: 'DT' }, { name: 'OT' }, { name: 'ST'}],
        credits: { enabled: false }
    };


    ////////////////////////////////
    // Show Sum Of Hours For Week //
    ////////////////////////////////
    var byWeeklHours = [];
    function getHoursByWeek() {
        var getHoursByWeekAjax = new AsyncServerMethod();
        getHoursByWeekAjax.add('EmpID', $('#hlblEID')[0].innerHTML);
        getHoursByWeekAjax.exec("/SIU_DAO.asmx/GetWeeklyHoursThisYear", getHoursByWeekSuccess);
    }
    function getHoursByWeekSuccess(data) {

        ////////////////////////////
        // Clean Up The Data Some //
        ////////////////////////////
        byWeeklHours = data.d.replace('{', '').replace('}', '');
        byWeeklHours = byWeeklHours.split(/],/g);

        /////////////////////
        // Load Hours Data //
        // Idx 0 = HT      //
        // Idx 1 = AB      //
        // Idx 2 = DT      //
        // Idx 3 = OB      //
        // Idx 4 = ST     //
        // Idx 5 = Sum     //
        /////////////////////
        for (var loadIdx = 0; loadIdx < 5; loadIdx++) {
            var hoursArr = byWeeklHours[loadIdx].split(',');
            var hoursData = [];

            /////////////////////////////////////////////////
            // Assume 32 Days In A Month and Day 0 Invalid //
            /////////////////////////////////////////////////
            for (var hoursIdx = 1; hoursIdx < hoursArr.length; hoursIdx++) {
                hoursData.push(parseFloat((hoursArr[hoursIdx])));
            }

            //////////////////////////////////
            // Load The Graph With The Data //
            //////////////////////////////////
            yearWeeklyOptions.series[loadIdx].data = hoursData;
        }

        ///////////////////////
        // Produce The Chart //
        ///////////////////////
        yearWeeklyChart = new Highcharts.Chart(yearWeeklyOptions);

        ///////////////////////////
        // Product X Axis Labels //
        ///////////////////////////
        var xAxisLabels = [];
        for (var hoursIdx = 1; hoursIdx < hoursArr.length; hoursIdx++) {
            xAxisLabels.push(hoursIdx);
        }
        yearWeeklyChart.xAxis[0].setCategories(xAxisLabels);






        ////////////////////////////////
        // Build This Weeks Pie Chart //
        ////////////////////////////////
        var pieData = [];
        var pieData1 = [];
        var pieData2 = [];

        var sumHours = byWeeklHours[5].split(',');
        var sumJobHours = byWeeklHours[6].split(',');
        var weekSum = sumHours[sumHours.length - 1];
        var weekJob = sumJobHours[sumJobHours.length - 1].replace(']','');

        pieData1.push('Billable', parseFloat(weekJob));
        pieData2.push('OH', parseFloat(weekSum - weekJob));
        pieData.push(pieData1);
        pieData.push(pieData2);
        profitPieChartWeekOptions.series[0].data = pieData;
        profitPieChartWeek = new Highcharts.Chart(profitPieChartWeekOptions);






        /////////////////////////
        // Build YTD Pie Chart //
        /////////////////////////
        pieData = [];
        pieData1 = [];
        pieData2 = [];
        var ytdSum = 0;
        var ytdJob = 0;
        for (var idx = 1; idx < sumHours.length; idx++) {
            ytdSum += parseFloat(sumHours[idx]);
            ytdJob += parseFloat(sumJobHours[idx]);
        }

        pieData1.push('Billable', parseFloat(ytdJob));
        pieData2.push('OH', parseFloat(ytdSum - ytdJob));
        pieData.push(pieData1);
        pieData.push(pieData2);
        profitPieChartYearOptions.series[0].data = pieData;
        profitPieChartYear = new Highcharts.Chart(profitPieChartYearOptions);
        getJobOhHoursForMonth();
    }








    var profitPieChartWeek;
    var profitPieChartWeekOptions = {
        chart: { renderTo: 'ProfitPieWeek',  plotBorderWidth: null, plotShadow: false, plotBackgroundColor: 'rgb(200, 200, 255)', marginRight: 0, marginLeft: 0, marginBottom: 0 },
        title: { text: 'Week', x: -0 },
        plotOptions: {
            pie: { allowPointSelect: true, cursor: 'pointer',
                dataLabels: { enabled: false, color: '#000000', connectorColor: '#000000',
                    formatter: function () { return '<b>' + this.point.name + '</b>: ' + this.percentage + ' %'; }
                }
            }
        },

        tooltip: { pointFormat: ' <b>{point.percentage}%</b>', percentageDecimals: 1 },

        series: [{ type: 'pie', name: 'Billable vs Overhead'}],

        credits: { enabled: false }
    };





    var profitPieChartMonth;
    var profitPieChartMonthOptions = {
        chart: { renderTo: 'ProfitPieMonth', plotBorderWidth: null, plotShadow: false, plotBackgroundColor: 'rgb(200, 200, 255)', marginRight: 0, marginLeft: 0, marginBottom: 0 },
        title: { text: 'Month', x: -0 },
        plotOptions: {
            pie: { allowPointSelect: true, cursor: 'pointer',
                dataLabels: { enabled: false, color: '#000000', connectorColor: '#000000',
                    formatter: function () { return '<b>' + this.point.name + '</b>: ' + this.percentage + ' %'; }
                }
            }
        },

        tooltip: { pointFormat: ' <b>{point.percentage}%</b>', percentageDecimals: 1 },

        series: [{ type: 'pie', name: 'Billable vs Overhead'}],

        credits: { enabled: false }
    };






    var profitPieChartMonthPrev;
    var profitPieChartMonthPrevOptions = {
        chart: { renderTo: 'ProfitPieMonthPrev', plotBorderWidth: null, plotShadow: false, plotBackgroundColor: 'rgb(200, 200, 255)', marginLeft: 0, marginRight: 0, marginBottom: 0 },
        title: { text: 'Prev Month', x: -10 },
        plotOptions: { pie: { allowPointSelect: true, cursor: 'pointer', dataLabels: { enabled: false}} },

        tooltip: { pointFormat: ' <b>{point.percentage}%</b>', percentageDecimals: 1 },

        series: [{ type: 'pie', name: 'Billable vs Overhead'}],

        credits: { enabled: false }
    };






    var profitPieChartYear;
    var profitPieChartYearOptions = {
        chart: { renderTo: 'ProfitPieYear', plotBorderWidth: null, plotShadow: false, plotBackgroundColor: 'rgb(200, 200, 255)', marginLeft: 0, marginRight: 0, marginBottom: 0 },
        title: { text: 'Year', x: -0 },
        plotOptions: { pie: { allowPointSelect: true, cursor: 'pointer', dataLabels: { enabled: false}} },

        tooltip: { pointFormat: ' <b>{point.percentage}%</b>', percentageDecimals: 1 },

        series: [{ type: 'pie', name: 'Billable vs Overhead'}],

        credits: { enabled: false }
    };









    var jobsAcctsChart;
    var jobsAcctsOptions = {
        chart: { renderTo: 'JobsAccts', plotBackgroundColor: 'rgb(200, 200, 255)' },
        title: { text: 'This Months Breakdown By Job', x: -20 },
        xAxis: [{ reversed: false, labels: { rotation: -45, align: 'right', style: { fontSize: '11px', fontFamily: 'Verdana, sans-serif'}} },
                         { reversed: false, labels: { rotation: 45, align: 'right', style: { fontSize: '11px', fontFamily: 'Verdana, sans-serif'} }, opposite: true, linkedTo: 0}  // mirror axis on right side
                       ],
        yAxis: { title: { text: null }, plotLines: [{ color: '#FF0000', width: 1.5, value: 0}] },
        tooltip: { formatter: function () { return this.y; } },
        plotOptions: { series: { stacking: 'normal'} },
        series: [{ name: 'HT', type: 'column', color: '#4572a7' }, { name: 'AB', type: 'column', color: '#aa4643' }, { name: 'DT', type: 'column', color: '#89a54e' }, { name: 'OT', type: 'column', color: '#80699b' }, { name: 'ST', type: 'column', color: '#3d96ae' },
                         { name: 'HT', type: 'column', color: '#4572a7' }, { name: 'AB', type: 'column', color: '#aa4643' }, { name: 'DT', type: 'column', color: '#89a54e' }, { name: 'OT', type: 'column', color: '#80699b' }, { name: 'ST', type: 'column', color: '#3d96ae' }
                        ],
        credits: { enabled: false }
    };



    function getJobOhHoursForMonth() {
        var getJobOhHoursForMonthAjax = new AsyncServerMethod();
        getJobOhHoursForMonthAjax.add('EmpID', $('#hlblEID')[0].innerHTML);
        getJobOhHoursForMonthAjax.exec("/SIU_DAO.asmx/GetJobOhHoursForMonth", getJobOhHoursForMonthSuccess);
    }

    function getJobOhHoursForMonthSuccess(data) {

        ////////////////////////////
        // Clean Up The Data Some //
        ////////////////////////////
        byWeeklHours = data.d.replace('{', '').replace('}', '');
        byWeeklHours = byWeeklHours.split(/],/g);


        /////////////////////
        // Load Hours Data //
        // Idx 0 = HT      //
        // Idx 1 = AB      //
        // Idx 2 = DT      //
        // Idx 3 = OT      //
        // Idx 4 = ST      //
        // Idx 5 = Sum     //
        // Idx 6 = JobNo   //
        // Idx 7 = AcctNo  //
        /////////////////////
        var hoursHt = byWeeklHours[0].split(',');
        var hoursAb = byWeeklHours[1].split(',');
        var hoursDt = byWeeklHours[2].split(',');
        var hoursOt = byWeeklHours[3].split(',');
        var hoursSt = byWeeklHours[4].split(',');
        var jobs = byWeeklHours[6].split(',');
        var accts = byWeeklHours[7].split(',');

        var jobHt = [];
        var jobAb = [];
        var jobDt = [];
        var jobOt = [];
        var jobSt = [];
        var acctHt = [];
        var acctAb = [];
        var acctDt = [];
        var acctOt = [];
        var acctSt = [];
        var jobCat = [];
        var acctCat = [];

        for (var loadIdx = 1; loadIdx < hoursHt.length; loadIdx++) {
            if (jobs[loadIdx].length > 3) {
                jobCat.push(jobs[loadIdx].replace(/"/g, ''));
                jobHt.push(parseFloat(hoursHt[loadIdx]));
                jobAb.push(parseFloat(hoursAb[loadIdx]));
                jobDt.push(parseFloat(hoursDt[loadIdx]));
                jobOt.push(parseFloat(hoursOt[loadIdx]));
                jobSt.push(parseFloat(hoursSt[loadIdx]));

                acctCat.push('');
                acctHt.push(0);
                acctAb.push(0);
                acctDt.push(0);
                acctOt.push(0);
                acctSt.push(0);
            }

            else {

                acctCat.push(accts[loadIdx].replace(/"/g, '').replace(']', ''));
                acctHt.push(-parseFloat(hoursHt[loadIdx]));
                acctAb.push(-parseFloat(hoursAb[loadIdx]));
                acctDt.push(-parseFloat(hoursDt[loadIdx]));
                acctOt.push(-parseFloat(hoursOt[loadIdx]));
                acctSt.push(-parseFloat(hoursSt[loadIdx]));

                jobCat.push('');
                jobHt.push(0);
                jobAb.push(0);
                jobDt.push(0);
                jobOt.push(0);
                jobSt.push(0);
            }
        }

        //////////////////////////////////
        // Load The Graph With The Data //
        //////////////////////////////////
        jobsAcctsOptions.series[0].data = jobHt;
        jobsAcctsOptions.series[1].data = jobAb;
        jobsAcctsOptions.series[2].data = jobDt;
        jobsAcctsOptions.series[3].data = jobOt;
        jobsAcctsOptions.series[4].data = jobSt;

        jobsAcctsOptions.series[5].data = acctHt;
        jobsAcctsOptions.series[6].data = acctAb;
        jobsAcctsOptions.series[7].data = acctDt;
        jobsAcctsOptions.series[8].data = acctOt;
        jobsAcctsOptions.series[9].data = acctSt;


        ///////////////////////
        // Produce The Chart //
        ///////////////////////
        jobsAcctsChart = new Highcharts.Chart(jobsAcctsOptions);
        jobsAcctsChart.xAxis[0].setCategories(acctCat);
        jobsAcctsChart.xAxis[1].setCategories(jobCat);

    }

});
