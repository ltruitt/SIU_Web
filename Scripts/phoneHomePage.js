$(document).ready(function () {

    $('#ehsMenu').hide();
    
    $('#mainMenuClickEhs').click(function () {
        $('#mainMenu').hide();
        $('#ehsMenu').show('slow');
    });
    
    $('#ehsMenuClickHome').click(function () {
        $('#ehsMenu').hide();
        $('#mainMenu').show('slow');
    });
});