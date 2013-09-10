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
    $.SiLibrary.GetSubDocumentFolders(VirtualRoot, SubPath + DocPath, $('#DocumentFolders'));
    $.SiLibrary.GetFileList(VirtualRoot, SubPath + DocPath, $('#FileList'));

    var SubDirParts = SubPath.split('/');
    var LastSubDir = '';
    for (var idx = 0; idx < SubDirParts.length; idx++) {
        if (SubDirParts[idx] != '' ) {
            LastSubDir = SubDirParts[idx];
            $("#breadcrumbs").append('<li id="' + LastSubDir + '" >' + LastSubDir.replace('/', '') + '</li>');
        }
    }
    


    if (DocPath != "") {
        $("#breadcrumbs").append('<li id="' + DocPath + '" >' + DocPath.replace('/', '') + '</li>');
    }
    $("#breadcrumbs").breadcrumbs();

}); 