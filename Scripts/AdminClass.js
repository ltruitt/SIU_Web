
$(document).ready(function () {
  
    var timestamp = new Date();
    
    
    $('#jTableClass').jtable({
        title: 'Select Line To Load',
        edit: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'workDate ASC',

        actions: {
            listAction: '/SIU_DAO.asmx/GetMeetingLogAdmin',
            deleteAction: '/SIU_DAO.asmx/RemoveMeetingLogAdmin'
        },
        fields: {
            TL_UID: {
                title: 'No',
                width: '0%',
                key: true,
                create: false,
                edit: false,
                list: false
            },
            Date: {
                title: 'Date',
                type: 'date',
                displayFormat: 'mm-dd',
                sorting: true,
                width: '3%',
                listClass: 'jTableTD'
            },

            Topic: {
                title: 'Topic',
                sorting: false,
                width: '10%',
                list: true
            },
            Description: {
                title: 'Description',
                sorting: false,
                width: '15%',
                list: true
            },
            Instructor: {
                title: 'Instructor',
                sorting: false,
                width: '5%',
                list: true
            },
            Location: {
                title: 'Location',
                width: '5%',
                sorting: false,
                list: true
            },
            MeetingType: {
                title: 'Type',
                sorting: false,
                width: '5%',
                list: true
            },
            Points: {
                title: 'Pts',
                sorting: false,
                width: '1%',
                list: true
            },
            
            VideoFile: {
                title: 'Video',
                sorting: false,
                list: false
            },
            
            QuizLink: {
                title: 'Quiz URL',
                sorting: false,
                list: false
            },
            
            QuizName: {
                title: 'Quiz Name',
                sorting: false,
                list: false
            },
            
            StartTime: {
                list: false
            },
            StopTime: {
                list: false
            },

            InstructorID: { list: false }
        },
        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableClass').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    tlUid = record.TL_UID;

                    $('#txtTopic').val(record.Topic);
                    $('#txtType').val(record.MeetingType);
                    $('#txtPts').val(record.Points);
                    $('#txtDesc').val(record.Description);
                    $('#txtInst').val(record.Instructor);
                    
                    $('#txtDate').datepicker("setDate", parseJsonDate(record.Date));
                    $('#txtStart').timeEntry('setTime', parseJsonTime(record.StartTime));
                    $('#txtEnd').timeEntry('setTime', parseJsonTime(record.StopTime));
                    $('#txtLoc').val(record.Location);
                    
                    $('#txtVideo').val(record.VideoFile);
                    $('#txtQuiz').val(record.QuizLink);
                    
                    $('#jTableClass').hide();
                    $('#jTableQual').jtable('load', { MeetingID: tlUid, T: timestamp.getTime() });
                    validate();
                });
            } 
        }
    });
    


    $('#jTableQual').jtable({
        title: '',
        edit: true,
        selecting: true,
        sorting: false,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'workDate ASC',

        actions: {
            listAction: '/SIU_DAO.asmx/GetMeetingQual',
            deleteAction: '/SIU_DAO.asmx/RemoveMeetingQual'
        },
        fields: {
            TLC_UID: {
                title: 'No',
                width: '3%',
                key: true,
                create: false,
                edit: false,
                list: false
            },
            TL_UID: {
                title: 'No',
                width: '3%',
                key: false,
                create: false,
                edit: false,
                list: false
            },
            QualCode: {
                title: 'Certificates',
                sorting: false,
                listClass: 'jTableTD'
            }
        }
    });
    

    $('#btnAddCert').click(function () {
        addCert();
    });
    


    // When clicking on the button close or the mask layer the popup closed
    $(document).on('click', 'a.close, #popupBtnOK', function () {
        $('#mask , .popup').fadeOut(300, function () {
            $('#mask').remove();
        });
        return false;
    });
    


    function addCert() {
        //Get The Popup Window Container
        var addBox = $('#popup-box');

        // Fade in the Popup
        $(addBox).fadeIn(300);

        //Set the center alignment padding + border see css style
        var popMargTop = ($(addBox).height() + 24) / 2;
        var popMargLeft = ($(addBox).width() + 24) / 2;

        $(addBox).css({
            'margin-top': -popMargTop,
            'margin-left': -popMargLeft
        });

        // Add the mask to body
        $('body').append('<div id="mask"></div>');
        $('#acCertList').val('');
        $('#mask').fadeIn(300);
        $('#acCertList').focus();
    }






    function parseJsonDate(jsonDateString) {
        if (typeof (jsonDateString) == "undefined")
            return '';

        if (jsonDateString.indexOf('/Date') == -1) {
            var x = new Date(jsonDateString);
            var y = x.toDateString();
            return new Date(jsonDateString).toDateString();
        }

        return new Date(parseInt(jsonDateString.replace('/Date(', ''))).toDateString();
    }

    function parseJsonTime(jsonTimeString) {
        if (typeof (jsonTimeString) == "undefined")
            return '';

        if (jsonTimeString.indexOf('/Date') == -1)
            return jsonTimeString;

        return new Date(parseInt(jsonDateString.replace('/Date(', ''))).toTimeString()();
    }
    



    ////////////////////////////////////////
    // Event Management For Job Selection //
    ////////////////////////////////////////
    var listOfCerts = [];
    function getCertsSuccess(data) {
        listOfCerts = data.d.split("\r");

        $("#acCertList").autocomplete(
            { source: listOfCerts },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: false,
                cacheLength: 20,
                max: 5,
                select: function (event, ui) {
                    var dataPieces = ui.item.value.split(' ');
                    $(this).val(dataPieces[0].replace(/\n/g, ""));
                    var xxx = $(this).val();
                    $("#acCertList").autocomplete("close");
                    $('#popupBtnOK').show();
                    $('#popupBtnOK').focus();
                    return false;
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $(this).val(dataPieces[0].replace(/\n/g, ""));
                        $("#acCertList").autocomplete("close");
                        $('#popupBtnOK').show();
                        $('#popupBtnOK').focus();
                    }
                    else {
                        $('#popupBtnOK').hide();
                    }

                    return ui;
                }
            });
    }
    function getCerts() {
        var getCertsAjax = new AsyncServerMethod();
        getCertsAjax.exec("/SIU_DAO.asmx/GetCerts", getCertsSuccess);
    }
    

    function setPoints(TypeString) {
        for (var c = 0; c < listOfTypes.length; c++) {
            var strParts = listOfTypes[c].split(",");
            if (listOfTypes[c] == TypeString)
                $("#txtPts").val(listOfPoints[c]);
        }
    }


    ///////////////////////////////////////////////////////////////
    // Load List Of Employees So Supr Can Change Viewed Employee //
    ///////////////////////////////////////////////////////////////
    var listOfTypes = [];
    var listOfPoints = [];
    function getTypesSuccess(data) {
        listOfTypes = data.d.split("\r");

        for (var c = 0; c < listOfTypes.length; c++) {
            var strParts = listOfTypes[c].split(",");
            listOfPoints[c] = strParts[1];
            listOfTypes[c] = strParts[0];
        }


        $("#txtType").autocomplete({ source: listOfTypes },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: false,
                cacheLength: 20,
                max: 20,
                delay: 0,
                select: function (event, ui) {
                    setPoints(ui.item.value);

                    var dataPieces = ui.item.value.split('-');
                    $('#hlblPointsType')[0].innerHTML = dataPieces[0];
                    $("#txtType").autocomplete("close");
                    $('#txtDesc').focus();
                    return false;
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split('-');
                        $('#hlblPointsType')[0].innerHTML = dataPieces[0];
                        $("#txtType").autocomplete("close");
                        $("#txtType").val(ui.content[0].value);

                        setPoints(ui.content[0].value);
                        $('#txtDesc').focus();
                    }
                    else {
                        $('#hlblPointsType')[0].innerHTML = '';
                    }

                    return ui;
                }
            });
    };
    ////////////////////////////////////////
    // Load Points Type AutoComplete List //
    ////////////////////////////////////////
    var getTypesCall = new AsyncServerMethod();
    getTypesCall.exec("/SIU_DAO.asmx/GetAutoCompleteClassTypes", getTypesSuccess);






    ///////////////////////////////////////////////////////////////
    // Load List Of Employees So Supr Can Change Viewed Employee //
    ///////////////////////////////////////////////////////////////
    var listOfEmps = [];
    function getEmpsSuccess(data) {
        listOfEmps = data.d.split("\r");
        $("#txtInst").autocomplete({ source: listOfEmps },
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
                    $('#hlblInstID')[0].innerHTML = dataPieces[0];
                    $("#txtInst").autocomplete("close");
                    $("#txtInst").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $('#hlblInstID')[0].innerHTML = dataPieces[0];
                        $("#txtInst").autocomplete("close");
                        $("#txtInst").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                    }

                    return ui;
                }
            });
    }

    /////////////////////////////////
    // Load Emps AutoComplete List //
    /////////////////////////////////
    var getEmpsCall = new AsyncServerMethod();
    getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);
    












    $('#popupBtnOK').click(function () {
                
        $('#jTableQual').jtable('addRecord', {
            record: {
                TLC_UID: 0,
                TL_UID: tlUid,
                QualCode: $("#acCertList").val()
            },
            clientOnly: true
        });
            
    });
    
    function validate()
    {
        var hasError = 0;
        
        $('#btnSubmit').show();
        $('#txtTopic').removeClass('ValidationError');
        $('#txtType').removeClass('ValidationError');
        $('#txtPts').removeClass('ValidationError');
        $('#txtDesc').removeClass('ValidationError');


        ///////////////////////////
        // Topic Must Be Present //
        ///////////////////////////
        if ($('#txtTopic').val().length == 0) {
            $('#txtTopic').addClass('ValidationError');
            $('#btnSubmit').hide();
            hasError = 1;
        }

        //////////////////////////
        // Type Must Be Present //
        //////////////////////////
        if ($('#txtType').val().length == 0) {
            $('#txtType').addClass('ValidationError');
            $('#btnSubmit').hide();
            hasError = 1;
        }
        
        //////////////////////////////
        // Type Must Come From List //
        //////////////////////////////
        //$("#txtType").blur(function () {
        //    if (!listOfTypes.containsCaseInsensitive(this.value)) {
        //        $(this).val("");
        //    }
        //});
        
        
        //////////////////////////
        // Desc Must Be Present //
        //////////////////////////
        if ($('#txtDesc').val().length == 0) {
            $('#txtDesc').addClass('ValidationError');
            $('#btnSubmit').hide();
            hasError = 1;
        }

        /////////////////////////
        // Pts Must Be Present //
        /////////////////////////
        if ($('#txtPts').val().length == 0) {
            $('#txtPts').addClass('ValidationError');
            $('#btnSubmit').hide();
            hasError = 1;
        }
        
        //////////////////////////
        // Time Must Be Numeric //
        //////////////////////////
        if (!isNumber($('#txtPts').val())) {
            $('#txtPts').addClass('ValidationError');
            $('#btnSubmit').hide();
            hasError = 1;
        }

        return hasError;
    }
    

    function FocusChange() {
        if (document.activeElement.className.indexOf("jtable") != -1) 
            return;
        $('#jTableClass').hide();
        validate();
    }


    $('#txtTopic').blur(function () { FocusChange() });
    $('#txtType').blur(function () { FocusChange() });
    $('#txtPts').blur(function () { FocusChange() });
    $('#txtDesc').blur(function () { FocusChange() });





    ////////////////////////
    // Clear / Reset Form //
    ////////////////////////
    $("#btnClear").click(function () {
        
        $('#btnSubmit').hide();
        
        ////////////////////////////////////////
        // Clear Data Confirmation Containers //
        ////////////////////////////////////////        
        $('#txtTopic').val('');
        $('#txtType').val('');
        $('#txtPts').val('');
        $('#txtDesc').val('');
        
        $('#txtInst').val('');
        $('#txtVideo').val('');
        $('#txtQuiz').val('');
        $('#txtDate').val('');
        $('#txtStart').val('');
        $('#txtEnd').val('');
        $('#txtLoc').val('');
        
        tlUid = 0;
        $('#hlblInstID')[0].innerHTML = 0;
        $('#hlblPointsType')[0].innerHTML = '';
        
        $('#jTableClass').show();
        validate();
    });

    
    $("#btnSubmit").click(function () {
        $('#btnSubmit').hide();

        var recordMeetingCall = new AsyncServerMethod();
        recordMeetingCall.add('MeetingID', tlUid);
        recordMeetingCall.add('cTopic', $('#txtTopic').val() );
        recordMeetingCall.add('cType', $('#hlblPointsType')[0].innerHTML);
        recordMeetingCall.add('cPts', $('#txtPts').val());
        recordMeetingCall.add('cDesc', $('#txtDesc').val());
        recordMeetingCall.add('cInstID', $('#hlblInstID')[0].innerHTML);
        recordMeetingCall.add('cVideo', escape($('#txtVideo').val()));
        recordMeetingCall.add('cQuiz', escape($('#txtQuiz').val()));
        recordMeetingCall.add('cDate', $('#txtDate').val());
        recordMeetingCall.add('cStart', $('#txtStart').val());
        recordMeetingCall.add('cStop', $('#txtEnd').val());
        recordMeetingCall.add('cLoc', $('#txtLoc').val());

        //var data = { d: 'ID:99' };
        //recordMeetingLogAdminSuccess(data);
        recordMeetingCall.exec("/SIU_DAO.asmx/RecordMeetingLogAdmin", recordMeetingLogAdminSuccess);

    });

    ///////////////////////////
    // Submit Button Handler //
    ///////////////////////////
    function recordMeetingLogAdminSuccess(data) {
        var errorMsg = data.d;

        if (errorMsg.length > 3 && errorMsg.substring(0, 3) == "ID:") {
            var stlUID = errorMsg.split(":");

            $('#jTableQual').jtable('selectRows', $('*'));
            if (typeof ($selectedRows) != "undefined") {
                var $selectedRows = $('#jTableQual').jtable('selectedRows');

                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    if (typeof (record) != "undefined") {
                        if (record.TLC_UID == 0) {
                            var recordQualCall = new AsyncServerMethod();
                            recordQualCall.add('MeetingID', stlUID[1]);
                            recordQualCall.add('Code', record.QualCode);
                            recordQualCall.exec("/SIU_DAO.asmx/RecordMeetingQual");
                        }
                    }
                });
            }

            
            ////////////////////
            // Reset The Form //
            ////////////////////
            if ($('#txtDate').val().length == 0)
                $('#txtDate').val('1/1/2099');
            var a = $.datepicker.formatDate('yy-mm-dd', new Date($('#txtDate').val())  )    ;
            var b = $('#txtType').val().split(' ')[0];
            var c = $('#txtInst').val().split(' ');
            var d = c.splice(0, 1);
            var UID = stlUID[1];
            

            $('#jTableClass').jtable('addRecord', {
                record: {
                    TL_UID: String(UID),
                    Date: a,
                    Topic: $('#txtTopic').val(),
                    Description: $('#txtDesc').val(),
                    Instructor: c,
                    Location: $('#txtLoc').val(),
                    MeetingType: b,
                    Points: $('#txtPts').val()
                },
                clientOnly: true
            });


             $("#btnClear").trigger('click');
             $('#jTableClass').jtable('selectRows', $('xxx'));
             $('#jTableQual').jtable('selectRows', $('xxx'));

        } else {
            if (errorMsg.length == 0) {
                errorMsg = 'Unknown Server Error Occured';
            }
            // $('#lblErrServer')[0].innerHTML = errorMsg;
            alert(errorMsg);
        }
    };
    







    $('#popupBtnOK').hide();
    $('#jTableClass').jtable('load', { T: timestamp.getTime() });
    $('#txtDate').datepicker();
    $('#txtStart').timeEntry({ spinnerImage: '' });
    $('#txtEnd').timeEntry({ spinnerImage: '' });
    
    // Look Up List Of Valid Cert Codes
    getCerts();
    $('#jTableClass').jtable('load', { T: timestamp.getTime() });
    $('#jTableQual').jtable('load', { MeetingID: 0, T: timestamp.getTime() });
    
    $("#btnClear").click();
    validate();
    $('#txtTopic').focus();
    
    
})