<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster2.master" AutoEventWireup="true"   %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Safety Pays Chart</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div>
        <div style="float: left; ">
            <div style="background-color: lightslategray; height: 63px;">
                <h3 style="margin-top: 0; padding-top: 15px; font-size: 1.5em; text-align: center; ">Top 10<br/>Points Awards</h3>
            </div>
            <table>
                <tr>
                    <th>Place</th>
                    <th>Name</th>
                    <th>Pts</th>
                </tr>
                
                <tr>
                    <td>1</td>
                    <td>Eric Bishop</td>
                    <td>110</td>
                </tr>
                
                <tr>
                    <td>2</td>
                    <td>J. D. Maloy</td>
                    <td>106</td>
                </tr>
                
                <tr>
                    <td>3</td>
                    <td>Eric Rejcek</td>
                    <td>103</td>
                </tr>

                <tr>
                    <td>4</td>
                    <td>Britt Gittings</td>
                    <td>87</td>
                </tr>
                
                <tr>
                    <td>5</td>
                    <td>Kyle Gaffaney</td>
                    <td>84</td>
                </tr>
                
                <tr>
                    <td>6</td>
                    <td>Trey Garcia</td>
                    <td>82</td>
                </tr>
                
                <tr>
                    <td>7</td>
                    <td>Matthew Shipley</td>
                    <td>67</td>
                </tr>
                
                <tr>
                    <td>8</td>
                    <td>Mike Schulz</td>
                    <td>66</td>
                </tr>
                
                <tr>
                    <td>9</td>
                    <td>Sean Nelms</td>
                    <td>50</td>
                </tr>
                
                <tr>
                    <td>10</td>
                    <td>Victor Leeber</td>
                    <td>44</td>
                </tr>
            </table>
        </div>
        

        <div style="float: left; border-left: black 2px solid; -moz-min-width: 600px; -ms-min-width: 600px; -o-min-width: 600px; -webkit-min-width: 600px; min-width: 600px;  max-width: 1400px;">
            <img src="/Images/SafetyPaysChart.jpg" alt="Safety Pays Charts" style="width:100%; "/>
        </div>
    </div>
</asp:Content>
