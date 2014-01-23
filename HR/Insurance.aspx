<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="Insurance.aspx.cs" Inherits="HR_HrHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
	<title>Human Resources Home Page</title>
	      
    <link rel="stylesheet" href="/Styles/Insurance.css" type="text/css" />

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div style="visibility: hidden; height: 0; width: 0;">
        <span id="Sx"  runat="server"/>
    </div>
 
    <div id="HomeWrapper" >
    
               
        <section id="HomeMain" >
            <section class="ui-widget-header ui-corner-all" style="height: 45px; width: 99%; margin-top: 10px; margin-bottom: 30px;" >
                <div  style="text-align: center; font-size: 2em; width: 100%;  position:relative; margin-left: auto; margin-right: auto;">2014 Insurance Benefits</div>
            </section>        
            
            <ul class="DeptGallery2" style="width: 100%;">
                <li style="text-align: center; width: 31%; padding-left: 0; padding-right: 0;">
                        <img    alt="HR Documents" src="/Images/BenefitReady.png"  />
                        <ul id="DirectoryReady">
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/2014 Enrollment Presentation V2.pdf"  target="_DocView"  id="A35" >2014 Benefit Enrollment Presentation</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/2014_Shermco_TRIFOLD_HR.pdf"  target="_DocView"  id="A31" >2014 - Shermco Benefits Highlights </a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Medical - 480 Volt Benefit Plan.pdf"  target="_DocView"  id="A19" >Medical - 480 Volt Benefit Summary</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Medical - 5kV Plan.pdf"  target="_DocView"  id="A17" >Medical - 5kV Benefit Summary</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Medical - 15kV Plan.pdf"  target="_DocView"  id="A18" >Medical - 15kV Benefit Summary</a></li>
                            
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/FW_ Plan Cost Calculator.xlsm"  target="_DocView"  id="A6" >Medical Plan Cost Decision Calculator</a></li>
                            <li><a  href="http://www.shophealthmarkets.com"  target="_DocView"  id="A30" >Shop Health Markets </a></li>

                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/2014_Shermco_HSA.pdf"  target="_DocView"  id="A28" >How your Health Savings Account Works</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Aetna Dental Basic Plan Summary.pdf"  target="_DocView"  id="A14" >Dental Basic Plan Summary</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Aetna Dental BuyUp Plan Summary.pdf"  target="_DocView"  id="A25" >Dental BuyUp Plan Summary</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/2013VSP.pdf"  target="_DocView"  id="A27" >VSP - Vision Benefit Summary</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Shermco In_LTD_Other Full Time.pdf"  target="_DocView"  id="A32" >LTD Plan Summary</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Shermco In_STD_Full Time.pdf"  target="_DocView"  id="A33" >STD Plan Summary</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Shermco In_LIFEADD_Full Time.pdf"  target="_DocView"  id="A34" >Life and Accidental Death Plan Summary</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/401k_Aflac_LegalShield -Final.pdf"  target="_DocView"  id="A26" >401(k), Aflac and Legal Shield Information</a></li>
                            <li><a  href="/Files/Library/Human Resources/Retirement Benefits/401K_Desc.pdf"  target="_DocView"  id="A24" >401(k) Plan Description</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/2014_Shermco_REQ_NOTICES.pdf"  target="_DocView"  id="A10" >Shermco Required Notices</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/CHIPs_ENG_10.31.16.pdf"  target="_DocView"  id="A11" >Children's Health Care Notice</a></li>
                        </ul>                    
                </li>
                
                <li style="text-align: center; width: 31%; padding-left: 0; padding-right: 0;">
                        <img    alt="HR Videos" src="/Images/BenefitSet.png" />
                        <ul  id="DirectoryVideos">
                            <li><a  href="https://www.eenroller.net/btrac/eeaccessguide.asp?ST=SHMC6800"  target="_DocView"  id="A1" >How to enroll in Benetrac</a></li>
                            <li><a  href="https://www.eenroller.net/btrac/broker.asp?ST=SHMC6800&"  target="_DocView"  id="A7" >Click here to Enroll in your Insurance Benefits</a></li>
                            <li><a  href="/Files/Library/Human Resources/Retirement Benefits/401K_Enroll.pdf"  target="_DocView"  id="A5" >How to enroll in 401(k) Plan</a></li>
                            <li><a  href="http://www.gwrs.com/"  target="_DocView"  id="A8" >Click here to enroll in 401(k) Plan</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Life EOI only 0713 67829.pdf"  target="_DocView"  id="A20" >Evidence of Insurability Form for Additional Voluntary Life</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/HSA Paper Account Application Form.pdf"  target="_DocView"  id="A29" >HSA Account Application Form</a></li>
                        </ul>                    
                </li>
                
                
                
                

                <li style="text-align: center; width: 31%; padding-left: 0; padding-right: 0;">
                        <img    alt="HR Videos" src="/Images/BenefitGo.png" />
                        <ul id="DirectoryLinks">
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Aetna Navigator.pdf"  target="_DocView"  id="A21" >How to use Aetna website</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Shermco_BenefitLink Employer Postcard_HR.pdf"  target="_DocView"  id="A9" >How to Use your Lockton Benefit Link App</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Compass_Brochure.pdf"  target="_DocView"  id="A15" >About Your Personal Health Pro Concierge</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/COMPASS Whiteboard Video.mov" target="_DocView"  id="A16" >How to use your Health Pro Concierge</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Rx Home Delivery.pdf"  target="_DocView"  id="A22" >How to use RX Home Delivery</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Teladoc.pdf"  target="_DocView"  id="A13" >Teledoc Phone or Video Based Doctor Consults</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/WhiteGlove English - Aetna.pdf"  target="_DocView"  id="A12" >White Glove - Healthcare at Home, Office or Online</a></li>
                            
                            <li><a  href="http://www.aetnamedia.com/MPE"  target="_DocView"  id="A2" >How to Estimate your Payments</a></li>
                            <li><a  href="http://www.aetnamedia.com/membertools" target="_DocView"   id="A3" >How to Use Aetna Navigator</a></li>
                            <li><a  href="http://www.aetna.com/docfind/home.do" target="_DocView"   id="A4" >Online Doc Find directory</a></li>
                        </ul>                    
                </li>
                
                <li style="text-align: center; width: 31%; padding-left: 0; padding-right: 0;">
                        <img    alt="HR Videos" src="/Images/BenifitRun.png" />
                        <ul id="Ul1">
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/exercise.pdf"  target="_DocView"  id="A36" >Exercise – Get Motivate to get Moving</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/stress_management.pdf"  target="_DocView"  id="A37" >Stress – Five Ways to ease the Stress in your Life</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Smoking_Cessation.pdf"  target="_DocView"  id="A38" >Smoking – You Really can Quit smoking</a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/weight_management.pdf" target="_DocView"  id="A39" >Weight Management – Five Tips for Success to Stay at Healthy Weight </a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/Member Simple Steps Brochure.pdf" target="_DocView"  id="A40" >Simple Steps to a Healthier Life Online Wellness Coaching Sessions </a></li>
                            <li><a  href="/Files/Library/Human Resources/Insurance Benefits/EAP Wallet Card 26 03 347 1.pdf"  target="_DocView"  id="A23" >Aetna’s Employee Assistance Program</a></li>
                        </ul>                    
                </li>
                
            </ul>
                                    
        </section>
    </div>


</asp:Content>

