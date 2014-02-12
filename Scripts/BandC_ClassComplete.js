$(document).ready(function () {
    

    ////////////////////////////////////////
    // Event Management For Job Selection //
    ////////////////////////////////////////
    var listOfCerts = [];
    function getCertsSuccess(data) {
        listOfCerts = data.d.split("\r");

        $("#acCertList").autocomplete(
            { source: listOfCerts },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: false,
                cacheLength: 20,
                max: 20,
                select: function (event, ui) {
                    var dataPieces = ui.item.value.split(' ');
                    $(this).val(dataPieces[0].replace(/\n/g, ""));
                    getCertDetails();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $(this).val(dataPieces[0].replace(/\n/g, ""));
                        getCertDetails();
                        $("#acCertList").autocomplete("close");
                    }

                    return ui;
                }
            });
    }
    function getCerts() {
        var getCertsAjax = new AsyncServerMethod();
        getCertsAjax.exec("/SIU_DAO.asmx/GetCerts", getCertsSuccess);
    }




    //////////////////////////////////////////////////////
    // Make Sure Text Entered For Cert Was In Jobs List //
    //////////////////////////////////////////////////////
    //    $("#acCertList").blur(function () {
    //        if (!listOfCerts.containsCaseInsensitive(this.value)) {
    //            $(this).val("");
    //        }
    //    });




    /////////////////////////////////////////
    // Get Details Of A Job And Job Report //
    /////////////////////////////////////////
    function getCertSuccess(data) {

        var certDetails = $.parseJSON(data.d);

        $('#hlblCertCode')[0].innerHTML = certDetails[0].Code;

        timestamp = new Date();
        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, ClassCode: $('#hlblCertCode')[0].innerHTML, ClassDate: $('#txtClassDate')[0].value, T: timestamp.getTime() });
    }

    function getCertDetails() {
        var cert = $('#acCertList').val();

        var getCertAjax = new AsyncServerMethod();
        getCertAjax.add('Cert', cert);
        getCertAjax.exec("/SIU_DAO.asmx/GetCert", getCertSuccess);
    }


    $('#txtClassDate').datepicker({
        constrainInput: true,
        onSelect: showHoursForDate
    });

    $('#txtClassDate').blur(function () {
        showHoursForDate();
    });

    function showHoursForDate() {

    }






    ///////////////////////////////////////////////////////////////
    // Load List Of Employees So Supr Can Change Viewed Employee //
    ///////////////////////////////////////////////////////////////
    var listOfEmps = [];
    function getEmpsSuccess(data) {
        listOfEmps = data.d.split("\r");
        $("#txtStudent").autocomplete({ source: listOfEmps },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: false,
                cacheLength: 20,
                max: 20,
                delay: 0,
                select: function (event, ui) {
                    var dataPieces = ui.item.value.split(' ');
                    $("#txtStudent").autocomplete("close");
                    $("#txtStudent").val('');
                    recordClass(dataPieces[0]);
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $("#txtStudent").autocomplete("close");
                        $("#txtStudent").val('');
                        recordClass(dataPieces[0]);
                    }

                    return ui;
                }
            });
    }
    
    function recordClass(studentId) {
        var recordClassCall = new AsyncServerMethod();
        recordClassCall.add('QualCode', $('#hlblCertCode')[0].innerHTML );
        recordClassCall.add('ClassDate', $('#txtClassDate')[0].value);
        recordClassCall.add('InstID', $('#hlblEID')[0].innerHTML);
        recordClassCall.add('StudentID', studentId);
        recordClassCall.exec("/SIU_DAO.asmx/RecordUnPostedClassStudent", recordUnPostedClassStudentSuccess);
        
    }
    
    function recordUnPostedClassStudentSuccess(data) {
        //var recordedStudent = $.parseJSON(data.d);
        //$('#jTableContainer').jtable('addRecord', {
        //    record: {
        //        TLC_UID: 0,
        //        TL_UID: tlUid,
        //        QualCode: $("#acCertList").val()
        //    },
        //    clientOnly: true
        //});
        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, ClassCode: $('#hlblCertCode')[0].innerHTML, ClassDate: $('#txtClassDate')[0].value, T: timestamp.getTime() });
    }



    ///////////////////////////////////////
    // Setup Report Of Unposted Expenses //
    ///////////////////////////////////////
    $('#jTableContainer').jtable({
        title: 'All Unposted Students For Instructor',
        defaultSorting: 'UID ASC',
        edit: true,
        actions: {
            listAction: '/SIU_DAO.asmx/mdd556asa',
            deleteAction: '/SIU_DAO.asmx/g61aaqaaq61g'
        },
        fields: {
            UID: {
                key: true,
                create: false,
                edit: false,
                list: false
            },
            class_date: {
                title: 'Date',
                width: '3%',
                type: 'date',
                displayFormat: 'mm-dd',
                listClass: 'jTableTD'
            },
            qual_code: {
                title: 'Code',
                width: '2%',
                listClass: 'jTableTD'
            },
            qual_name: {
                title: 'Description',
                width: '8%',
                listClass: 'jTableTD'
            },
            emp_no: {
                title: 'Student ID',
                width: '3%',
                listClass: 'jTableTD'
            },
            student_name: {
                title: 'Student Name',
                width: '8%',
                listClass: 'jTableTD'
            },
            class_instructor: {
                title: 'Instructor ID',
                width: '1%',
                listClass: 'jTableTD',
                list: false
            },
            instructor_name: {
                title: 'Instructor Name',
                width: '4%',
                listClass: 'jTableTD'
            }
        }
    });

    var timestamp = new Date();
    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, ClassCode: $('#hlblCertCode')[0].innerHTML, ClassDate: $('#txtClassDate')[0].value, T: timestamp.getTime() });








    // Look Up List Of Valid Cert Codes
    getCerts();

    // Load Emps AutoComplete List
    var getEmpsCall = new AsyncServerMethod();
    getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);
    

    ///////////////////////////
    // Submit Button Handler //
    ///////////////////////////
    function dsaasd222_Success(data) {
        
        timestamp = new Date();
        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, ClassCode: $('#hlblCertCode')[0].innerHTML, ClassDate: $('#txtClassDate')[0].value, T: timestamp.getTime() });
        $('#btnSubmit').show();

    };
    $("#btnSubmit").click(function () {
        $('#btnSubmit').hide();

        var timeSubmitCall = new AsyncServerMethod();
        //timeSubmitCall.add('CertCode', $('#hlblCertCode').html());
        //timeSubmitCall.add('Classdate', $('#txtClassDate').html());
        
        timeSubmitCall.exec("/SIU_DAO.asmx/dsaasd222", dsaasd222_Success);

    });

});
