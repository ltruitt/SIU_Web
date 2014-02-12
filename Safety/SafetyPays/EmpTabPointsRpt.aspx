<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="EmpTabPointsRpt.aspx.cs" Inherits="Safety_SafetyPays_EmpTabPointsRpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Safety Pays Tabular Report</title>

    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    <script type="text/javascript" src="/Scripts/EmpTabPointsRpt.js"></script> 

    <style>
        .MonBtn { }

        .SearchBtnCSS {
            margin-top: 40px;
            margin-bottom: 20px;
            color: blue;
        }
    </style>    

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

        <!----------------->
        <!-- Form Header -->
        <!----------------->                     
        <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 30px;" >
            <a href="/Safety/SafetyHome.aspx" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">Safety home</span>
                <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;margin-top: -10px;">Safety Pays Tabulated Points Reporting</div>
            </a>
        </section> 
    


        <section style="visibility: hidden; height: 0; width: 0;">
            <span id="hlblEID"           runat="server"/> 
            <span ID="lblEmpName"        runat="server"/>
        </section>   
        
        <div  class="ui-helper-clearfix" />
    
        <div xclass="ui-state-error ui-corner-all " style="width: 98%; clear: both; border-width: 5px; padding: 3px; margin-left: 3px;">

            <!-------------------------------------->
            <!-- 
            <!-------------------------------------->
            <section id="SearchDates">
                <div style=" width: 100%; text-align: center;">
                    <div style="display: inline-block;">
                        <div style="font-size: 1em; font-weight: bold">Start Date</div>
                        <input type="text" id="StartDate" Class="DataInputCss DateEntryCss"  runat="server" style="width: 123px;" />              
                    </div>
                    <div style="display: inline-block">
                        <div style="font-size: 1em; font-weight: bold">End Date</div>
                        <input type="text" id="EndDate" Class="DataInputCss DateEntryCss"  runat="server" style="width: 123px;" />              
                    </div>
                </div>

                <div style="height: 13px;"></div>

                 <!-- Show Days For This Week Where Time Enetered -->
                 <div style="margin-bottom: 3px; margin-top: -6px;  width: 100%; text-align: center;">
                         
                    <table style=" width: auto;  margin-left: auto; margin-right: auto; ">
                        <tr >
                            <td style="padding: 0 2px 0 10px;"><input ID="btnQ1" type="button" class="DowBtnCSS" value="Q1"  style="width: 40px; margin-top: -5px;"  /></td>
                            <td style="padding: 0 2px 0 0;"><input ID="btnQ2" type="button" class="DowBtnCSS" value="Q2"   style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input ID="btnQ3" type="button" class="DowBtnCSS" value="Q3"   style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input ID="btnQ4" type="button" class="DowBtnCSS" value="Q4"   style="width: 40px;  margin-top: -5px;" /></td>
                        </tr>
                    </table>
                 </div>

                <div style="height: 7px;"></div>

                 <!-- Show Days For This Week Where Time Enetered -->
                 <div style="margin-bottom: 3px; margin-top: -6px;  width: 100%; text-align: center;">
                         
                    <table style=" width: auto;  margin-left: auto; margin-right: auto; ">
                        <tr >
                            <td style="padding: 0 2px 0 10px;"><input id="btnJan" type="button" class="MonBtn" value="Jan"  runat="server" style="width: 40px; margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnFeb" type="button" class="MonBtn" value="Feb"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnMar" type="button" class="MonBtn" value="Mar"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnApr" type="button" class="MonBtn" value="Apr"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnMay" type="button" class="MonBtn" value="May"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnJun" type="button" class="MonBtn" value="Jun"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnJul" type="button" class="MonBtn" value="Jul"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnAug" type="button" class="MonBtn" value="Aug"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnSep" type="button" class="MonBtn" value="Sep"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnOct" type="button" class="MonBtn" value="Oct"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnNov" type="button" class="MonBtn" value="Nov"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                            <td style="padding: 0 2px 0 0;"><input id="btnDec" type="button" class="MonBtn" value="Dec"  runat="server" style="width: 40px;  margin-top: -5px;" /></td>
                        </tr>
                    </table>
                 </div>
                <div style=" width: 100%; text-align: center;">
                    <asp:Button runat="server" ID="ConsidateToExcelButton"   CssClass="SearchBtnCSS"       Text="Build Employee Report"         OnClick="Consolidated_Click" />
                </div>

            </section>
        </div>

    

        <br />
    
</asp:Content>

