<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="TemplatesSearch.aspx.cs" Inherits="Corporate_BandC_TemplatesSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Badge and Certificate Templates</title>
    <link rel="stylesheet" href="../Styles/Corp.css" type="text/css"/>       
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div id="CorpHomeWrapper" style="height: 100%;">
        
        <!-- Corprate Page Header -->
        <h2 style="text-align:center; margin-top: 5px; font-size: 2.3em;" >
	        <span style="color: whitesmoke">The Following Badge or Certificate Documents Where Found</span>
        </h2>

        <div id="FrameWrapper" style="position: fixed; left: 1%; width: 97.8%; top: 110px;; height: 100%; background-color: transparent;">                   
            <iframe name="main" 
                    src="http://localhost/tiki/tiki-browse_categories.php?parentId=16&deep=off&type=file" 
                    seamless
                    width="100%" 
                    height="100%" 
                    style="overflow: hidden">
            </iframe>
        </div>
        
    </div>

</asp:Content>

