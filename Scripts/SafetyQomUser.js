﻿$(document).ready(function () {


    var id = $.fn.getURLParameter('id');

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
                title: 'Id',
                width: '1%',
                sorting: false,
                key: true,
                create: false,
                edit: false,
                list: true
            },
            
            R_IncNo: {
                title: 'RSP ID',
                width: '2%',
                sorting: false,
                key: false,
                create: false,
                edit: false,
                list: true,
                listClass: 'jTableTD'
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
                list: false,
                type: 'date',
                displayFormat: 'mm-dd',
                sorting: false,
                width: '5%',
                listClass: 'jTableTD'
            },


            EndDate: {
                title: 'Ends',
                list: false,
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
            //$('#jTableQomList').jtable('selectRows', $('.jtable-data-row').first());
            
            if (!id)
                $('#jTableQomList').jtable('selectRows', $('.jtable-data-row').first());
            else
                $('#jTableQomList').jtable('selectRows',
                    $("tr").filter(function () {
                        return $.trim($(this).find('td:eq(0)').text()) == id;
                    })
                );
            


        },
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableQomList').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows

                $selectedRows.each(function () {
                    var record = $(this).data('record');

                    $('#hlblIncNo').val(record.R_IncNo);
                    $('#hlblUID').val(record.Q_Id);
                    $('#hlblDept').val(record.Q_Grp);
                    
                    $('#QomQ').html(record.Question);
                    $('#txtResponse').val(record.Response);
                    $('#ehsResponse').html(record.EhsResponse);
                    
                    $('#btnSubmit').hide();
                    
                    //if (record.Response.length == 0)
                    if (record.Status != "Accepted" && record.Status != "Decline")
                        $('#txtResponse').attr('disabled', false).css({ 'background-color': 'antiquewhite', 'border-color': 'brown' }).focus();
                    else
                        $('#txtResponse').attr('disabled', true).css({ 'background-color': 'gray', 'border-color': 'black' });
                    
                    $('#DivResponse').show();
                    
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
        qomResponseSubmitCall.add('IncidentNo', $('#hlblIncNo').val());
        qomResponseSubmitCall.add('EID', $('#hlblEID').html());
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

        qomResponseSubmitCall.add('Comments', encodeURIComponent($('#txtResponse').val()));

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
    $('#DivResponse').hide();
    
    $('#jTableQomList').jtable('load', { Eid: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });
    

    /////////////////////////////////////////////////////
    // Load List Of Employees into Observed Data Field //
    /////////////////////////////////////////////////////
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
                $('#jTableQomList').jtable('load', { Eid: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var dataPieces = ui.content[0].value.split(' ');
                    $('#hlblEID')[0].innerHTML = dataPieces[0];
                    $("#ddEmpIds").autocomplete("close");
                    $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                    $('#jTableQomList').jtable('load', { Eid: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });
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
});