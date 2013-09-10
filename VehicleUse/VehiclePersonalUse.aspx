<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeBehind="VehiclePersonalUse.aspx.cs" Inherits="ShermcoYou.VehicleUse.VehiclePersonalUse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Page Layout -->
	<link rel="stylesheet" href="Styles/VehiclePersonalUse.css" type="text/css" />
    <title>Personal Use Policy For Shermco Vehicles</title>    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    


    <div style="width: 100%;">
        <a href="http://www.irs.gov/publications/p15b/index.html" target="blank">
            <img class="IrsImg" src="Images/irslogo.gif" alt="Irs Logo"> 
        </a>
    </div>

    <div class="FloatIrsText">
        <a href="http://www.irs.gov/publications/p15b/index.html" target="blank">
            Link To Fringe <br>Benefit IRS Publication 15B 
        </a>
    </div>

    <div style="position: relative; top: -50px;" class="FaqGradiant">
        <br>
        <h1 class="BlogHeader">Frequently Asked Questions</h1>

        <div class="padded">
            <p class="QA">Q: <span class="Q">What is Personal Mileage Reporting?</span> </p>
            <p class="QA">A: <span class="A">The U.S. Internal Revenue Code considers the personal use of a company-provided vehicle to be an economic benefit to the employee. Shermco is required to measure the taxable fringe benefit associated with personal use of the vehicle they have provided to you. Shermco's program is designed to allow drivers to report via the ELO time tracking system the amount of percent personal mileage driven during each reporting period, this will allow Shermco to easily and efficiently comply with governmental regulations. </span></p>

            <p class="QA">Q: <span class="Q">What is the driver's responsibility?</span> </p>
            <p class="QA">A: <span class="A">It is the driver's responsibility to track and report business and personal use of the Company provided vehicle. The driver should report precent personal mileage on a weekly basis. </span></p>

            <p class="QA">Q: <span class="Q">What happens if I do not keep or report records?</span> </p>
            <p class="QA">A: <span class="A">If you do not keep or report records to support your personal use of the vehicle, Shermco is required to consider that you have used the vehicle 100 percent for <b><u>PERSONAL</u></b> use. </span></p>

            <p class="QA">Q: <span class="Q">What is personal use?</span> </p>
            <p class="QA">A: <span class="A">Personal use of a company-provided vehicle is any non-business use of that vehicle. Examples: Weekend driving not related to your job function, vacation driving, weekday driving away from the office for personal reasons (lunch, banking, medical appointments, etc.).</span></p>

            <p class="QA">Q: <span class="Q">What are commuting miles?</span> </p>
            <p class="QA">A: <span class="A">The Internal Revenue Service (IRS) considers miles you drive from home to the office as commuting miles. This mileage is viewed as personal mileage.</span></p>

            <p class="QA">Q: <span class="Q">How will this affect my taxes?</span> </p>
            <p class="QA">A: <span class="A">Shermco will withhold FICA taxes on the taxable value determined. Federal income taxes will not be withheld.</span></p>

            <p class="QA">Q: <span class="Q">How is the taxable benefit calculated?</span> </p>
            <p class="QA">A: <span class="A">The IRS allows companies several methods of determining the value of personal use. Shermco uses the accounting method called the Annual Lease Value (ALV).</span></p>

            <p class="QA">Q: <span class="Q">What is Annual Lease Value (ALV)?</span> </p>
            <p class="QA">A: <span class="A">The IRS has determined an average cost to lease a vehicle for a one year period. This cost is based on the market value of the vehicle. This ALV is calculated and retained for a four year period. The result is multiplied by the employees personal use percentage (personal miles/total miles). Shermco includes personal fuel use, which is calculated by multiplying personal miles driven by $.055. The reported taxable benefit is the sum total of the calculations. </span></p>

            <p class="QA">Q: <span class="Q">Is Personal Use Credit shown on the report?</span> </p>
            <p class="QA">A: <span class="A">For most companies, the Personal Use Credit ($25.00) is handled by the payroll department. This amount will be included in the calculation.</span></p>

            <p class="QA">Q: <span class="Q">What happens to the weekly deduction for gas usage that is withheld from my paycheck?</span> </p>
            <p class="QA">A: <span class="A">This amount will be deducted from the calculated income associated with the use of the vehicle.</span></p>

            <p class="QA"><span class="Q">Example of Calculation of Non-Taxable Income:</span></p>
            <p class="LeftList"><span>Vehicle</span><br><span>FMV</span><br><span>Annual Lease Value</span><br><span>Total Miles</span><br><span>Personal %</span><br><span>Personal Miles</span><br><br><span>Personal Use Value</span><br><span>Fuel Charge</span><br><br><br><span>Employee Contribution</span><br><br><span>Total Taxable Income</span></p>
            <p class="RightList">2008 Chevy 2500 HD Crew<br>$13,500.00<br>$3,850.00 (Taken from the IRS table in Pub 15-B)<br>20,225<br>26%<br>5,295<br><br>$1,001.00 ($3,850.00 x 26%)<br>$291.22 (5,295 miles x $0.055)<br>$1,292.22<br><br>$(1,300.00) ($25.00 X 52 weeks))<br><br>($7.78)</p><br>

            <p class="QA"><span class="Q">Example of Calculation of Taxable Income:</span></p>
            <p class="LeftList"><span>Vehicle</span><br><span>FMV</span><br><span>Annual Lease Value</span><br><span>Total Miles</span><br><span>Personal %</span><br><span>Personal Miles</span><br><br><span>Personal Use Value</span><br><span>Fuel Charge</span><br><br><br><span>Employee Contribution</span><br><br><span>Total Taxable Income</span><br><br><span>Ex: 25% Tax Bracket</span></p>
            <p class="RightList">2011 Chevy 2500 HD Crew<br>$32,000.00<br>$8,750.00 (Taken from the IRS table in Pub 15-B)<br>22,740<br>16.5%<br>3,752<br><br>$1,443.75 ($8,750.00 x 16.5%)<br>$206.36 (3,752 miles x $0.055)<br>$1,650.11<br><br>$(1,300.00) ($25.00 X 52 weeks))<br><br>$350.11<br><br>$87.52</p>
        </div>
    </div>
</asp:Content>
