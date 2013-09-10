// Initialize The Sliders
$(document).ready(function () {

    $('#BlogSlider').anythingSlider({
        hashTags: false,
        theme: "HomeBlogSliderTheme",
        delay: 3000,
        mode: "vertical",
        autoPlay: true,
        buildArrows: false,
        buildNavigation: false,
        buildStartStop: false
    });

    //            function () {
    //                $("a.lnkPopup").click(function () { $("div.popupDiv").Init({ closeButtonCSS: "lnkClose" }); });
    //            }
});  