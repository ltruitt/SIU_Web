// Crude Device Type Matching
// Not Setup as JQuery Function So It Can Execute Before Document Load
var isMobile = {
    Android: function () {
        return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function () {
        return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function () {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    iPad: function () {
        return navigator.userAgent.match(/iPad/i);
    },
    Opera: function () {
        return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function () {
        return navigator.userAgent.match(/IEMobile/i);
    },
    any: function () {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
    }
};








$(document).ready(function () {

    Array.prototype.containsCaseInsensitive = function (obj) {
        var i = this.length;
        while (i--) {

            if (this[i].toUpperCase().replace(/\r?\n|\r/g, "") === obj.toUpperCase()) {
                return true;
            }
        }
        return false;
    };

    jQuery.fn.getURLParameter = function(name) {
        return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace( /\+/g , '%20')) || null;
    };


    ////////////////////////////////////////////////
    // Methods to track changes to data on a form //
    ////////////////////////////////////////////////
    jQuery.fn.extend({
        trackChanges: function () {
            $(":input", this).change(function () {
                $(this.form).data("changed", true);
            });
        }
     ,
        isChanged: function () {
            return this.data("changed");
        }
    });


    //jQuery.fn.parseJsonDate = function (jsonDateString, format) {
    //    if (typeof (jsonDateString) == "undefined")
    //        return '';

    //    var xxx = jsonDateString.replace('/Date(', '').replace(')/', '');
    //    var yyy = new Date(parseInt(xxx));
    //    return $.datepicker.formatDate(format, yyy);
    //};
    
    jQuery.fn.parseJsonDate = function (jsonDateString) {
        if (typeof (jsonDateString) == "undefined")
            return '';

        if ( !jsonDateString )
            return '';

        if (jsonDateString == 'null' )
            return '';

        //if (jsonDateString != null && jsonDateString !== undefined) {
        //    return '';
        //}

        if (jsonDateString.indexOf('/Date') == -1) {
            return new Date(jsonDateString).toDateString();
        }

        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    };
    

    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // I THINK THIS IS THE CORRECT METHOD TO HET A JSOM DATE AND LOAD IT INTO A DATEPICKER OBJECT                                // 
    // $('#SafetyMeetingDate').datepicker("setDate", $.datepicker.parseDate('mm-dd-yy', $.fn.dateTest(data.SafetyMeetingDate))); //
    //                                                                                                                           //
    // WHEREAS THIS WOULD WORK FOR DIV / SPAN                                                                                    //
    // $('#SafetyMeetingDate').html($.datepicker.parseDate('mm-dd-yy', $.fn.dateTest(data.SafetyMeetingDate)));                  //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    jQuery.fn.dateTest = function (datefield) {
        var i = -1;
        if (datefield != null)
            i = parseInt(datefield.substr(6));

        if (i > 0)
            return $.datepicker.formatDate('mm-dd-yy', new Date(i));
        else
            return '';
    };

    ////////////////////////////////////
    // Define A Default Text Function //
    ////////////////////////////////////
    jQuery.fn.DefaultText = function () {
        var DataFld = $(this[0]);

        $(document).on('focus', DataFld, function () {
            if (DataFld.val() == this.title) {
                DataFld.removeClass("defaultText");
                DataFld.val("");
            }
        });

        $(document).on('blur', DataFld, function () {
            if (DataFld.val() == "") {
                DataFld.addClass("defaultText");
                DataFld.val(this.title);
            }
        });
        DataFld.blur();
    };


    ////////////////////////////
    // File Folder Icon Hover //
    ////////////////////////////
    //    $('*[id*=mbi_mb73g0_]').hover(function () {
    //        var xxx = $(this).attr("src");
    //        
    //        if ($(this).attr("src") == "/Images/LibraryFolder.png")
    //            $(this).attr("src", "/Images/LibraryFolder-hover.png");
    //    }, function () {
    //        if ($(this).attr("src") == "/Images/LibraryFolder-hover.png")
    //            $(this).attr("src", "/Images/LibraryFolder.png");
    //    });


    //    $('.VideoMenuImg').hover(function () {
    //        $(this).attr("src", "/Images/LibraryFolder-hover.png");
    //    }, function () {
    //        $(this).attr("src", "/Images/LibraryFolder.png");
    //    });
});





// Define Standard Ajax Call Method.  Built to be able to globally change all AJAX calls due to Safari Bug
// http: //www.isurinder.com/blog/post/2012/09/24/iOS-Safari-Browser-Has-A-Massive-Caching-Issue!.aspx#.USBY-Credyw
//jQuery.AsyncServerMethod = function () {
function AsyncServerMethod() {
    this.ParamArray = Array();
    this.url = null;
    this.successMethod = null;
    this.failMethod = null;
    this.ParamStr = null;

    this.add = function(paramName, paramValue) {
        this.ParamArray[this.ParamArray.length] = { n: paramName, v: paramValue };
        if (paramName == 'StartDate' && paramValue.length > 10)
            alert('Request To Add Bad Param Trapped: ' + this.ParamArray[pCnt].v + ' : ' + this.ParamArray[pCnt].v);
    };

    this.exec = function (_url, _successMethod, _failMethod) {
        if (_url.length > 0)
            this.url = _url;

        if (_successMethod == undefined)
            _successMethod = '';
        else
            this.successMethod = _successMethod;

        if (_failMethod == undefined)
            _failMethod = '';
        else
            this.failMethod = _failMethod;

        var timestamp = new Date();
        this.ParamStr = '{';
        for (var pCnt = 0; pCnt < this.ParamArray.length; pCnt++) {
            if (this.ParamArray[pCnt].n == 'StartDate' && this.ParamArray[pCnt].v.length > 10)
                alert('Ready To Send With Trapped Bad Param: ' + this.ParamArray[pCnt].v + ' : ' + this.ParamArray[pCnt].v);
            this.ParamStr += '"' + this.ParamArray[pCnt].n + '":';
            this.ParamStr += '"' + this.ParamArray[pCnt].v + '",';
        }
        this.ParamStr += '"T":"' + timestamp.getTime() + '"}';

        var xmlRequest = $.ajax({
            type: "POST",
            url: this.url,
            cache: false,
            context: this,
            contentType: "application/json",
            dataType: "json",
            data: this.ParamStr
        });

        xmlRequest.done(function (data, textStatus, xhr) {
            if (data.d == null) {
                if (this.failmethod != null) {
                    this.failMethod(xhr);
                    return;
                }
            }

            if (this.successMethod != null)
                this.successMethod(data);
        });

        xmlRequest.fail(function (xhr, textStatus) {
            if (this.failMethod != null)
                this.failMethod(xhr);
            else {
                alert(xhr.responseText + '\r\r'  +   this.url + '\r\r' + this.ParamStr);
            }

        });
    };
}


function isAlpha(t) {
    var output = t
    .replace(/[\r]/g, '')
    .replace(/[\b]/g, '')
    .replace(/[\f]/g, '')
    .replace(/[\n]/g, '')
    .replace(/\\/g, '');
    
    if (output.length > 0) return true;
    return false;
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function number_format(number, decimals, dec_point, thousands_sep) {
    // *     example 1: number_format(1234.56);
    // *     returns 1: '1,235'
    // *     example 2: number_format(1234.56, 2, ',', ' ');
    // *     returns 2: '1 234,56'
    // *     example 3: number_format(1234.5678, 2, '.', '');
    // *     returns 3: '1234.57'
    // *     example 4: number_format(67, 2, ',', '.');
    // *     returns 4: '67,00'
    // *     example 5: number_format(1000);
    // *     returns 5: '1,000'
    // *     example 6: number_format(67.311, 2);
    // *     returns 6: '67.31'
    // *     example 7: number_format(1000.55, 1);
    // *     returns 7: '1,000.6'
    // *     example 8: number_format(67000, 5, ',', '.');
    // *     returns 8: '67.000,00000'
    // *     example 9: number_format(0.9, 0);
    // *     returns 9: '1'
    // *    example 10: number_format('1.20', 2);
    // *    returns 10: '1.20'
    // *    example 11: number_format('1.20', 4);
    // *    returns 11: '1.2000'
    // *    example 12: number_format('1.2000', 3);
    // *    returns 12: '1.200'
    // *    example 13: number_format('1 000,50', 2, '.', ' ');
    // *    returns 13: '100 050.00'
    // Strip all characters but numerical ones.
    number = (number + '').replace(/[^0-9+\-Ee.]/g, '');
    var n = !isFinite(+number) ? 0 : +number,
      prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
      sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
      dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
      s = '',
      toFixedFix = function (n, prec) {
          var k = Math.pow(10, prec);
          return '' + Math.round(n * k) / k;
      };
    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
    if (s[0].length > 3) {
        s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }
    if ((s[1] || '').length < prec) {
        s[1] = s[1] || '';
        s[1] += new Array(prec - s[1].length + 1).join('0');
    }
    return s.join(dec);
}


function NumericInput(selector) {
    $('#' + selector).keydown(function (event) {
        
        ////////////////////////////////////////////////////////
        // Allow: backspace, delete, tab, escape, enter and . //
        ////////////////////////////////////////////////////////
        if ($.inArray(event.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 ||
            
            ///////////////////
            // Allow: Ctrl+A //
            ///////////////////
            (event.keyCode == 65 && event.ctrlKey === true) ||
            
            ///////////////////////////////////
            // Allow: home, end, left, right //
            ///////////////////////////////////
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            
            //////////////////////////////////////
            // let it happen, don't do anything //
            //////////////////////////////////////
            return;
        }
        else {
            
            //////////////////////////////////////////////////////
            // Ensure that it is a number and stop the keypress //
            //////////////////////////////////////////////////////
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });
}

function mutex(className) {
        
    ///////////////////////////////////
    // Catch CheckBox Change Events  //
    // Test For At Least One Checked //
    // Check For MutEx               //
    ///////////////////////////////////
    $('input:checkbox[class=' + className + ']').change(function () {

        ///////////////////////////////////////
        // Make Sure At Least On Box Checked // 
        ///////////////////////////////////////
        if ($('input:checkbox[class=' + className + ']:checked').length == 0) {
            $(this).prop('checked', true);
            return;
        }

        ///////////////////////////////////////////////////
        // Ignore Auto Uncheck Events (MutEx Code Below) //
        ///////////////////////////////////////////////////
        if (!(this.checked)) {
            return;
        }

        //////////////////////////
        // Which Button Changed //
        //////////////////////////
        var chkboxName = this.id;

        /////////////////////////////////////
        // Make Buttons Mutually Exclusive //
        /////////////////////////////////////
        $('input:checkbox[class=' + className + ']:checked').each(function () {
            if (this.id != chkboxName) {
                $(this).prop('checked', false);
            };
        });

    });
}