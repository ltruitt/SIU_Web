<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true" CodeFile="PersonalUseInfo.aspx.cs" Inherits="My_PersonalUseInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link rel="stylesheet" href="/Styles/MyPersonalUseInfo.css"/>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<DIV style="WIDTH: 100%">
    <a href="http://www.irs.gov/publications/p15b/index.html" target=blank><img class=IrsImg src="/Images/irslogo.gif" alt="IRS Logo"> </a>
</DIV> <DIV class=FloatIrsText>
           
<A href="http://www.irs.gov/publications/p15b/index.html" target=blank>Link To Fringe <BR>Benefit IRS Publication 15B </A>

</DIV> <DIV style="POSITION: relative; TOP: -50px" class=FaqGradiant>
    <BR> 

    <H1 class=BlogHeader>Frequently Asked Questions</H1> 
    
    <DIV class=padded>
        <P class=QA>Q: <SPAN class=Q>What is Personal Mileage Reporting?</SPAN> </P> 
        <P class=QA>A: <SPAN class=A>
            The U.S. Internal Revenue Code considers the personal use of a company-provided 
            vehicle to be an economic benefit to the employee. Shermco is required to measure 
            the taxable fringe benefit associated with personal use of the vehicle they have 
            provided to you. Shermco's program is designed to allow drivers to report via the ELO time 
            tracking system the amount of percent personal mileage driven during each reporting 
            period, this will allow Shermco to easily and efficiently comply with governmental 
            regulations. </SPAN></P> 
            
            <P class=QA>Q: <SPAN class=Q>What is the driver's responsibility?</SPAN> </P> 
            <P class=QA>A: <SPAN class=A>
                It is the driver's responsibility to track and report business and personal use of 
                the Company provided vehicle. The driver should report precent personal mileage on a 
                weekly basis. </SPAN></P> 
                
            <P class=QA>Q: <SPAN class=Q>What happens if I do not keep or report records?</SPAN> </P> 
            <P class=QA>A: <SPAN class=A>
                If you do not keep or report records to support your personal use of the vehicle, Shermco is 
                required to consider that you have used the vehicle 100 percent for <B><U>PERSONAL</U></B> use. 
                </SPAN></P> 
                
            <P class=QA>Q: <SPAN class=Q>What is personal use?</SPAN> </P> 
            
            <P class=QA>A: <SPAN class=A>
                Personal use of a company-provided vehicle is any non-business use of that vehicle. 
                Examples: Weekend driving not related to your job function, vacation driving, weekday 
                driving away from the office for personal reasons (lunch, banking, medical appointments, 
                etc.). </SPAN></P> 
                
            <P class=QA>Q: <SPAN class=Q>What are commuting miles?</SPAN> </P> 
            <P class=QA>A: <SPAN class=A>
                The Internal Revenue Service (IRS) considers miles you drive from home to the office as 
                commuting miles. This mileage is viewed as personal mileage. </SPAN></P> 
                
            <P class=QA>Q: <SPAN class=Q>How will this affect my taxes?</SPAN> </P> 
            <P class=QA>A: <SPAN class=A>
                Shermco will withhold FICA taxes on the taxable value determined. Federal income taxes 
                will not be withheld. </SPAN></P> 
                
            <P class=QA>Q: <SPAN class=Q>How is the taxable benefit calculated?</SPAN> </P> 
            <P class=QA>A: <SPAN class=A>
                The IRS allows companies several methods of determining the value of personal use. 
                Shermco uses the accounting method called the Annual Lease Value (ALV).</SPAN></P> 
                
            <P class=QA>Q: <SPAN class=Q>What is Annual Lease Value (ALV)?</SPAN> </P> 
            <P class=QA>A: <SPAN class=A>
                The IRS has determined an average cost to lease a vehicle for a one year period. 
                This cost is based on the market value of the vehicle. This ALV is calculated and 
                retained for a four year period. The result is multiplied by the employees personal 
                use percentage (personal miles/total miles). Shermco includes personal fuel use, 
                which is calculated by multiplying personal miles driven by $.055. The reported 
                taxable benefit is the sum total of the calculations. </SPAN></P> 
                
            <P class=QA>Q: <SPAN class=Q>Is Personal Use Credit shown on the report?</SPAN> </P> 
            <P class=QA>A: <SPAN class=A>
                For most companies, the Personal Use Credit ($25.00) is handled by the payroll 
                department. This amount will be included in the calculation.</SPAN></P> 
                
            <P class=QA>Q: <SPAN class=Q>
                What happens to the weekly deduction for gas usage that is withheld from my 
                paycheck? </SPAN> </P> 
            <P class=QA>A: <SPAN class=A>
                This amount will be deducted from the calculated income associated with the 
                use of the vehicle.</SPAN></P> 
                

            <p class="QA">
	            <span class="Q">Calculation example of over-paid fuel charges: (dollar for dollar reimbursement)</span>
            </p>



            <p class="LeftList">
	            <span>Vehicle</span>	<br>
	            <span>FMV</span>	<br>
	            <span>Annual Lease Value</span>	<br>
	            <span>Total Miles</span>	<br>
	            <span>Personal %</span><br>
	            <span>Personal Miles</span><br>
	            <br>
	            <span>Personal Use Value</span><br>
	            <span>Fuel Charge</span><br>
	            <br>
	            <br>
	            <span>Employee Contribution</span><br>
	            <span>Total Taxable Income</span><br>
            </p>


            <p class="RightList">
	            2011 Chevy 2500 HD Crew<br>
	            $32,000.00<br>
	            $8,750.00 {taken from the IRS table in Pub 15-B}<br>
	            22,750<br>
	            2.20%<br>
	            500<br>
	            <br>
	            $192.50 {$8,750.00 * 2.2%}<br>
	            $27.50 {500 x $0.055}<br>
	            $220.00<br>
	            <br>
	            $(1,300.00) {$25.00 * 52 weeks}<br>
	            $(1,080.00) {$192.50 + $27.50 - $1,300}<br>
            </p>
            <p class="QA">
	            <span class="Q">$1,080.00 will be reimbursed by direct deposit as non-taxable income before the end of January in conjunction with W2 distribution</span>
            </p>



            <P class=QA>
	            <SPAN class=Q>Example of Calculation of Taxable Income:</SPAN>
            </P> 

            <P class=LeftList>
	            <SPAN>Vehicle</SPAN><BR>
	            <SPAN>FMV</SPAN><BR>
	            <SPAN>Annual Lease Value</SPAN><BR>
	            <SPAN>Total Miles</SPAN><BR>
	            <SPAN>Personal %</SPAN><BR>
	            <SPAN>Personal Miles</SPAN><BR>
	            <BR>
	            <SPAN>Personal Use Value</SPAN><BR>
	            <SPAN>Fuel Charge</SPAN><BR>
	            <BR>
	            <BR>
	            <SPAN>Employee Contribution</SPAN><BR>
	            <BR>
	            <SPAN>Total Taxable Income</SPAN><BR>
	            <BR>
	            <SPAN>Ex: 25% Tax Bracket</SPAN>
            </P> 

            <P class=RightList>
	            2011 Chevy 2500 HD Crew<BR>
	            $32,000.00<BR>
	            $8,750.00 {taken from the IRS table in Pub 15-B}<BR>
	            22,740<BR>
	            16.5%<BR>
	            3,752<BR>
	            <BR>
	            $1,443.75 ($8,750.00 x 16.5%)<BR>
	            $206.36 {3,752 x $0.055}
	            <BR>
	            $1,650.11<BR>
	            <BR>
	            $(1,300.00) {$25.00 X 52 weeks}<BR>
	            <BR>
	            $350.11<BR>
	            <BR>
	            $87.52
            </P>




    </div>
</div>
</asp:Content>

