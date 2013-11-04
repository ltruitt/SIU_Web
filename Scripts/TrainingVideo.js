var $ = jQuery;

function FloPostMovieFinish(clip) {
    if (clip.extension == 'jpg')
        return;

    var movieStartAjax = new AsyncServerMethod();
    movieStartAjax.add('empNo', eid);
    movieStartAjax.add('TL_UID', id);
    movieStartAjax.exec("/SIU_DAO.asmx/TrainingMovieComplete");
}
var eid;
var id;
var userName;
var vp;     // Video Pointer
var qp;     // Quiz Pointer;

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


    ///////////////////////////////////////////////////////////////
    // 
    ///////////////////////////////////////////////////////////////
    function getRcdSuccess(data) {
        var trainingRcd = JSON.parse( data.d );
        qp = trainingRcd.QuizName.replace(/ /g, '-');
        
        //  Video    Quiz    Watched    Taken   State
        //   Y         Y       N          -     (3)  On,  VideoFirst
        //   Y         Y       Y          Y     (15) On,  VideoFirst
        //   Y         Y       Y          N     (7)  Off, On
        
        //   Y         N       N          -     (1)  On,  NoQuiz        
        //   Y         N       Y          N     (5)  On,  NoQuiz
        

        //   N         Y       -          N     (2)  NoVideo, On
        //   N         Y       -          Y     (10) NoVideo, On
        

        //   N         N      ----------------  (0)  Off, Off  (Error)
        var state = 0;
        
        ///////////////////////////////
        // No Quiz                   //
        // Hide Video  --  Show Quiz //
        ///////////////////////////////
        if (trainingRcd.VideoFile.length > 0) state += 1;
        if (trainingRcd.QuizName.length > 0) state += 2;
        if (trainingRcd.VideoComplete == true) state += 4;
        if (trainingRcd.QuizComplete > -1) state += 8;
        
        $('#videoPlayer').hide();
        $('#proprofs').hide();
        $('#NoQuiz').hide();
        $('#VidFirst').hide();
        $('#NoVideo').hide();
        
        qp = 'http://www.proprofs.com/quiz-school/story.php?title=' + qp + '&ew=430&user_id=' + eid + ':' + id + '&user_name=' + userName;
        vp = "/Videos/ehsTraining/" + trainingRcd.VideoFile;
        
        switch (state) {
            case 3:
                $('#videoPlayer').show();
                loadVideo(vp);
                $('#VidFirst').show();
                break;
                
            case 1:
                $('#videoPlayer').show();
                loadVideo(vp);
                $('#NoQuiz').hide();
                break;
                
            case 15:
                $('#videoPlayer').show();
                loadVideo(vp);
                $('#VidFirst').show();
                break;
                
            case 7:
                loadVideo(vp);
                $('#proprofs').show();
                $('#proprofs')[0].src = qp;
                break;
                
            case 5:
                $('#videoPlayer').show();
                loadVideo(vp);
                $('#NoQuiz').hide();
                break;
                
            case 2:
                $('#NoVideo').show();
                $('#proprofs').show();
                $('#proprofs')[0].src = qp;
                break;
                
            case 12:
                $('#NoVideo').show();
                $('#proprofs').show();
                $('#proprofs')[0].src = qp;
                break;
               
                
            default:
                break;
        }
    };



    //////////////////////
    // Load Flow Player //
    //////////////////////
    function loadVideo(videoFile) {
        
        $f("videoPlayer", "/Scripts/FlowPlayer/flowplayer-3.2.11.swf",
        {
            play:
                {
                    opacity: 0.0,
                    label: null,
                    replayLabel: 'This video is complete.  Start the quiz now.'
                },
            clip: {
                autoPlay: true,
                autoBuffering: false,
                scaling: 'fit',
                onError: function (errorCode, errorMessage) {
                    alert(errorCode + " " + errorMessage);
                },
                onFinish: function (clip) {
                    FloPostMovieFinish(clip);
                    $('#videoPlayer').hide();
                    $('#VidFirst').hide();
                    
                    if (qp.length > 0) {
                        $('#proprofs').show();
                        $('#proprofs')[0].src = qp;
                        $('#NoQuiz').hide();
                    }
                }
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
                    backgroundGradient: [0.5, 0, 0.3],
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
                    opacity: 1.0,
                    fastForward: true,
                    scrubber: true,
                    time: true
                }
            },
            playlist: [videoFile]
        });
    }
    




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




    //if (trainingRcd.VideoFile.length == 0) {
    //    $('#videoPlayer').hide();

    //    $('#NoQuiz').hide();
    //    $('#VidFirst').hide();
    //    qp = 'http://www.proprofs.com/quiz-school/story.php?title=' + qp + '&ew=430&user_id=' + eid + ':' + id + '&user_name=' + userName;
    //    $('#proprofs')[0].src = qp;
    //}

    ///////////////////////////////////////////////////////////
    //// If There Is A Video                                 //
    //// They Must Complete The Video Before Taking The Quiz //
    ///////////////////////////////////////////////////////////
    //else {

    //    ////////////////////////////////////
    //    // Video Complete Or Badge In Rcd //
    //    // Hide Video -- Show Quiz        //
    //    ////////////////////////////////////
    //    if (trainingRcd.VideoComplete == true) {
    //        $('#videoPlayer').hide();

    //        $('#NoQuiz').hide();
    //        $('#VidFirst').hide();
    //        qp = 'http://www.proprofs.com/quiz-school/story.php?title=' + qp + '&ew=430&user_id=' + eid + ':' + id + '&user_name=' + userName;
    //        $('#proprofs')[0].src = qp;
    //        $('#proprofs').show();
    //    }

    //    //////////////////////////////////////////////////
    //    // There Is A Video And It Has Not Been Watched //
    //    // Show Video -- Hide Quiz                      //
    //    //////////////////////////////////////////////////
    //    else {
    //        $('#videoPlayer').show();
    //        $('#proprofs').hide();
    //        $('#NoQuiz').hide();
    //        $('#VidFirst').hide();

    //        if (trainingRcd.QuizName.length > 0)
    //            $('#VidFirst').show('slow');
    //        else
    //            $('#NoQuiz').show('slow');

    //    }
    //}


    //if ( trainingRcd.QuizComplete == true )
    //    $('#proprofs').hide();
});
