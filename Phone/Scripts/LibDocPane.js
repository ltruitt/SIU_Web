$(document).ready(function () {

    var VirtualRoot = $.fn.getURLParameter('VR');
    var DocPath = $.fn.getURLParameter('Path');
    var SubPath = $.fn.getURLParameter('Sub');

    if (VirtualRoot == null) VirtualRoot = "/Files";
    if (DocPath == null) DocPath = "";
    if (SubPath == null) SubPath = "/Library";

    ////////////////////////////////////////////////
    // Load The Root Folders Of The Video Library //
    ////////////////////////////////////////////////
    $.SiLibrary.GetPhoneDocumentList(VirtualRoot, SubPath + DocPath, $('#DocumentFolders'));






//    try {
//        $(function () {
//        });
//    } catch (error) {
//        console.error("Your javascript has an error: " + error);
//    }
}); 