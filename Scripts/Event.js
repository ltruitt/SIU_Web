$(document).ready(function () {

    /////////////////
    // Format Tabs //
    /////////////////
    $("#tabs").tabs();
    
    ////////////////////////
    // Format Date Fields //
    ////////////////////////
    $('#Inc_Occur_Date').datepicker();
    
    NumericInput('Osha_Restrict_Days');
    NumericInput('Osha_Lost_Days');
    




    function dateTest(datefield) {
        var i = -1;
        if (datefield != null)
            i = parseInt(datefield.substr(6));

        if (i > 0)
            return $.datepicker.formatDate('mm-dd-yy', new Date(i));
        else
            return '';
    }
    
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
            listAction: '/SIU_DAO.asmx/GetOpenIncidentAccident',
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
                    return dateTest(data.record.iaList.Inc_Occur_Date);
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
            $('#jTableContainer').jtable('selectRows', $('.jtable-data-row').first());
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
                    
                    $('#Inc_Type').val(record.iaList.Inc_Type);
                    $('#Inc_Type_Sub').val(record.iaList.Inc_Type_Sub);
                    $('#Inc_Loc').val(record.iaList.Inc_Loc);
                    $('#Emp_Veh_Involved').val(record.iaList.Emp_Veh_Involved);
                    $('#Emp_Job_No').val(record.iaList.Emp_Job_No);
                    $('#Claim_ID').val(record.iaList.Claim_ID);
                    $('#Inc_Desc').val(record.iaList.Inc_Desc);
                    $('#Inc_Unsafe_Act_or_Condition').val(record.iaList.Inc_Unsafe_Act_or_Condition);
                    $('#Osha_Restrict_Days').val(record.iaList.Osha_Restrict_Days);
                    $('#Osha_Lost_Days').val(record.iaList.Osha_Lost_Days);
                    $('#Emp_ID').val(record.iaList.Emp_ID);
                    $('#Emp_Comments').val(record.iaList.Emp_Comments);
                    $('#Follow_Discipline').val(record.iaList.Follow_Discipline);
                    $('#Follow_Prevent_Reoccur').val(record.iaList.Follow_Prevent_Reoccur);
                    $('#Follow_Comments').val(record.iaList.Follow_Comments);
                    $('#Cost_inHouse').val(record.iaList.Cost_inHouse);
                    $('#Cost_Incurred').val(record.iaList.Cost_Incurred);
                    $('#Cost_Reserve').val(record.iaList.Cost_Reserve);
                    $('#Cost_Total').val(record.iaList.Cost_Total);
                    $('#Follow_Responsible').val(record.iaList.Follow_Responsible);
                    
                    
                    $('#Inc_Occur_Date').datepicker("setDate", $.fn.parseJsonDate(record.iaList.Inc_Occur_Date));
                    
                    $('#Osha_Record_Med')[0].checked = record.iaList.Osha_Record_Med;
                    $('#Emp_Drug_Alchol_Test')[0].checked = record.iaList.Emp_Drug_Alchol_Test;
                    $('#Follow_Discipline_Issued_Flag')[0].checked = record.iaList.Follow_Discipline_Issued_Flag;

                    getEmpDetail();
                });
            } 


        }
    });
    


    ////////////////////////////////////////
    // Event Management For Job Selection //
    ////////////////////////////////////////
    var listOfLocations = [ '(D) Side Window', '(P) Side Window', 'Abdomen', 'Ankle', 'Back', 'Bed', 'Bumper',
                            'Buttox', 'Calf', 'Chest', 'Drivers Side', 'Ear', 'Elbow', 'Eye', 'Finger', 'Foot',
                            'Forearm', 'Front End', 'Front Quarter', 'Groin', 'Hand', 'Head', 'Headlight', 'Hip',
                            'Hood', 'Interior', 'Knee', 'Leg', 'Mirror', 'Motor', 'Motor', 'Neck', 'Organ', 'Passenger Side',
                            'Rear Quarter', 'Rear Window', 'Scalp',  'Shin', 'Shoulder', 'Thigh', 'Tire', 'Toe', 'Totaled Vehicle',
                            'Upper Arm', 'Upper Body', 'Windshield', 'Wrist', 'Other', 'Trailer', 'Rearend', 'Rear Bumper', 'Front Bumper',
                            'Trailer Connection', 'Thumb' ];

    var listOfTypeDesc = ['Abrasion ', 'Amputation ', 'Burn ', 'Blowout ', 'Collision ', 'Contusion ', 'FOD ', 'Fracture ', 'Infection ',
                            'Laceration ', 'Puncture ', 'Sprain ', 'Strain ', 'Theft ', 'Vandalism ', 'Other ', 'Elect Shock', 'Backing ',
                            'Turning Right', 'Turning Left', 'Towing Trailer', 'Sideswipe ', 'Bursitis '];
     
    var listOfType = ['Auto ', 'First Aid', 'Illness ', 'Injury ', 'Near Miss', 'Property ', 'Theft ', 'Other '];
        
    $("#Inc_Type").autocomplete({ source: listOfType },
    {
        matchContains: false,
        minChars: 1,
        autoFill: false,
        mustMatch: false,
        cacheLength: 20,
        max: 20,
        delay: 0,
        select: function (event, ui) {
            $("#Inc_Type").val(ui.item.value);
            $("#Inc_Type").autocomplete("close");

        },
        response: function (event, ui) {
            if (ui.content.length == 1) {
                $("#Inc_Type").val(ui.content[0].value);
                $("#Inc_Type").autocomplete("close");
            }
            return ui;
        }

    });



    $("#Inc_Type_Sub").autocomplete({ source: listOfTypeDesc },
    {
        matchContains: false,
        minChars: 1,
        autoFill: false,
        mustMatch: false,
        cacheLength: 20,
        max: 20,
        delay: 0,
        select: function (event, ui) {
            $("#Inc_Type_Sub").val(ui.item.value);
            $("#Inc_Type_Sub").autocomplete("close");

        },
        response: function (event, ui) {
            if (ui.content.length == 1) {
                $("#Inc_Type_Sub").val(ui.content[0].value);
                $("#Inc_Type_Sub").autocomplete("close");
            }
            return ui;
        }

    });
    


    $("#Inc_Loc").autocomplete({ source: listOfLocations },
    {
        matchContains: false,
        minChars: 1,
        autoFill: false,
        mustMatch: false,
        cacheLength: 20,
        max: 20,
        delay: 0,
        select: function (event, ui) {
            $("#Inc_Loc").val(ui.item.value);
            $("#Inc_Loc").autocomplete("close");
            
        },
        response: function (event, ui) {
            if (ui.content.length == 1) {
                $("#Inc_Loc").val(ui.content[0].value);
                $("#Inc_Loc").autocomplete("close");
            }
            return ui;
        }
        
    });




    /////////////////////
    // Get Report Data //
    /////////////////////
    var listOfVehs = [];
    function getVehilceListSuccess(data) {
        var d = data.d.replace(/\"/g, "").replace(/\[/g, "").replace(/\}/g, "");
        listOfVehs = d.split(",");

        $("#Emp_Veh_Involved").autocomplete(
            { source: listOfVehs },
            {
                matchContains: false,
                minChars: 1,
                autoFill: false,
                mustMatch: true,
                cacheLength: 20,
                max: 20,
                select: function (event, ui) {
                    $('#Emp_Veh_Involved').val(ui.item.value);
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        $('#Emp_Veh_Involved').val(ui.content[0].value);
                        $("#Emp_Veh_Involved").autocomplete("close");
                    }

                    return ui;
                },
                close: function () {
                    $('#Emp_Veh_Involved').focus();
                }
            });
    }
    function getAllVehList() {
        var getVehilceMileageRptAvailAjax = new AsyncServerMethod();
        getVehilceMileageRptAvailAjax.exec("/SIU_DAO.asmx/GetVehilceList", getVehilceListSuccess);
    }
    getAllVehList();






    ////////////////////////////////////////
    // Event Management For Job Selection //
    ////////////////////////////////////////
    var listOfJobs = [];
    function getJobsSuccess(data) {

        listOfJobs = data.d.split("\r");

        $("#Emp_Job_No").autocomplete({ source: listOfJobs },
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
                $("#Emp_Job_No").val( dataPieces[0].replace(/\n/g, "") );
                return false;
            },
            response: function (event, ui) {
                if (ui.content.length == 1) {
                    var dataPieces = ui.content[0].value.split(' ');
                    $(this).val(dataPieces[0].replace(/\n/g, ""));
                    $("#Emp_Job_No").autocomplete("close");
                }

                return ui;
            }
        });
    }
    function getJobs() {
        var getJobsCall = new AsyncServerMethod();
        getJobsCall.exec("/SIU_DAO.asmx/GetTimeJobs", getJobsSuccess);
    }
    getJobs();


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
                    $('#Emp_ID').html(dataPieces[0]);
                    $('#Ename').html('');
                    $('#Supr').html('');
                    $('#Edept').html('');
                    $("#ddEmpIds").autocomplete("close");
                    $("#ddEmpIds").hide();
                    getEmpDetail();
                    return false;
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var dataPieces = ui.content[0].value.split(' ');
                        $('#Emp_ID').html(dataPieces[0]);
                        $('#Ename').html('');
                        $('#Supr').html('');
                        $('#Edept').html('');
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").hide();
                        getEmpDetail();
                    }

                    return ui;
                },
            });
    }


    /////////////////////////////////
    // Load Emps AutoComplete List //
    /////////////////////////////////
    var getEmpsCall = new AsyncServerMethod();
    getEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", getEmpsSuccess);


    function getEmpDetail() {
        $('#Ename').html('');
        $('#Supr').html('');
        $('#Edept').html('');
        var getEmpCall = new AsyncServerMethod();
        getEmpCall.add('EID', $('#Emp_ID').html());
        getEmpCall.exec("/SIU_DAO.asmx/GetEmpBasic", getEmpSuccess);
    }
    
    function getEmpSuccess(data) {
        $("#ddEmpIds").val('');
        $("#ddEmpIds").show();
        var empDetails = $.parseJSON(data.d);
        $('#Ename').html(empDetails.Name);
        $('#Supr').html(empDetails.Super);
        $('#Edept').html(empDetails.Dept);
    }
    

    ////////////////////////
    // Clear / Reset Form //
    ////////////////////////
    $("#btnClear").click(function () {

        $('#hlblUID').html('');
        
        ////////////////////////////////////////
        // Clear Data Confirmation Containers //
        ////////////////////////////////////////   
        $('#Inc_Type').val('');
        $('#Inc_Type_Sub').val('');
        $('#Inc_Loc').val('');
        $('#Emp_Veh_Involved').val('');
        $('#Emp_Job_No').val('');
        $('#Claim_ID').val('');
        $('#Inc_Desc').val('');
        $('#Inc_Unsafe_Act_or_Condition').val('');
        $('#Osha_Restrict_Days').val('');
        $('#Osha_Lost_Days').val('');
        $('#Emp_ID').val('');
        $('#Emp_Comments').val('');
        $('#Follow_Discipline').val('');
        $('#Follow_Prevent_Reoccur').val('');
        $('#Follow_Comments').val('');
        $('#Cost_inHouse').val('');
        $('#Cost_Incurred').val('');
        $('#Cost_Reserve').val('');
        $('#Cost_Total').val('');
        $('#Follow_Responsible').val('');
        $('#Inc_Occur_Date').val('');

        $('#Osha_Record_Med')[0].checked = false;
        $('#Emp_Drug_Alchol_Test')[0].checked = false;
        $('#Follow_Discipline_Issued_Flag')[0].checked = false;

        $('#Ename').html('');
        $('#Supr').html('');
        $('#Edept').html('');
    });

    //////////////////////////
    // Save Data Processing //
    //////////////////////////
    $("#btnSave").on("click", function () {
        var submitIncidentAccidentAjax = new AsyncServerMethod();

        submitIncidentAccidentAjax.add('hlblUID', $('#hlblUID').html() );
        
        submitIncidentAccidentAjax.add('_Inc_Type', $('#Inc_Type').val() );
        submitIncidentAccidentAjax.add('_Inc_Type_Sub', $('#Inc_Type_Sub').val() );
        submitIncidentAccidentAjax.add('_Inc_Loc', $('#Inc_Loc').val() );
        submitIncidentAccidentAjax.add('_Emp_Veh_Involved', $('#Emp_Veh_Involved').val() );
        submitIncidentAccidentAjax.add('_Emp_Job_No', $('#Emp_Job_No').val() );
        submitIncidentAccidentAjax.add('_Claim_ID', $('#Claim_ID').val() );
        submitIncidentAccidentAjax.add('_Inc_Desc', $('#Inc_Desc').val() );
        submitIncidentAccidentAjax.add('_Inc_Unsafe_Act_or_Condition', $('#Inc_Unsafe_Act_or_Condition').val() );
        submitIncidentAccidentAjax.add('_Osha_Restrict_Days', $('#Osha_Restrict_Days').val() );
        submitIncidentAccidentAjax.add('_Osha_Lost_Days', $('#Osha_Lost_Days').val() );
        submitIncidentAccidentAjax.add('_Emp_ID', $('#Emp_ID').val() );
        submitIncidentAccidentAjax.add('_Emp_Comments', $('#Emp_Comments').val() );
        submitIncidentAccidentAjax.add('_Follow_Discipline', $('#Follow_Discipline').val() );
        submitIncidentAccidentAjax.add('_Follow_Prevent_Reoccur', $('#Follow_Prevent_Reoccur').val() );
        submitIncidentAccidentAjax.add('_Follow_Comments', $('#Follow_Comments').val() );
        submitIncidentAccidentAjax.add('_Cost_inHouse', $('#Cost_inHouse').val() );
        submitIncidentAccidentAjax.add('_Cost_Incurred', $('#Cost_Incurred').val() );
        submitIncidentAccidentAjax.add('_Cost_Reserve', $('#Cost_Reserve').val() );
        submitIncidentAccidentAjax.add('_Cost_Total', $('#Cost_Total').val() );
        submitIncidentAccidentAjax.add('_Follow_Responsible', $('#Follow_Responsible').val() );
        submitIncidentAccidentAjax.add('_Inc_Occur_Date', $('#Inc_Occur_Date').val() );
        
        submitIncidentAccidentAjax.add('_Osha_Record_Med', $('#Osha_Record_Med')[0].checked);
        submitIncidentAccidentAjax.add('_Emp_Drug_Alchol_Test', $('#Emp_Drug_Alchol_Test')[0].checked);
        submitIncidentAccidentAjax.add('_Follow_Discipline_Issued_Flag', $('#Follow_Discipline_Issued_Flag')[0].checked);

        submitIncidentAccidentAjax.exec("/SIU_DAO.asmx/recordIncidentAccident", recordIncidentAccidentSuccess);
    });
    

    ////////////////////////////
    // Submit Data Processing //
    ////////////////////////////
    $("#btnSubmit").on("click", function () {
        var submitIncidentAccidentAjax = new AsyncServerMethod();

        submitIncidentAccidentAjax.add('hlblUID', $('#hlblUID').html());

        submitIncidentAccidentAjax.add('Inc_Type', $('#Inc_Type').val());
        submitIncidentAccidentAjax.add('Inc_Type_Sub', $('#Inc_Type_Sub').val());
        submitIncidentAccidentAjax.add('Inc_Loc', $('#Inc_Loc').val());
        submitIncidentAccidentAjax.add('Emp_Veh_Involved', $('#Emp_Veh_Involved').val());
        submitIncidentAccidentAjax.add('Emp_Job_No', $('#Emp_Job_No').val());
        submitIncidentAccidentAjax.add('Claim_ID', $('#Claim_ID').val());
        submitIncidentAccidentAjax.add('Inc_Desc', $('#Inc_Desc').val());
        submitIncidentAccidentAjax.add('Inc_Unsafe_Act_or_Condition', $('#Inc_Unsafe_Act_or_Condition').val());
        submitIncidentAccidentAjax.add('Osha_Restrict_Days', $('#Osha_Restrict_Days').val());
        submitIncidentAccidentAjax.add('Osha_Lost_Days', $('#Osha_Lost_Days').val());
        submitIncidentAccidentAjax.add('Emp_ID', $('#Emp_ID').val());
        submitIncidentAccidentAjax.add('Emp_Comments', $('#Emp_Comments').val());
        submitIncidentAccidentAjax.add('Follow_Discipline', $('#Follow_Discipline').val());
        submitIncidentAccidentAjax.add('Follow_Prevent_Reoccur', $('#Follow_Prevent_Reoccur').val());
        submitIncidentAccidentAjax.add('Follow_Comments', $('#Follow_Comments').val());
        submitIncidentAccidentAjax.add('Cost_inHouse', $('#Cost_inHouse').val());
        submitIncidentAccidentAjax.add('Cost_Incurred', $('#Cost_Incurred').val());
        submitIncidentAccidentAjax.add('Cost_Reserve', $('#Cost_Reserve').val());
        submitIncidentAccidentAjax.add('Cost_Total', $('#Cost_Total').val());
        submitIncidentAccidentAjax.add('Follow_Responsible', $('#Follow_Responsible').val());
        submitIncidentAccidentAjax.add('Inc_Occur_Date', $('#Inc_Occur_Date').val());

        submitIncidentAccidentAjax.add('Osha_Record_Med', $('#Osha_Record_Med')[0].checked);
        submitIncidentAccidentAjax.add('Emp_Drug_Alchol_Test', $('#Emp_Drug_Alchol_Test')[0].checked);
        submitIncidentAccidentAjax.add('Follow_Discipline_Issued_Flag', $('#Follow_Discipline_Issued_Flag')[0].checked);

        submitIncidentAccidentAjax.exec("/SIU_DAO.asmx/submitIncidentAccident", recordIncidentAccidentSuccess);
    });




    function recordIncidentAccidentSuccess() {
        $('#jTableContainer').jtable('reload');
    }



    var timestamp = new Date();
    $('#jTableContainer').jtable('load', { DataFilter: 'New', isA: '1', T: timestamp.getTime() });

});
