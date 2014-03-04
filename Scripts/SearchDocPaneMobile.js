$(document).ready(function () {

    $('#searchBtn').click(function () {
        if ($('#txtSearch').val().length == 0)
            return;
        
        $('#SearchArea').hide();
        $('#loading').show();
        getPhoneDocumentList();
    });
    
    ///////////////////////////////
    // CR Check For Search Field //
    ///////////////////////////////
    $("#txtSearch").keyup(function (e) {
        var keyId = (window.event) ? event.keyCode : e.keyCode;

        if (keyId == 13) {
            $("#searchBtn").click();
        }
    });
    
    //////////////////////////////////////
    // Request List Of Document Folders //
    // Used On Library Document Page    //
    //////////////////////////////////////
    function getPhoneDocumentList () {

        if ($('#txtSearch').val().length == 0) {
            return;
        }
        
        var getRootFoldersAjax = new AsyncServerMethod();
        getRootFoldersAjax.add('SearchText', $('#txtSearch').val() );
        getRootFoldersAjax.exec("/SIU_DAO.asmx/g77", phoneDocumentListSuccess);
    }


    //////////////////////////////////////
    // Process List Of Document Folders //
    // Used On Library Document Page    //
    //////////////////////////////////////
    function phoneDocumentListSuccess(data) {
        $('#loading').hide();
        
        if (data.d.length > 3) {
            var docItems = $.parseJSON(data.d);

            if (docItems == null) {
                $('#SearchArea').show();
                return;
            }

            if (docItems.length == 0) {
                alert('No Files Found');
                $('#SearchArea').show();
                return;
            }

            $('#SearchArea').show();
            var fileBtns = "";

            var docUl = "";
            var subDirUl = "";
            if (subDirUl.length == 0) {
                subDirUl += ' <ul class="NavUl" data-role="listview" data-inset="true" data-divider-theme="b">';
            }


            for (var c = 0; c < docItems.length; c++) {
                ///////////////////////////////
                // Build Sub Dirtectory Item //
                ///////////////////////////////
                if (docItems[c].FileName == null) {
                    
                    /////////////////////////
                    // Now Build File List //
                    /////////////////////////
                    if (fileBtns.length == 0) {
                        docUl += ' <div style="margin-left: 30px;"> ';
                    }

                    fileBtns += '<a href="' + docItems[c].Href + '"xdata-icon="' + docItems[c].Icon + '" target="LibDoc" data-role="button" data-theme="a" >';
                    fileBtns += docItems[c].DisplayName;
                    fileBtns += '</a>';
                }

            }
            
            /////////////////////////////////////////////////////
            // Close Out Any List Of Files We May Have Started //
            /////////////////////////////////////////////////////
            if (fileBtns.length > 0) {
                fileBtns += ' </div>';
                docUl += fileBtns;
            }

            ///////////////////////////////////////////////////////////////
            // Close Out Any List Of Sub Directories We May Have Started //
            ///////////////////////////////////////////////////////////////
            if (subDirUl.length > 0) {
                subDirUl += ' </ul>';
                docUl += subDirUl;
            }

            //////////////////////////////////////////////////////////////////
            // Destroy Any Mobile Formatting, Insert New Content, Re-format //
            //////////////////////////////////////////////////////////////////
            $('#page1').page('destroy').page();
            $('.NavSet').html(docUl);
            $('.NavSet').trigger("create");
            





        }
    };

});