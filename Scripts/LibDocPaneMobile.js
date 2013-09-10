$(document).ready(function () {

    var virtualRoot = $.fn.getURLParameter('VR');
    var docPath = $.fn.getURLParameter('Path');
    var subPath = $.fn.getURLParameter('Sub');
    var backPath = $.fn.getURLParameter('Back');

    if (virtualRoot == null) virtualRoot = "/Files";
    if (docPath == null) docPath = "";
    if (subPath == null) subPath = "/Library";
    if (backPath != null) {
        $('#hrefBack').attr("href", window.location.protocol + '//' + window.location.hostname + backPath);
        //$('#hrefBackText')[0].innerText = backPath.replace(/\//g, '  ').replace('.aspx', '');
    } else {
        $('#hrefBackText').hide();
        $('#headerText').css('margin-top', 6);
    }

    
    ////////////////////////////////////////////////
    // Load The Root Folders Of The Video Library //
    ////////////////////////////////////////////////
    $.SiLibrary.GetPhoneDocumentList(virtualRoot, subPath + docPath, $('#DocumentFolders'));

});


// Javascript Module
// Immediately Self Invoking Anonymous Function //
// With Jquery As A Paremeter                   //
(function ($) {

    /////////////////////////
    // Virtual Folder Root //
    /////////////////////////
    var vrDoc = null;

    ///////////////////////////////////////////////////
    // Listen for any attempts to call changePage(). //
    ///////////////////////////////////////////////////
    $(document).bind("pagebeforechange", function (e, data) {

        // We only want to handle changePage() calls where the caller is
        // asking us to load a page by URL.
        if (typeof data.toPage === "string") {

            // We are being asked to load a page by URL, but we only
            // want to handle URLs that request the data for a specific
            // category.
            var u = $.mobile.path.parseUrl(data.toPage);
            var re = /^#directory-item/;

            if (u.hash.search(re) !== -1) {

                var subdir = '/' + data.options.link[0].pathname;

                $.SiLibrary.GetPhoneDocumentList(vrDoc, subdir, $('#DocumentFolders'));

                // Make sure to tell changePage() we've handled this call so it doesn't
                // have to do anything.
                e.preventDefault();
            }



            re = /^#backpath/;
            if (u.hash.search(re) !== -1) {
                subdir = '/' + data.options.link[0].pathname;

                $.SiLibrary.GetPhoneDocumentList(vrDoc, subdir, $('#DocumentFolders'));

                // Make sure to tell changePage() we've handled this call so it doesn't
                // have to do anything.
                e.preventDefault();
            }
        }
    });



    // Extend Jquery With A New Module Definition //
    $.SiLibrary = {

        //////////////////////////
        // AJAX REQUEST METHODS //
        //////////////////////////

        //////////////////////////////////////
        // Request List Of Document Folders //
        // Used On Library Document Page    //
        //////////////////////////////////////
        GetPhoneDocumentList: function (virtualRoot, subDirectory) {

            /////////////////////////////////
            // Set These First Time Only.. //
            /////////////////////////////////
            if (vrDoc == null) vrDoc = virtualRoot;

            var getRootFoldersAjax = new AsyncServerMethod();
            getRootFoldersAjax.add('VirtualDirectory', virtualRoot);
            getRootFoldersAjax.add('SubDirectory', subDirectory);
            getRootFoldersAjax.exec("/SIU_DAO.asmx/PhoneDocumentList", this.PhoneDocumentList_success);
        },


        ///////////////////////////
        // AJAX RESPONSE METHODS //
        ///////////////////////////


        //////////////////////////////////////
        // Process List Of Document Folders //
        // Used On Library Document Page    //
        //////////////////////////////////////
        PhoneDocumentList_success: function (data) {
            if (data.d.length > 3) {
                var docItems = $.parseJSON(data.d);

                if (docItems.Docs == null)
                    return;

                if (docItems.Docs.length == 0)
                    return;

                $('#BackPath')[0].href = docItems.BackPath + '#backpath';

                var prevRootDir = null;

                var docUl = "";
                var subDirUl = "";
                var fileBtns = "";

                if (docItems.Docs == null) {
                    alert('No Files or Directories Found');
                    return;
                }


                for (var c = 0; c < docItems.Docs.length; c++) {

                    ///////////////////////////////
                    // Build Root Directory Item //
                    ///////////////////////////////
                    if (prevRootDir != docItems.Docs[c].RootDirName && docItems.Docs[c].FileName == null) {

                        /////////////////////////////////////////////////////
                        // Close Out Any List Of Files We May Have Started //
                        /////////////////////////////////////////////////////
                        if (fileBtns.length > 0) {
                            fileBtns += ' </div>';
                            docUl += fileBtns;
                            fileBtns = "";
                        }

                        ///////////////////////////////////////////////////////////////
                        // Close Out Any List Of Sub Directories We May Have Started //
                        ///////////////////////////////////////////////////////////////
                        if (subDirUl.length > 0) {
                            subDirUl += ' </ul>';
                            docUl += subDirUl;
                            subDirUl = "";
                        }

                        ////////////////////////////////////////////////////////////////
                        // Close Out Any List Of Root Directories We Have In Progress //
                        ////////////////////////////////////////////////////////////////
                        if (docUl.length > 0) {
                            docUl += ' </div>';
                        }

                        ///////////////////////////////
                        // Build Root Directory Item //
                        ///////////////////////////////
                        docUl += '<div data-role="collapsible" data-collapsed="true" data-role="collapsible" >';
                        docUl += ' <h3>';
                        docUl += docItems.Docs[c].RootDirName;
                        docUl += ' <div style="float: right; "> ';
                        docUl += ' <div class="ui-li-count ui-btn-up-c ui-btn-corner-all"  >&nbsp;';
                        docUl += docItems.Docs[c].RootSubDirCnt + " / " + docItems.Docs[c].RootFileCnt;
                        docUl += ' &nbsp;</div>';
                        docUl += ' </div>';
                        docUl += ' </h3>';

                    }


                    else {

                        ///////////////////////////////
                        // Build Sub Dirtectory Item //
                        ///////////////////////////////
                        if (docItems.Docs[c].FileName == null) {

                            if (subDirUl.length == 0) {
                                subDirUl += ' <ul class="NavUl" data-role="listview" data-inset="true" data-divider-theme="b">';
                            }


                            subDirUl += '<li data-theme="c">';
                            subDirUl += '  <a href="/' + docItems.Docs[c].SubDirPath + '#directory-item" data-transition="turn" class="DirLink">';
                            subDirUl += docItems.Docs[c].SubDirName;
                            subDirUl += '  </a>';
                            subDirUl += '  <span class="ui-li-count ui-btn-up-c ui-btn-corner-all">' + docItems.Docs[c].SubDirDirCnt + ' / ' + docItems.Docs[c].SubFileCnt + '</span>';
                            subDirUl += '</li>';
                        }


                            ///////////////////////////////
                            // Build Sub Dirtectory Item //
                            ///////////////////////////////
                        else {

                            ///////////////////////////////////
                            // Close List Of Sub Directories //
                            ///////////////////////////////////
                            if (subDirUl.length > 0) {
                                subDirUl += ' </ul>';
                                docUl += subDirUl;
                                subDirUl = "";
                            }


                            /////////////////////////
                            // Now Build File List //
                            /////////////////////////
                            if (fileBtns.length == 0) {
                                docUl += ' <div style="margin-left: 30px;"> ';
                            }

                            fileBtns += '<a href="' + docItems.Docs[c].FilePath + '" target="LibDoc" data-role="button" data-theme="a" data-icon="' + docItems.Docs[c].FileType + '">';
                            fileBtns += docItems.Docs[c].FileName;
                            fileBtns += '</a>';
                        }
                    }



                    prevRootDir = docItems.Docs[c].RootDirName;
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
        }


    };
})(jQuery);



