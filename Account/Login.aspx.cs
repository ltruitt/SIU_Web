using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ShermcoYou;



public partial class Login : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string domainUser = string.Empty;
        string domain = string.Empty;
        string empNo = string.Empty;

        const string adPath = "LDAP://10.6.1.11";
        LDAP LdapObj = new LDAP(adPath);

        ///////////////////////////
        // Get Windows Auth Data //
        ///////////////////////////
        System.Security.Principal.WindowsIdentity wi = Context.Request.LogonUserIdentity;

        if (wi != null)
        {                
            string[] paramsLogin = wi.Name.Split('\\');
            domainUser = paramsLogin[1];
            domain = paramsLogin[0];

            /////////////////////////////////////////
            // Record User ID In SQL Session State //
            /////////////////////////////////////////
            empNo = LdapObj.LookupAdEmpId((WindowsIdentity)User.Identity);

            if (empNo == null)
                SqlServer_Impl.LogDebug("Login.Page_Load", "Failed To Get AD Record For: " + wi.Name );
            else
            {
                Session.Add("UserEmail", LdapObj.LdapUserEmail);
                Session.Add("UserEmpID", empNo);
                Session.Add("UserUser", domainUser);
                Session.Add("UserDomain", domain);

                Shermco_Employee Emp = SqlServer_Impl.GetEmployeeByNo(empNo);
                if (Emp != null)
                {
                    /////////////////////////
                    // Set Users Full Name //
                    /////////////////////////
                    Session.Add("UserFullName", Emp.Last_Name + ", " + Emp.First_Name);
                    Session.Add("UserSuprEmpID", Emp.Manager_No_);

                    ///////////////////////////////
                    // Get Supervisors Full Name //
                    ///////////////////////////////
                    Emp = SqlServer_Impl.GetEmployeeByNo(Emp.Manager_No_);
                    Session.Add("UserSuprFullName", Emp.Last_Name + ", " + Emp.First_Name);

                    /////////////////////////////////////
                    // Create the authetication ticket //
                    /////////////////////////////////////
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, domainUser, DateTime.Now,
                                                                                            DateTime.Now.AddMinutes(30),
                                                                                            false, "");
                    ////////////////////////
                    // Encrypt The Ticket //
                    ////////////////////////
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    //////////////////////////////////////////////////////////////
                    // Create A Cookie And Add The Ticket To The Cookie As Data //
                    //////////////////////////////////////////////////////////////
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    ///////////////////////////////////////////////////////
                    // Add The Cookie To The Outgoing Cookies Collection //
                    ///////////////////////////////////////////////////////
                    Response.Cookies.Add(authCookie);

                    /////////////////////////////////////////////
                    // Build List Of AD Groups User Belongs To //
                    /////////////////////////////////////////////
                    StringCollection _Groups = new StringCollection();
                    foreach (IdentityReference identRef in wi.Groups)
                    {
                        IdentityReference account = identRef.Translate(typeof(NTAccount));
                        string GroupID = account.Value.Substring(account.Value.IndexOf('\\') > 0 ? account.Value.IndexOf('\\') + 1 : 0);

                        _Groups.Add(GroupID);
                    }
                    HttpContext.Current.Session.Add("UserGroups", _Groups);
                }
                else
                {
                    SqlServer_Impl.LogDebug("Login.Page_Load", "Missing Navision Record For: " + wi.Name + " (" + LdapObj.EmpID + ")" );
                }
            }

            ///////////////////////////////////////////////////////////
            // Redirect the user to the originally requested page    //
            // If User Not Validated Through Navision, Back To Login //
            ///////////////////////////////////////////////////////////
            string CallingPage = FormsAuthentication.GetRedirectUrl(domainUser, false);
            if (CallingPage.ToLower().Contains("sessionabandon"))
                CallingPage = "/";
            Response.Redirect(CallingPage);
        }


        ////////////////////////
        // Force forms login  //
        ////////////////////////
        if (IsPostBack)
        {
            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(domainUser, false);
        }
    }
}

