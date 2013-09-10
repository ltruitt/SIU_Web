<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="BandC_Support_Links.aspx.cs" Inherits="Corporate_BandC_BandC_Support_Links" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <!-- Page Layout -->
    <title>Badges and Certificates Support Links</title>
    <link rel="stylesheet" href="../Styles/Corp.css" type="text/css"/>    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="CorpHomeWrapper">
        
        <!-- Corprate Page Header -->
        <h2 style="text-align:center; margin-top: 5px; font-size: 2.3em;" >
	        <span style="color: gray">Use The Links On This Page As Shortcuts For Badge And Certification Research and Recording</span>
        </h2>
                    
        <nav id="B and C Links" style="margin-left: auto; margin-right: auto;">
            <div class="content2" style="height:750px; width:550px; background-color: transparent; top: 0;">
                <ul style="color: gray; font-size: 1.5em;">
                    <li>
<!--                    <a href="http://localhost/tiki/tiki-download_file.php?fileId=17" target="_blank">(Draft) Manual</a> -->
                        <a href="BandC_Manual.aspx" target="_blank">(Draft) Manual</a> 
                        <br>
                        <span style="font-size: small;">Read up on how the Badges and Certifications process works </span>
                        <br><br>
                    </li>
                    
                    <li>
                        <a href="/Install/reportviewer/ReportViewer.exe"><b>INSTALL Step 1</b> Of Qualifications Invesigator</a>
                        <br>
                        <span style="font-size: small;">Install Report Viewing Plug In</span> 
                    </li>

                    <li>
                        <a href="/Install/QualificationsInvestigator/setup.exe"><b>INSTALL Step 2</b> Of Qualifications Invesigator</a>
                        <br>
                        <span style="font-size: small;">  Installs Microsoft Application Framework If Necessary<br/>
                                                          Installs or Updates the Qualifications Investigator Application
                        </span>
                        <br><br>
                    </li>



                    <li>
                        <a href="TemplatesSearch.aspx">Badge And Certification <b>Templates</b></a>
                        <br>
                        <span style="font-size: small;">Documents Used To Generate Badge And Certification Documents</span> 
                    </li>

                    <li>
                        <a href="http://localhost/tiki/tiki-download_file.php?fileId=18">List Of Valid Badge And <b>Certification Codes</b></a> 
                        <br>
                        <span style="font-size: small;">  A Spreadsheet that Looks Up Current Badge And Certification Codes
                                                            The First Time You Open This Document: You Must "ENABLE" Content
                                                            And You Must "Trust" This Document </span>
                    </li>



                    <li>
                            <a href="file://shermco/dfs/Public/NavisionScan/Qualifications">The <b>Scan</b> Processing Folders</a>
                            <br>
                            <span style="font-size: small;">Use This Link For Researching Faulty Documents</span>
                            <br><br>
                    </li>
                    
                    
                    
                    <li>
                        <a href="/Install/QualificationsInvestigator/setup.exe"><b>RUN / UPDATE </b>Qualifications Invesigator</a>
                        <br>
                        <span style="font-size: small;">Checks For Updates and Then Runs The Qualifications Investigator</span>
                        <br>
                    </li>
                    
                    

                </ul>
            </div>
            
        </nav>
    </div>
</asp:Content>

