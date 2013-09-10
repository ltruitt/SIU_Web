
// Javascript Module
// Immediately Self Invoking Anonymous Function //
// With Jquery As A Paremeter                   //
(function ($) {

    /////////////////////////
    // Virtual Folder Root //
    /////////////////////////
    var VrDoc = null;

    ///////////////////
    // Parent Folder //
    ///////////////////
    var BackPath = null;

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

                $.SiLibrary.GetPhoneDocumentList(VrDoc, subdir, $('#DocumentFolders'));

                // Make sure to tell changePage() we've handled this call so it doesn't
                // have to do anything.
                e.preventDefault();
            }



            re = /^#backpath/;
            if (u.hash.search(re) !== -1) {
                subdir = '/' + data.options.link[0].pathname;

                $.SiLibrary.GetPhoneDocumentList(VrDoc, subdir, $('#DocumentFolders'));

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
        GetPhoneDocumentList: function (VirtualRoot, SubDirectory, DispObj) {

            /////////////////////////////////
            // Set These First Time Only.. //
            /////////////////////////////////
            if (VrDoc == null) VrDoc = VirtualRoot;

            var GetRootFolders_Ajax = new AsyncServerMethod();
            GetRootFolders_Ajax.add('VirtualDirectory', VirtualRoot);
            GetRootFolders_Ajax.add('SubDirectory', SubDirectory);
            GetRootFolders_Ajax.exec("/SIU_DAO.asmx/PhoneDocumentList", this.PhoneDocumentList_success);
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
                var DocItems = $.parseJSON(data.d);

                if (DocItems.Docs == null)
                    return;

                if (DocItems.Docs.length == 0)
                    return;

                $('#BackPath')[0].href = DocItems.BackPath + '#backpath';
                var xxx = $('#BackPath')[0].href;

                var PrevRootDir = null;

                var DocUL = "";
                var SubDirUL = "";
                var FileBtns = "";

                if (DocItems.Docs == null) {
                    alert('No Files or Directories Found');
                    return;
                }


                for (var c = 0; c < DocItems.Docs.length; c++) {

                    ///////////////////////////////
                    // Build Root Directory Item //
                    ///////////////////////////////
                    if (PrevRootDir != DocItems.Docs[c].RootDirName && DocItems.Docs[c].FileName == null) {

                        /////////////////////////////////////////////////////
                        // Close Out Any List Of Files We May Have Started //
                        /////////////////////////////////////////////////////
                        if (FileBtns.length > 0) {
                            FileBtns += ' </div>';
                            DocUL += FileBtns;
                            FileBtns = "";
                        }

                        ///////////////////////////////////////////////////////////////
                        // Close Out Any List Of Sub Directories We May Have Started //
                        ///////////////////////////////////////////////////////////////
                        if (SubDirUL.length > 0) {
                            SubDirUL += ' </ul>';
                            DocUL += SubDirUL;
                            SubDirUL = "";
                        }

                        ////////////////////////////////////////////////////////////////
                        // Close Out Any List Of Root Directories We Have In Progress //
                        ////////////////////////////////////////////////////////////////
                        if (DocUL.length > 0) {
                            DocUL += ' </div>';
                        }

                        ///////////////////////////////
                        // Build Root Directory Item //
                        ///////////////////////////////
                        DocUL += '<div data-role="collapsible" data-collapsed="true" data-role="collapsible" >';
                        DocUL += ' <h3>';
                        DocUL += DocItems.Docs[c].RootDirName;
                        DocUL += ' <div style="float: right; "> ';
                        DocUL += ' <div class="ui-li-count ui-btn-up-c ui-btn-corner-all"  >&nbsp;';
                        DocUL += DocItems.Docs[c].RootSubDirCnt + " / " + DocItems.Docs[c].RootFileCnt;
                        DocUL += ' &nbsp;</div>';
                        DocUL += ' </div>';
                        DocUL += ' </h3>';

                    }


                    else {

                        ///////////////////////////////
                        // Build Sub Dirtectory Item //
                        ///////////////////////////////
                        if (DocItems.Docs[c].FileName == null) {

                            if (SubDirUL.length == 0) {
                                SubDirUL += ' <ul class="NavUl" data-role="listview" data-inset="true" data-divider-theme="b">';
                            }


                            SubDirUL += '<li data-theme="c">';
                            SubDirUL += '  <a href="/' + DocItems.Docs[c].SubDirPath + '#directory-item" data-transition="turn" class="DirLink">';
                            SubDirUL += DocItems.Docs[c].SubDirName;
                            SubDirUL += '  </a>';
                            SubDirUL += '  <span class="ui-li-count ui-btn-up-c ui-btn-corner-all">' + DocItems.Docs[c].SubDirDirCnt + ' / ' + DocItems.Docs[c].SubFileCnt + '</span>';
                            SubDirUL += '</li>';
                        }


                        ///////////////////////////////
                        // Build Sub Dirtectory Item //
                        ///////////////////////////////
                        else {

                            ///////////////////////////////////
                            // Close List Of Sub Directories //
                            ///////////////////////////////////
                            if (SubDirUL.length > 0) {
                                SubDirUL += ' </ul>';
                                DocUL += SubDirUL;
                                SubDirUL = "";
                            }


                            /////////////////////////
                            // Now Build File List //
                            /////////////////////////
                            if (FileBtns.length == 0) {
                                DocUL += ' <div style="margin-left: 30px;"> ';
                            }

                            FileBtns += '<a href="' + DocItems.Docs[c].FilePath + '" target="LibDoc" data-role="button" data-theme="a" data-icon="' + DocItems.Docs[c].FileType + '">';
                            FileBtns += DocItems.Docs[c].FileName;
                            FileBtns += '</a>';
                        }
                    }



                    PrevRootDir = DocItems.Docs[c].RootDirName;
                }

                /////////////////////////////////////////////////////
                // Close Out Any List Of Files We May Have Started //
                /////////////////////////////////////////////////////
                if (FileBtns.length > 0) {
                    FileBtns += ' </div>';
                    DocUL += FileBtns;
                }

                ///////////////////////////////////////////////////////////////
                // Close Out Any List Of Sub Directories We May Have Started //
                ///////////////////////////////////////////////////////////////
                if (SubDirUL.length > 0) {
                    SubDirUL += ' </ul>';
                    DocUL += SubDirUL;
                }

                //////////////////////////////////////////////////////////////////
                // Destroy Ant Mobile Formatting, Insert New Content, Re-format //
                //////////////////////////////////////////////////////////////////
                $('#page1').page('destroy').page();
                $('.NavSet').html(DocUL);
                $('.NavSet').trigger("create");

            }
        }


    };
})(jQuery);



