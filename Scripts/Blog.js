$(document).ready(function () {


    $('#BlogTOC')[0].innerHTML = $.fn.getURLParameter('BlogName');

    $('#jTableContainer').jtable({
        title: '<span style="text-align: center;"><h1>Click A Title</h1></span><hr/>',
        edit: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'BlogTitle ASC',
        actions: {
            listAction: '/SIU_DAO.asmx/GetBlogRecords'
        },
        fields: {
            RefID: {
                title: 'No',
                width: '3%',
                key: true,
                create: false,
                edit: false,
                list: false
            },
            BlogTitle: {
                title: 'Title',
                sorting: true,
                width: '30%',
                listClass: 'jTableTD'
            },
            BlogText: {
                title: 'Head',
                sorting: false,
                width: '50%',
                listClass: 'jTableTD',
                display: function (data) { return data.record.BlogText.substring(0, 40); }
            },
            BlogName: { list: false },
            BlogTitleURL: { list: false }
        },
        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableContainer').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');

                    if (record.BlogTitleURL != null) {
                        if (record.BlogTitleURL.length > 0) {
                            window.open(record.BlogTitleURL, '_blank');
                        }
                    } else {
                        $('#BlogItem')[0].innerHTML = record.BlogText;    
                    }

                    
                });
            } else {
                //No rows selected
                $('#TimeRptDtlDiv').hide();
            }
        }
    });


    $('#jTableContainer').jtable('load', { BlogName: $('#BlogTOC')[0].innerHTML });


});

//,
//            deleteAction: '/SIU_DAO.asmx/DeleteBlogRecord',
//            insertAction: '/SIU_DAO.asmx/AddBlogRecord'