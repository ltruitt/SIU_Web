$(document).ready(function () {
    



    var isAdmin = $("#Sx")[0].innerHTML;
    if (isAdmin.length > 0) {
        $('#AdminIconA').prop('href', isAdmin);
        $('#AdminFooterLi').show();
    }
    



    ////////////////////////////
    // Initialize The Sliders //
    ////////////////////////////
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

    $('#BlogSlider').anythingSlider({
        hashTags: false,
        theme: "HomeBlogSliderTheme",
        delay: 6000,
        mode: "vertical",
        autoPlay: true,
        buildArrows: false,
        buildNavigation: false,
        buildStartStop: false
    });


    $('#BlogSlider').show();



    $(window).resize(
        function () {
            var NoSlides = 3;


            if ($(window).width() < 1250) {
                NoSlides = 2;
            }

            if ($(window).width() < 930) {
                NoSlides = 1;
            }

            try {
                $('#slider').data('AnythingSlider').options.showMultiple = NoSlides;    
            } catch(e) {

            } 
            
        }
    );

});