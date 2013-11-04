$(document).ready(function () {



    var getQomCall = new AsyncServerMethod();
    getQomCall.add('_dept', D);
    getQomCall.exec("/SIU_DAO.asmx/GetSafetyQomQ", getQomSuccess);
    
    ///////////////////////////
    // Submit Button Handler //
    ///////////////////////////
    function getQomSuccess(data) {

    }
    

})