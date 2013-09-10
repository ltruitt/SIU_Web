var $ = jQuery;
        

function JwPostMovieStart() {
    // Get A Name Value Pair Description Of THe Currently Playing Video
    var mid = jwplayer().getPlaylistItem();

    // Extract The File Name From The Name Value Pair Object
    var midFile = mid.file;

    var pos = jwplayer().getPosition();
    var dur = jwplayer().getDuration();
    var MovieStart_Ajax = new AsyncServerMethod();
    
    MovieStart_Ajax.add('EmpName', $('#hlblEmpName')[0].innerHTML);
    MovieStart_Ajax.add('EmpEmail', $('#hlblEmpEmail')[0].innerHTML);
    MovieStart_Ajax.add('movieId', midFile);
    MovieStart_Ajax.add('pos', pos);
    MovieStart_Ajax.add('dur', dur);
    MovieStart_Ajax.exec("/SIU_DAO.asmx/MovieStart");
}
function JwPostMovieComplete() {
    // Get A Name Value Pair Description Of THe Currently Playing Video
    var mid = jwplayer().getPlaylistItem();

    // Extract The File Name From The Name Value Pair Object
    var midFile = mid.file;
    var MovieComplete_Ajax = new AsyncServerMethod();
    
    MovieComplete_Ajax.add('EmpName', $('#hlblEmpName')[0].innerHTML);
    MovieComplete_Ajax.add('EmpEmail', $('#hlblEmpEmail')[0].innerHTML);
    MovieComplete_Ajax.add('movieId', midFile);
    MovieComplete_Ajax.exec("/SIU_DAO.asmx/MovieComplete");

}



function FloPostMovieStart(clip) {
    if (clip.extension == 'jpg')
        return;
    
    var midFile = clip.url;
    var dur = clip.duration;
    var pos = 0;
    
    var MovieStart_Ajax = new AsyncServerMethod();

    MovieStart_Ajax.add('empNo', eid );
    MovieStart_Ajax.add('movieId', midFile);
    MovieStart_Ajax.add('pos', pos);
    MovieStart_Ajax.add('dur', dur);
    MovieStart_Ajax.exec("/SIU_DAO.asmx/MovieStart");
}

function FloPostMovieSeek(clip, TimeIdx) {
    if (clip.extension == 'jpg')
        return;
    
    var midFile = clip.url;
    var dur = clip.duration;
    var pos = TimeIdx;

    var MovieStart_Ajax = new AsyncServerMethod();

    MovieStart_Ajax.add('empNo', eid );
    MovieStart_Ajax.add('movieId', midFile);
    MovieStart_Ajax.add('pos', pos);
    MovieStart_Ajax.add('dur', dur);
    MovieStart_Ajax.exec("/SIU_DAO.asmx/MovieStart");
}


function FloPostMovieFinish(clip) {
    if (clip.extension == 'jpg')
        return;
    
    var midFile = clip.url;
    var dur = clip.duration;

    var MovieStart_Ajax = new AsyncServerMethod();

    MovieStart_Ajax.add('empNo', eid );
    MovieStart_Ajax.add('movieId', midFile);
    MovieStart_Ajax.add('pos', dur);
    MovieStart_Ajax.add('dur', dur);
    MovieStart_Ajax.exec("/SIU_DAO.asmx/MovieComplete");
}


var eid;

$(document).ready(function () {

    ///////////////////////////////////////////
    // Get Jason Array Of Total Hours By Day //
    ///////////////////////////////////////////
    function GetSupportDocuments_success(data) {
        $('#SupportDocumentsInsertPoint')[0].innerHTML = $.parseJSON(data.d);
    }
    function GetSupportDocuments() {
        var GetSupportDocuments_Ajax = new AsyncServerMethod();
        GetSupportDocuments_Ajax.add('Path', $.fn.getURLParameter('Path'));
        GetSupportDocuments_Ajax.add('File', $.fn.getURLParameter('Video'));
        GetSupportDocuments_Ajax.exec("/SIU_DAO.asmx/ListVideoSupportDocuments", GetSupportDocuments_success);
    }
    GetSupportDocuments();


    //////////////////////////////
    // Get Passed In Parameters //
    //////////////////////////////
    var p = $.fn.getURLParameter('Path');
    var v = $.fn.getURLParameter('Video');
    //eid = $.fn.getURLParameter('EID');
    eid = $('#hlblEID')[0].innerHTML;

    if (p == null) return;
    if (v == null) return;
    if (eid == null) return;

    if (p.length == 0) return;
    if (v.length == 0) return;
    if (eid.length == 0) return;



    ///////////////////////////////
    // Build A Path To The Video //
    ///////////////////////////////
    var vp = p + "/" + v;
    vp = vp.replace(/ /g, '%20');

    //////////////////////
    // Load Flow Player //
    //////////////////////
    $f("videoPlayer", "/Scripts/FlowPlayer/flowplayer-3.2.11.swf",
    {
        play:
            {
                opacity: 0.0,
                label: null, // label text; by default there is no text
                replayLabel: null // label text at end of video clip
            },
        clip: {
            autoPlay: true,
            autoBuffering: true,
            scaling: 'fit',
            onStart: function (clip) { FloPostMovieStart(clip); },
            onError: function (errorCode, errorMessage) {
                 alert(errorCode + " " + errorMessage);
            },
            onSeek: function (clip, TimeIdx) { FloPostMovieSeek(clip, TimeIdx); },
            onFinish: function (clip) { FloPostMovieFinish(clip); }
        },
        playlist: [
                    { url: "/images/Si-Shermco-OneLine.jpg", duration: 5 },
                    vp
                    ]
    });

    $('.VideoLink').click(function (event) {
        event.preventDefault();
        $f("videoPlayer").play(this.href);
    });

});
