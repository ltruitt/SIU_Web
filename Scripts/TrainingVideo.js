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
var userName;

$(document).ready(function () {
    
    //////////////////////////////
    // Get Passed In Parameters //
    //////////////////////////////
    id = $.fn.getURLParameter('id');
    eid = $('#hlblEID')[0].innerHTML;
    userName = $('#hlblName')[0].innerHTML;

    if (id == null) return;
    if (eid == null) return;

    if (id.length == 0) return;
    if (eid.length == 0) return;

    var vp;     // Video Pointer
    var qp;     // Quiz Pointer;


    ///////////////////////////////////////////////////////////////
    // Load List Of Employees So Supr Can Change Viewed Employee //
    ///////////////////////////////////////////////////////////////
    function getRcdSuccess(data) {
        var trainingRcd = JSON.parse( data.d );
        
        ////////////////////////////////////////////
        // Setup ProProf iFrame with Link to Quiz //
        ////////////////////////////////////////////
        qp = trainingRcd.QuizName.replace(/ /g, '-');
        if (qp.length == 0) {
            $('#proprofs').hide();
            $('#NoQuiz').show('slow');
        }
        else {
            qp = 'http://www.proprofs.com/quiz-school/story.php?title=' + qp + '&ew=430&user_id=' + eid + ':' + id + '&user_name=' + userName;
            $('#proprofs')[0].src = qp;
        }
        
        //////////////////////
        // Load Flow Player //
        //////////////////////
        if (trainingRcd.VideoFile.length == 0) {
            $('#videoPlayer').hide();
            $('#NoVideo').show('slow');
        }
        else {
            vp = "/Videos/ehsTraining/" + trainingRcd.VideoFile;
            $f("videoPlayer", "/Scripts/FlowPlayer/flowplayer-3.2.11.swf",
            {
                play:
                    {
                        opacity: 0.0,
                        label: null,
                        replayLabel: 'This video is complete.  Start the quiz now.'
                    },
                clip: {
                    autoPlay: false,
                    autoBuffering: true,
                    scaling: 'fit',
                    onError: function (errorCode, errorMessage) {
                        alert(errorCode + " " + errorMessage);
                    },
                    onFinish: function (clip) { FloPostMovieFinish(clip); }
                },
                
                plugins: {
                    controls: {
                        backgroundColor: '#222222',
                        callType: 'default',
                        sliderGradient: 'none',
                        timeBorder: '0px solid rgba(0, 0, 0, 0.3)',
                        timeColor: '#ffffff',
                        volumeColor: '#ffffff',
                        bufferGradient: 'none',
                        timeBgColor: 'rgb(0, 0, 0, 0)',
                        tooltipColor: '#000000',
                        sliderColor: '#000000',
                        timeSeparator: ' ',
                        buttonOffColor: 'rgba(130,130,130,1)',
                        volumeSliderColor: '#ffffff',
                        volumeBorder: '1px solid rgba(128, 128, 128, 0.7)',
                        volumeSliderGradient: 'none',
                        backgroundGradient: [0.5,0,0.3],
                        buttonColor: '#ffffff',
                        sliderBorder: '1px solid rgba(128, 128, 128, 0.7)',
                        disabledWidgetColor: '#555555',
                        durationColor: '#a3a3a3',
                        progressGradient: 'none',
                        tooltipTextColor: '#ffffff',
                        progressColor: '#112233',
                        autoHide: 'never',
                        borderRadius: '0px',
                        buttonOverColor: '#ffffff',
                        bufferColor: '#445566',
                        height: 24,
                        opacity: 1.0
                    }
                },
                playlist: [ vp ]
            });
        }
    };


    ////////////////////////////////////////
    // Load Points Type AutoComplete List //
    ////////////////////////////////////////
    var getTrainingCall = new AsyncServerMethod();
    getTrainingCall.add('rcdId', id);
    getTrainingCall.exec("/SIU_DAO.asmx/GetTrainingRcd", getRcdSuccess);
    











    //$('.VideoLink').click(function (event) {
    //    event.preventDefault();
    //    $f("videoPlayer").play(this.href);
    //});

});
