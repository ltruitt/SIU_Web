<%@ Page Title="" Language="C#" MasterPageFile="/Phone/PhoneMaster.master" AutoEventWireup="true" CodeFile="MyVehMileage.aspx.cs" Inherits="My_MyVehMileage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Vehicle Mileage</title>
      
    <script type="text/javascript" src="/Scripts/MyVehicleMileage.js"></script> 

        <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 

    <style>     
        .jTableTD_ChkBox {
            text-align: center;
            font-size: 2em;
        }     
    </style>       
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="SuprArea" style="margin-top: 5px;" runat="server" >
        <span style="float:left; margin-right: 20px; width: auto;  font-weight: bold; display:inline">Change Employee:</span>
        <input ID="ddEmpIds" class="DataInputCss" runat="server"  style="width: 400px;" />         
    </div>

    <div id="FormWrapper" class="ui-widget ui-form">
        
        <section style="visibility: hidden; height: 0; width: 0;">
            <span id="hlblEID"           runat="server"/>      
        </section>            
            

        <section class="ui-widget-header ui-corner-all"  >

            
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/Phone/ELO/MainMenu.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">elo menu</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Vehicle Mileage</span>
                </a>
            </section> 
                                   
               
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Viewer ID" Section -->
                <!---------------------------->
<%--                    <div style="float: left; ">
                    <label style="float: left; padding: 0; width:auto; margin-top: -20px;"  runat="server" ID="lblEmpName"></label><br/>
                </div>--%>
                    
                <div style="float: left; margin-top: -20px; width: 100%;">
                    <label style="float: none; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label><br/>
                    <span ID="imgReport" runat="server"  style="float: right; margin-left: 7px; "  >
                        <a href="/Phone/ELO/VehMileage.aspx" style="color:white; font-weight: bold;">Enter Miles</a>
                    </span> 
                </div>

                <p style="height: 5px;"></p> 
                                 
                    <div id="jTableContainer"></div>
         
            </section>                                   
        </section>
    </div>
        
</asp:Content>

