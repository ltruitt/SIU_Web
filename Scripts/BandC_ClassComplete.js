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

        var timestamp = new Date();
        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML, ClassCode: $('#hlblCertCode')[0].innerHTML, ClassDate: $('#txtClassDate')[0].value, T: timestamp.getTime() });


        //        $('#Comments').show('slow');
        //        if (!/phone/i.test(window.location)) {
        //            $('#Comments').show('slow');
        //            $('#btnSubmit').attr('disabled', false);
        //        }

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
        $("#ddEmpIds").autocomplete({ source: listOfEmps },
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
                    $("#ddEmpIds").autocomplete("close");
                    $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                    clear();
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(dataPieces[0] + ' ' + dataPieces[2] + ', ' + dataPieces[3]);
                    }

                    return ui;
                }
            });
    }



    ///////////////////////////////////////
    // Setup Report Of Unposted Expenses //
    ///////////////////////////////////////
    $('#jTableContainer').jtable({
        title: 'All Unposted Students For Instructor',
        defaultSorting: 'UID ASC',
        edit: true,
        actions: {
            listAction: '/SIU_DAO.asmx/GetUnpostedClass',
            deleteAction: '/SIU_DAO.asmx/DeleteMyExpensexxx'
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
                visibility: hidden
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

});
