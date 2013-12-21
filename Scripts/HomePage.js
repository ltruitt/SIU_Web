////////////////////////////////////
// Before Document is Rendered    //
// Check For Device Type          //
// And Redirect If A Phone Device //
////////////////////////////////////
if (!window.location.href.match(/force/i)) {
    if (isMobile.any()) {
        if (!isMobile.iPad()) {
            window.location = window.location.protocol + "//" + window.location.hostname + "/" + "phone/homepage.aspx";
        }
    }
}



////////////////////////////
// Initialize The Sliders //
////////////////////////////
$(document).ready(function () {

    //$('#NoJS').hide();
    $('#HomeWrapper').show('slow');
    
    var NoSlides = 3;


    if ($(window).width() < 1200) {
        NoSlides = 2;
    }

    if ($(window).width() < 930) {
        NoSlides = 1;
    }
    
    if ($(window).width() > 1450) {
        NoSlides = 4;
    }

    $('#slider').anythingSlider({
        hashTags: false,
        showMultiple: NoSlides,
        changeBy: 1,
        autoPlay: true,
        buildArrows: true,
        buildNavigation: false,
        buildStartStop: false
    });

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


    //$('#BlogSlider').show();



    $(window).resize(
        function () {
            NoSlides = 3;


            if ($(window).width() < 1200) {
                NoSlides = 2;
            }

            if ($(window).width() < 930) {
                NoSlides = 1;
            }

            if ($(window).width() > 1450) {
                NoSlides = 4;
            }

            try {
                $('#slider').data('AnythingSlider').options.showMultiple = NoSlides;    
            } catch(e) {

            } 
            
        }
    );



}); 