$(document).ready(function () {
    
    var timestamp = new Date();
    
    ////////////////
    // Form Reset //
    ////////////////
    $("#btnClear").click(function () {
            
        $("#acCertList").val('');
        $('#hlblCertCode').html('');
        
        $("#txtInstructor").val('');
        $('#hlblEID').html('');
        
        $('#panelA').hide();
        $('#panelB').hide();
    });
    $("#btnClear").click();

    ////////////////////////////////////
    // Get List Of All Possible Certs //
    ////////////////////////////////////
    var listOfCerts = [];
    function getCertsListSuccess(data) {
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
    function getCertsList() {
        var getCertsAjax = new AsyncServerMethod();
        getCertsAjax.exec("/SIU_DAO.asmx/GetCerts", getCertsListSuccess);
    }


    //////////////////////////////////
    // Get Details Of Selected Cert //
    //////////////////////////////////
    function getCertSuccess(data) {

        var certDetails = $.parseJSON(data.d);

        if (certDetails.length == 0) {
            $('#acCertList').val('');
            $('#ServerError').html('Certificate Lookup Failed');
            return;
        }
        
        $('#ServerError').html('');
        $('#hlblCertCode')[0].innerHTML = certDetails[0].Code;
        $('#panelB').show('slow');
        
        timestamp = new Date();
        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, ClassCode: $('#hlblCertCode')[0].innerHTML, ClassDate: $('#txtClassDate')[0].value, T: timestamp.getTime() });
    }
    function getCertDetails() {
        var cert = $('#acCertList').val();

        var getCertAjax = new AsyncServerMethod();
        getCertAjax.add('Cert', cert);
        getCertAjax.exec("/SIU_DAO.asmx/GetCert", getCertSuccess);
    }

    ////////////////////////////////////
    // Setup Class Date As Date Field //
    ////////////////////////////////////
    $('#txtClassDate').datepicker({
        constrainInput: true
    });

    /////////////////////////////////////////////////////////////////
    // Load List Of Employees For Student and Instructor Selection //
    /////////////////////////////////////////////////////////////////
    var listOfEmps = [];
    function getEmpsSuccess(data) {
        listOfEmps = data.d.split("\r");
        


        $("#txtInstructor").autocomplete({ source: listOfEmps },
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
                $('#hlblEID')[0].innerHTML = dataPieces[0];
                $("#txtInstructor").autocomplete("close");
                $("#txtInstructor").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, ClassCode: $('#hlblCertCode')[0].innerHTML, ClassDate: $('#txtClassDate')[0].value, T: timestamp.getTime() });
                $('#panelA').show('slow');
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var dataPieces = ui.content[0].value.split(' ');
                    $('#hlblEID')[0].innerHTML = dataPieces[0];
                    $("#txtInstructor").autocomplete("close");
                    $("#txtInstructor").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, ClassCode: $('#hlblCertCode')[0].innerHTML, ClassDate: $('#txtClassDate')[0].value, T: timestamp.getTime() });
                    $('#panelA').show('slow');
                }

                return ui;
            }
        });

        




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
    
    /////////////////////////////
    // Record Class Completion //
    /////////////////////////////
    function recordClass(studentId) {
        var recordClassCall = new AsyncServerMethod();
        recordClassCall.add('QualCode', $('#hlblCertCode')[0].innerHTML );
        recordClassCall.add('ClassDate', $('#txtClassDate')[0].value);
        recordClassCall.add('InstID', $('#hlblEID')[0].innerHTML);
        recordClassCall.add('StudentID', studentId);
        recordClassCall.exec("/SIU_DAO.asmx/RecordUnPostedClassStudent", recordUnPostedClassStudentSuccess);
        
    }    
    function recordUnPostedClassStudentSuccess() {
        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, ClassCode: $('#hlblCertCode')[0].innerHTML, ClassDate: $('#txtClassDate')[0].value, T: timestamp.getTime() });
    }



    ///////////////////////////////////////
    // Setup Report Of Unposted Expenses //
    ///////////////////////////////////////
    $('#jTableContainer').jtable({
        title: 'All Unposted Certificates For Instructor',
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










    //////////////////////////////////////
    // Look Up List Of Valid Cert Codes //
    //////////////////////////////////////
    getCertsList();

    // Load Emps AutoComplete List
    var getEmpsCall = new AsyncServerMethod();
    getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);
    









    /////////////////////////////////
    // Post Button Success Handler //
    /////////////////////////////////
    function dsaasd222_Success(data) {
        $('#btnClear').click();

    };
    
    $("#btnSubmit").click(function () {
        var timeSubmitCall = new AsyncServerMethod();
        timeSubmitCall.add('instID', $('#hlblEID').html());
        timeSubmitCall.exec("/SIU_DAO.asmx/dsaasd222", dsaasd222_Success);

    });

});
