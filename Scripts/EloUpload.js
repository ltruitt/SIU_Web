// common variables
var iBytesUploaded = 0;
var iBytesTotal = 0;
var iPreviousBytesLoaded = 0;
var iMaxFilesize = 1048576; // 1MB
var oTimer = 0;
var sResultFileSize = '';



function secondsToTime(secs) { // we will use this function to convert seconds in normal time format
    var hr = Math.floor(secs / 3600);
    var min = Math.floor((secs - (hr * 3600)) / 60);
    var sec = Math.floor(secs - (hr * 3600) - (min * 60));

    if (hr < 10) { hr = "0" + hr; }
    if (min < 10) { min = "0" + min; }
    if (sec < 10) { sec = "0" + sec; }
    if (hr) { hr = "00"; }
    return hr + ':' + min + ':' + sec;
};

function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB'];
    if (bytes == 0) return 'n/a';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return (bytes / Math.pow(1024, i)).toFixed(1) + ' ' + sizes[i];
};





function fileSelected() {
    /////////////////////////////
    // hide different warnings //
    /////////////////////////////
    document.getElementById('upload_response').style.display = 'none';
    document.getElementById('error').style.display = 'none';
    document.getElementById('error2').style.display = 'none';
    document.getElementById('abort').style.display = 'none';
    document.getElementById('warnsize').style.display = 'none';
    document.getElementById('txtFile').style.display = 'none';
    
   
    startUploading();
}





function startUploading() {

    // cleanup all temp states
    iPreviousBytesLoaded = 0;
    document.getElementById('upload_response').style.display = 'none';
    document.getElementById('error').style.display = 'none';
    document.getElementById('error2').style.display = 'none';
    document.getElementById('abort').style.display = 'none';
    document.getElementById('warnsize').style.display = 'none';
    document.getElementById('progress_percent').innerHTML = '';
    var oProgress = document.getElementById('progress');
    oProgress.style.display = 'block';
    oProgress.style.width = '0px';

    // get form data for POSTing
    var vFd = new FormData(document.getElementById('SiteWrapper'));
    var vURL = '/upload/upload.php?SUB=ELO';

    // create XMLHttpRequest object, adding few event listeners, and POSTing our data
    var oXhr = new XMLHttpRequest();
    oXhr.upload.addEventListener('progress', uploadProgress, false);
    oXhr.addEventListener('load', uploadFinish, false);
    oXhr.addEventListener('error', uploadError, false);
    oXhr.addEventListener('abort', uploadAbort, false);
    oXhr.open('POST', vURL);
    oXhr.send(vFd);

    // set inner timer
    oTimer = setInterval(doInnerUpdates, 300);
}

function doInnerUpdates() { // we will use this function to display upload speed
    var iCb = iBytesUploaded;
    var iDiff = iCb - iPreviousBytesLoaded;

    // if nothing new loaded - exit
    if (iDiff == 0)
        return;

    iPreviousBytesLoaded = iCb;
    iDiff = iDiff * 2;
    var iBytesRem = iBytesTotal - iPreviousBytesLoaded;
    var secondsRemaining = iBytesRem / iDiff;

    // update speed info
    var iSpeed = iDiff.toString() + 'B/s';
    if (iDiff > 1024 * 1024) {
        iSpeed = (Math.round(iDiff * 100 / (1024 * 1024)) / 100).toString() + 'MB/s';
    } else if (iDiff > 1024) {
        iSpeed = (Math.round(iDiff * 100 / 1024) / 100).toString() + 'KB/s';
    }

    document.getElementById('speed').innerHTML = iSpeed;
    document.getElementById('remaining').innerHTML = '| ' + secondsToTime(secondsRemaining);
}

function uploadProgress(e) { // upload process in progress
    if (e.lengthComputable) {
        iBytesUploaded = e.loaded;
        iBytesTotal = e.total;
        var iPercentComplete = Math.round(e.loaded * 100 / e.total);
        var iBytesTransfered = bytesToSize(iBytesUploaded);

        document.getElementById('progress_percent').innerHTML = iPercentComplete.toString() + '%';
        document.getElementById('progress').style.width = (iPercentComplete * 4).toString() + 'px';
        document.getElementById('b_transfered').innerHTML = iBytesTransfered;
        if (iPercentComplete == 100) {
            var oUploadResponse = document.getElementById('upload_response');
            oUploadResponse.innerHTML = 'Please wait...processing';
            oUploadResponse.style.display = 'block';
        }
    } else {
        document.getElementById('progress').innerHTML = 'unable to compute';
    }
}

function uploadFinish(e) {
    
    clearInterval(oTimer);
    
    document.getElementById('progress').style.display = 'none';
    document.getElementById('progress_percent').style.display = 'none';
    document.getElementById('UploadHead').style.display = 'none';
    document.getElementById('ExpAmountDiv').style.display = 'block';
    
    var oUploadResponse = document.getElementById('upload_response');
    oUploadResponse.innerHTML = e.target.responseText;
    oUploadResponse.style.display = 'block';
    document.getElementById('remaining').innerHTML = '';

    document.getElementById('lblFile').style.display = 'block';
    
    document.getElementById('lblFile').innerHTML = '<b>Copy of Expense:</b>&nbsp;' + document.getElementById('txtFile').files[0].name;
    
}

function uploadError(e) { // upload error
    document.getElementById('lblFile').innerHTML = '';
    document.getElementById('error2').style.display = 'block';
    clearInterval(oTimer);
}

function uploadAbort(e) { // upload abort
    document.getElementById('lblFile').innerHTML = '';
    document.getElementById('abort').style.display = 'block';
    clearInterval(oTimer);
}