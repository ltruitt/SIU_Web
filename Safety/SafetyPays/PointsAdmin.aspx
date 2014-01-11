<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="PointsAdmin.aspx.cs" Inherits="Safety_SafetyPays_PointsAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>ELO Time Entry</title>

    <script type="text/javascript" src="/Scripts/PointsAdmin.js?0000"></script>  
    <link rel="stylesheet" type="text/css" media="screen" href="/Styles/DeskTop-Forms.css" />

    <!-- Include one of jTable styles. -->
    <link href="/Scripts/jtable.2.2.1/themes/metro/blue/jtable.min.css" rel="stylesheet" type="text/css" />
 
    <!-- Include jTable script file. -->
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/jquery.jtable.js">                             </script>        
    <script type="text/javascript" src="/Scripts/jtable.2.2.1/extensions/jquery.jtable.aspnetpagemethods.js"></script>  

<style>

    .ValidationError {
        color:              red;
        -ms-border-radius:      7px;
        border-radius:      7px; 
    border:             2px solid red;
    }

    .ValidationSuccess {
        -ms-border-radius: 7px;
        border-radius: 7px;
        border: 2px solid green;
    }

    .focused {
        background: #FFCC66;
    }


             

</style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <!-- Hidden Labels Keeping Selected Charge Account From DD Selections -->
    <section style="visibility: hidden; height: 0; width: 0;">
        <span runat="server" id="hlblEID"                      /> 
        <span runat="server" id="hlblPointsType"               /> 
        <span runat="server" id="hlblUID"               />
    </section>

    <div >
        <div id="FormWrapper" class="ui-widget ui-form">        
            <!----------------->
            <!-- Form Header -->
            <!----------------->                   
            <section class="ui-widget-header ui-corner-all" style="height: 45px;" >
                <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Safety Pays Points Administration</span>
            </section>  
    
            <section class="ui-widget-content ui-corner-all">
                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <div style="margin-left: -15px;  width: auto;">
                    
                    <div style="margin-top: -20px;">
                        <label style="float: none; padding: 0; width:auto; margin-top: -20px;"  runat="server" ID="lblEmpName"></label>
                    </div>
                </div>

                <p style="height: 25px;"></p> 

                <div id="datacollection" style="margin-top: 5px; width: 600px; float: left;" >
                    <span style="float:left; margin-right: 20px; width: 150px;  font-weight: bold; display:inline">Employee ID:</span>
                    <input ID="ddEmpIds" class="DataInputCss cursor"  style="width: 400px;" />  
                           
                    <p style="height: 15px;"></p> 

                    <span style="float:left; margin-right: 20px; width: 150px;  font-weight: bold; display:inline">Event Date:</span>
                    <input type="text" id="txtEntryDate" Class="DataInputCss DateEntryCss"  runat="server" style="width: 100px; color: black;" />   

                    <p style="height: 15px;"></p> 

                    <span style="float:left; margin-right: 20px; width: 150px;  font-weight: bold; display:inline">Points For:</span>
                    <input ID="ddTypes" class="DataInputCss"  style="width: 400px;" />         

                    <p style="height: 15px;"></p> 

                    <span style="float:left; margin-right: 20px; width: 150px;  font-weight: bold; display:inline">Number of Points:</span>
                    <input ID="txtNumPts" class="DataInputCss"  style="width: 80px;" />  

                    <p style="height: 15px;"></p> 

                </div>

                <div style="margin-top: 5px; width: 425px; float: left; background-color: yellow;" >
                    <div id="EmpPointsRpt"></div>
                </div>

                <div style="font-size:x-large;"  >
                    <span runat="server" id="lblErr" Class="errorTextCss"  ></span>
                    <span runat="server" id="lblErrServer" Class="errorTextCss"  ></span>
                </div>

                <%--------------------%>                    
                <%-- Submit Buttons --%>                    
                <%--------------------%>
                <div style="width: 100%;  display: inline-block; margin-top: 25px;">
                    <div style="float: left; width: 30%; ">
                        <input type="button" ID="btnSubmit" value="Submit" class="SearchBtnCSS" />
                    </div>

                    <div style="float: left; width: 30%;">
                        <input type="button" id="btnClear" value="Clear" class="SearchBtnCSS" />
                    </div>
                </div>

            </section>
        </div>


    </div>
</asp:Content>

