$(document).ready(function () {
    

    //localhost/ProProfs/submit.aspx?quiz_id=1&quiz_name=test
    

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

        this.exec = function (url, successMethod, failMethod) {
            if (url.length > 0)
                this.url = url;

            if (successMethod == undefined) {
                successMethod = '';
            } else
                this.successMethod = successMethod;

            if (failMethod == undefined) {
                failMethod = '';
            } else
                this.failMethod = failMethod;

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


    var quizId = "";
    var quizName = "";
    var attemptDate = "";
    var userName = "";
    var totalMarks = "";
    var userObtainedMarks = "";
    var userPercentMarks = "";
    var userTotalcorrectAnswers = "";
    var userTotalwrongAnswers = "";
    var userEmail = "";
    var userAddress = "";
    var userCity = "";
    var userState = "";
    var userZipcode = "";
    var userPhone = "";
    var userId = "";

    quizId = getURLParameter('quiz_id');
    quizName = getURLParameter('quiz_name');
    attemptDate = getURLParameter('attempt_date');
    userName = getURLParameter('user_name');
    totalMarks = getURLParameter('total_marks');
    userObtainedMarks = getURLParameter('user_obtained_marks');
    userPercentMarks = getURLParameter('user_percent_marks');
    userTotalcorrectAnswers = getURLParameter('user_totalcorrect_answers');
    userTotalwrongAnswers = getURLParameter('user_totalwrong_answers');
    userEmail = getURLParameter('user_Email');
    userAddress = getURLParameter('user_Address');
    userCity = getURLParameter('user_City');
    userState = getURLParameter('user_State');
    userZipcode = getURLParameter('user_Zipcode');
    userPhone = getURLParameter('user_Phone');
    userId = getURLParameter('user_Id');


    var ajaxCall = new AsyncServerMethod();

    ajaxCall.add('quiz_id', quizId);
    ajaxCall.add('quiz_name', quizName);
    ajaxCall.add('attempt_date', attemptDate);
    ajaxCall.add('user_name', userName);
    ajaxCall.add('total_marks', totalMarks);
    ajaxCall.add('user_obtained_marks', userObtainedMarks);
    ajaxCall.add('user_percent_marks', userPercentMarks);
    ajaxCall.add('user_totalcorrect_answers', userTotalcorrectAnswers);
    ajaxCall.add('user_totalwrong_answers', userTotalwrongAnswers);
    ajaxCall.add('user_Id', userId);
    ajaxCall.add('user_Email', userEmail);
    ajaxCall.add('user_Address', userAddress);
    ajaxCall.add('user_City', userCity);
    ajaxCall.add('user_State', userState);
    ajaxCall.add('user_Zipcode', userZipcode);
    ajaxCall.add('user_Phone', userPhone);

    ajaxCall.exec("/SIU_DAO.asmx/RecordTest", recordProProfsSuccess, recordProProfsFail);

    function recordProProfsSuccess() {
        $('#Status')[0].innerHTML = 'Done';
    }

    function recordProProfsFail() {
        $('#Status')[0].innerHTML = 'Fail';
    }

});