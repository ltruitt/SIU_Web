using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using ShermcoYou.DataTypes;

public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            FailureText.Text = "Sign on To Shermco YOU!";
            return;
            string domainUser = string.Empty;
            string domain = string.Empty;
            string empNo = string.Empty;

            const string adPath = "LDAP://10.6.1.11";
            LDAP LdapObj = new LDAP(adPath);

            ///////////////////////////
            // Get Windows Auth Data //
            ///////////////////////////
            System.Security.Principal.WindowsIdentity wi = Context.Request.LogonUserIdentity;

            bool xxx = Request.IsAuthenticated;

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
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            int passwordSuccess = 1;

            /////////////////////////////////////////
            // Lookup Up User Based On EMployee ID //
            /////////////////////////////////////////
            Shermco_Employee emp = SqlServer_Impl.GetEmployeeByNo(UserName.Text);
            Shermco_Employee suprEmp;

            if (emp == null)
            {
                FailureText.Text = "Please Try Again";
                passwordSuccess = 0;
            }
            else
            {
                if (emp.Employee_Password != Password.Text)
                {
                    FailureText.Text = "Please Try Again";
                    passwordSuccess = 0;
                }

                if (emp.Blocked == 1)
                {
                    FailureText.Text = "Please Contact H.R.";
                    passwordSuccess = 0;
                }

                if (emp.Status != (int)Employee_Status.Active)
                {
                    FailureText.Text = "Please Contact H.R.";
                    passwordSuccess = 0;
                }


                if (!emp.Company_E_Mail.Contains("@"))
                {
                    FailureText.Text = "Logon Record Incomplete";
                    passwordSuccess = 0;
                }
            }


            ///////////////////////////////////////////
            // Log The Attempt And Check For Probing //
            ///////////////////////////////////////////
            string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            SIU_LogonProbeRcd logonResp = SqlServer_Impl.LogonProbe(UserName.Text, Password.Text, ip, passwordSuccess);

            
            // if Valid = -99 and Mail = 1, email Probe Warning

            ////////////////////////////////////////
            // If Valid = -99, Show Lockout Error //
            ////////////////////////////////////////
            if (logonResp.Valid == -99)
            {
                FailureText.Text = "Device AND ID blocked for 15 minutes.";
                System.Threading.Thread.Sleep(5000);
                passwordSuccess = 0;
            }

            ////////////////////////////////////////////////////////////
            // if  Valid = -1 then a db error occured.  Email Warning //
            ////////////////////////////////////////////////////////////
            //if (logonResp.Valid == -1)
            //{
            //    FailureText.Text = "Please try again.";
            //    PasswordSuccess = 0;
            //}


            //////////////////////////////////////////////////
            // If the signon failed at any point, stop here //
            //////////////////////////////////////////////////
            if (passwordSuccess == 0)
            {
                FailureText.Visible = true;
                Password.Text = "";
                return;
            }

            //////////////////////////////////////////////////
            // At this point, the signon is considered good //
            //////////////////////////////////////////////////
            FailureText.Visible = false;

            //////////////////////////////////
            // Setup some session Variables //
            //////////////////////////////////
            string[] domUsr = emp.Company_E_Mail.Split(('@'));
            if ( domUsr[1].Contains(".") )
            {
                domUsr[1] = domUsr[1].Substring(0, domUsr[1].IndexOf('.'));
            }

            Session.Add("UserUser", domUsr[0]);
            Session.Add("UserDomain", domUsr[1]   );
            Session.Add("UserFullName", emp.Last_Name + ", " + emp.First_Name);
            Session.Add("UserEmail", emp.Company_E_Mail);
            Session.Add("UserEmpID", emp.No_);
            Session.Add("UserDept", emp.Global_Dimension_1_Code );
            Session.Add("UserSuprEmpID", emp.Manager_No_);

            suprEmp = SqlServer_Impl.GetEmployeeByNo(emp.Manager_No_);
            if (suprEmp != null)
                Session.Add("UserSuprFullName", suprEmp.Last_Name + ", " + suprEmp.First_Name);
            else
                Session.Add("UserSuprFullName", "");

            /////////////////////////////////
            // Lookup AD Group Memberships //
            /////////////////////////////////
            LDAP cred = new LDAP();
            StringCollection roles = cred.GetGroups(BusinessLayer.UserName);
            foreach (string role in SqlServer_Impl.SIU_GetRoles(UserName.Text))
                roles.Add(role);

            Session.Add("UserGroups", roles);



            ////////////////////////////////////
            // Build Session Info Into Cookie //
            ////////////////////////////////////
            string userDataString = string.Concat(emp.Last_Name + ", " + emp.First_Name, "|", emp.Manager_No_);


            /////////////////////////////////////////////////////////////////
            // Create Cookie that contains the forms authentication ticket //
            /////////////////////////////////////////////////////////////////
            //HttpCookie authCookie = FormsAuthentication.GetAuthCookie(UserName.Text, RememberMe.Checked);
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(emp.Last_Name + ", " + emp.First_Name, false);

            /////////////////////////////////////////////////////////////
            // Get the FormsAuthenticationTicket from encrypted cookie //
            /////////////////////////////////////////////////////////////
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            ///////////////////////////////////////////////////////////////////////
            // Create a new FormsAuthenticationTicket that includes Session Data //
            ///////////////////////////////////////////////////////////////////////
            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, userDataString);

            /////////////////////////////////////////////////////////////////////////////
            // Update the authCookie's Value to use the encrypted version of newTicket //
            /////////////////////////////////////////////////////////////////////////////
            authCookie.Value = FormsAuthentication.Encrypt(newTicket);

            ///////////////////////////////////////////////////////////
            // Manually add the authCookie to the Cookies collection //
            ///////////////////////////////////////////////////////////
            Response.Cookies.Add(authCookie);

            ////////////////////////////////////////////////
            // Determine redirect URL and send user there //
            ////////////////////////////////////////////////
            string redirUrl = FormsAuthentication.GetRedirectUrl(UserName.Text, false);
            Response.Redirect(redirUrl);
        }
}


