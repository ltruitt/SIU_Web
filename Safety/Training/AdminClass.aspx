﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="AdminClass.aspx.cs" Inherits="Safety_Training_AdminClass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Safety Classes Admin</title>
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    
    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 
    
    <link rel="stylesheet" href="/Styles/Popup.css" type="text/css" /> 
    <link rel="stylesheet" type="text/css" href="/Styles/jquery.timeentry.css"> 
    <script type="text/javascript" src="/Scripts/TimePicker/jquery.timeentry.min.js"></script>

    <script type="text/javascript" src="/Scripts/AdminClass.js"></script>     
    
<style>
    .ui-autocomplete {
        z-index: 99999;
    }

    .ValidationError {
        -ms-border-radius:      7px;
        border-radius:          7px; 
        border:                 2px solid red;
    }

</style>
</asp:Content>





<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="FormWrapper" class="ui-widget ui-form">
                    
        <section style="visibility: hidden; height: 0; width: 0;">
            <span id="hlblEID" runat="server"></span>
            <span  id="hlblInstID"></span>
            <span  id="hlblPointsType"></span>
                     
        </section>            
            
        <!----------------->
        <!-- Form Header -->
        <!----------------->                   
        <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
            <a href="/Safety/SafetyAdminHome.aspx" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">admin page</span>
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Class Administration</span>
            </a>
        </section>

        <section class="ui-widget-content ui-corner-all">

            <!---------------------------->
            <!-- "Viewer ID" Section -->
            <!---------------------------->
            <div style="float: left; margin-top: -20px; width: 100%;">
                <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label><br/>
            </div>
            
            <div style="height: 40px;">
                <span  id="lblErrServer" Class="errorTextCss"  ></span>
            </div>
            
       <%--     <asp:FileUpload ID="FileUpload1" runat="server" />--%>
<table>
<tr>
<td>            
                <div>
                    <div style="width: 350px; height: 55px; display: inline-block;">
                        <div style="font-size: 1.2em; font-weight: bold">Class Topic</div>
                        <input type="text" id="txtTopic" Class="DataInputCss DateEntryCss" style="width: 300px;" /> 
                    </div>
                    <div style="width: 360px; display: inline-block;">
                        <div style="font-size: 1.2em; font-weight: bold">Class Type</div>
                        <input type="text" id="txtType" Class="DataInputCss DateEntryCss" style="width: 300px;" /> 
                    </div>
                    <div style="display: inline-block;">
                        <div style="font-size: 1.2em; font-weight: bold">Safety Points</div>
                        <input type="text" id="txtPts" Class="DataInputCss DateEntryCss"  style="width: 30px; margin-left: 30px; text-align: center;" /> 
                    </div>                    

                </div>
            
                        
                <div style="width: 350px; height: 55px;">
                    <div style="font-size: 1.2em; font-weight: bold">Class Description</div>
                    <input type="text" id="txtDesc" Class="DataInputCss DateEntryCss"  style="width: 655px;" /> 
                </div>
            

                <div style="width: 850px;">
                    <div style="width: 700px; height: 55px; display: inline-block;">
                        <div style="font-size: 1.2em; font-weight: bold">Instructor</div>
                        <input type="text" id="txtInst" Class="DataInputCss DateEntryCss"  style="width: 300px;" /> 
                    </div>
                                               
                    <div style="display: inline-block;">
                        <div style="font-size: 1.2em; font-weight: bold">Prerequisite ID</div>
                        <input type="text" id="txtReq" Class="DataInputCss DateEntryCss"  style="width: 50px; margin-left: 40px;  text-align: center;" /> 
                    </div>  
                </div>
            
                <div  class="ui-helper-clearfix" />
            
            

                <!---------------------------->
                <!-- Materials Link Section -->
                <!---------------------------->
                <div class="ui-state-error ui-corner-all " style="width: 820px; clear: both; margin-top: 10px;">
                    <div style="font-size: 1.2em; font-weight: bold; display: inline-block; width: 100%; padding-top: 10px;">
                        <div style="float: left; width: 120px;">Video Name</div>
                        <input type="text" id="txtVideo" Class="DataInputCss"  style=" margin-left: 10px; width: 670px; margin-top: -1px; font-size: .8em;" placeholder="DirName / FileName" /> 
                    </div>
                    
                    <div style="font-size: 1.2em; font-weight: bold; display: inline-block; width: 100%; padding-top: 10px;">
                        <div style="float: left; width: 120px;">Quiz Name</div>
                        <input type="text" id="txtQuiz" Class="DataInputCss"  style=" margin-left: 10px; width: 670px; margin-top: -1px; font-size: .8em;" placeholder="Extract the Title Value From ProProf Link..."/> 
                    </div>
                </div>
            


                <!-------------------------------------->
                <!-- Report Type Section -->
                <!-------------------------------------->          
                <div class="ui-state-error ui-corner-all " style="clear: both; width: 820px;  display: inline-block; margin-top: 25px;">
                    <div>
                        
                        <div style="font-size: 1.2em; font-weight: bold; background-color: burlywood;color: brown;"> For Instructor Lead Class</div>
                        
                        <div style="font-size: 1.2em; font-weight: bold; display: inline-block; width: 170px; padding-top: 10px; padding-bottom: 5px; ">
                            <div style="float: left;">Class Date</div>
                            <input type="text" id="txtDate" Class="DataInputCss DateEntryCss"  style="width: 110px; text-align: center;" /> 
                        </div>

                        <div style="font-size: 1.2em; font-weight: bold; display: inline-block; width: 140px; padding-top: 10px; padding-bottom: 5px; ">
                            <div style="float: left;">Start Time</div>
                            <input type="text" id="txtStart" Class="DataInputCss"  style="width: 120px;" /> 
                        </div>
                        
                        <div style="font-size: 1.2em; font-weight: bold; display: inline-block; width: 140px; padding-top: 10px; padding-bottom: 5px; ">
                            <div style="float: left;">End Time</div>
                            <input type="text" id="txtEnd" Class="DataInputCss"  style="width: 120px;" /> 
                        </div>
                        
                        <div style="font-size: 1.2em; font-weight: bold; display: inline-block; width: 320px; padding-top: 10px; padding-bottom: 5px; height: 55px;">
                            <div style="float: left;">Location</div>
                            <select id="txtLoc" Class="DataInputCss"  style="width: 300px; float: left;">
                               <option value=""></option>
                               <option value="Training">Training</option>
                               <option value="Shermco U">Shermco YOU</option>
                            </select>
                        </div>
  
                    </div>  
                </div>
</td>
<td style="border: 2px solid grey; vertical-align: top;">
                    <div>
                        <input type="button" ID="btnAddCert" value="+ Add Qualification" Class="SearchBtnCSS" />
                    </div>
                    <br/>
                    <div id="jTableQual" style="max-width: 165px; margin-top: 10px;"></div>
</td>
</tr>
</table>   



            
            
                <%--------------------%>                    
                <%-- Submit Buttons --%>                    
                <%--------------------%>
                <div style="width: 100%;  display: inline-block; margin-top: 25px; max-width: 1025px;">
                    <div style="float: left; width: 250px; ">
                        <input type="button" ID="btnSubmit" value="Submit" Class="SearchBtnCSS" style="width: 75px; color: green;" />
                    </div>
                    
                    <div style="float: left;  ">
                        <input type="button" ID="btnClear" value="Clear" Class="SearchBtnCSS" style="width: 75px;"  />
                    </div>
                    
                    <div style="float: right; margin-top: 30px; ">
                        <span style="float: left; font-size: 1.3em;">Starting Date To Load</span>
                        <input type="text" ID="txtLoadDate" Class="DataInputCss"  style="width: 90px; margin-left: 20px; text-align: center;  "   />
                    </div>
                </div>           
            
            <div id="jTableClass" style="max-width: 1025px; margin-bottom: 5px; margin-top: 0;"></div>
        </section>
            
        

    </div>      
    
    
    
    

    <!-- Add Meeting Certification Popup -->
    <div id="popup-box" class="popup">
        <a href="#" class="close"><img src="/Images/Delete.png" class="btn_close" title="Close Window" alt="Close" />
        </a>
        <span style="color: white; position: absolute; top:  15px; margin-left: 20px; font-weight: bold;">Add Certification Award To Class</span>
        

        <div  class="popupDiv" >
            
            <div style="width: 350px; height: 200px; margin-top: 20px;">
                <div style="font-size: 1.2em; font-weight: bold; color: white;">Qualification Code</div>
                <input type="text" id="acCertList" Class="DataInputCss DateEntryCss"  style="width: 350px;" /> 
            </div>

            <div style="width: 130px; ">
                <input type="button" ID="popupBtnOK" Class="SearchBtnCSS" style="width: 100px; color: red;" value="OK"/>
            </div>
        </div>
    </div>                
</asp:Content>

