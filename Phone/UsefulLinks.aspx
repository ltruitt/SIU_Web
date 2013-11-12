<%@ Page Title="" Language="C#" MasterPageFile="/Phone/PhoneMaster.master" AutoEventWireup="true" CodeBehind="UsefulLinks.aspx.cs" Inherits="ShermcoYou.Scripts.UsefullLinks" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Useful Shermco Contacts</title>
    
    <link rel="stylesheet" href="/Phone/Styles/Phone.css"/>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.9.1/jquery-ui.min.js"></script>    
    <script type="text/javascript" src="/Scripts/UsefulLInks.js"></script>  
    
    <style>
        .ui-tabs .ui-tabs-nav {
            background-image: none;
        }            
        .ui-form .ui-widget-content {
            padding-left: 3px;
            padding-right: 3px;   
        }

        a {
            font-weight: bold;
            color: #1d4596 !important;
            
            text-decoration: underline;
        }

        a:hover {
            font-weight: bold;
            font-style:italic;
        }
    </style>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <div style="background-color: #45473f; ">
        <div id="FormWrapper" class="ui-widget ui-form">
                                               

            <div id="tabs">                   

                <!----------------->
                <!-- Form Header -->
                <!----------------->   
                <div style="margin-top: -17px; margin-bottom: 40px;">
                    <a href="/Phone/HomePage.aspx" style="text-decoration: none;  color: white !important;">
                        <span style="position: relative; left: 3px; vertical-align: top; font-size: .8em; line-height: 10px;">home</span>
                        <span  style="text-align: center; font-size: 2em; width: 100%;  position:absolute; left: 0; margin-left: auto; margin-right: auto;">Useful Links</span>
                    </a>
                 </div>                
                 


                <%--Tabs--%>
                <ul>
                    <li><a style="color: white !important;" href="#HR">Human Resources</a></li>
                    <li><a style="color: white !important;" href="#Risk">Risk</a></li>
                    <li><a style="color: white !important;" href="#Corporate">Corporate</a></li>
                    <li><a style="color: white !important;" href="#IT">IT and Development</a></li>
                    <li><a style="color: white !important;" href="#Facilities">Facilities</a></li>
                    <li><a style="color: white !important;" href="#Procurement">Procurement</a></li>
                </ul>    
                
                <div id="HR">

			        <div style="text-align: center;">
				        <a href="mailto:hr@shermco.com">
				            Email: (hr@shermco.com)
                        </a>
			            <div class="contact" style="text-align: center;">
				            Contact Krista Hunter at &nbsp; <a href="tel:972-793-5540,10234">972.793.5523</a><br/>ext. 10234
			            </div>
			        </div>

			        <br/>

			        <div style="padding-left: 20px;">
			            <ul style='font: 12pt/normal "Arial", "sans-serif";  list-style:square; color: gainsboro;'>
				            <li>Hiring / recruiting need / status </li>
				            <li>Employee insurance &amp; retirement benefits (medical, dental, life, 401k, disability)  </li>
				            <li>Employee performance reviews and warning notices </li>
				            <li>Employee termination/separation questions </li>
				            <li>Employee personal information changes (address, phone, email)</li>
				            <li>Employee life event (marriage, divorce, birth of a child, etc.) </li>																
				            <li>Employee personal concerns </li>
				            <li>Employee leaves of absence</li>
				            <li>Requests for flowers, gifts, etc. for employee due to bereavement, illness, etc</li>
				            <li>Notification of any pending international work and employee traveling internationally</li>
			            </ul>
                    </div>

                </div>
                
                
                <div id="Risk">

			        <div style="text-align: center;">
				        <a href="mailto:risk@shermco.com">
				            Email: (risk@shermco.com)
                        </a>
<%--			            <div class="contact" style="text-align: center;">
				            Contact Krista Hunter at &nbsp; <a href="tel:972-793-5523">972.793.5523</a><br/>ext. 10234
			            </div>--%>
			        </div>

			        <br/>

			        <div style="padding-left: 20px;">
			            <ul style='font: 12pt/normal "Arial", "sans-serif";  list-style:square; color: gainsboro;'>
				        <li>Employee badges/certifications &amp; customer site access (copies/requests /questions)</li>
				        <li>Vehicle &amp; property damage</li>
				        <li>Accident/incident reports</li>
				        <li>Request to review and collect signatures on customer contracts, PO's &amp; T&amp;C ' s</li>
				        <li>Request for certificate of insurance</li>
				        <li>Request to complete customer questionnaires</li>
				        <li>Bid/performance bond requests</li>
				        <li>CCIP/OCIP/ROCIP/etc.</li>
				        <li>State licensing</li>
				        <li>Questions about property/casualty insurance</li>
				        <li>Notification of pending/new international work and employee traveling internationally</li>
			            </ul>
                    </div>

                </div>                
                
                
                <div id="Corporate">
			        <div style="text-align: center;">
				        <a href="mailto:corporateservices@shermco.com">
				            Email: (corporateservices@shermco.com)
                        </a>
			            <div class="contact" style="text-align: center;">
				            Contact Susan Strong at &nbsp; <a href="tel:972-793-5540,10196">972.793.5523</a><br/>ext. 10196
			            </div>
			        </div>

			        <br/>

			        <div style="padding-left: 20px;">
			            <ul style='font: 12pt/normal "Arial", "sans-serif";  list-style:square; color: gainsboro;'>
				        <li>Out of Office Notification (email informing the receptionist you will be out of the office, dates you will be out and how your calls should be transferred) </li>
				        <li>Scanner/copier problems</li>
				        <li>LOTO tag requests</li>
				        <li>Job report requests</li>
				        <li>Cell phone issues</li>
				        <li>New cell phone requests (with supervisor approval)</li>																
				        <li>Tether requests</li>
				        <li>New desk phone extension request</li>
				        <li>International phone setup when traveling outside the US on ompany business</li>
				        <li>Soda fountain/snack machine</li>
			            </ul>
                    </div>

                </div>                
                

                <div id="IT">
			        <div style="text-align: center;">
				        <a href="mailto:support@shermco.com">
				            Email: (support@shermco.com)
                        </a>
                        

			            <div class="contact" style="text-align: center; padding-top: 5px; text-align: center;">
                            <div style="padding-top: 8px;">For Technical Support call:<br/><a href="tel:972-652-3800">972.652.3800</a></div>
                            <div style="padding-top: 8px;">For I. T. call:<br/>Reuben Najera &nbsp; <a href="tel:972-793-5540,10264">972.793.5523</a> ext. 10264</div>
                            <div style="padding-top: 8px;">For Development call:<br/>Carlos Silva &nbsp; <a href="tel:972-793-5540,10264">972.793.5523</a> ext. 10264  </div>
			            </div>
			        </div>

			        <br/>

			        <div style="padding-left: 20px;">
			            <ul style='font: 12pt/normal "Arial", "sans-serif";  list-style:square; color: gainsboro;'>
				            <li>Computer &amp; peripheral requests (monitor, printer, keyboard, mouse, etc.)  </li>
				            <li>Login credentials </li>
				            <li>Locked account </li>
				            <li>Network issues </li>
				            <li>Software/hardware requests  </li>
				            <li>Navision support  </li>
				            <li>Shermco YOU!  </li>
			            </ul>
                    </div>

                </div> 
                
                
                <div id="Facilities">
			        <div style="text-align: center;">
				        <a href="mailto:facilities@shermco.com">
				            Email: (facilities@shermco.com)
                        </a>
			            <div class="contact" style="text-align: center;">
				            Contact Bob Jett at &nbsp; <a href="tel:972-793-5540,10342">972.793.5523</a><br/>ext. 10342
			            </div>
			        </div>

			        <br/>

			        <div style="padding-left: 20px;">
			            <ul style='font: 12pt/normal "Arial", "sans-serif";  list-style:square; color: gainsboro;'>
				            <li>Irving — building maintenance requests (lighting, a/c, heat, electrical, plumbing, interior/exterior building issues, etc.)  </li>
				            <li>Recycling  </li>
			            </ul>
                    </div>

                </div>  
                
                <div id="Procurement">
			        <div style="text-align: center;">
				        <a href="mailto:procurement@shermco.com">
				            Email: (procurement@shermco.com)
                        </a>
			            <div class="contact" style="text-align: center;">
				            Contact Tanner Cook at &nbsp; <a href="tel:972-793-5540,10389">972.793.5523</a><br/>ext. 10389
			            </div>
			        </div>

			        <br/>

			        <div style="padding-left: 20px;">
			            <ul style='font: 12pt/normal "Arial", "sans-serif";  list-style:square; color: gainsboro;'>
				            <li>Office Supplies</li>
				            <li>Business Cards</li>
				            <li>Apparel</li>
				            <li>Facilities Supplies</li>
			            </ul>
                    </div>

                </div>  
            </div>
            

        
        </div>
    </div>
    
    
    
<div id="UsefulLinksWrapper">
    



     
</div>    
</asp:Content>
