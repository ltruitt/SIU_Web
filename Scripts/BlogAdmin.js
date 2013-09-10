$(document).ready(function () {






    $('#jTableBlogItems').jtable({
        title: '<span style="text-align: center;"><h1>Click A Title</h1></span><hr/>',
        edit: true,
        insert: true,
        selecting: true,
        sorting: true,
        multiselect: false,
        selectingCheckboxes: false,
        defaultSorting: 'BlogTitle ASC',
        actions: {
            listAction: '/SIU_DAO.asmx/GetBlogRecords',
            deleteAction: '/SIU_DAO.asmx/DeleteBlogRecord',
            createAction: '/SIU_DAO.asmx/AddBlogRecord',
            updateAction: '/SIU_DAO.asmx/UpdBlogRecord'
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
                title: 'Content',
                sorting: false,
                width: '50%',
                listClass: 'jTableTD',
                display: function (data) { return data.record.BlogText.substring(0, 40); }
            },
            BlogName: { list: false, edit: false, create: false },
            BlogTitleURL: { list: false, create: true, edit: true, title: 'URL' }
        },
        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableBlogItems').jtable('selectedRows');

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







    $('#BlogTOC')[0].innerHTML = $.fn.getURLParameter('BlogName');
    var BlogList_Ajax = new AsyncServerMethod();
    BlogList_Ajax.exec("/SIU_DAO.asmx/ListBlogs", ListBlogs_Success);


    ///////////////////////////////////////////
    // Get Jason Array Of Total Hours By Day //
    ///////////////////////////////////////////
    function ListBlogs_Success(data) {
        var BlogList = $.parseJSON(data.d);

        var BlogUL = "";
        for (var c = 0; c < BlogList.length; c++) {
            BlogUL += '<li class="VideoLI">';

            BlogUL += '<a href="#" class="BlogMenu">';

            BlogUL += BlogList[c];
            BlogUL += '</a></li>';
        }

        $('#BlogsUl').html(BlogUL);


        $('.BlogMenu').on('click', function () {
            var SelectedBlog = $(this)[0].innerHTML;
            $('#jTableBlogItems').jtable('load', { BlogName: SelectedBlog });
        });
    }

});

