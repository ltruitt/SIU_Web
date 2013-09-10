<%@ Page Title="" Language="C#" MasterPageFile="/Phone/PhoneMaster.master" AutoEventWireup="true" CodeFile="HomePage.aspx.cs" Inherits="Phone_HomePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <title>Phone Home page</title>


    <style type="text/css"> 
        .AdminMenu ul li {
            margin-left: auto; 
            margin-right: auto; 
            width: 100%; 
            background-color: red;        
        }
    </style>
       
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    

        <div id="FormWrapper" class="ui-widget ui-form">
<!----------------->
<!-- Form Header -->
<!----------------->           
            <div class="ui-widget-header ui-corner-all" >
                <div  style="text-align: center; font-size: 2em; height: 60px;">Home Menu</div>
            </div>
            

            <div class="ui-widget-content ui-corner-top AdminMenu" >
            
                <!---------------------------->
                <!-- "Requestor ID" Section -->
                <!---------------------------->
                <label runat="server" ID="lblEmpName" ></label>

                <p style="height: 10px;"></p> 
                
                

                <div  class="ui-helper-clearfix" >
                    <ul >
				        <li class="l3"><a href="/Phone/ELO/MainMenu.aspx" >ELO</a></li>
                        <li class="l3"><a href="/Phone/LibDocPane.aspx" >Library</a></li>
                    </ul>
                </div>
            </div>
        </div>

    

    
   
	<%--<li class="l2"><a href="Forms/Jobs.aspx" id="PhoneJobsIcon">JOBS</a></li>--%>
                

	<%--<li class="l2"><a href="/Phone/ELO/MainMenu.aspx">ELO</a></li>--%>
    <%--<li class="l2"><a href="/Phone/FAQ.aspx">FAQ</a></li>--%>
                

    <%--<li class="l2"><a href="/Forms/SafetyPays.aspx">SAFETY PAYS</a></li>--%>
    <%--<li class="l2"><a href="/Forms/SafetyQoMResponse.aspx">SAFETY Q.O.M</a></li>--%>
    <%--<li class="l5"><a href="UsefulLinks/UsefulLinks.aspx">USEFUL LINKS</a></li>--%>
    <%--<li class="l5"><a href="UsefulLinks/QuickLinks.aspx">QUICK LINKS</a></li>--%>

      
      


	  <!-- Footer -->
	  <section id="footer" class="footer_gallery" >		
            <ul class="footer_gallery">
            </ul>
      </section>
      
    <div style="position: absolute; bottom: 0; right: 10px; z-index: 100;">
        <a href="javascript:  window.location = window.location.protocol + '//' + window.location.hostname + '/' + '/homepage.aspx?force';">
            <img src="/Phone/Images/desktop1.png" alt="Desktop view" style="margin: 0; border: none; height: 60px;"/>
        </a>
    </div>
    
</asp:Content>

