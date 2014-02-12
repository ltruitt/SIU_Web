<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="BandC_Support_Links.aspx.cs" Inherits="Corporate_BandC_BandC_Support_Links" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <!-- Page Layout -->
    <title>Badges and Certificates Support Links</title>
    <link rel="stylesheet" href="/Styles/Corp.css" type="text/css"/>    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="CorpHomeWrapper">
        
        <!-- Corprate Page Header -->
        <h2 style="text-align:center; margin-top: 20px; font-size: 2.3em;" >
	        <span style="color: gray">Use The Links On This Page As Shortcuts For Badge And Certification Research and Recording</span>
        </h2>

	    <div style="height: 20px;"></div>                    

        <nav id="B and C Links" style="margin-left: auto; margin-right: auto; width: 100%; text-align: center;">
            <div class="content2">
                <ul style="color: gray; font-size: 1.5em;">

                    <li>
			            <hr style="width: 100%"/>
                        <br>
		            </li>

                    <li>
                        <a href="/Install/QualificationsInvestigator/reportviewer/ReportViewer.exe"><b>INSTALL Step 1</b> Of Qualifications Invesigator</a>
                        <br>
                        <span style="font-size: small;">Install Report Viewing Plug In</span> 
                        <br><br>
                    </li>

                    <li>
                        <a href="/Install/QualificationsInvestigator/QualificationsInvestigation.application"><b>INSTALL Step 2</b> Of Qualifications Invesigator</a>
                        <br>
                        <span style="font-size: small;">  Installs Microsoft Application Framework If Necessary<br/>
                                                            Installs or Updates the Qualifications Investigator Application
                        </span>
                        <br><br>                                        
                    </li>

                    <li>
			            <hr style="width: 100%"/>
			            <br>
		            </li>
                    
                    
                    
                    <li>
                        <a href="/Install/QualificationsInvestigator/QualificationsInvestigation.application"><b>RUN / UPDATE </b>Qualifications Invesigator</a>
                        <br>
                        <span style="font-size: small;">Checks For Updates and Then Runs The Qualifications Investigator</span>
                        <br><br>
                    </li>


                    <li>                        
			            <hr style="width: 100%"/>
			            <br>
		            </li>



                    <li>
                        <a href="/install/Cert/QualCodesLookup.xlsx">List Of Valid Badge And <b>Certification Codes</b></a> 
                        <br>
                        <span style="font-size: small;">  A Spreadsheet that Looks Up Current Badge And Certification Codes
                                                            The First Time You Open This Document: You Must "ENABLE" Content
                                                            And You Must "Trust" This Document </span>
                        <br><br>
                    </li>

                    <li>                        
			            <hr style="width: 100%"/>
			            <br>
		            </li>


                    <div id="BncAdmin" runat="server">
                        <li>
                            <a href="/install/Cert/WordTemplates"><b>Certification Templates</b></a> 
                            <br>
                            <span style="font-size: small;">Database Connected Certification Templates</span>
                            <br><br>
                        </li>
                    
                        <li>
                            <a href="/Corporate/BandC/BandC_ClassComplete.aspx"><b>Record Cedrtification Without Document</b></a> 
                            <br>
                            <span style="font-size: small;">Record Cedrtification Without Document</span>
                            <br><br>
                        </li>
                        
                        <li>                        
			                <hr style="width: 100%"/>
			                <br>
		                </li>
                    </div>
               
                    
            </ul>
                
 

        </div>
            
    </nav>
</div>
</asp:Content>

