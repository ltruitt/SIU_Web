using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShermcoYou.DataTypes;

public partial class Forms_HardwareRequest : System.Web.UI.Page
{
#region "Private Members"
    private List<SIU_SortedEmployees> SortedEmps;
    private Shermco_Employee Emp;
    private List<SIU_IT_HW_Req_Computer> Computers;
    
#endregion



    protected void Page_Load(object sender, EventArgs e)
    {
        string Method = "Forms_HardwareRequest.PageLoad";

        try
        {
           
            // If this is the first visit to the page, bind the role data to the datalist 
            if (Page.IsPostBack == false)
            {
                //////////////////////////////////////
                // Get AVailable Jobs And Employees //
                //////////////////////////////////////
                SortedEmps = SqlServer_Impl.GetActiveEmployeesSorted();

                //////////////////////////
                // Build Requestor Info //
                //////////////////////////
                lblEmpName.InnerText = BusinessLayer.UserFullName;
                lblEmpID.InnerText = BusinessLayer.UserEmpID;
                lblEmpSuprID.InnerText = BusinessLayer.UserSuprEmpId;
                lblEmpSuprName.InnerText = BusinessLayer.UserSuprFullName;


                ///////////////////////////////////
                // Bind List Of Employee Numbers //
                ///////////////////////////////////
                ddEmpIdEquipFor.DataSource = SortedEmps;
                ddEmpIdEquipFor.DataTextField = "EmpDisplayNoName";
                ddEmpIdEquipFor.DataValueField = "sEmpNo";
                ddEmpIdEquipFor.DataBind();
                ddEmpIdEquipFor.Items.Insert(0, "");

                ////////////////////
                // Bind Job Roles //
                ////////////////////
                Computers = SqlServer_Impl.GetHardwareRequestComputers();

                List<string> Roles = (from JobRoles in Computers select JobRoles.Role).Distinct().ToList();
                ddUserRole.DataSource = Roles;
                ddUserRole.DataBind();
                ddUserRole.Items.Insert(0, "");
            }
        }

        catch (Exception exc)
        {
            //Module failed to load 
            string err = exc.Message;
        }
    }

    protected void EquipForSelected(object sender, EventArgs e)
    {
        string Method = "Forms_HardwareRequest.EquipForSelected";

        //////////////////////////////
        // Build Requestor For Info //
        //////////////////////////////
        Emp = SqlServer_Impl.GetEmployeeByNo(ddEmpIdEquipFor.Text);
        lblEquipForName.InnerText = Emp.Last_Name + ", " + Emp.First_Name;

        SIU_IT_HW_Req OpenRcd = SqlServer_Impl.GetHardwareOpenRequest(Emp.No_);
        if ( OpenRcd != null )
        {
            ddUserRole.Text = OpenRcd.Role;

            RoleSelected(null, null);
            ddCompOpts.Enabled = true;
            ddCompOpts.Text = OpenRcd.Hardware;

            lblPrice.InnerText = OpenRcd.Price.ToString("C2");

            /////////////////////
            // Add Accessories //
            /////////////////////
            txtMonitorCnt.Text = OpenRcd.MonitorCount.ToString();
            txtStandCnt.Text = OpenRcd.MonitorStandCount.ToString();
            chkCase.Checked = OpenRcd.ComputerCase;
            chkDock.Checked = OpenRcd.ComputerDock;
            chkDock.Checked = OpenRcd.BackPack;
            chkAdobe.Checked = OpenRcd.Adobe;
            chkCAD.Checked = OpenRcd.CAD;
            chkMsPrj.Checked = OpenRcd.MsPrj;
            chkVisio.Checked = OpenRcd.Visio;
        }
        else
        {
            ddUserRole.Text = "";
            ddCompOpts.Text = "";
            lblPrice.InnerText = "$0.00";

            /////////////////////
            // Add Accessories //
            /////////////////////
            txtMonitorCnt.Text = "0";
            txtStandCnt.Text = "0";
            chkCase.Checked = false;
            chkDock.Checked = false;
            chkDock.Checked = false;
            chkAdobe.Checked = false;
            chkCAD.Checked = false;
            chkMsPrj.Checked = false;;
            chkVisio.Checked = false;            
        }
    }

    protected void RoleSelected(object sender, EventArgs e)
    {
        string Method = "Forms_HardwareRequest.RoleSelected";

        //////////////////////////////////////////////////////
        // Bind Available Computer Types Based On User Role //
        //////////////////////////////////////////////////////
        Computers = (   from aComputer in SqlServer_Impl.GetHardwareRequestComputers()
                        where aComputer.Role == ddUserRole.Text
                        select aComputer
                    ).ToList();

        ddCompOpts.DataSource = Computers;
        ddCompOpts.DataTextField = "Computer";
        ddCompOpts.DataBind();

        if ( Computers.Count > 1)
            ddCompOpts.Items.Insert(0, "");
        else
        {
            if (ddCompOpts.Items.Count > 0)
            {
                ddCompOpts.Text = ddCompOpts.Items[0].Text;
                UpdatePrice(null, null);
            }
        }

        ddCompOpts.Enabled = true;
    }

    protected void UpdatePrice(object sender, EventArgs e)
    {
        ///////////////////////
        // Update Price Info //
        ///////////////////////
        lblPrice.InnerText = BusinessLayer.CalcHardwarePrice(ddCompOpts.Text, txtMonitorCnt.Text, txtStandCnt.Text, chkCase.Checked,
                                                             chkDock.Checked, chkBackPack.Checked, chkAdobe.Checked, chkCAD.Checked,
                                                             chkMsPrj.Checked, chkVisio.Checked);
    }

    protected void UpdateCmd(object sender, EventArgs e)
    {
        List<SIU_IT_HW_Req_Add> AddOns;

        string Method = "Forms_HardwareRequest.UpdateCmd";

        SIU_IT_HW_Req HwReq = new SIU_IT_HW_Req();

        ///////////////////////
        // Get Add-Ons Table //
        ///////////////////////
        AddOns = SqlServer_Impl.GetHardwareRequestAddOns();

        ///////////////////////////////////////
        // Save Base Hardware Request Record //
        ///////////////////////////////////////
        HwReq.Req_EmpID = lblEmpID.InnerText;
        HwReq.For_EmpID = ddEmpIdEquipFor.Text;
        HwReq.Role = ddUserRole.Text;
        HwReq.Hardware = ddCompOpts.Text;
        HwReq.Price = decimal.Parse(lblPrice.InnerText.Replace('$', '0'));

        ////////////////////////////////
        // Build Computer Type Record //
        ////////////////////////////////
        decimal Price = (from aComputer in SqlServer_Impl.GetHardwareRequestComputers()
                         where aComputer.Computer == ddCompOpts.Text
                         select aComputer.Price
                        ).Take(1).SingleOrDefault();

        HwReq.ComputerDesc = ddCompOpts.Text;
        HwReq.ComputerPrice = Price;

        /////////////////////
        // Add Accessories //
        /////////////////////
        HwReq.MonitorCount = int.Parse(txtMonitorCnt.Text);
        HwReq.MonitorStandCount = int.Parse(txtStandCnt.Text);
        HwReq.ComputerCase = chkCase.Checked;
        HwReq.ComputerDock = chkDock.Checked;
        HwReq.BackPack = chkDock.Checked;
        HwReq.Adobe = chkAdobe.Checked;
        HwReq.CAD = chkCAD.Checked;
        HwReq.MsPrj = chkMsPrj.Checked;
        HwReq.Visio = chkVisio.Checked;

        int XtnID = SqlServer_Impl.RecordHardwareRequest(HwReq);
        BusinessLayer.HardwareRequstSendNewEmail(HwReq, BusinessLayer.UserEmail);

        // Redirect back to the portal home page 
        Response.Redirect("/", true);
    }

    protected void CancelCmd(object sender, EventArgs e)
    {
        string Method = "Forms_HardwareRequest.CancelCmd";

        // Redirect back to the portal home page 
        Response.Redirect("/", true);
    }

    
}