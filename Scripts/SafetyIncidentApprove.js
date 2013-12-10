$(document).ready(function () {

    /////////////////
    // Format Tabs //
    /////////////////
    $("#tabs").tabs();
    
    //////////////////////////////////////////////
    // Pickup Incident Record Number To Display //
    //////////////////////////////////////////////
    var rcdId = $.fn.getURLParameter('RptID');
   

    ///////////////////////////////////////
    // Setup Report Of Unposted Expenses //
    ///////////////////////////////////////
    $('#JTableIncidentComments').jtable({
        title: 'Incident / Accident Approval Notes',
        defaultSorting: 'CommentDate ASC',
        edit: false,
        selecting: true,
        sorting: false,
        multiselect: false,
        selectingCheckboxes: false,
        paging: false,
        actions: {
            listAction: '/SIU_DAO.asmx/GetIncidentAccidentApprovalNotes',
            createAction: 'None',
        },
        formSubmitting: function (event, data) {           
            var saveNoteCall = new AsyncServerMethod();
            saveNoteCall.add('UID', $('#hlblUID').html());
            saveNoteCall.add('Note', data.form.find('textarea[name="Comment"]').val());
            saveNoteCall.exec("/SIU_DAO.asmx/RecordIncidentAccidentApprovalNote");
            
            $('#JTableIncidentComments').jtable('load', { UID: $('#hlblUID').html(), T: timestamp.getTime() });
            return false;
        },
        fields: {
            CommentsBy: {
                key: false,
                title: 'Comment By',
                create: false,
                edit: false,
                list: true,
                width: '5%'
            },
            CommentDate: {
                key: false,
                title: 'Date',
                inputClass: 'DataInputCss',
                create: false,
                edit: false,
                list: true,
                width: '3%',
                displayFormat: 'mm-dd-yy',
                display: function (data) {
                    return $.fn.dateTest(data.record.CommentDate);
                }
            },
            Comment: {
                key: false,
                title: 'Comment',
                type: 'textarea',
                create: true,
                edit: false,
                list: true,
                width: '10%'
            }
        }
    });
      


    ///////////////////////////////////////
    // Setup Report Of Unposted Expenses //
    ///////////////////////////////////////
    $('#jTableContainer').jtable({
        title: 'Open Incident / Accident',
        defaultSorting: 'UID ASC',
        edit: false,
        selecting: true,
        sorting: false,
        multiselect: false,
        selectingCheckboxes: false,
        paging: false,
        pagesize: 20,
        actions: {
            listAction: '/SIU_DAO.asmx/GetSubmitIncidentAccident',
        },
        fields: {
            UID: {
                key: true,
                title: 'ID',
                create: false,
                edit: false,
                list: true,
                width: '1%',
                display: function (data) {
                    return data.record.iaList.UID;
                }
            },

            Claim_ID: {
                key: true,
                title: 'Claim',
                create: false,
                edit: false,
                list: true,
                width: '1%',
                display: function (data) {
                    return data.record.iaList.Claim_ID;
                }
            },
            
            Inc_Occur_Date: {
                title: 'Date',
                width: '5%',
                type: 'date',
                displayFormat: 'mm-dd-yy',
                listClass: 'jTableTD',
                list: true,
                display: function (data) {
                    return $.fn.dateTest(data.record.iaList.Inc_Occur_Date);
                }
            },
            
            Inc_Type: {
                title: 'Type',
                width: '5%',
                display: function (data) { return data.record.iaList.Inc_Type + ':' + data.record.iaList.Inc_Type_Sub; },
                listClass: 'jTableTD',
                list: true
            },
            
            
            Last_Name: {
                title: 'Name',
                width: '10%',
                listClass: 'jTableTD',
                display: function (data) {
                    return data.record.Last_Name;
                }
            }
           
        },
        recordsLoaded: function () {
            
            if ( ! rcdId )
                $('#jTableContainer').jtable('selectRows', $('.jtable-data-row').first());
            else
                $('#jTableContainer').jtable('selectRows', 
                    $("tr").filter(function() {
                        return $.trim($(this).find('td:eq(0)').text()) == rcdId;
                    })
                );
        },
        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableContainer').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    
                    $('#hlblUID').html(record.iaList.UID);
                    
                    if (!record.iaList.Inc_Type) record.iaList.Inc_Type = 'none';
                    $('#Inc_Type').html(record.iaList.Inc_Type);
                    
                    if (!record.iaList.Inc_Type_Sub) record.iaList.Inc_Type_Sub = 'none';
                    $('#Inc_Type_Sub').html(record.iaList.Inc_Type_Sub);
                    
                    if (!record.iaList.Inc_Loc) record.iaList.Inc_Loc = 'none';
                    $('#Inc_Loc').html(record.iaList.Inc_Loc);
                    
                    if (!record.iaList.Emp_Veh_Involved) record.iaList.Emp_Veh_Involved = 'none';
                    $('#Emp_Veh_Involved').html(record.iaList.Emp_Veh_Involved);
                    
                    if (!record.iaList.Emp_Job_No) record.iaList.Emp_Job_No = 'none';
                    $('#Emp_Job_No').html(record.iaList.Emp_Job_No);
                    
                    if (!record.iaList.Claim_ID) record.iaList.Claim_ID = 'none';
                    $('#Claim_ID').html(record.iaList.Claim_ID);
                    
                    if (!record.iaList.Inc_Desc) record.iaList.Inc_Desc = 'none';
                    $('#Inc_Desc').html(record.iaList.Inc_Desc);
                    
                    if (!record.iaList.Inc_Unsafe_Act_or_Condition) record.iaList.Inc_Unsafe_Act_or_Condition = 'none';
                    $('#Inc_Unsafe_Act_or_Condition').html(record.iaList.Inc_Unsafe_Act_or_Condition);
                    
                    if (!record.iaList.Osha_Restrict_Days) record.iaList.Osha_Restrict_Days = '0';
                    $('#Osha_Restrict_Days').html(record.iaList.Osha_Restrict_Days);
                    
                    if (!record.iaList.Osha_Lost_Days) record.iaList.Osha_Lost_Days = '0';
                    $('#Osha_Lost_Days').html(record.iaList.Osha_Lost_Days);
                    
                    $('#Emp_ID').html(record.iaList.Emp_ID);
                    
                    if (!record.iaList.Emp_Comments) record.iaList.Emp_Comments = 'none';
                    $('#Emp_Comments').html(record.iaList.Emp_Comments);
                    
                    if (!record.iaList.Follow_Discipline) record.iaList.Follow_Discipline = 'none';
                    $('#Follow_Discipline').html(record.iaList.Follow_Discipline);
                    
                    if (!record.iaList.Follow_Prevent_Reoccur) record.iaList.Follow_Prevent_Reoccur = 'none';
                    $('#Follow_Prevent_Reoccur').html(record.iaList.Follow_Prevent_Reoccur);
                    
                    if (!record.iaList.Follow_Comments) record.iaList.Follow_Comments = 'none';
                    $('#Follow_Comments').html(record.iaList.Follow_Comments);
                    

                    if (!record.iaList.Cost_inHouse) record.iaList.Cost_inHouse = '0';
                    $('#Cost_inHouse').html(record.iaList.Cost_inHouse);
                    
                    if (!record.iaList.Cost_Incurred) record.iaList.Cost_Incurred = '0';
                    $('#Cost_Incurred').html(record.iaList.Cost_Incurred);
                    
                    if (!record.iaList.Cost_Reserve) record.iaList.Cost_Reserve = '0';
                    $('#Cost_Reserve').val(record.iaList.Cost_Reserve);
                    
                    if (!record.iaList.Cost_Total) record.iaList.Cost_Total = '0';
                    $('#Cost_Total').html(record.iaList.Cost_Total);
                    
                    if (!record.iaList.Follow_Responsible) record.iaList.Follow_Responsible = 'none';
                    $('#Follow_Responsible').html(record.iaList.Follow_Responsible);
                    
                    $('#Inc_Occur_Date').html( $.fn.dateTest(record.iaList.Inc_Occur_Date));
                    if (  $('#Inc_Occur_Date').html().length == 0) $('#Inc_Occur_Date').html('none');


                    $('#Osha_Record_Med')[0].checked = record.iaList.Osha_Record_Med;
                    $('#Emp_Drug_Alchol_Test')[0].checked = record.iaList.Emp_Drug_Alchol_Test;
                    $('#Follow_Discipline_Issued_Flag')[0].checked = record.iaList.Follow_Discipline_Issued_Flag;

                    getEmpDetail();
                    $('#JTableIncidentComments').jtable('load', { UID: $('#hlblUID').html(), T: timestamp.getTime() });
                    getEmpReportingChain();
                    
                });
            } 


        }
    });
    



    function getEmpDetail() {
        $('#Ename').html('');
        $('#Supr').html('');
        $('#Edept').html('');
        var getEmpCall = new AsyncServerMethod();
        getEmpCall.add('EID', $('#Emp_ID').html());
        getEmpCall.exec("/SIU_DAO.asmx/GetEmpBasic", getEmpSuccess);
    }
    
    function getEmpSuccess(data) {
        if (!data.d)
            return;
        $("#ddEmpIds").val('');
        $("#ddEmpIds").show();
        var empDetails = $.parseJSON(data.d);
        $('#Ename').html(empDetails.Name);
        $('#Supr').html(empDetails.Super);
        $('#Edept').html(empDetails.Dept);
    }
    
    function getEmpReportingChain() {
        $('#AppSuprDiv').hide();
        $('#AppDeptDiv').hide();
        $('#AppDivDiv').hide();
        $('#AppVpDiv').hide();
        $('#AppGmDiv').hide();
        $('#AppLegalDiv').hide();
        $('#AppEhsDiv').hide();
        $('#btnApp').hide();
        $('#btnCls').hide();
        
        
        $('#AppSuprID').html('');
        $('#AppSuprName').html('');
        $('#AppSuprDate').html('');

        $('#AppDeptId').html('');
        $('#AppDeptName').html('');
        $('#AppDeptDate').html('');

        $('#AppDivId').html('');
        $('#AppDivName').html('');
        $('#AppDivDate').html('');

        $('#AppVpId').html('');
        $('#AppVpName').html('');
        $('#AppVpDate').html('');

        $('#AppGmId').html('');
        $('#AppGmName').html('');
        $('#AppGmDate').html('');
        
        $('#AppLegalId').html('');
        $('#AppLegalName').html('');
        $('#AppLegalDate').html('');

        $('#AppEhsId').html('');
        $('#AppEhsName').html('');
        $('#AppEhsDate').html('');
        
        var getEmpChainCall = new AsyncServerMethod();
        getEmpChainCall.add('UID', $('#hlblUID').html());
        getEmpChainCall.exec("/SIU_DAO.asmx/ReportingChain", getChainSuccess);
    }

    function getChainSuccess(data) {
        
        var empChain = $.parseJSON(data.d);

        if (empChain.SuprEmpId) {
            $('#AppSuprDiv').show();
            $('#AppSuprID').html(empChain.SuprEmpId);
            $('#AppSuprName').html(empChain.SuprName);
            $('#AppSuprDate').html($.fn.dateTest(empChain.SuprDate));
            
            if (empChain.SuprEmpId == $('#hlblEID').html() )
                if ( $('#AppSuprDate').html().length < 10)
                    if (empChain.readyToClose)
                        $('#btnCls').show();
                    else
                        $('#btnApp').show();
        }

        if (empChain.DeptMgrEmpId) {
            $('#AppDeptDiv').show();
            $('#AppDeptId').html(empChain.DeptMgrEmpId);
            $('#AppDeptName').html(empChain.DeptMgrName);
            $('#AppDeptDate').html($.fn.dateTest(empChain.DeptMgrDate));
            
            if (empChain.DeptMgrEmpId == $('#hlblEID').html() )
                if ( $('#AppDeptDate').html().length < 10)
                    if (empChain.readyToClose)
                        $('#btnCls').show();
                    else
                        $('#btnApp').show();
        }
        
        if (empChain.DivMgrEmpId) {
            $('#AppDivDiv').show();
            $('#AppDivId').html(empChain.DivMgrEmpId);
            $('#AppDivName').html(empChain.DivMgrName);
            $('#AppDivDate').html($.fn.dateTest(empChain.DivMgrDate));
            
            if (empChain.DivMgrEmpId == $('#hlblEID').html() )
                if ( $('#AppDivDate').html().length < 10)
                    if (empChain.readyToClose)
                        $('#btnCls').show();
                    else
                        $('#btnApp').show();
        }

        if (empChain.VpEmpId) {
            $('#AppVpDiv').show();
            $('#AppVpId').html(empChain.VpEmpId);
            $('#AppVpName').html(empChain.VpName);
            $('#AppVpDate').html($.fn.dateTest(empChain.VpDate));
            
            if (empChain.VpEmpId == $('#hlblEID').html())
                if ( $('#AppVpDate').html().length < 10)
                    if (empChain.readyToClose)
                        $('#btnCls').show();
                    else
                        $('#btnApp').show();
        }

        if (empChain.GmEmpId) {
            $('#AppGmDiv').show();
            $('#AppGmId').html(empChain.GmEmpId);
            $('#AppGmName').html(empChain.GmName);
            $('#AppGmDate').html($.fn.dateTest(empChain.GmDate));
            
            if (empChain.GmEmpId == $('#hlblEID').html())
                if ( $('#AppGmDate').html().length < 10)
                    if (empChain.readyToClose)
                        $('#btnCls').show();
                    else
                        $('#btnApp').show();
        }
        
        if (empChain.LegalMgrEmpId) {
            $('#AppLegalDiv').show();
            $('#AppLegalId').html(empChain.LegalMgrEmpId);
            $('#AppLegalName').html(empChain.LegalMgrName);
            $('#AppLegalDate').html($.fn.dateTest(empChain.LegalMgrDate));
            
            if (empChain.LegalMgrEmpId == $('#hlblEID').html())
                if ( $('#AppLegalDate').html().length < 10)
                    if (empChain.readyToClose)
                        $('#btnCls').show();
                    else
                        $('#btnApp').show();
        }

        if (empChain.SafetyMgrEmpId) {
            $('#AppEhsDiv').show();
            $('#AppEhsId').html(empChain.SafetyMgrEmpId);
            $('#AppEhsName').html(empChain.SafetyMgrName);
            $('#AppEhsDate').html($.fn.dateTest(empChain.SafetyMgrDate));
            
            if (empChain.SafetyMgrEmpId == $('#hlblEID').html())
                if ( $('#AppEhsDate').html().length < 10)
                    if (empChain.readyToClose)
                        $('#btnCls').show();
                    else
                        $('#btnApp').show();
        }
        
        
    }
    


    

    /////////////////////////////
    // Approve Data Processing //
    /////////////////////////////
    $("#btnApp").on("click", function () {
        var submitIncidentAccidentAjax = new AsyncServerMethod();

        submitIncidentAccidentAjax.add('hlblUID', $('#hlblUID').html());
        submitIncidentAccidentAjax.exec("/SIU_DAO.asmx/ApproveIncidentAccident", recordIncidentAccidentSuccess);
    });
    
    ///////////////////////////
    // Close Data Processing //
    ///////////////////////////
    $("#btnCls").on("click", function () {
        var submitIncidentAccidentAjax = new AsyncServerMethod();

        submitIncidentAccidentAjax.add('hlblUID', $('#hlblUID').html());
        submitIncidentAccidentAjax.exec("/SIU_DAO.asmx/ApproveIncidentAccident", recordIncidentAccidentSuccess);
    });




    function recordIncidentAccidentSuccess() {
        $('#jTableContainer').jtable('reload');
    }



    var timestamp = new Date();
    $('#jTableContainer').jtable('load', { DataFilter: 'New', isA: '1', T: timestamp.getTime() });
    

});
