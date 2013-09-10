////////////////////////////
// Initialize The Sliders //
////////////////////////////
$(document).ready(function () {

    var noSlides = 3;


    if ($(window).width() < 1250) {
        noSlides = 2;
    }

    if ($(window).width() < 930) {
        noSlides = 1;
    }

    $('#slider').anythingSlider({
        hashTags: false,
        showMultiple: noSlides,
        changeBy: 1,
        autoPlay: true,
        buildArrows: true,
        buildNavigation: false,
        buildStartStop: false
    });

    $(window).resize(
        function () {
            var noSlides = 3;


            if ($(window).width() < 1250) {
                noSlides = 2;
            }

            if ($(window).width() < 930) {
                noSlides = 1;
            }

            try {
                $('#slider').data('AnythingSlider').options.showMultiple = NoSlides;    
            } catch(e) {

            } 
            
        }
    );

});