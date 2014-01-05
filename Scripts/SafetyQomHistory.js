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
            listAction: '/SIU_DAO.asmx/GetSafetyQomQHList',
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

                    $('#QomQ').html(record.Question);
                    $('#QomA').html(record.Response);
                    $('#QomE').html(record.EhsResponse);
                    $('#Q_Ans').html(record.Q_Ans);
                    
                });
            }
        }
    });


    ////////////////
    // Form Setup //
    ////////////////   
    $('#jTableQomList').jtable('load', { Eid: $('#hlblEID')[0].innerHTML, T: timestamp.getTime() });

});