$(document).ready(function () {





    $('#jTableContainer').jtable({
        title: 'Select Line To View',
        edit: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'SubmitTimeStamp ASC',
        //selectOnRowClick: true,

        actions: {
            listAction: '/SIU_DAO.asmx/GetOpenDOT_WeekView'
        },
        fields: {
            SubmitTimeStamp: {
                title: 'Date',
                //type: 'date',
                sorting: true,
                width: '2%',
                listClass: 'jTableTD'
            },
            Vehicle: {
                title: 'Veh',
                width: '1%',
                list: true,
                sorting: true
            },
            Hazard: {
                title: 'Hazard',
                sorting: false,
                width: '3%',
                listClass: 'jTableTD',
                display: function (data) { return showHazard(data.record.Hazard, data.record.Correction); }
            },
            SubmitEmpID: { list: false },
            CorrectionTimeStamp: { list: false },
            CorrectionEmpID: { list: false },
            RefID: { list: false, key: true },
            Make: { list: false },
            Model: { list: false },
            PLate: { list: false },
            SubmitEmpName: { list: false }
        },
        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableContainer').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');

                    $('#lblVehicle')[0].innerHTML = record.Vehicle;
                    $('#lblMake')[0].innerHTML = record.Make;
                    $('#lblModel')[0].innerHTML = record.Model;
                    $('#lblPlate')[0].innerHTML = record.Plate;

                    $('#lblSubmitTimeStamp')[0].innerHTML = record.SubmitTimeStamp;
                    $('#lblSubmitEmpID')[0].innerHTML = record.SubmitEmpName;
                    $('#lblHazard')[0].innerHTML = record.Hazard;
                    $('#lblRefID')[0].innerHTML = record.RefID;


                    $('#DotRptDtlDiv').show('slow');

                    if (record.Correction == null || record.Correction.length == 0) {
                        $('#lblCorrection')[0].innerHTML = '';
                        $('#uncorrectedDiv').show('slow');
                    } else {
                        $('#lblCorrection')[0].innerHTML = record.Correction;
                        $('#uncorrectedDiv').hide();
                    }

                });
            } else {
                //No rows selected
                $('#DotRptDtlDiv').hide();
            }
        }
    });





    function showHazard(hazard, correction) {
        if (correction == null) {
            return '<span style="color: red; font-weight: bold;">' + hazard.substring(0, 20) + '</span>';
        }


        if (correction.length == 0) {
            return '<span style="color: red; font-weight: bold;">' + hazard.substring(0, 20) + '</span>';
        } else {
            return hazard.substring(0, 20);
        }
    }




    function recordDotCorrectionSuccess() {
        clear();
    }
    
    $('#btnSubmit').click(function () {

        var recordDotCorrectionAjax = new AsyncServerMethod();
        
        recordDotCorrectionAjax.add('RefID', $('#lblRefID')[0].innerHTML );
        recordDotCorrectionAjax.add('EmpID', $('#hlblEID')[0].innerHTML );
        recordDotCorrectionAjax.add('CorrectiveAction', $('#txtCorrection').val() );
        recordDotCorrectionAjax.exec("/SIU_DAO.asmx/RecordDotCorrection", recordDotCorrectionSuccess);
    });



    function clear() {
        $('#txtCorrection').val('');
        $('#DotRptDtlDiv').hide();
        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });
    }





    clear();

});