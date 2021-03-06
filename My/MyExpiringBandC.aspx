﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="MyExpiringBandC.aspx.cs" Inherits="My_ExpiringBandC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Expriring Badges and Certification</title>
       
    <script type="text/javascript" src="/Scripts/MyExpiringBandC.js"></script> 

        <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 

    <style>
        .markedDay a.ui-state-default {
            background: green !important;
        }

        .markedDay a.ui-state-hover {
            background: red !important;
        }
        
        .jTableTDcj { text-align: center; }        
        .jTableTDlj { text-align: left; }        
    </style>  
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="SuprArea" style="margin-top: 5px;" runat="server"  >
        <span style="float:left; margin-right: 20px; width: auto;  font-weight: bold; display:inline">Change Employee:</span>
        <input ID="ddEmpIds" class="DataInputCss" runat="server"   style="width: 400px;" />         
    </div>
    

    <div style="background-color: #45473f; ">
        <div id="FormWrapper" class="ui-widget ui-form">
        
            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblEID"           runat="server"/>      
            </section>            
            

            <section class="ui-widget-header ui-corner-all"  >
                <div  style="text-align: center; font-size: 1.8em;">Expiring Badges and Certifications</div>
            </section>    
            
    
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Viewer ID" Section -->
                <!---------------------------->
                <div style="float: left; ">
                    <label style="float: left; padding: 0; width:auto; margin-top: -20px;"  runat="server" ID="lblEmpName"></label><br/>
                </div>

                <p style="height: 5px;"></p> 
                                 
                 <div id="jTableContainer"></div>

             </section>                 

        </div>
    </div>      
</asp:Content>

