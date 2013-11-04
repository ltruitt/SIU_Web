// Initialize The Sliders
$(document).ready(function () {


    $("a").click(function () {
        var aHash = this.hash.split(':');
        var SyncCall = new AsyncServerMethod();
        SyncCall.add('System', aHash[0]);
        SyncCall.add('Dept', aHash[1]);
        SyncCall.add('Dir', aHash[2]);
        SyncCall.exec("/SIU_DAO.asmx/DirSync");
    });

});