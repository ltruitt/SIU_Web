$(document).delegate('.ui-navbar ul li > a', 'click', function () {
    //un-highlight and highlight only the buttons in the same navbar widget
    $(this).closest('.ui-navbar').find('a').removeClass('ui-btn-active');
    
    //this starts the same but then only selects the sibling `.content_div` elements to hide rather than all in the DOM
    $('#' + $(this).attr('data-href')).show().siblings('.ui-content').hide();
    
    $(this).addClass('ui-btn-active');
});
