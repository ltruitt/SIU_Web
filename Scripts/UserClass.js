$(document).ready(function () {

    var timestamp = new Date();


    $('#jTableClass').jtable({
        title: 'Click A Video or Quiz',
        edit: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'workDate ASC',

        actions: {
            listAction: '/SIU_DAO.asmx/GetMeetingLogAdmin',
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
                displayFormat: 'mm-dd-yy',
                sorting: true,
                width: '8%',
                listClass: 'jTableTD'
            },

            Topic: {
                title: 'Topic',
                sorting: false,
                width: '15%',
                list: true
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
                list: true
            },
            Points: {
                title: 'Pts',
                sorting: false,
                width: '1%',
                list: false
            },

            VideoFile: {
                title: 'Video',
                sorting: false,
                list: true,
                display: function (Data) {
                    var $img = $('<a href="/Safety/Training/TrainingVideo.aspx?v=' + Data.record.VideoFile + '&id=' + Data.record.TL_UID + '"><img style="width:40px; height: 40px;" src="/Images/VideoFolder-Hover.png" title="Open Form" /></a>');
                    return $img;
                },
                width: '5%'
            },

            QuizLink: {
                title: 'Quiz URL',
                sorting: false,
                list: true,
                display: function (Data) {
                    var $img = $('<a href="/Safety/Training/TrainingQuiz.aspx?q=' + Data.record.QuizLink + '&id=' + Data.record.TL_UID  + '"><img style="width:50px; height: 50px;" src="/Images/SI-Corp-Certifications.png" title="Open Form" /></a>');
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
                width: '8%',
                sorting: false,
                display: function (data) { return new Date(parseInt(data.record.StartTime.replace('/Date(', ''))).toTimeString().split(' ')[0]; }
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
                });
            }
        }
    });


    $('#jTableClass').jtable('load', { T: timestamp.getTime() });

});