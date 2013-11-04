<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="SafetyAdminHome.aspx.cs" Inherits="Safety_SafetyAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
     <title>Safety Administration Page</title>
     <link rel="stylesheet" href="/Scripts/JqueryUI/development-bundle/themes/trontastic/jquery.ui.all.css" />
     <link rel="stylesheet" href="/styles/jquery.ui.form.css">

    <script type="text/javascript" src="/Scripts/SafetyAdminHome.js"></script>
     
    <style type="text/css"> 
        .toolbar ul { 
            display: block;
            margin: 0;
        } 
        .toolbar ul li 
        { 
            display: inline-block;
            height: 100px; 
            list-style-type: none; 
            margin: 10px; 
            vertical-align: middle;
            margin-right: 10px;
            
        }
        .toolbar ul li a { 
            display:table-cell; 
            vertical-align: middle; 
            height:100px; 
            border: none 1px black; 
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
       

        <!-- Admin Tasks -->
        <div id="SafetyAdmin" class="AdminMenu"  style="background-color: white; margin-top: 0; ">
            
            <div class="ui-widget-header ui-corner-all" >
                <div  style="text-align: center; font-size: 2em;">Safety Administration</div>
            </div>      
               

                                                        

			<ul class="menu_gallery" style="background-color: transparent; ">
                <!-- Student Classes -->
<%--                <li style="height: 170px; border: none; margin-right: 5px;">
                    <a href="SafetyPays/SafetyPays.aspx" >
						<img  style="width: 85px; height: 85px; margin: 0px; padding-top: 10px; margin-bottom: 5px; margin-left: 30px;"  alt="Student Classes" src="/Images/SI-Corp-Certifications.png" />
					</a>         
                    <a href="/Corporate/BandC/BandC_ClassComplete.aspx"  ID="A2" runat="server" style="color: black; font-size: .9em; text-decoration: underline;">Record Students Completing a Class</a>
                    <hr/>                   
                    <a href="/Safety/Training/AdminClass.aspx"  ID="A1" runat="server" style="color: black; font-size: .9em; text-decoration: underline;">Add / Edit Classes</a>
                    <a href="/Safety/Training/UserClass.aspx"  ID="A3" runat="server" style="color: black; font-size: .9em; text-decoration: underline;">Classes / User View</a>
                </li> --%>


                <!-- Safety Pays -->
                <li style="height: 190px;  width: 150px; border: none; margin-right: 5px;">
                        <a href="/Safety/SafetyPays/SubmitSafetyPays.aspx" >
						    <img  style="width: 150px; height: 75px; margin: 0; padding-top: 10px; margin-bottom: 5px;"  alt="Safety Pays" src="/Images/SI-SafetyPays.png" />
					    </a>         

                        <a href="/Safety/SafetyPays/PointsAdmin.aspx">
                            <div style="color: black; font-size: .9em; text-align: center; width: 150px;" id="Span1">Record Points</div>
                        </a>

                        <a href="/Safety/SafetyPays/SubmitSafetyPays.aspx" >
                            <div style="color: black; font-size: 1em; text-align: center; width: 150px;" id="Span2">Submit Report</div>
                        </a>      

                        <a href="/Safety/SafetyPays/SafetyPaysSubmitted.aspx">
                            <div style="color: black; font-size: .9em; text-align: center; width: 150px;" id="SafetyPaysTotalWaiting">? New Submissions</div>
                        </a>

                        <a href="/Safety/SafetyPays/SafetyPaysWorking.aspx?s=non">
                            <div style="color: black; font-size: .9em;" id="SafetyPaysTotalNotTask"> Need Tasks</div>
                        </a>

                        <a href="/Safety/SafetyPays/SafetyPaysWorking.aspx?s=tsk">
                            <div style="color: black; font-size: .9em;" id="SafetyPaysTotalLateTask">Late Tasks</div>
                        </a>

                        <a href="/Safety/SafetyPays/SafetyPaysWorking.aspx?s=sta">
                            <div style="color: black; font-size: .9em;" id="SafetyPaysTotalLateStatus">Late Status</div>
                        </a>

                        <a href="/Safety/SafetyPays/SafetyPaysWorking.aspx?s=clo">
                            <div style="color: black; font-size: .9em;" id="SafetyPaysTotalCloseReady">? Ready to Close</div>
                        </a>
                </li>     
                
                
                <li style="height: 190px;  width: 150px; border: none; margin-right: 5px;">
                        <a href="/Safety/SafetyPays/SafetyQoMAdmin.aspx" >
						    <img src="/Images/QOM.png" style="width: 100px; height: 85px;  padding-bottom: 8px; margin-top: 20px;" alt="QOM">
					    </a>         

                        <a href="/Safety/SafetyPays/SafetyQoMAdmin.aspx">
                            <div style="color: black; font-size: .9em; text-align: center; width: 150px;" id="Div2">Record Q.O.M</div>
                        </a>

                </li> 
                

            </ul>




        </div>

</asp:Content>

