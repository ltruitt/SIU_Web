$(document).ready(function () {

    var refTaskDefinition = $("#txtQContent");
    var tlUid;
    
    refTaskDefinition.blur(function () {
        refTaskDefinition.height(20);
    });

    refTaskDefinition.focus(function () {
        refTaskDefinition.height(200);
    });


    $('#StartDate').datepicker();
    $('#EndDate').datepicker({
        minDate: $('#StartDate')[0].innerHTML
    });
    



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
            listAction: '/SIU_DAO.asmx/GetSafetyQomQList',
            deleteAction: '/SIU_DAO.asmx/RemoveSafetyQomQuestion'
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

            QuestionGroup: {
                title: 'Dept',
                sorting: false,
                width: '5%',
                list: true
            },
            
            StartDate: {
                title: 'Start',
                type: 'date',
                displayFormat: 'mm-dd',
                sorting: false,
                width: '5%',
                listClass: 'jTableTD'
            },
            

            EndDate: {
                title: 'End',
                type: 'date',
                displayFormat: 'mm-dd',
                sorting: false,
                width: '5%',
                listClass: 'jTableTD'
            },
            

            Question: {
                title: 'Text',
                sorting: false,
                width: '40%',
                list: true
            },
            
            QuestionFile: {
                title: 'File',
                sorting: false,
                width: '40%',
                list: true
            }
        },

        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableQomList').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows

                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    
                    tlUid = record.Q_Id;
                    
                    $('#StartDate').datepicker("setDate", parseJsonDate(record.StartDate));
                    $('#EndDate').datepicker("setDate", parseJsonDate(record.EndDate));
                    $('#txtGroup').val(record.QuestionGroup);
                    $('#lblFile')[0].innerHTML = record.QuestionFile;
                    $('#txtQContent').val(record.Question);
                    $('#ContentTypeBlock').hide();
                    $('#btnUpload').hide();
                    $('#FileBlock').hide();
                    $('#TextBlock').hide();
                    $('#lblFile').show();
                    
                    if (record.Question.length > 0) {
                        $('#TextBlock').show('slow');
                    } else {
                        $('#FileBlock').show('slow');
                    }
                });
                
                validate();
            }
        }
    });
    



    function parseJsonDate(jsonDateString) {
        if (typeof (jsonDateString) == "undefined")
            return '';

        if (jsonDateString.indexOf('/Date') == -1) {
            return new Date(jsonDateString).toDateString();
        }

        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }
    


    function validate() {
        var hasError = 0;

        $('#btnSubmit').show();
        $('#StartDate').removeClass('ValidationError');
        $('#EndDate').removeClass('ValidationError');
        $('#txtGroup').removeClass('ValidationError');
        $('#txtFile').removeClass('ValidationError');
        $('#txtQContent').removeClass('ValidationError');


        ////////////////////////////////
        // Start Date Must Be Present //
        ////////////////////////////////
        if ($('#StartDate').val().length == 0) {
            $('#StartDate').addClass('ValidationError');
            $('#btnSubmit').hide();
            hasError = 1;
        }

        //////////////////////////////
        // End Date Must Be Present //
        //////////////////////////////
        if ($('#EndDate').val().length == 0) {
            $('#EndDate').addClass('ValidationError');
            $('#btnSubmit').hide();
            hasError = 1;
        }

        //////////////////////////
        // Dept Must Be Present //
        //////////////////////////
        if ($('#txtGroup').val().length == 0) {
            $('#txtGroup').addClass('ValidationError');
            $('#btnSubmit').hide();
            $('#ContentTypeBlock').hide();
            $('#FileBlock').hide();
            $('#TextBlock').hide();
            hasError = 1;
        } else {
            $('#ContentTypeBlock').show();
        }
            

        //////////////////////////////////////////////////////////////////
        // Either a Text Question or Question Document Must Be Supplied //
        //////////////////////////////////////////////////////////////////
        if ($('#txtFile').val().length == 0 && $('#txtQContent').val().length == 0 && $('#lblFile')[0].innerHTML.length == 0) {
            $('#btnSubmit').hide();
            hasError = 1;
            
            
        }

        return hasError;
    }


    $('#StartDate').on("blur", function () { validate(); });
    $('#EndDate').on("blur", function () { validate(); });
    $('#txtGroup').on("change", function () { validate(); });
    $('#txtQContent').on("blur", function () { validate(); });
    $('#txtFile').on("change", function () {
        $('#btnUpload')[0].value = "UPLOAD:   " + $('#txtFile')[0].value;
        $('#lblFile')[0].innerHTML = $('#txtFile')[0].value;
        $('#lblFile').hide();
        $('#txtFile').hide();
        $('#btnUpload').show();
        $('#UploadStats').hide();
    });


    ////////////////////////
    // Clear / Reset Form //
    ////////////////////////
    $("#btnClear").click(function () {
        
        tlUid = 0;
        
        $('#btnSubmit').hide();
        
        $('#btnUpload').hide();
        $('#ContentTypeBlock').show();
        $('#FileBlock').hide();
        $('#TextBlock').hide();
        $('#txtFile').show();
        $('#UploadStats').hide();
        $('#lblFile').hide();
        $('#ContentTypeBlock').hide();
        
        
        $('#StartDate').val('');
        $('#EndDate').val('');
        $('#txtGroup').val('');
        $('#txtFile').val('');
        $('#txtQContent').val('');
        $('#lblFile')[0].innerHTML = "";

        validate();
    });
    
    $('#txtQContent').keyup(function () {
        validate();
    });

    $("#btnSubmit").click(function () {
        

        var FN = $('#lblFile')[0].innerHTML;
        var splitAt = FN.lastIndexOf('\\') > FN.lastIndexOf('/') ? FN.lastIndexOf('\\') : FN.lastIndexOf('/');
        var filename =  $('#txtGroup').val() + '/' + FN.substr(splitAt + 1, FN.length);
        
        var recordQomCall = new AsyncServerMethod();
        recordQomCall.add('_UID', tlUid);
        recordQomCall.add('_dept', $('#txtGroup').val() );
        recordQomCall.add('_start', $('#StartDate').val() );
        recordQomCall.add('_end', $('#EndDate').val() );
        recordQomCall.add('_text', $('#txtQContent').val() );
        recordQomCall.add('_file', encodeURIComponent( filename ) );

        recordQomCall.exec("/SIU_DAO.asmx/RecordSafetyQomQuestion", recordQomSuccess);
        $('#btnSubmit').hide();
    });
    
    ///////////////////////////
    // Submit Button Handler //
    ///////////////////////////
    function recordQomSuccess(data) {
        $('#btnClear').click();
        $('#jTableQomList').jtable('load', { T: timestamp.getTime() });
    }





    $('#btnHTML').click(function () {
        $('#FileBlock').show('slow');
        //$('#txtFile').val('');
        $('#TextBlock').hide();
        //$('#ContentTypeBlock').hide();
        $('#btnUpload').hide();
    });

    $('#btnText').click(function () {
        $('#TextBlock').show('slow');
        $('#FileBlock').hide();
        //$('#txtQContent').val('');
        //$('#ContentTypeBlock').hide();
        $('#btnUpload').hide();
    });
    
    $('#btnUpload').click(function () {
        $('#UploadStats').show();
        validate();
    });

    ////////////////
    // Form Setup //
    ////////////////   
    $('#jTableQomList').jtable('load', { T: timestamp.getTime() });
    $('#btnClear').click();
})