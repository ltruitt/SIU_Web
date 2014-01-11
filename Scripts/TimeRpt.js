$(document).ready(function () {

    Date.prototype.getWeek = function () {
        var onejan = new Date(this.getFullYear(), 0, 1);
        return Math.ceil((((this - onejan) / 86400000) + onejan.getDay() + 1) / 7);
    };
    var timestamp = new Date();
    

    $('#jTableContainer').jtable({
        title: 'Select Line For Detail',
        edit: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'workDate ASC',

        actions: {
            listAction: '/SIU_DAO.asmx/rrrrrT',
            deleteAction: '/SIU_DAO.asmx/hfghfg'
        },
        fields: {
            EntryNo: {
                title: 'No',
                width: '3%',
                key: true,
                create: false,
                edit: false,
                list: false
            },
            workDate: {
                title: 'Date',
                sorting: true,
                width: '3%',
                listClass: 'jTableTD',
                display: function (data) { return showRejected(data.record.workDate, data.record.Status == 7); }
            },
            
            JobNo: {
                title: 'Job',
                sorting: true,
                width: '10%',
                listClass: 'jTableTD',
                display: function (data) {
                    if (window.location.href.match(/phone/i))
                        return data.record.JobNo + data.record.OhAcct;
                    return data.record.JobNo;
                }
            },
            OhAcct: {
                title: 'O/H',
                sorting: true,
                list: true
            },
            

            Task: {
                title: 'Task',
                sorting: false,
                list: true
            },
            
            ST: {
                title: 'ST',
                width: '1%',
                sorting: false,
                listClass: 'jTableTD',
                display: function (data) { return showRejected(data.record.ST, data.record.Status == 7); }
            },
            OT: {
                title: 'OT',
                width: '1%',
                sorting: false,
                listClass: 'jTableTD',
                display: function (data) { return showRejected(data.record.OT, data.record.Status == 7); }
            },
            DT: {
                title: 'DT',
                width: '1%',
                sorting: false,
                listClass: 'jTableTD',
                display: function (data) { return showRejected(data.record.DT, data.record.Status == 7); }
            },
            AB: {
                title: 'AB',
                width: '1%',
                sorting: false,
                listClass: 'jTableTD',
                display: function (data) { return showRejected(data.record.AB, data.record.Status == 7); }
            },
            HT: {
                title: 'HT',
                width: '1%',
                sorting: false,
                listClass: 'jTableTD',
                display: function (data) { return showRejected(data.record.HT, data.record.Status == 7); }
            },
            Total: {
                list: true,
                title: 'Tot',
                width: '2%',
                sorting: false,
                listClass: 'jTableTD',
                display: function (data) { return showRejected(data.record.Total, data.record.Status == 7); }
            },
            Dept: { list: false },
            
            Status: { list: false }
        },
        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableContainer').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');

                    $('#lblDeptSelection')[0].innerHTML = record.Dept;
                    $('#lblJobNoSelection')[0].innerHTML = record.JobNo;
                    $('#lblTaskCodeSelection')[0].innerHTML = record.Task;
                    $('#lblOhAcctSelection')[0].innerHTML = record.OhAcct;
                    $('#lblStatusSelection')[0].innerHTML = statusToText(record.Status);

                    $('#TimeRptDtlDiv').show('slow');
                });
            } else {
                //No rows selected
                $('#TimeRptDtlDiv').hide();
            }
        }
    });

    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });
    
    //////////////////////////////////
    //Disable Some Columns On Phone //
    //////////////////////////////////
    if (window.location.href.match(/phone/i)) {
        $('#jTableContainer').jtable('changeColumnVisibility', 'Task', 'hidden');
        $('#jTableContainer').jtable('changeColumnVisibility', 'OhAcct', 'hidden');
    }



    $('#TimeRptDtlDiv').hide();
    timestamp = new Date();


    function showRejected(displayValue, rejected) {
        if (displayValue == 0)
            displayValue = "";
        
        if (rejected) {
            return '<span style="color: red; font-weight: bold;">' + displayValue + '</span>';
        } else {
            return displayValue;
        }
    }



    function statusToText(statusValue) {
        switch (statusValue) {
            case 0:
                return "New";
            case 1:
                return "Pending Employee Approval";
            case 2:
                return "Employee Approved";
            case 3:
                return "Pending Verification";
            case 4:
                return "Being Reviewed by Verifier";
            case 5:
                return "Pending Manager Approval";
            case 6:
                return "Manager Approved";
            case 7:
                return "Rejected";
            case 8:
                return "Posted";
            default:
                return "INVALID !!!!!";
        }
    }


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
                    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = dataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);

                        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });
                    }

                    return ui;
                }
            });
    }

    /////////////////////////////////
    // Load Emps AutoComplete List //
    /////////////////////////////////
    if ($("#SuprArea").length > 0) {
        var getEmpsCall = new AsyncServerMethod();
        getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);
    }

});