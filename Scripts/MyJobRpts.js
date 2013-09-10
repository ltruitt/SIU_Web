$(document).ready(function () {

    $('#jTableContainer').jtable({
        title: 'In-Process Job Reports',
        defaultSorting: 'JobNo ASC',
        edit: true,
        actions: {
            listAction: '/SIU_DAO.asmx/GetMyOpenJobRptsSum'
        },
        fields: {
            JobNo2: {
                title: '',
                width: '1%',
                sorting: false,
                edit: true,
                create: false,
                display: function (JobData) {

                    // Create an image that will be used to open child table
                    var $img = $('<img src="/Images/search_button.png" title="View Details" />');

                    //Open child table when user clicks the image
                    $img.click(function () {
                        $('#lblJobNo')[0].innerHTML = JobData.record.JobNo;
                        $('#lblCost')[0].innerHTML = JobData.record.CostingDate;
                        //$('#lblTurnInTech')[0].innerHTML = JobData.record.TurnInTech;
                        $('#lblReqSales')[0].innerHTML = JobData.record.ReqSalesFollowUp;
                        $('#lblSalesDate')[0].innerHTML = JobData.record.SalesFollupDate;
                        $('#lblSubmitDate')[0].innerHTML = JobData.record.SubmitDate;
                        $('#lblJhaDate')[0].innerHTML = JobData.record.JhaDate;
                        $('#lblIrSubmitDate')[0].innerHTML = JobData.record.IrSubmitDate;
                        $('#lblIrCompleteDate')[0].innerHTML = JobData.record.IrCompleteDate;

                        $('#lblLoginDate')[0].innerHTML = JobData.record.LoggedInDate;
                        $('#lblDataEntryDate')[0].innerHTML = JobData.record.DataEntryDate;
                        $('#lblProofDate')[0].innerHTML = JobData.record.ProofDate;
                        $('#lblCorrectDate')[0].innerHTML = JobData.record.CorrectDate;
                        $('#lblReviewDate')[0].innerHTML = JobData.record.ReviewDate;
                        $('#lblReadyDate')[0].innerHTML = JobData.record.ReadyDate;

                        $('#DetailDiv').show('slow');
                    });

                    //Return image to show on the person row
                    return $img;
                }
            },

            JobNo: {
                title: 'Job No',
                width: '2%',
                listClass: 'jTableTD'
            },
            JobCust: {
                title: 'Cust ',
                width: '4%',
                listClass: 'jTableTD'
            },
            CostingDate: {
                title: 'Cost Date',
                width: '3%',
                listClass: 'jTableTD'
            }
        }
    });


    //GetSubmitJobReportByNo
    $('#DetailDiv').hide();
    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });


    ///////////////////////////////////////////////////////////////
    // Load List Of Employees So Supr Can Change Viewed Employee //
    ///////////////////////////////////////////////////////////////
    var listOfEmps = [];
    function GetEmps_success(data) {
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
                    var DataPieces = ui.item.value.split(' ');
                    $('#hlblEID')[0].innerHTML = DataPieces[0];
                    $("#ddEmpIds").autocomplete("close");
                    $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);
                    $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });
                },
                response: function (event, ui) {
                    if (ui.content.length == 1) {
                        var DataPieces = ui.content[0].value.split(' ');
                        $('#hlblEID')[0].innerHTML = DataPieces[0];
                        $("#ddEmpIds").autocomplete("close");
                        $("#ddEmpIds").val(DataPieces[0] + ' ' + DataPieces[2] + ', ' + DataPieces[3]);

                        $('#jTableContainer').jtable('load', { EmpID: $('#hlblEID')[0].innerHTML });
                    }

                    return ui;
                }
            });
    }


    // Load Emps AutoComplete List
    if ($("#SuprArea").length > 0) {
        var GetEmpsCall = new AsyncServerMethod();
        GetEmpsCall.exec("/SIU_DAO.asmx/GetAutoCompleteActiveEmployees", GetEmps_success);
    }



});