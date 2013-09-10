$(document).ready(function () {

    $(document).on('click', '#MonDown', function () {
        var ctl = $('#txtMonitorCnt');
        var str = Number(ctl.val());
        if (str > 0) {
            str = str - 1;
        }
        if (str == 'NaN') {
            str = 0;
        }
        ctl.val(str);
        ctl.trigger('change');
    });
    
    $(document).on('click', '#MonUp', function () {
        var ctl = $('#txtMonitorCnt');
        var str = Number(ctl.val());
        if (str < 4) {
            str = str + 1;
        }
        if (str == 'NaN') {
            str = 0;
        }
        ctl.val(str);
        ctl.trigger('change');
    });

    $(document).on('click', '#StdDown', function () {
        var ctl = $('#txtStandCnt');
        var str = Number(ctl.val());
        if (str > 0) {
            str = str - 1;
        }
        if (str == 'NaN') {
            str = 0;
        }
        ctl.val(str);
        ctl.trigger('change');
    });

    $(document).on('click', '#StdUp', function () {
        var ctl = $('#txtStandCnt');
        var str = Number(ctl.val());
        if (str < 4) {
            str = str + 1;
        }
        if (str == 'NaN') {
            str = 0;
        }
        ctl.val(str);
        ctl.trigger('change');
    });
    
});