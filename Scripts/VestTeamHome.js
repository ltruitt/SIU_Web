$(document).ready(function () {

    //////////////////////
    // Get Blog Content //
    //////////////////////
    function getBlogContent() {
        var getWeekSumHoursAjax = new AsyncServerMethod();
        getWeekSumHoursAjax.add('BlogName', $('#TeamPageBlog')[0].title);
        getWeekSumHoursAjax.exec("/SIU_DAO.asmx/GetMarquee", getAdvertisedBlogSuccess);
    }

    function getAdvertisedBlogSuccess(data) {
        var content = data.d.replace(/SliderText/g, 'None');
        $('#TeamPageBlog')[0].innerHTML = content;
    }


    //////////////////////
    // Get Blog Content //
    //////////////////////
    function getMarqueeContent() {
        var getWeekSumHoursAjax = new AsyncServerMethod();
        getWeekSumHoursAjax.add('BlogName', $('#TeamPageMarquee')[0].title);
        getWeekSumHoursAjax.exec("/SIU_DAO.asmx/GetMarquee", getMarqueeContentSuccess);
    }

    function getMarqueeContentSuccess(data) {
        var content = data.d.replace(/div/g, 'span');
        content = content.replace(/<li>/g, '');
        content = content.replace(/<\/li>/g, '<img alt="Scrolling Container Example 1" src="/Images/Si-EHS-VEST.png">');
        content = content.replace(/<p>/g, '').replace(/<\/p>/g, '');
        content =content.replace(/ class="SliderText"/g, '');
        $('#TeamPageMarquee')[0].innerHTML = content;
        $('#TeamPageMarquee')[0].innerHTML = content;
    }

    getBlogContent();
    getMarqueeContent();



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


    function constructionWarning() {
        //Get The Popup Window Container
        var warnBox = $('#popup-box');

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

    //ConstructionWarning();

});