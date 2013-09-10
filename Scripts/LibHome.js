$(document).ready(function () {

    ////////////////////////////////
    // Initialize The Blog Slider //
    ////////////////////////////////
    //$('#BlogSlider').anythingSlider({
    //    hashTags: false,
    //    theme: "HomeBlogSliderTheme",
    //    delay: 6000,
    //    mode: "vertical",
    //    autoPlay: true,
    //    buildArrows: false,
    //    buildNavigation: false,
    //    buildStartStop: false
    //});

    /////////////////////////////
    // Turn The Blog Slider On //
    /////////////////////////////
    //$('#BlogSlider').show();

    ///////////////////////////////////////////////////
    // Load The Root Folders Of The Document Library //
    ///////////////////////////////////////////////////
    //$.SiLibrary.GetRootDocumentFolders('/Files', '/Library', $('#DocumentFolders'));
    //$.SiLibrary.SetFileDisplayObject($('#FileList'));

    ////////////////////////////////////////////////
    // Load The Root Folders Of The Video Library //
    ////////////////////////////////////////////////
    $.SiLibrary.GetRootVideoFolders('/Videos', '/', $('#VideoFolders'));
    $.SiLibrary.SetVideoDisplayObject($('#VideoList'));
}); 