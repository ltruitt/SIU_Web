using System;
using System.Activities.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.DirectoryServices;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;



/// <summary>
/// Summary description for LDAP
/// </summary>
public class LDAP
{

    private string _path;
    private string _LdapUserName;
    private string _LdapUserEmail;
    private string _EmpID;
    private readonly StringCollection _groups = new StringCollection();

    const string adPath = "LDAP://10.6.1.11";

    public LDAP()
    {
        _path = adPath;
    }

	public LDAP(string path)
	{
        _path = path;
	}

    public string LdapUserName
    {
        get { return _LdapUserName; }
    }
    public string LdapUserEmail
    {
        get { return _LdapUserEmail;  }
    }
    public StringCollection LdapUserGroups
    {
        get { return _groups; }
    }
    public string EmpID
    {
        get { return _EmpID; }
    }

    public bool IsAuthenticated(string domain, string username, string pwd)
    {
        string domainAndUsername = domain + @"\" + username;
        DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);


        try
        {
            // Bind to the native ADS Object to force authentication.
            //Object obj = entry.NativeObject;
            DirectorySearcher search = new DirectorySearcher(entry);
            
            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("mail");
            SearchResult result = search.FindOne();
            if (null == result)
            {
                return false;
            }

            /////////////////////////////////////////////
            // Extract The Users Name, EMail, and Path //
            /////////////////////////////////////////////
            _path = result.Path;
            _LdapUserName = (String)result.Properties["cn"][0];
            _LdapUserEmail = (String)result.Properties["mail"][0];

            ///////////////////////////////////////////////////
            // Get A directory Entry For The Signing In User //
            ///////////////////////////////////////////////////
            DirectoryEntry obUser = new DirectoryEntry(result.Path);

            /////////////////////////////////////////////////////////
            // Extract the Name Of Each LDAP Group User Belongs To //
            /////////////////////////////////////////////////////////
            object obGroups = obUser.Invoke("Groups");
            foreach (object ob in (IEnumerable)obGroups)
            {
                // Create object for each group.
                DirectoryEntry obGpEntry = new DirectoryEntry(ob);
                _groups.Add(obGpEntry.Name.Replace("CN=", "").Replace("\"", "") );
            }
        }

        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("LDAP.IsAuthenticated", "AD Lookup Failed " + ex.Message);
            return false;
        }

        return true;
    }


    public string LookupEmail(WindowsIdentity id)
    {

        const AuthenticationTypes SECURE_BINDING = AuthenticationTypes.Secure | AuthenticationTypes.Signing | AuthenticationTypes.Sealing;

        //string path = string.Format("LDAP://<SID={0}>", id.User);
        string path = string.Format("LDAP://<SID={0}>", "S-1-5-21-1129156865-3431479564-2330212978-16767");
        
        SqlServer_Impl.LogDebug("LDAP.LookupEmail: " + id.User, id.Name + " isAuth:"  + id.IsAuthenticated.ToString() );

        try
        {
            //using (DirectoryEntry user = new DirectoryEntry(path, "ltruitt", "Pete5ret1red", SECURE_BINDING))
            using (DirectoryEntry user = new DirectoryEntry(path, null, null, SECURE_BINDING))
            {
                _LdapUserEmail = (string) user.Properties["mail"].Value;
                //SqlServer_Impl.LogDebug("LDAP.LookupEmail: " + id.User, _LdapUserEmail);

                _EmpID = (string)user.Properties["employeeID"].Value;
                SqlServer_Impl.LogDebug("LDAP.LookupEmail: " + id.User, _LdapUserEmail + " (" + _EmpID + ")");
            }
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("LDAP.LookupEmail: " + id.User, ex.Message);
            _LdapUserEmail = "";
        }


        return _LdapUserEmail;
    }

    public string LookupAdEmpId(WindowsIdentity id)
    {
        const AuthenticationTypes SECURE_BINDING = AuthenticationTypes.Secure | AuthenticationTypes.Signing | AuthenticationTypes.Sealing;

        string path = string.Format("LDAP://<SID={0}>", id.User);
        //string path = string.Format("LDAP://<SID={0}>", "S-1-5-21-1129156865-3431479564-2330212978-16767");

        //SqlServer_Impl.LogDebug("LDAP.LookupAdEmpId: " + id.User, id.Name + " isAuth:" + id.IsAuthenticated.ToString());

        try
        {
            using (DirectoryEntry user = new DirectoryEntry(path, null, null, SECURE_BINDING))
            {
                _LdapUserEmail = (string)user.Properties["mail"].Value;
                _EmpID = (string)user.Properties["employeeID"].Value;
                //SqlServer_Impl.LogDebug("LDAP.LookupAdEmpId: " + id.User, _LdapUserEmail + " (" + _EmpID + ")");
            }
        }
        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("LDAP.LookupAdEmpId: " + id.User, ex.Message);
            _LdapUserEmail = "";
        }

        if ( _EmpID == null )
            SqlServer_Impl.LogDebug("LDAP.LookupAdEmpId", "AD Lookup Failed for" + id.Name);


        return _EmpID;
    }

    //public string LookupEmail(string strUser)
    //{
    //    try
    //    {
    //        /////////////////////////////////////////////////
    //        // Get An Active Directory Entry For User Name //
    //        /////////////////////////////////////////////////
    //        DirectoryEntry obEntry = new DirectoryEntry(_path);
    //        DirectorySearcher srch = new DirectorySearcher(obEntry, "(sAMAccountName=" + strUser + ")");
    //        SearchResult result = srch.FindOne();

    //        if (null != result)
    //            return (String)result.Properties["mail"][0];
    //    }

    //    catch (Exception ex)
    //    {
    //        Trace.Write(ex.Message);
    //    }

    //    return "";

    //}


    public StringCollection GetGroups(string strUser)
    {
        DirectoryEntry entry = new DirectoryEntry(_path, @"Shermco\ltruitt", "Payday2013");

        try
        {
            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(SAMAccountName=" + strUser + ")";
            //search.Filter = "(SAMAccountName=aschumacher)";
            SearchResult result = search.FindOne();
            if (null == result)
                return _groups;


            DirectoryEntry obUser = new DirectoryEntry(result.Path);
            object obGroups = obUser.Invoke("Groups");
            foreach (object ob in (IEnumerable)obGroups)
            {
                DirectoryEntry obGpEntry = new DirectoryEntry(ob);
                _groups.Add(obGpEntry.Name.Replace("CN=", "").Replace("\"", ""));
            }
        }

        catch (Exception ex)
        {
            SqlServer_Impl.LogDebug("LDAP.GetGroups", "AD Lookup Failed " + ex.Message);
        }

        return _groups;;        
    }


    public bool IsUserMemberOfGroup( string strUser, string Group)
    {

        List<string> Groups = new List<string>();

        //initialize the directory entry object 
        DirectoryEntry dirEntry = new DirectoryEntry(_path);

        //directory searcher
        DirectorySearcher dirSearcher = new DirectorySearcher(dirEntry);

        //enter the filter
        dirSearcher.Filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", strUser);

        //get the member of properties for the search result
        dirSearcher.PropertiesToLoad.Add("memberOf");
        int propCount;
        SearchResult dirSearchResults = dirSearcher.FindOne();
        propCount = dirSearchResults.Properties["memberOf"].Count;
        string dn;
        int equalsIndex;
        int commaIndex;
        for (int i = 0; i <= propCount - 1; i++)
        {
            dn = dirSearchResults.Properties["memberOf"][i].ToString();

            equalsIndex = dn.IndexOf("=", 1);
            commaIndex = dn.IndexOf(",", 1);
            if (equalsIndex == -1)
            {
                return false;
            }
            if (!Groups.Contains(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1)))
            {
                Groups.Add(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
            }
        }

        try
        {
            /////////////////////////////////////////////////
            // Get An Active Directory Entry For User Name //
            /////////////////////////////////////////////////
            DirectoryEntry obEntry = new DirectoryEntry(_path);
            DirectorySearcher search = new DirectorySearcher(obEntry);
            SearchResult result = search.FindOne();

            ///////////////////////////////////////////////////////////
            // If The DIrectory Was Found, Search For THe Group Name //
            ///////////////////////////////////////////////////////////
            if (null != result)
            {
                DirectoryEntry obUser = new DirectoryEntry(result.Path);

                ///////////////////
                // Search Groups //
                ///////////////////
                object obGroups = obUser.Invoke("Groups");
                foreach (object ob in (IEnumerable)obGroups)
                {
                    // Create object for each group.
                    DirectoryEntry obGpEntry = new DirectoryEntry(ob);

                     if (obGpEntry.Name.ToLower()  == Group )
                         return true;
                }
            }
        }
        catch (Exception ex)
        {
            Trace.Write(ex.Message);
        }

        return false;
    }
    





}




/// http://www.codeproject.com/Articles/36670/Active-Directory-Forms-Authentication-User-IsInRol   ///
/// 
public class CustomActiveDirectoryRoleProvider : RoleProvider
{
    string _connectionString = string.Empty;
    string _applicationName = string.Empty;
    string _userName = string.Empty;
    string _userPassword = string.Empty;

    public void Initialize(string UserName, string PWD)
    {
        _connectionString = "LDAP://10.6.1.11/";
        _userName = UserName;
        _userPassword = PWD;
        
        base.Initialize("CustomRoleProvider", null);
    }

    public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
    {
        _connectionString = config["connectionStringName"];

        if (!string.IsNullOrEmpty(config["applicationName"]))
            _applicationName = config["applicationName"];

        if (!string.IsNullOrEmpty(config["connectionUsername"]))
            _userName = config["connectionUsername"];

        if (!string.IsNullOrEmpty(config["connectionPassword"]))
            _userPassword = config["connectionPassword"];

        base.Initialize(name, config);
    }

    public override string ApplicationName {
        get {
            return _applicationName;
        }
        set {
            _applicationName = value;
        }
    }

    public override string[] GetRolesForUser(string userName) 
    {
        userName = RemoveADGroup(userName);
        return GetUserRoles(userName);
    }

    string RemoveADGroup(string name) {
        string[] ary = name.Split(new char[] { '\\' });
        return ary[ary.Length - 1];
    }



    string[] GetUserRoles(string userName) 
    {
        DirectoryEntry obEntry = new DirectoryEntry(WebConfigurationManager.ConnectionStrings[_connectionString].ConnectionString,_userName, _userPassword);
        DirectorySearcher srch = new DirectorySearcher(obEntry, "(sAMAccountName=" + userName + ")");

        SearchResult res = srch.FindOne();

        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        if (null != res) {
            DirectoryEntry obUser = new DirectoryEntry
			(res.Path, _userName, _userPassword);

            string rootPath = WebConfigurationManager.ConnectionStrings[_connectionString].ConnectionString;
            rootPath = rootPath.Substring(0,rootPath.LastIndexOf(@"/") + 1);

            GetMemberships(obUser, dictionary, rootPath);
        }

        string[] ary = new string[dictionary.Count];
        dictionary.Values.CopyTo(ary, 0);
        return ary;
    }

    void GetMemberships(DirectoryEntry entry, Dictionary<string, string> dictionary, string rootPath) 
    {
        List<DirectoryEntry> childrenToCheck = new List<DirectoryEntry>();
        PropertyValueCollection children = entry.Properties["memberOf"];
        foreach (string childDN in children) {
            if (!dictionary.ContainsKey(childDN)) {
                DirectoryEntry obGpEntry = 
		new DirectoryEntry(rootPath + childDN, _userName, _userPassword);
                string groupName = 
		obGpEntry.Properties["sAMAccountName"].Value.ToString();

                dictionary.Add(childDN, groupName);
                childrenToCheck.Add(obGpEntry);
            }
        }
        foreach (DirectoryEntry child in childrenToCheck) {
            GetMemberships(child, dictionary, rootPath);
        }
    }
    
    
    public override void AddUsersToRoles(string[] usernames, string[] roleNames) 
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void CreateRole(string roleName) 
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) 
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override string[] FindUsersInRole(string roleName, string usernameToMatch) 
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override string[] GetAllRoles()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override string[] GetUsersInRole(string roleName)
    {
        return GetGroupMembers(RemoveADGroup(roleName));
    }


    public override bool IsUserInRole(string username, string roleName)
    {
        string[] ary = GetRolesForUser(username);
        foreach (string s in ary)
        {
            if (roleName.ToLower() == s.ToLower())
                return true;
        }
        return false;
    }


    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) 
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool RoleExists(string roleName) 
    {
        throw new Exception("The method or operation is not implemented.");
    }

    string[] GetGroupMembers(string groupName)
    {
        DirectoryEntry obEntry = new DirectoryEntry(
            WebConfigurationManager.ConnectionStrings[_connectionString].ConnectionString,
            _userName, _userPassword);
        DirectorySearcher srch = new DirectorySearcher
            (obEntry, "(sAMAccountName=" + groupName + ")");
        SearchResult res = srch.FindOne();

        Dictionary<string, string> groups = new Dictionary<string, string>();
        Dictionary<string, string> members = new Dictionary<string, string>();

        if (null != res)
        {
            DirectoryEntry entry = new DirectoryEntry(res.Path, _userName, _userPassword);

            GetMembers(entry, groups, members);
        }
        string[] ary = new string[members.Count];
        members.Values.CopyTo(ary, 0);
        return ary;
    }

    void GetMembers(DirectoryEntry entry,Dictionary<string, string> groups, Dictionary<string, string> members)
    {
        List<DirectoryEntry> childrenToCheck = new List<DirectoryEntry>();
        object children = entry.Invoke("Members", null);

        foreach (object childObject in (IEnumerable)children)
        {
            DirectoryEntry child = new DirectoryEntry(childObject);
            string type = getEntryType(child);
            if (type == "G" && !groups.ContainsKey(child.Path))
            {
                childrenToCheck.Add(child);
            }
            else if (type == "P" && !members.ContainsKey(child.Path))
            {
                members.Add(child.Path, child.Properties
            ["sAMAccountName"].Value.ToString());
            }
        }
        foreach (DirectoryEntry child in childrenToCheck)
        {
            GetMembers(child, groups, members);
        }
    }

    string getEntryType(DirectoryEntry inEntry)
    {
        if (inEntry.Properties.Contains("objectCategory"))
        {
            string fullValue = inEntry.Properties["objectCategory"].Value.ToString();
            if (fullValue.StartsWith("CN=Group"))
                return "G";
            else if (fullValue.StartsWith("CN=Person"))
                return "P";
        }
        return "";
    }
} 