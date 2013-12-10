<%@ Page Title="" Language="C#" MasterPageFile="/Phone/PhoneMaster.master" AutoEventWireup="true" CodeFile="MainMenu.aspx.cs" Inherits="ELO_MainMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>ELO Main Menu</title>


    <style type="text/css"> 
        .AdminMenu ul li {
            margin-left: auto; 
            margin-right: auto; 
            width: 100%; 
            background-color: red;        
        }
    </style>
    

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

        <div id="FormWrapper" class="ui-widget ui-form">
<!----------------->
<!-- Form Header -->
<!----------------->           
            <div class="ui-widget-header ui-corner-all" >
                <div  style="text-align: center; font-size: 2em; height: 60px;">ELO Menu</div>
            </div>
            

            <div class="ui-widget-content ui-corner-top AdminMenu" >
            
                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <label runat="server" ID="lblEmpName" ></label>

                <p style="height: 10px;"></p> 
                <div style="border: 2px solid red; background-color: yellow; color: red; font-weight: bold; padding: 5px;">
                    Effective Dec. 30th Vehicle Inspections must be entered into Shermco YOU! for Time Approval
                </div>

                <div  class="ui-helper-clearfix" >
                    <ul >
                        
				        <li class="l3"><a href="/Phone/ELO/TimeEntry.aspx" ID="A2" runat="server">Record Time</a></li>
                        <li class="l3"><a href="/Phone/ELO/MealsExpEntry.aspx" ID="A5" runat="server">Reimbursable Meals and Mileage</a></li>
                        <li class="l3"><a href="/Phone/ELO/VehMileage.aspx" ID="A8" runat="server">Vehicle Mileage</a></li>
                        <%--<li class="l3"><a href="/Phone/My/MyExpenses.aspx" ID="A12" runat="server">Expense History</a></li>--%>
                        <li id="TimeMangDiv" runat="server" class="l3"><a href="/Phone/ELO/TimeRpt.aspx" >Day Total</a></li>
                        <div style="margin-top: 30px;"></div>
                        <li id="SubmitJobDiv" runat="server" class="l3" style="margin-top: 24px;" ><a href="/Phone/ELO/SubmitJobReport.aspx" >Submit Report</a></li>
                        <div style="margin-top: 30px;"></div>
                        <li class="l3"><a href="/Phone/ELO/VehDotEntry.aspx" ID="A7" runat="server">Vehicle Inspection</a></li>
                        

                        

                        
                        
                        
                    </ul>
                </div>
            </div>
        </div>
                    
</asp:Content>

