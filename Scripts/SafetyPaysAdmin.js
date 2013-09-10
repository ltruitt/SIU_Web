$(document).ready(function () {
    $('.RejectScript').click(function () {

        //Get The Popup Window Container
        var loginBox = $(this).attr('href');
        loginBox = $('#popup-box');

        // Fade in the Popup
        $(loginBox).fadeIn(300);

        //Set the center alignment padding + border see css style
        var popMargTop = ($(loginBox).height() + 24) / 2;
        var popMargLeft = ($(loginBox).width() + 24) / 2;

        $(loginBox).css({
            'margin-top': -popMargTop,
            'margin-left': -popMargLeft
        });

        // Add the mask to body
        $('body').append('<div id="mask"></div>');
        $('#mask').fadeIn(300);

        return false;
    });

    // When clicking on the button close or the mask layer the popup closed
    $(document).on('click', 'a.close, #mask', function () {
        $('#mask , .popup').fadeOut(300, function () {
            $('#mask').remove();
        });
        return false;
    });
});