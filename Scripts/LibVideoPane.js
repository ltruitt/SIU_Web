$(document).ready(function () {

    var VirtualRoot = $.fn.getURLParameter('VR');
    var VidPath = $.fn.getURLParameter('Path');
    var SubPath = $.fn.getURLParameter('Sub');

    if (VirtualRoot == null) VirtualRoot = "/Videos";
    if (VidPath == null) VidPath = "";
    if (SubPath == null) SubPath = "";

    ////////////////////////////////////////////////
    // Load The Root Folders Of The Video Library //
    ////////////////////////////////////////////////
    $.SiLibrary.GetSubVideoFolders(VirtualRoot, SubPath + VidPath, $('#DocumentFolders'));
    $.SiLibrary.GetVideoList(VirtualRoot, SubPath + VidPath, $('#VideoList'));

    $("#breadcrumbs").append('<li id="Videos" >Videos</li>');
    
    var SubDirParts = SubPath.split('/');
    var LastSubDir = '';
    for (var idx = 0; idx < SubDirParts.length; idx++) {
        if (SubDirParts[idx] != '') {
            LastSubDir = SubDirParts[idx];
            $("#breadcrumbs").append('<li id="' + LastSubDir + '" >' + LastSubDir.replace('/', '') + '</li>');
        }
    }
    



    if (VidPath != "") {
        $("#breadcrumbs").append('<li id="' + VidPath + '" >' + VidPath.replace('/', '') + '</li>');
    }
    $("#breadcrumbs").breadcrumbs();

}); 