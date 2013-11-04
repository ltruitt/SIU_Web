$(document).ready(function () {




    var timestamp = new Date();


    $('#jTableQomList').jtable({
        title: '',
        edit: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'workDate ASC',

        actions: {
            listAction: '/SIU_DAO.asmx/GetSafetyQomQRList',
        },

        fields: {
            Q_Id: {
                title: 'No',
                width: '0%',
                key: true,
                create: false,
                edit: false,
                list: false
            },

            Q_Grp: {
                title: 'Dept',
                sorting: false,
                width: '5%',
                list: true,
                listClass: 'jTableTD'
            },

            StartDate: {
                title: 'Starts',
                type: 'date',
                displayFormat: 'mm-dd',
                sorting: false,
                width: '5%',
                listClass: 'jTableTD'
            },


            EndDate: {
                title: 'Ends',
                type: 'date',
                displayFormat: 'mm-dd',
                sorting: false,
                width: '5%',
                listClass: 'jTableTD'
            },
            
            Status: {
                title: 'Status',
                sorting: false,
                width: '5%',
                listClass: 'jTableTD'                
            },


            Points: {
                title: 'Points',
                sorting: false,
                width: '5%',
                list: true,
                listClass: 'jTableTD'
            },



            Question: {
                title: '',
                sorting: false,
                width: '0%',
                list: false
            },

            Response: {
                title: '',
                sorting: false,
                width: '0%',
                list: false
            },
            
            EhsResponse: {
                title: '',
                sorting: false,
                width: '0%',
                list: false
            },
        },
        recordsLoaded: function () {
            $('#jTableQomList').jtable('selectRows', $('.jtable-data-row').first());
        },
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableQomList').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows

                $selectedRows.each(function () {
                    var record = $(this).data('record');

                    $('#hlblUID').val(record.Q_Id);
                    $('#hlblDept').val(record.Q_Grp);
                    
                    $('#QomQ').html(record.Question);
                    $('#txtResponse').val(record.Response);
                    $('#ehsResponse').html(record.EhsResponse);
                    
                    $('#btnSubmit').hide();
                    
                    if (record.Response.length == 0)
                        $('#txtResponse').attr('disabled', false).css({ 'background-color': 'antiquewhite', 'border-color': 'brown' }).focus();
                    else
                        $('#txtResponse').attr('disabled', true).css({ 'background-color': 'gray', 'border-color': 'black' });
                        
                });
            }
        }
    });


    ////////////////////////////////////////////////////////////////////////
    // Bind To The OtherData Field And Enable / Disable The Submit Button //
    ////////////////////////////////////////////////////////////////////////
    $('#txtResponse').keyup(function () {
        if ($('#txtResponse').val().length > 0 )
            $('#btnSubmit').show();
        else
            $('#btnSubmit').hide();
    });
        


    $("#btnSubmit").click(function () {

        $('#btnSubmit').hide();

        var qomResponseSubmitCall = new AsyncServerMethod();
        qomResponseSubmitCall.add('EID', $('#hlblEID')[0].innerHTML);
        qomResponseSubmitCall.add('JobNo', '');

        qomResponseSubmitCall.add('IncTypeSafeFlag', 0);
        qomResponseSubmitCall.add('IncTypeUnsafeFlag', 0);
        qomResponseSubmitCall.add('IncTypeSuggFlag', 0);
        qomResponseSubmitCall.add('IncTypeTopicFlag', 0);
        qomResponseSubmitCall.add('IncTypeSumFlag', 0);

        qomResponseSubmitCall.add('IncidentDate', '');
        qomResponseSubmitCall.add('ObservedEmpID', '');
        qomResponseSubmitCall.add('InitialResponse', '');

        qomResponseSubmitCall.add('SafetyMeetingType', '');
        qomResponseSubmitCall.add('SafetyMeetingDate', '');

        qomResponseSubmitCall.add('Comments', $('#txtResponse').val() );

        qomResponseSubmitCall.add('JobSite', '-');
        qomResponseSubmitCall.add('IncTypeText', $('#hlblDept').val());
        
        qomResponseSubmitCall.add('QomID', $('#hlblUID').val() );

        qomResponseSubmitCall.exec("/SIU_DAO.asmx/RecordSafetyPays", qomResponseSubmitCallSuccess);
    });
    
    function qomResponseSubmitCallSuccess(data) {
        $('#jTableQomList').jtable('reload');
    }

    ////////////////
    // Form Setup //
    ////////////////   
    $('#btnSubmit').hide();
    $('#jTableQomList').jtable('load', { Eid: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });
    
});