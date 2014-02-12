﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="BandC_ClassComplete.aspx.cs" Inherits="Corporate_BandC_BandC_ClassComplete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <title>Job Report</title>
    
    <link href="/Styles/SubmitTimeRpt.css" rel="stylesheet"  type="text/css" />
    
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script>     
    
    <script type="text/javascript" src="/Scripts/BandC_ClassComplete.js"></script>

    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />

    <style>
         .footer_gallery { display: none; }
         div.jtable-main-container table.jtable thead th { text-align:  center;  }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <section style="visibility: hidden; height: 0; width: 0;">
        <span runat="server" id="hlblCertCode"         />
        <span runat="server" id="hlblEID"              /> 
    </section>

    <div id="FormWrapper" class="ui-widget ui-form">
                    
        <!----------------->
        <!-- Form Header -->
        <!----------------->                               
        <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
            <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Class Completion Form</span>
        </section>  
            
        <section class="ui-widget-content ui-corner-all">

            <!---------------------------->
            <!-- "Requestor ID" Section -->
            <!---------------------------->
            <div style="float: left; margin-top: -20px; ">
                <span style="padding-right: 8px;">Instructor:</span>
                <span runat="server" id="lblEmpName" />
            </div>

            <p style="height: 20px;"></p> 
                
            <!-- Class Identifier -->
            <div class="TimeRow" id="CertDiv"  runat="server" style="margin-top: 20px;">    
                <div style="float:left;  margin-right: 15px; padding-top: 2px; font-weight: bold; display:inline">Certification:</div>
                <input ID="acCertList" class="DataInputCss" style="width: 400px;" /> 

                    
                <div style="display: inline-block;">
                    <span style="display: inline-block; padding-top: 5px; margin-left: 40px; margin-right: 10px; font-weight: bold;">Date Of Class:</span>
                    <input type="text" id="txtClassDate" Class="DataInputCss DateEntryCss" style="width: 100px; float: none;" runat="server"  />   
                </div>
                </div>

            <div class="TimeRow" id="Div2"  runat="server" style="margin-top: 5px; display: none;">    
                <div style="display: inline-block;">
                    <span style="padding-top: 2px; margin-left: 0; margin-right: 10px; font-weight: bold;">Instructor:</span>
                    <input type="text" id="ddEmpIds" Class="DataInputCss DateEntryCss" style="width: 400px; float: none; margin-left: 18px;" runat="server"  />   
                </div>
                </div>
                                  
                <p style="height: 0;"/>

            <!-- Student Identifier -->
            <section class="ui-widget-header ui-corner-all" style="height: 20px; width: 85%;" >
                <span  style="font-size: 1.5em; width: 100%; line-height: 16px;  position:absolute; left: 0; margin-left: 35%; ">Student ID</span>
            </section>  
                            
            <div class="TimeRow" id="Div1"  runat="server" style="margin-top: 5px;">    
                <div style="float:left;  margin-right: 15px; padding-top: 5px; font-weight: bold; display:inline">Employee ID or DL<br /> Lookup (Scan Gun):</div>
                <input ID="txtStudent" class="DataInputCss" style="width: 90px; margin-top: 10px;" /> 

                <span runat="server" id="ServerError"   style="color: red; width: auto; font-size: 1.5em; margin-left: 20px;  font-weight: bold; padding: 5px;"/>
                
                <div style="width: 100%;  display: inline-block; margin-top: 25px;">
                    <div style="float: left; width: 30%; ">
                        <input type="button" ID="btnSubmit" value="Post All Of These Certifications" Class="SearchBtnCSS" />
                    </div>
                </div>

                </div>
                
                
            <!-- Display Open Expenses -->
            <div style="width: 790px; padding-top: 20px; margin-left: auto; margin-right: auto;">
                <div id="jTableContainer"></div>
            </div>
        </section>
            
    </div>
                     
</asp:Content>

