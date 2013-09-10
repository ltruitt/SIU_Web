$(document).ready(function () {

    //////////////////////
    // Get Blog Content //
    //////////////////////
    function GetBlogContent() {
        var GetWeekSumHours_ajax = new AsyncServerMethod();
        GetWeekSumHours_ajax.add('BlogName', $('#TeamPageBlog')[0].title);
        GetWeekSumHours_ajax.exec("/SIU_DAO.asmx/GetMarquee", GetAdvertisedBlog_success);
    }

    function GetAdvertisedBlog_success(data) {
        var Content = data.d.replace(/SliderText/g, 'None');
        $('#TeamPageBlog')[0].innerHTML = Content;
    }


    //////////////////////
    // Get Blog Content //
    //////////////////////
    function GetMarqueeContent() {
        var GetWeekSumHours_ajax = new AsyncServerMethod();
        GetWeekSumHours_ajax.add('BlogName', $('#TeamPageMarquee')[0].title);
        GetWeekSumHours_ajax.exec("/SIU_DAO.asmx/GetMarquee", GetMarqueeContent_success);
    }

    function GetMarqueeContent_success(data) {
        var Content = data.d.replace(/div/g, 'span');
        Content = Content.replace(/<li>/g, '');
        Content = Content.replace(/<\/li>/g, '<img alt="Scrolling Container Example 1" src="/Images/Si-EHS-VEST.png">');
        Content = Content.replace(/<p>/g, '').replace(/<\/p>/g, '');
        Content = Content.replace(/ class="SliderText"/g, '');
        var x1 = $('#TeamPageMarquee')[0].innerHTML;
        $('#TeamPageMarquee')[0].innerHTML = Content;

        var x2 = $('#TeamPageMarquee')[0].innerHTML;
        $('#TeamPageMarquee')[0].innerHTML = Content;
    }

    GetBlogContent();
    GetMarqueeContent();



    $('.horizontal_scroller').SetScroller({
        velocity: 89,
        direction: 'horizontal',
        startfrom: 'right',
        loop: 'infinite',
        movetype: 'linear',
        onmouseover: 'play',
        onmouseout: 'play',
        onstartup: 'play',
        cursor: 'default'
    });
    


    // When clicking on the button close or the mask layer the popup closed
    $(document).on('click', 'a.close, #btnOK', function () {
        $('#mask , .popup').fadeOut(300, function () {
            $('#mask').remove();
        });
        return false;
    });
    

    function ConstructionWarning() {
        //Get The Popup Window Container
        warnBox = $('#popup-box');

        // Fade in the Popup
        $(warnBox).fadeIn(300);

        //Set the center alignment padding + border see css style
        var popMargTop = ($(warnBox).height() + 24) / 2;
        var popMargLeft = ($(warnBox).width() + 24) / 2;

        $(warnBox).css({
            'margin-top': -popMargTop,
            'margin-left': -popMargLeft
        });

        // Add the mask to body
        $('body').append('<div id="mask"></div>');
        $('#mask').fadeIn(300);
    }
    
    ConstructionWarning();

});