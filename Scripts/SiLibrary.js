
// Javascript Module
// Immediately Self Invoking Anonymous Function //
// With Jquery As A Paremeter                   //
 (function ($) {

     //////////////////////////////////////////////////////
     // Object Refs That Display Directory and File List //
     //////////////////////////////////////////////////////
     var docDispObj = null;
     var vidDispObj = null;
     var fileDispObj = null;
     var videoDispObj = null;

     var departmentDocumentFoldersTarget = null;
     var departmentVideoFoldersTarget = null;

     /////////////////////////
     // Virtual Folder Root //
     /////////////////////////
     var vrDoc = null;
     var vrVid = null;

     /////////////////////////////////////////////
     // Name Of Folder Clicked On By User       //
     // Held Until Ajax Response of Sub Folders //
     // Update Breadcrumbs IF sub folders only  //
     /////////////////////////////////////////////
     var clickedFolder = null;



     ///////////////////////////////////////
     // Create Click Event For Menu Items //
     ///////////////////////////////////////
     $(document).on("click", ".breadcrumb_item", function () {
         var clickedCrumb = this.id;

         ////////////////////////////////////////////////////
         // If They Clicked On The Root Library BreadCrumb //
         // Send Them To The Library Page                  //
         ////////////////////////////////////////////////////
         if (clickedCrumb.toLowerCase() == 'library') {
             window.location.href = "http://" + window.location.hostname + "/Library/LibHome.aspx";
             return;
         }

         ////////////////////////////////////////////////////
         // If They Clicked On The Root Library BreadCrumb //
         // Send Them To The Library Page                  //
         ////////////////////////////////////////////////////
         if (clickedCrumb.toLowerCase() == 'videos') {
             window.location.href = "http://" + window.location.hostname + "/Library/LibHome.aspx";
             return;
         }
         
         //////////////////////////////////////////////////
         // Ignore Breadcrumb Click On Current Directory //
         //////////////////////////////////////////////////
         if (clickedCrumb == buildPathFromBreadCrumbs())
             return;

         //////////////////////////////////////////////
         // Don't Re-Add This Folder As A Breadcrumb //
         // Will Leave It Showing Below              //
         //////////////////////////////////////////////
         clickedFolder = null;

         //////////////////////////////////////////////////////
         // Otherwise, Start Lookup Of Sub Directory Folders //
         //////////////////////////////////////////////////////
         var subPath = buildPathFromBreadCrumbs(clickedCrumb);
         subPath = clickedCrumb;
         subPath = subPath.replace(vrVid, '');
         if (fileDispObj != null) {
             $.SiLibrary.GetSubDocumentFolders(vrDoc, subPath, docDispObj);
             $.SiLibrary.GetFileList(vrDoc, subPath, fileDispObj);
         }
         else {
             $.SiLibrary.GetSubVideoFolders(vrVid, subPath, docDispObj);
             $.SiLibrary.GetVideoList(vrVid, subPath, videoDispObj);
         }


         ///////////////////////////////////////////////////
         // Strip Down Breadcrumbs To Match Selected Item //
         ///////////////////////////////////////////////////
         var beginStrip = 0;
         $(".breadcrumb_e").each(function () {

             ////////////////////////////////////////////
             // Once We Located The Clicked Breadcrumb //
             // Remove This and Each Trailing Crumb    //
             ////////////////////////////////////////////
             if (beginStrip == 1) {
                 if (!$(this).hasClass('breadcrumb_end'))
                     $(this).remove();
             }

             ///////////////////////////////////////
             // Is This the Clicked On Breadcrumb //
             ///////////////////////////////////////
             if ($(this)[0].id == clickedCrumb)
                 beginStrip = 1;


         });
     });

     ///////////////////////////////////////
     // Create Click Event For Menu Items //
     ///////////////////////////////////////
     $(document).on('click', '*[id*=DocumentFolderA_]', function () {
         clickedFolder = this.id.substring(16);
         var p = buildPathFromBreadCrumbs() + '/' + clickedFolder;
         $.SiLibrary.GetSubDocumentFolders(vrDoc, p, docDispObj);
         $.SiLibrary.GetFileList(vrDoc, p, fileDispObj);
     });

     ///////////////////////////////////////
     // Create Click Event For Menu Items //
     ///////////////////////////////////////
     $(document).on('click', '*[id*=VideoFolderA_]', function () {
         clickedFolder = this.id.substring(13);
         var p = buildPathFromBreadCrumbs() + '/' + clickedFolder;
         p = String(p).replace(vrVid, '');
         $.SiLibrary.GetSubVideoFolders(vrVid, p, docDispObj);
         $.SiLibrary.GetVideoList(vrVid, p, videoDispObj);
     });

     function buildPathFromBreadCrumbs(stopOnThisItem) {
         var p = '';
         var s = String(stopOnThisItem).toLowerCase().replace('/', '');
         $(".breadcrumb_item").each(function () {

             ////////////////////////////////////////////
             // Once We Located The Clicked Breadcrumb //
             // Remove This and Each Trailing Crumb    //
             ////////////////////////////////////////////
             p += '/' + $(this)[0].innerText;
             if ($(this)[0].innerText.toLowerCase() == s) {
                 return (false);  // = break;
             }
         });

         return p;
     }

     // Extend Jquery With A New Module Definition //

     $.SiLibrary = {

         //////////////////////////
         // AJAX REQUEST METHODS //
         //////////////////////////
         //////////////////////////////////////
         // Request List Of Document Folders //
         // Used On Library Page             //
         //////////////////////////////////////
         GetRootDocumentFolders: function (virtualRoot, subDirectory, dispObj) {
             docDispObj = dispObj;
             vrDoc = virtualRoot;

             var getRootFoldersAjax = new AsyncServerMethod();
             getRootFoldersAjax.add('VirtualDirectory', virtualRoot);
             getRootFoldersAjax.add('SubDirectory', subDirectory);
             getRootFoldersAjax.exec("/SIU_DAO.asmx/ListDocumentDirectories", this.GetRootDocumentFolders_success);
         },

         ///////////////////////////////////
         // Request List Of Video Folders //
         ///////////////////////////////////
         GetRootVideoFolders: function (virtualRoot, subDirectory, dispObj) {
             vidDispObj = dispObj;
             vrVid = virtualRoot;

             var getRootFoldersAjax = new AsyncServerMethod();
             getRootFoldersAjax.add('VirtualDirectory', virtualRoot);
             getRootFoldersAjax.add('SubDirectory', subDirectory);
             getRootFoldersAjax.exec("/SIU_DAO.asmx/ListVideoDirectories", this.GetRootVideoFolders_success);
         },


         //////////////////////////////////////
         // Request List Of Document Folders //
         // Used On Library Document Page    //
         //////////////////////////////////////
         GetSubDocumentFolders: function (virtualRoot, subDirectory, dispObj) {

             /////////////////////////////////
             // Set These First Time Only.. //
             /////////////////////////////////
             if (docDispObj == null) docDispObj = dispObj;
             if (vrDoc == null) vrDoc = virtualRoot;

             var getRootFoldersAjax = new AsyncServerMethod();
             getRootFoldersAjax.add('VirtualDirectory', virtualRoot);
             getRootFoldersAjax.add('SubDirectory', subDirectory);
             getRootFoldersAjax.exec("/SIU_DAO.asmx/ListDocumentDirectories", this.GetSubDocumentFolders_success);
         },

         //////////////////////////////////////
         // Request List Of Document Folders //
         //////////////////////////////////////
         GetSubVideoFolders: function (virtualRoot, subDirectory, dispObj) {

             //////////////////////////////////
             // Set These First Time Only..  //
             //////////////////////////////////
             if (vidDispObj == null) vidDispObj = dispObj;
             if (vrVid == null) vrVid = virtualRoot;

             var getRootFoldersAjax = new AsyncServerMethod();
             getRootFoldersAjax.add('VirtualDirectory', virtualRoot);
             getRootFoldersAjax.add('SubDirectory', subDirectory);
             getRootFoldersAjax.exec("/SIU_DAO.asmx/ListDocumentDirectories", this.GetSubVideoFolders_success);
         },


         /////////////////
         // Work Around //
         /////////////////
         SetFileDisplayObject: function (dispObj) {
             fileDispObj = dispObj;
         },

         ///////////////////////////////
         // Request List Of Documents //
         ///////////////////////////////         
         GetFileList: function (virtualRoot, subDirectory, dispObj) {
             fileDispObj = dispObj;
             fileDispObj.html('');

             var GetFileList_Ajax = new AsyncServerMethod();
             GetFileList_Ajax.add('VirtualDirectory', virtualRoot);
             GetFileList_Ajax.add('SubDirectory', subDirectory);
             GetFileList_Ajax.exec("/SIU_DAO.asmx/ListFiles", this.GetFileList_success);
         },


         /////////////////
         // Work Around //
         /////////////////
         SetVideoDisplayObject: function (dispObj) {
             videoDispObj = dispObj;
         },

         ////////////////////////////
         // Request List Of Videos //
         ////////////////////////////
         GetVideoList: function (virtualRoot, subDirectory, dispObj) {
             videoDispObj = dispObj;
             videoDispObj.html('');

             var getVideoListAjax = new AsyncServerMethod();
             getVideoListAjax.add('VirtualDirectory', virtualRoot);
             getVideoListAjax.add('SubDirectory', subDirectory);
             getVideoListAjax.exec("/SIU_DAO.asmx/ListVideos", this.GetVideoList_success);
         },


         //////////////////////////////////////////////////////
         // Get List Of Department Specific Document Folders //
         //////////////////////////////////////////////////////
         GetDepartmentDocumentFolders: function (virDir, subDir, target) {
             departmentDocumentFoldersTarget = target;
             vrDoc = virDir;

             var getDepartmentDocumentFoldersAjax = new AsyncServerMethod();
             getDepartmentDocumentFoldersAjax.add('VirtualDirectory', virDir);
             getDepartmentDocumentFoldersAjax.add('SubDirectory', subDir);
             getDepartmentDocumentFoldersAjax.exec("/SIU_DAO.asmx/ListDocumentDirectories", this.GetDepartmentDocumentFolders_success);
         },


         ///////////////////////////////////////////////////
         // Get List Of Department Specific Video Folders //
         ///////////////////////////////////////////////////
         GetDepartmentVideoFolders: function (virDir, subDir, target) {
             departmentVideoFoldersTarget = target;
             vrVid = virDir;

             var getDepartmentVideoFoldersAjax = new AsyncServerMethod();
             getDepartmentVideoFoldersAjax.add('VirtualDirectory', virDir);
             getDepartmentVideoFoldersAjax.add('SubDirectory', subDir);
             getDepartmentVideoFoldersAjax.exec("/SIU_DAO.asmx/ListDocumentDirectories", this.GetDepartmentVideoFolders_success);
         },




         ///////////////////////////
         // AJAX RESPONSE METHODS //
         ///////////////////////////
         //////////////////////////////////////
         // Process List Of Document Folders //
         // Used On Library Page             //
         //////////////////////////////////////
         GetRootDocumentFolders_success: function (data) {
             if (data.d.length > 3) {
                 var docItems = $.parseJSON(data.d);

                 if (docItems == null)
                     return;


                 var docUl = "";
                 for (var c = 0; c < docItems.length; c++) {
                     docUl += '<li class="VideoLI">';

                     docUl += '<a href="/Library/LibDocPaneMobile.aspx?VR=' + vrDoc + '&Path=/' + docItems[c].ShortName + '&Sub=/Library"' + ' class="VideoMenu">';

                     docUl += docItems[c].ShortName;
                     docUl += '</a></li>';
                 }

                 ///////////////////////
                 // Load Menu Options //
                 ///////////////////////
                 if (docItems.length > 0) {
                     docDispObj.html(docUl);
                 }
             }
         },

         //////////////////////////////////////
         // Process List Of Document Folders //
         // Used On Library Document Page    //
         //////////////////////////////////////
         GetSubDocumentFolders_success: function (data) {
             if (data.d.length > 3) {
                 var subDocItems = $.parseJSON(data.d);

                 if (subDocItems == null)
                     return;


                 var docUl = "";
                 for (var c = 0; c < subDocItems.length; c++) {
                     docUl += '<li class="VideoLI">';
                     docUl += '<a id="DocumentFolderA_' + subDocItems[c].ShortName + '" href="#" class="VideoMenu">';
                     docUl += subDocItems[c].ShortName;
                     docUl += '</a></li>';
                 }

                 ////////////////////////////////////////////////////////////////////////////////
                 // A Folder Item Was Clicked On And Possibly A List Of Sub Folders Returned   //
                 // If Sub Folders Were Returned, Update The Breadcrumbs And Current Directory //
                 ////////////////////////////////////////////////////////////////////////////////
                 ///////////////////////
                 if (subDocItems.length > 0) {
                     docDispObj.html(docUl);

                     if (clickedFolder != null) {

                         ///////////////////////////////////
                         // Update Current Folder Pointer //
                         ///////////////////////////////////
                         var p = buildPathFromBreadCrumbs() + '/' + clickedFolder;

                         ///////////////////////////////
                         // Update Breadcrumb display //
                         ///////////////////////////////
                         $("#breadcrumbs").last().append('<li id="' + p + '" >' + clickedFolder + '</li>');
                         $("#breadcrumbs").breadcrumbs();
                     }
                 }
             }
         },



         ///////////////////////////////////
         // Process List Of Video Folders //
         // Used On Library Page          //
         ///////////////////////////////////        
         GetRootVideoFolders_success: function (data) {
             if (data.d.length > 3) {
                 var vidItems = $.parseJSON(data.d);

                 if (vidItems == null)
                     return;


                 var videoUl = "";
                 for (var c = 0; c < vidItems.length; c++) {
                     videoUl += '<li class="VideoLI">';
                     videoUl += '<a href="/Library/LibVideoPane.aspx?VR=' + vrVid + '&Path=/' + vidItems[c].ShortName + '"' + ' class="VideoMenu">';

                     videoUl += vidItems[c].ShortName;
                     videoUl += '</a></li>';
                 }

                 ///////////////////////
                 // Load Menu Options //
                 ///////////////////////
                 if (vidItems.length > 0) {
                     vidDispObj.html(videoUl);
                 }
             }
         },

         ///////////////////////////////////
         // Process List Of Video Folders //
         ///////////////////////////////////
         GetSubVideoFolders_success: function (data) {
             if (data.d.length > 3) {
                 var subVidItems = $.parseJSON(data.d);

                 if (subVidItems == null)
                     return;


                 var vidUl = "";
                 for (var c = 0; c < subVidItems.length; c++) {
                     vidUl += '<li class="VideoLI">';
                     vidUl += '<a id="VideoFolderA_' + subVidItems[c].ShortName + '" href="#" class="VideoMenu">';
                     vidUl += subVidItems[c].ShortName;
                     vidUl += '</a></li>';
                 }

                 ////////////////////////////////////////////////////////////////////////////////
                 // A Folder Item Was Clicked On And Possibly A List Of Sub Folders Returned   //
                 // If Sub Folders Were Returned, Update The Breadcrumbs And Current Directory //
                 ////////////////////////////////////////////////////////////////////////////////
                 ///////////////////////
                 if (subVidItems.length > 0) {
                     vidDispObj.html(vidUl);

                     if (clickedFolder != null) {

                         ///////////////////////////////////
                         // Update Current Folder Pointer //
                         ///////////////////////////////////
                         var p = buildPathFromBreadCrumbs() + '/' + clickedFolder;

                         ///////////////////////////////
                         // Update Breadcrumb display //
                         ///////////////////////////////
                         $("#breadcrumbs").last().append('<li id="' + p + '" >' + clickedFolder + '</li>');
                         $("#breadcrumbs").breadcrumbs();
                     }
                 }



             }
         },




         ///////////////////////////
         // Process List Of Files //
         ///////////////////////////        
         GetFileList_success: function (data) {
             if (data.d.length > 3) {
                 var fileItems = $.parseJSON(data.d);

                 if (fileItems == null)
                     return;


                 var fileUl = "";
                 for (var c = 0; c < fileItems.length; c++) {
                     fileUl += '<li class="VideoLI">';
                     fileUl += '<a id="FileFileA_' + fileItems[c].ShortName + '" href="' + fileItems[c].Href + '" class="VideoMenu">';

                     fileUl += fileItems[c].ShortName;
                     fileUl += '</a></li>';
                 }

                 ///////////////////////
                 // Load Menu Options //
                 ///////////////////////
                 if (fileItems.length > 0) {
                     fileDispObj.html(fileUl);
                 }
             }
         },

         ///////////////////////////
         // Process List Of Files //
         ///////////////////////////        
         GetVideoList_success: function (data) {
             if (data.d.length > 3) {
                 var videoItems = $.parseJSON(data.d);

                 if (videoItems == null)
                     return;

                 var P = buildPathFromBreadCrumbs();
                 var videoUl = "";
                 for (var c = 0; c < videoItems.length; c++) {
                     videoUl += '<li class="VideoLI">';
                     videoUl += '<a id="VideoFileA_' + videoItems[c].ShortName + '" href="/video/FlowPlayerVideo.aspx?Video=' + videoItems[c].ShortName + videoItems[c].Ext + '&Path=' + buildPathFromBreadCrumbs();
                     

                     if (clickedFolder != null)
                         videoUl += '/' + clickedFolder;
                     videoUl += '" target="_VideoPlayer" class="VideoMenu">';

                     videoUl += videoItems[c].ShortName;
                     videoUl += '</a></li>';
                 }

                 ///////////////////////
                 // Load Menu Options //
                 ///////////////////////
                 if (videoItems.length > 0) {
                     videoDispObj.html(videoUl);
                 }
             }
         },



         //////////////////////////////////////
         // Process List Of Document Folders //
         //////////////////////////////////////
         GetDepartmentDocumentFolders_success: function (data) {
             if (data.d.length > 3) {
                 var subDocItems = $.parseJSON(data.d);

                 if (subDocItems == null)
                     return;

                 var docLi = "";
                 for (var c = 0; c < subDocItems.length; c++) {
                     docLi = '<li><a  href="/Library/LibDocPaneMobile.aspx?VR=' + vrDoc + '&Sub=' + subDocItems[c].SubPath + '&Path=' + subDocItems[c].ShortName + '&Back=' + window.location.pathname + '">';
                     docLi += subDocItems[c].DisplayName;
                     docLi += '</a></li>';
                     $('#' + departmentDocumentFoldersTarget).last().append(docLi);
                 }
             }
         },

         //////////////////////////////////////
         // Process List Of Document Folders //
         //////////////////////////////////////
         GetDepartmentVideoFolders_success: function (data) {
             if (data.d.length > 3) {
                 var subVidItems = $.parseJSON(data.d);

                 if (subVidItems == null)
                     return;

                 var docLi = "";
                 for (var c = 0; c < subVidItems.length; c++) {
                     docLi = '<li><a  href="/Library/LibVideoPane.aspx?VR=' + vrVid + '&Sub=' + subVidItems[c].SubPath + '&Path=' + subVidItems[c].ShortName + '">';
                     docLi += subVidItems[c].ShortName;
                     docLi += '</a></li>';
                     $('#' + departmentVideoFoldersTarget).last().append(docLi);
                 }
             }
         }
     };
 })(jQuery);



