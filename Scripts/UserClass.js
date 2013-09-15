$(document).ready(function () {

    var timestamp = new Date();









    $('#jTableClass').jtable({
        title: '',
        edit: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'workDate ASC',

        actions: {
            listAction: '/SIU_DAO.asmx/GetMeetingLogUser',
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
            
            VideoComplete: {
                title: 'video',
                sorting: false,
                width: '1%',
                list: true,
                display: function (data) {
                    var $img = '';

                    if (data.record.VideoComplete == true)
                        $img = $('<img style="width: 20px; height: 20px; margin-left: 10px;" src="/Images/GreenChk.png" title="Incomplete" />');
                    //else
                    //    $img = $('<img style="width: 10px; height: 10px; margin-left: 10px;" src="/Images/close.png" title="Incomplete" />');
                    return $img;
                }
            },

            QuizComplete: {
                title: 'quiz',
                sorting: false,
                width: '1%',
                list: true,
                display: function (data) {
                    var $img = '';

                    if (data.record.QuizComplete == 1)
                        $img = $('<img style="width: 20px; height: 20px; margin-left: 10px;" src="/Images/GreenChk.png" title="Incomplete" />');
                    if (data.record.QuizComplete == 0)
                        $img = $('<img style="width: 13px; height: 13px; margin-left: 10px;" src="/Images/close.png" title="Incomplete" />');
                    return $img;
                }
            },
            
            Date: {
                title: 'Class Date',
                type: 'date',
                displayFormat: 'mm-dd',
                sorting: false,
                width: '5%',
                listClass: 'jTableTD'
            },

            Topic: {
                title: 'Topic',
                sorting: false,
                width: '15%',
                list: true,
                display: function (Data) {
                    var t = '';
                    for (var c = 0; c < listOfTypes.length; c++) {
                        var strParts = listOfTypes[c].split(" ");
                        if (strParts[0] == Data.record.MeetingType) {
                            var mta = listOfTypes[c].split(' ');
                            mta.splice(0, 2);
                            t = '<span style="font-weight: bold !important">' + mta.join(' ') + '</span><br/>' + Data.record.Topic;
                        }
                    }
                    return t;
                },
            },
            Description: {
                title: 'Description',
                sorting: false,
                width: '20%',
                list: true
            },
            Instructor: {
                title: 'Instructor',
                sorting: false,
                width: '10%',
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
                list: false
            },
            Points: {
                title: 'Pts',
                sorting: false,
                width: '1%',
                list: false
            },

            VideoFile: {
                title: 'Take Class',
                sorting: false,
                list: true,
                display: function (data) {
                    var $img = $('<a href="/Safety/Training/TrainingVideo.aspx?id=' + data.record.TL_UID + '"><img style="width:40px; height: 40px; margin-left: 10px;" src="/Images/SI-Corp-Certifications.png" title="Open Form" /></a>');
                    return $img;
                },
                width: '5%'
            },

            QuizName: {
                title: 'Quiz',
                sorting: false,
                list: false
            },

            StartTime: {
                list: true,
                title: 'Start Time',
                width: '5%',
                sorting: false,
                display: function (data) {
                    var t = new Date(parseInt(data.record.StartTime.replace('/Date(', ''))).toTimeString().split(' ')[0];
                    if (t == '00:00:00')
                        return '';
                    return t;
                }
            },
            StopTime: {
                list: false
            },

            InstructorID: { list: false }
        },

        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableClass').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    tlUid = record.TL_UID;
                    
                    //window.location.href = "http://" + window.location.hostname + '/Safety/Training/TrainingVideo.aspx?id=' + record.TL_UID;
                });
            }
        }
    });


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
        
        $('#jTableClass').jtable('load', { T: timestamp.getTime() });
        
    };
    

    ////////////////////////////////////////
    // Load Points Type AutoComplete List //
    ////////////////////////////////////////
    var getTypesCall = new AsyncServerMethod();
    getTypesCall.exec("/SIU_DAO.asmx/GetAutoCompleteClassTypes", getTypesSuccess);
    

    

});