using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.Security;

using System.Security.Principal;
using System.Web.UI.WebControls;
using ShermcoYou.DataTypes;

namespace ShermcoYou
{
    public class CustomerMembershipProvider : MembershipProvider
    {
        private String _strName; 
        private String _strApplicationName;
        private Boolean _boolEnablePasswordReset;
        private Boolean _boolEnablePasswordRetrieval;
        private Boolean _boolRequiresQuestionAndAnswer;
        private Boolean _boolRequiresUniqueEMail;
//        private int _iPasswordAttemptThreshold;
        private const MembershipPasswordFormat _oPasswordFormat = MembershipPasswordFormat.Encrypted;

        public CustomerMembershipProvider()
        {
            _strName = "";
            _strApplicationName = "";

            _boolRequiresQuestionAndAnswer = false;
            _boolEnablePasswordReset = false;
            _boolEnablePasswordRetrieval = false;
            _boolRequiresQuestionAndAnswer = false;
            _boolRequiresUniqueEMail = false;
        } 


	    //  Read entries from web.config and initializes this class from those values
        //  Once the provider is loaded, the 
        //  runtime calls Initialize and passes the settings as name-value 
        //  pairs in an instance of the NameValueCollection class.
        //
	    public override void Initialize(string strName, System.Collections.Specialized.NameValueCollection config)
	    {

                _strName = strName;
                _strApplicationName = "/";

                _boolRequiresQuestionAndAnswer=false;
                _boolEnablePasswordReset = true;
                _boolEnablePasswordRetrieval = true;
                _boolRequiresQuestionAndAnswer=false ;
                _boolRequiresUniqueEMail=true;
	    }


        public override bool ValidateUser(string domainUser, string strPassword)
	    {
            string domain = "shermco";

            if ( HttpContext.Current.Session["UserValidated"] != null)
                return true;
            HttpContext.Current.Session.Add("UserValidated", System.DateTime.Now.ToString(CultureInfo.InvariantCulture));

            string[] paramsLogin = domainUser.Split('\\');
            if (paramsLogin.Length == 2)
            {
                domainUser = paramsLogin[1];
                domain = paramsLogin[0];
            }

	        //////////////////////////////////////////
            // Authenticate User Using WIndows LDAP //
            //////////////////////////////////////////
	        LDAP adAuth = new LDAP();
            if (true == adAuth.IsAuthenticated(domain, domainUser, strPassword))
            {
                HttpContext.Current.Session.Add("UserEmail",    adAuth.LdapUserEmail);
                HttpContext.Current.Session.Add("UserUser",     domainUser);
                HttpContext.Current.Session.Add("UserDomain",   domain);
                HttpContext.Current.Session.Add("UserGroups",   adAuth.LdapUserGroups);

                ////////////////////////////
                //// Build Requestor Info //
                ////////////////////////////
                Shermco_Employee Emp = SqlServer_Impl.GetEmployeeByEmail(adAuth.LdapUserEmail);
                if (Emp != null)
                {
                    HttpContext.Current.Session.Add("UserFullName", Emp.Last_Name + ", " + Emp.First_Name);
                    HttpContext.Current.Session.Add("UserEmpID", Emp.No_);
                    HttpContext.Current.Session.Add("UserSuprEmpID", Emp.Manager_No_);

                    Emp = SqlServer_Impl.GetEmployeeByNo(Emp.Manager_No_);
                    HttpContext.Current.Session.Add("UserSuprFullName", Emp.Last_Name + ", " + Emp.First_Name);
                }
                else
                {
                    HttpContext.Current.Session.Add("UserFullName", adAuth.LdapUserName);
                    HttpContext.Current.Session.Add("UserEmpID", "Missing");
                    HttpContext.Current.Session.Add("UserSuprEmpID", "Missing");
                    HttpContext.Current.Session.Add("UserSuprFullName", "Missing");
                }

                return true;

            }


            ///////////////////////////////////////////////////////
            // Perform Forms Authentication Using Local Database //
            ///////////////////////////////////////////////////////
            //LogonRcd Rcd = SqlServer_Impl.Validate_User(domainUser, strPassword);
            //if (Rcd.Email == domainUser) return true;

            /////////////////////////////////////////
            // All Attempts To Authenticate Failed //
            /////////////////////////////////////////
            return false;
	    }


	    public override string GetPassword(string strName, string strAnswer)
	    {
		    throw new Exception("The method or operation is not implemented.");
	    }



        public override string ApplicationName
        {
            get
            {

                return _strApplicationName;
            }
            set
            {
                _strApplicationName = value;
            }
        }
        public override string Name
        {
            get
            {
                return _strName;
            }
        }


        public override bool EnablePasswordReset
        {
            get
            {

                return _boolEnablePasswordReset;
            }
        }
        public override bool EnablePasswordRetrieval
        {
            get
            {
                return _boolEnablePasswordRetrieval;
            }
        }



        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return _oPasswordFormat;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 4; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return _boolRequiresQuestionAndAnswer;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get 
            { 
                throw new Exception("The method or operation is not implemented."); 
            }
        }
        public override bool RequiresUniqueEmail
        {
            get
            {
                return _boolRequiresUniqueEMail;
            }
        }



        public override MembershipUser CreateUser(	
		        	string username, 
	                string password, 
	                string email, 
	                string passwordQuestion, 
	                string passwordAnswer, 
	                bool isApproved, 
	                object userId, 
	                out MembershipCreateStatus status)
        {
            bool Success = false;

            //Success = SqlServer_Impl.CreateUser_Initial(email, password);

            System.Web.Security.MembershipUser rtn = new MembershipUser("Custom", username, 0, email, "", "", Success,
                                                                        false, DateTime.Now, DateTime.MinValue,
                                                                        DateTime.MinValue, DateTime.MinValue,
                                                                        DateTime.MinValue);
            if ( Success )
                status = MembershipCreateStatus.Success;
            else
                status = MembershipCreateStatus.UserRejected;

            return rtn;
        }


        public override string GetUserNameByEmail(string strEmail)
        {
            throw new Exception("The method or operation is not implemented."); 
        }
       

	    public override string ResetPassword(string strName, string strAnswer)
	    {
                throw new Exception("The method or operation is not implemented."); 
	    }
	    public override bool ChangePassword(string strName, string strOldPwd, string strNewPwd)
	    {
                throw new Exception("The method or operation is not implemented."); 
	    }
	    public override int GetNumberOfUsersOnline()
	    {
                throw new Exception("The method or operation is not implemented."); 
	    }
	    public override bool ChangePasswordQuestionAndAnswer(string strName, string strPassword, string strNewPwdQuestion, string strNewPwdAnswer)
	    {	
	        throw new Exception("The method or operation is not implemented.");
	
	    }
	    public override MembershipUser GetUser(string strName, bool boolUserIsOnline)
	    {
               throw new Exception("The method or operation is not implemented."); 
	    }

	    public override bool DeleteUser(string strName, bool boolDeleteAllRelatedData)
	    {
            throw new Exception("The method or operation is not implemented.");
        }
	    public override MembershipUserCollection FindUsersByEmail(string strEmailToMatch, int iPageIndex, int iPageSize, out int iTotalRecords)
	    {
                throw new Exception("The method or operation is not implemented.");
	    }
	    public override MembershipUserCollection FindUsersByName(string strUsernameToMatch, int iPageIndex, int iPageSize, out int iTotalRecords)
	    {
                throw new Exception("The method or operation is not implemented.");
	    }
	    public override MembershipUserCollection GetAllUsers(int iPageIndex, int iPageSize, out int iTotalRecords)
	    {
                throw new Exception("The method or operation is not implemented.");
	    }

        public override void UpdateUser(MembershipUser user)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override bool UnlockUser(string strUserName)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
