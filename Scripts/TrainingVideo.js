var $ = jQuery;

function FloPostMovieFinish(clip) {
    if (clip.extension == 'jpg')
        return;

    var midFile = clip.url;

    var MovieStart_Ajax = new AsyncServerMethod();
    MovieStart_Ajax.add('empNo', eid);
    MovieStart_Ajax.add('TL_UID', id);
    MovieStart_Ajax.exec("/SIU_DAO.asmx/TrainingMovieComplete");
}
var eid;
var id;

$(document).ready(function () {

    //////////////////////////////
    // Get Passed In Parameters //
    //////////////////////////////
    var v = $.fn.getURLParameter('v');
    id = $.fn.getURLParameter('id');
    eid = $('#hlblEID')[0].innerHTML;

    if (v == null) return;
    if (id == null) return;
    if (eid == null) return;

    if (v.length == 0) return;
    if (id.length == 0) return;
    if (eid.length == 0) return;


    ///////////////////////////////
    // Build A Path To The Video //
    ///////////////////////////////
    var vp = "/Videos/ehsTraining/" + v;
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
            onError: function (errorCode, errorMessage) {
                alert(errorCode + " " + errorMessage);
            },
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
