$(document).ready(function () {

    getURLParameter = function (name) {
        return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null;
    };

    function AsyncServerMethod() {
        this.ParamArray = Array();
        this.url = null;
        this.successMethod = null;
        this.failMethod = null;
        this.ParamStr = null;

        this.add = function (paramName, paramValue) {
            this.ParamArray[this.ParamArray.length] = { n: paramName, v: paramValue };
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
                        this.failMethod();
                        return;
                    }
                }

                if (this.successMethod != null)
                    this.successMethod(data);
            });

            xmlRequest.fail(function (xhr, textStatus) {
                if (this.failMethod != null)
                    this.failMethod();
                else {
                    alert(this.url + '  ' + this.ParamStr + '  ' + textStatus);
                }

            });
        };
    }


    var quiz_id = "";
    var quiz_name = "";
    var attempt_date = "";
    var user_name = "";
    var total_marks = "";
    var user_obtained_marks = "";
    var user_percent_marks = "";
    var user_totalcorrect_answers = "";
    var user_totalwrong_answers = "";
    var user_Email = "";
    var user_Address = "";
    var user_City = "";
    var user_State = "";
    var user_Zipcode = "";
    var user_Phone = "";
    var user_Id = "";

    quiz_id = getURLParameter('quiz_id');
    quiz_name = getURLParameter('quiz_name');
    attempt_date = getURLParameter('attempt_date');
    user_name = getURLParameter('user_name');
    total_marks = getURLParameter('total_marks');
    user_obtained_marks = getURLParameter('user_obtained_marks');
    user_percent_marks = getURLParameter('user_percent_marks');
    user_totalcorrect_answers = getURLParameter('user_totalcorrect_answers');
    user_totalwrong_answers = getURLParameter('user_totalwrong_answers');
    user_Email = getURLParameter('user_Email');
    user_Address = getURLParameter('user_Address');
    user_City = getURLParameter('user_City');
    user_State = getURLParameter('user_State');
    user_Zipcode = getURLParameter('user_Zipcode');
    user_Phone = getURLParameter('user_Phone');
    user_Id = getURLParameter('user_Id');


    var AjaxCall = new AsyncServerMethod();

    AjaxCall.add('quiz_id', quiz_id);
    AjaxCall.add('quiz_name', quiz_name);
    AjaxCall.add('attempt_date', attempt_date);
    AjaxCall.add('user_name', user_name);
    AjaxCall.add('total_marks', total_marks);
    AjaxCall.add('user_obtained_marks', user_obtained_marks);
    AjaxCall.add('user_percent_marks', user_percent_marks);
    AjaxCall.add('user_totalcorrect_answers', user_totalcorrect_answers);
    AjaxCall.add('user_totalwrong_answers', user_totalwrong_answers);
    AjaxCall.add('user_Id', user_Id);
    AjaxCall.add('user_Email', user_Email);
    AjaxCall.add('user_Address', user_Address);
    AjaxCall.add('user_City', user_City);
    AjaxCall.add('user_State', user_State);
    AjaxCall.add('user_Zipcode', user_Zipcode);
    AjaxCall.add('user_Phone', user_Phone);

    AjaxCall.exec("/ProProfs/ProProfs.asmx/RecordTest", RecordProProfs_success, RecordProProfs_fail);

    function RecordProProfs_success() {
        $('#Status')[0].innerHTML = 'Done';
    }

    function RecordProProfs_fail() {
        $('#Status')[0].innerHTML = 'Fail';
    }

});