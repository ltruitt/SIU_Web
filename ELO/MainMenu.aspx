<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="MainMenu.aspx.cs" Inherits="ELO_MainMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>ELO Main Menu</title>

    
    <style type="text/css"> 
        .AdminMenu ul li {
            margin-left: auto; 
            margin-right: auto; 
            width: 100%; 
            background-color: red;        
        }

        html {
            background-color: #45473f;
        }
        
         
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="background-color: #45473f;">
        <div id="FormWrapper" class="ui-widget ui-form">
<!----------------->
<!-- Form Header -->
<!----------------->           
           
            <section class="ui-widget-header ui-corner-all" style="height: 55px;" >
                <a href="/" style="text-decoration: none; font-weight: bold;">
                    <span style="text-decoration: underline; font-style: italic; position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">home</span>
                    <span id="PageTitle" style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">ELO Menu</span>
                </a>
            </section>  
            

            

            <div class="ui-widget-content ui-corner-top AdminMenu" >
            
                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label>

                <p style="height: 10px;"></p> 
                
                <div  class="ui-helper-clearfix" >
<%--                    <div style="border: 2px solid red; background-color: yellow; color: red; font-weight: bold; padding: 5px;">
                        Effective Dec. 30th Vehicle Inspections must be entered into Shermco YOU! for Time Approval
                    </div>--%>
                    <ul >
				        <li class="l1"><a href="/ELO/TimeEntry.aspx" ID="A5" runat="server">Record Time  ** Minor Modification **</a></li>
                        <li class="l3"><a href="/ELO/MealsExpEntry.aspx" ID="A7" runat="server">Reimbursable Meals and Mileage</a></li>
                        <li class="l3"><a href="/ELO/VehMileage.aspx" ID="A8" runat="server">Vehicle Mileage</a></li>
                        <li class="l3"><a href="/My/MyExpenses.aspx" ID="A12" runat="server">Expense History</a></li>
                        <li id="TimeMangDiv" runat="server" class="l3"><a href="/ELO/TimeRpt.aspx" >Day Total</a></li>
                        <div style="margin-top: 30px;"></div>
                        <li id="SubmitJobDiv" runat="server" class="l3" style="margin-top: 24px;" ><a href="/Reporting/SubmitJobReport.aspx" >Submit Report</a></li>
                        <div style="margin-top: 30px;"></div>
                        <li class="l3"><a href="/ELO/VehDotEntry.aspx"  runat="server">Vehicle Inspection</a></li>                     
                           
                        <li runat="server" ID="TestExp"                 class="l7" style="margin-top: 30px;"><a href="/ELO/MealsExpEntry2.aspx"        >(NEW) Reimbursable Meals and Mileage</a></li>
                        <li runat="server" ID="TestJobCompletion"       class="l7" style="margin-top: 30px;"><a href="/Jobs/JobsCompletion.aspx"       >(NEW) Job Completion</a></li>
                        <li runat="server" ID="TestJobCompletionReport" class="l7"                          ><a href="/Jobs/JobsView.aspx"             >(NEW) Completed Jobs</a></li>
                        <li runat="server" ID="TestJobReport"           class="l7" style="margin-top: 30px;"><a href="/Reporting/SubmitJobReport2.aspx">(NEW) Job Reports By Dept</a></li>
                        
                    </ul>
                </div>
            </div>
        </div>
    </div>
                
</asp:Content>

