$(document).ready(function () {
    $('#Down').click(function () {
        var ctl = $('#<%=txtNumMgrs.ClientID%>');
        var str = Number(ctl.val());
        if (str > 1) {
            str = str - 1;
        }
        if (str == 'NaN') {
            str = 1;
        }
        ctl.val(str);
    });

    $('#Up').click(function () {
        var ctl = $('#<%=txtNumMgrs.ClientID%>');
        var str = Number(ctl.val());
        if (str < 5) {
            str = str + 1;
        }
        if (str == 'NaN') {
            str = 1;
        }
        ctl.val(str);
    });
})