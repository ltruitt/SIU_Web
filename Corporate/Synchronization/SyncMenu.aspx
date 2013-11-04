<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SyncMenu.aspx.cs" Inherits="Corporate_Synchronization_SyncMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <title>Badges and Certificates Support Links</title>
    <link rel="stylesheet" href="/Styles/SyncMenu.css" type="text/css"/> 
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />
    
    <script type="text/javascript" src="/Scripts/SyncMenu.js"></script> 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="SuprArea" style="margin-top: 5px;" runat="server"  >
        <span style="float:left; margin-right: 20px; width: auto;  font-weight: bold; display:inline">Change Employee:</span>
        <input ID="ddEmpIds" class="DataInputCss" runat="server"   style="width: 400px;" />         
    </div>

    <div style="background-color: #45473f; ">
        <div id="FormWrapper" class="ui-widget ui-form">
        
            <section style="visibility: hidden; height: 0; width: 0;">
                <span id="hlblEID"            runat="server"/>  
            </section>            
            
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <a href="/Corporate/CorpHome.aspx" style="text-decoration: none;">
                    <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">Corp home</span>
                    <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Document Synchroinization</span>
                </a>
            </section>
            
    
            <section class="ui-widget-content ui-corner-all">

                <!---------------------------->
                <!-- "Viewer ID" Section -->
                <!---------------------------->
                <div style="float: left; margin-top: -20px; width: 100%;">
                    <label style="float: left; padding: 0; width:auto; "  runat="server" ID="lblEmpName"></label><br/>
                </div>

                <p style="height: 5px;"></p>  
                
                <fieldset class="Dev" >
                    <legend >DFS to Test</legend>
                
                    <ul>
                        <li>EHS

                            <ul>
                                <li>
                                    <a  href="#Dev:EHS:Documents">Documents</a> 
                                </li>
                    
                                <li>
                                    <a  href="#Dev:EHS:Forms">Form</a>
                                </li>
                   
                            </ul>
                        </li>
                        
                        <li>ESD

                            <ul style="font-size: 1em; margin-left: 20px;">
                                <li>
                                    <a href="#Dev:ESD:Documents">Documents</a> 
                                </li>
                    
                                <li>
                                    <a href="#Dev:ESD:Forms">Form</a>
                                </li>
                   
                            </ul>
                        </li>
                    </ul>
                </fieldset>
                
                
                
                

                <fieldset class="Prod">
                    <legend>Test to Prod</legend>
                
                    <ul>
                        <li>EHS

                            <ul>
                                <li>
                                    <a href="#Prod:EHS:Documents">Documents</a> 
                                </li>
                    
                                <li>
                                    <a href="#Prod:EHS:Forms">Form</a>
                                </li>
                   
                            </ul>
                        </li>
                        
                        <li>ESD

                            <ul style="font-size: 1em; margin-left: 20px;">
                                <li>
                                    <a href="#Prod:ESD:Forms">Documents</a> 
                                </li>
                    
                                <li>
                                    <a href="#Prod:ESD:Forms">Form</a>
                                </li>
                   
                            </ul>
                        </li>
                    </ul>

                </fieldset>
                
                <div style="clear: both;"></div>
            </section>
        </div>
    </div>   
</asp:Content>

