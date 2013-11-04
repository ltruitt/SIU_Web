<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SafetyQoMAdmin.aspx.cs" Inherits="Safety_SafetyQoMAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
 
    <title>Question of the Month Admin</title>
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    
    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
    
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script> 
    
    <script type="text/javascript" src="/Scripts/SafetyQomAdmin.js"></script>  
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/SafetyQomAdmin.css" />
    
    <link href="/styles/upload.css" rel="stylesheet" type="text/css" />
     <script src="/Scripts/qomUpload.js"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    
    
    <div id="FormWrapper" class="ui-widget ui-form">
                    
        <section style="visibility: hidden; height: 0; width: 0;">
            <span id="hlblEID" runat="server"></span>
            <span id="hlblUID" runat="server"></span>
        </section>
        
        <!----------------->
        <!-- Form Header -->
        <!----------------->                   
        <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
            <a href="/Safety/SafetyAdminHome.aspx" style="text-decoration: none;">
                <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">admin page</span>
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Question of the Month Definition</span>
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
            
            <div id="jTableQomList" style="max-width: 1025px; margin-bottom: 5px; margin-top: 0;"></div>
            
                <!---------------------------->
                <!-- Materials Link Section -->
                <!---------------------------->
                <div style="width: 900px; margin-top: 10px; display: inline-block; ">
                    
                    
                    <div style="font-size: 1.2em; font-weight: bold; float: left; width: 130px; padding-top: 0; padding-right: 20px; ">
                        <div style="text-align: center;">Start Date</div>
                        <input type="text" id="StartDate" Class="DataInputCss DateEntryCss"  style="width: 110px; text-align: center;" /> 
                    </div>

                    <div style="font-size: 1.2em; font-weight: bold; float: left; width: 130px; padding-right: 20px;">
                        <div style="text-align: center;">End Date</div>
                        <input type="text" id="EndDate" Class="DataInputCss DateEntryCss"  style="width: 110px; text-align: center;" /> 
                    </div>
                    

                    <div style="font-size: 1.2em; font-weight: bold; float: left; width: 155px;">
                        <div style="text-align: center; width: 130px;">Dept</div>
                        <select id="txtGroup" Class="DataInputCss DateEntryCss"  style="width: 130px; display: inline-block; height: 35px;">
                            <option value=""></option>
                            <option value="VPP">VPP</option>
                            <option value="VEST">VEST</option>
                        </select>
                    </div>

                    <div id="ContentTypeBlock" style="float: left; width: 200px; ">
                        <div style="text-align: center; font-size: 1.2em; font-weight: bold; ">Content Type</div>
                        <input id="btnHTML" type="button" value="HTML File" style="float: left;"/>
                        <input id="btnText" type="button" value="Simple Text"  style="float: right;"  /> 
                    </div>
                </div>                    
                
                <div style="clear: both"></div>

                <div style="width: 900px; margin-top: 10px; display: inline-block; ">
                    <div id="FileBlock" style="font-size: 1.2em; font-weight: bold; display: inline-block; width: 900px; padding-top: 0; padding-bottom: 5px;  ">
                        <div style="float: left; width: 100%; text-align: center;">Html Content File</div>
                        <div id="lblFile" style="float: left; font-weight: normal; width: 100%; text-align: center; padding-bottom: 10px;"></div>

                        <input type="file" id="txtFile" name="txtFile" style="clear:both;  width: 98%; height: 35px;"  onchange="fileSelected();"/>
                        <input id="btnUpload" type="button" value="Upload" onclick="startUploading()" style="color: red; font-weight: bold; width: 100%"  />     
                        
                    </div>

                    
                    
                    <div id="UploadStats">
                        <div id="progress_info">
                            <div id="progress"></div>
                            <div id="progress_percent">&nbsp;</div>
                            <div class="clear_both"></div>
                            <div>
                                <div id="speed">&nbsp;</div>
                                <div id="remaining">&nbsp;</div>
                                <div id="b_transfered">&nbsp;</div>
                                <div class="clear_both"></div>
                            </div>
                            <div id="upload_response"></div>
                        </div>
                        <div id="error">You should select valid files type!</div>
                        <div id="error2">An error occurred while uploading the file</div>
                        <div id="abort">The upload has been canceled by the user or the browser dropped the connection</div>
                        <div id="warnsize">Your file is very big. We can't accept it. Please select a smaller file</div>
                    </div>
                    
                    

                    <div id="TextBlock" style="font-size: 1.2em; font-weight: bold; display: inline-block; width: 100%;">
                        <div style="float: left; width: 300px;">Simple Question Text</div>
                        <textarea id="txtQContent" Class="DataInputCss DateEntryCss"  style="width: 95%; height: 200px;"> </textarea>
                    </div>
                    
                </div>

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
                </div>   
                        
   
        </section>   
        
      

    </div>    

</asp:Content>

