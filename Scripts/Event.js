$(document).ready(function () {

    /////////////////
    // Format Tabs //
    /////////////////
    $("#tabs").tabs();
    

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
                title: 'Incident Date',
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

        //Register to selectionChanged event to hanlde events
        selectionChanged: function () {

            //Get all selected rows
            var $selectedRows = $('#jTableContainer').jtable('selectedRows');

            if ($selectedRows.length == 1) {
                //Show selected rows
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                });
            } //else {
                //No rows selected
//                clearDtl();
            //}


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
        
    $("#acType").autocomplete({ source: listOfType },
    {
        matchContains: false,
        minChars: 1,
        autoFill: false,
        mustMatch: false,
        cacheLength: 20,
        max: 20,
        delay: 0,
        select: function (event, ui) {
            $("#acType").val(ui.item.value);
            $("#acType").autocomplete("close");

        },
        response: function (event, ui) {
            if (ui.content.length == 1) {
                $("#acType").val(ui.content[0].value);
                $("#acType").autocomplete("close");
            }
            return ui;
        }

    });



    $("#acTypeDesc").autocomplete({ source: listOfTypeDesc },
    {
        matchContains: false,
        minChars: 1,
        autoFill: false,
        mustMatch: false,
        cacheLength: 20,
        max: 20,
        delay: 0,
        select: function (event, ui) {
            $("#acTypeDesc").val(ui.item.value);
            $("#acTypeDesc").autocomplete("close");

        },
        response: function (event, ui) {
            if (ui.content.length == 1) {
                $("#acTypeDesc").val(ui.content[0].value);
                $("#acTypeDesc").autocomplete("close");
            }
            return ui;
        }

    });
    


    $("#acLocation").autocomplete({ source: listOfLocations },
    {
        matchContains: false,
        minChars: 1,
        autoFill: false,
        mustMatch: false,
        cacheLength: 20,
        max: 20,
        delay: 0,
        select: function (event, ui) {
            $("#acLocation").val(ui.item.value);
            $("#acLocation").autocomplete("close");
            
        },
        response: function (event, ui) {
            if (ui.content.length == 1) {
                $("#acLocation").val(ui.content[0].value);
                $("#acLocation").autocomplete("close");
            }
            return ui;
        }
        
    });

    $('#DateOccured').datepicker({
        constrainInput: true
        //onSelect: validate()
    });
    











    var timestamp = new Date();
    $('#jTableContainer').jtable('load', { DataFilter: 'New', isA: '1', T: timestamp.getTime() });

});
