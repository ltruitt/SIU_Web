<%@ WebService Language="C#" Class="ProProfs" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

using System.Configuration;
using System.Reflection;

[WebService(Namespace = "http://localhost/ProProfs")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class ProProfs  : System.Web.Services.WebService 
{


#region Database Connection
    public static string SqlServerProdNvdbConnectString
    {
        get
        {
            ////////////////////////////////////////////
            // Get the application configuration file //
            ////////////////////////////////////////////
            System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");

            ///////////////////////////////////////
            // Get The Sql Server Connect STring //
            ///////////////////////////////////////
            string ConnectStringName = "ShermcoConnectionString";

            //DeveloperConfigurationSection ConfigSection = (DeveloperConfigurationSection)System.Configuration.ConfigurationManager.GetSection(DeveloperConfigurationSection.ConfigurationSectionName);

            //if (ConfigSection.Enabled)
            //    ConnectStringName = ConfigSection.UseConnectString;


            ConnectionStringsSection csSection = config.ConnectionStrings;
            return csSection.ConnectionStrings[ConnectStringName].ConnectionString;
        }
    }
#endregion

    [WebMethod]
    public void RecordTest( string quiz_id, 
                            string quiz_name, 
                            string attempt_date,
                            string user_name,
                            string total_marks,
                            string user_obtained_marks,
                            string user_percent_marks,
                            string user_totalcorrect_answers,
                            string user_totalwrong_answers,
                            string user_Id,
                            string user_Email,
                            string user_Address,
                            string user_City,
                            string user_State,
                            string user_Zipcode,
                            string user_Phone)
    {
        SqlParameter param;
               
        try
        {

            using (SqlConnection con = new SqlConnection(SqlServerProdNvdbConnectString))
            {
                using (SqlCommand cmd = new SqlCommand("SIU_Record_ProProfs", con))
                {
                    if (quiz_id.Length == 0) quiz_id = "0";
                    if (quiz_id.ToLower() == "null" ) quiz_id = "0";
                    param = new SqlParameter("@quiz_id", SqlDbType.Int);
                    param.Value = int.Parse(quiz_id);
                    cmd.Parameters.Add(param);

                    if (quiz_name.ToLower() == "null") quiz_name = "unknown";
                    param = new SqlParameter("@quiz_name", SqlDbType.VarChar);
                    param.Value = quiz_name;
                    cmd.Parameters.Add(param);

                    if (attempt_date.ToLower() == "null") attempt_date = "1/1/2001";
                    attempt_date = "1/1/2001";
                    param = new SqlParameter("@attempt_date", SqlDbType.DateTime);
                    param.Value = attempt_date;
                    cmd.Parameters.Add(param);

                    if (total_marks.ToLower() == "null") total_marks = "0";
                    param = new SqlParameter("@total_marks", SqlDbType.Int);
                    param.Value = int.Parse(total_marks);
                    cmd.Parameters.Add(param);

                    if (user_obtained_marks.ToLower() == "null") user_obtained_marks = "0";
                    param = new SqlParameter("@user_marks", SqlDbType.Int);
                    param.Value = int.Parse(user_obtained_marks);
                    cmd.Parameters.Add(param);

                    if (user_percent_marks.ToLower() == "null") user_percent_marks = "0";
                    param = new SqlParameter("@user_pct_marks", SqlDbType.Int);
                    param.Value = user_percent_marks;
                    cmd.Parameters.Add(param);

                    if (user_totalcorrect_answers.ToLower() == "null") user_totalcorrect_answers = "0";
                    param = new SqlParameter("@total_correct", SqlDbType.Int);
                    param.Value = int.Parse(user_totalcorrect_answers);
                    cmd.Parameters.Add(param);

                    if (user_totalwrong_answers.ToLower() == "null") user_totalwrong_answers = "0";
                    param = new SqlParameter("total_wrong", SqlDbType.Int);
                    param.Value = int.Parse(user_totalwrong_answers);
                    cmd.Parameters.Add(param);

                    if (user_Id.ToLower() == "null") user_Id = "";
                    param = new SqlParameter("@user_Id", SqlDbType.VarChar);
                    param.Value = user_Id;
                    cmd.Parameters.Add(param);

                    if (user_Email.ToLower() == "null") user_Email = "";
                    param = new SqlParameter("@user_Email", SqlDbType.VarChar);
                    param.Value = user_Email;
                    cmd.Parameters.Add(param);

                    if (quiz_id.ToLower() == "null") quiz_id = "0";
                    param = new SqlParameter("@user_Address", SqlDbType.VarChar);
                    param.Value = user_Address;
                    cmd.Parameters.Add(param);

                    if (user_City.ToLower() == "null") user_City = "";
                    param = new SqlParameter("@user_City", SqlDbType.VarChar);
                    param.Value = user_City;
                    cmd.Parameters.Add(param);

                    if (user_State.ToLower() == "null") user_State = "";
                    param = new SqlParameter("@user_State", SqlDbType.VarChar);
                    param.Value = user_State;
                    cmd.Parameters.Add(param);

                    if (user_Zipcode.ToLower() == "null") user_Zipcode = "";
                    param = new SqlParameter("@user_Zipcode", SqlDbType.VarChar);
                    param.Value = user_Zipcode;
                    cmd.Parameters.Add(param);

                    if (user_Phone.ToLower() == "null") user_Phone = "0";
                    if (user_Phone.Length == 0) user_Phone = "0";
                    param = new SqlParameter("@user_Phone", SqlDbType.Int);
                    param.Value = int.Parse(user_Phone);
                    cmd.Parameters.Add(param);

                    if (user_name.ToLower() == "null") user_name = "";
                    param = new SqlParameter("@user_name", SqlDbType.VarChar);
                    param.Value = user_name;
                    cmd.Parameters.Add(param);                    

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }


            return;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }    
    
}