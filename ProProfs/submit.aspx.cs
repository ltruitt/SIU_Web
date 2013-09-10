using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;



public class DeveloperConfigurationSection : ConfigurationSection
{
    public const String ConfigurationSectionName = "DeveloperSettings";

    [ConfigurationProperty("enabled", DefaultValue = true)]
    public bool Enabled
    {
        get
        {
            return (bool)base["enabled"];
        }
        set
        {
            base["enabled"] = value;
        }
    }

    [ConfigurationProperty("useConnectString", DefaultValue = "ShermcoConnectionString")]
    public string UseConnectString
    {
        get
        {
            return (string)base["useConnectString"];
        }
        set
        {
            base["useConnectString"] = value;
        }
    }
}






public partial class ProProfs_submit : System.Web.UI.Page
{


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
            const string connectStringName = "ShermcoConnectionString";

            //DeveloperConfigurationSection ConfigSection = (DeveloperConfigurationSection)System.Configuration.ConfigurationManager.GetSection(DeveloperConfigurationSection.ConfigurationSectionName);

            //if (ConfigSection.Enabled)
            //    ConnectStringName = ConfigSection.UseConnectString;


            ConnectionStringsSection csSection = config.ConnectionStrings;
            return csSection.ConnectionStrings[connectStringName].ConnectionString;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;

        SqlParameter param;

        try
        {
            string quizId = Request["quiz_id"];
            string quizName = Request["quiz_name"];
            string attemptDate = Request["attempt_date"];
            string userName = Request["user_name"];
            string totalMarks = Request["total_marks"];
            string userObtainedMarks = Request["user_obtained_marks"];
            string userPercentMarks = Request["user_percent_marks"];
            string userTotalcorrectAnswers = Request["user_totalcorrect_answers"];
            string userTotalwrongAnswers = Request["user_totalwrong_answers"];
            string userEmail = Request["user_Email"];
            string userAddress = Request["user_Address"];
            string userCity = Request["user_City"];
            string userState = Request["user_State"];
            string userZipcode = Request["user_Zipcode"];
            string userPhone = Request["user_Phone"];
            string userId = Request["user_Id"];

            using (SqlConnection con = new SqlConnection(SqlServerProdNvdbConnectString))
            {
                using (SqlCommand cmd = new SqlCommand("SIU_Record_ProProfs", con))
                {
                    if (quizId.Length == 0) quizId = "0";
                    if (quizId.ToLower() == "null") quizId = "0";
                    param = new SqlParameter("@quiz_id", SqlDbType.Int) {Value = int.Parse(quizId)};
                    cmd.Parameters.Add(param);

                    if (quizName.ToLower() == "null") quizName = "unknown";
                    param = new SqlParameter("@quiz_name", SqlDbType.VarChar) {Value = quizName};
                    cmd.Parameters.Add(param);

                    if (attemptDate.ToLower() == "null") attemptDate = "1/1/2001";
                    param = new SqlParameter("@attempt_date", SqlDbType.DateTime) {Value = attemptDate};
                    cmd.Parameters.Add(param);

                    if (totalMarks.ToLower() == "null") totalMarks = "0";
                    param = new SqlParameter("@total_marks", SqlDbType.Int) {Value = int.Parse(totalMarks)};
                    cmd.Parameters.Add(param);

                    if (userObtainedMarks.ToLower() == "null") userObtainedMarks = "0";
                    param = new SqlParameter("@user_marks", SqlDbType.Int) {Value = int.Parse(userObtainedMarks)};
                    cmd.Parameters.Add(param);

                    if (userPercentMarks.ToLower() == "null") userPercentMarks = "0";
                    param = new SqlParameter("@user_pct_marks", SqlDbType.Int) {Value = userPercentMarks};
                    cmd.Parameters.Add(param);

                    if (userTotalcorrectAnswers.ToLower() == "null") userTotalcorrectAnswers = "0";
                    param = new SqlParameter("@total_correct", SqlDbType.Int) { Value = int.Parse(userTotalcorrectAnswers) };
                    cmd.Parameters.Add(param);

                    if (userTotalwrongAnswers.ToLower() == "null") userTotalwrongAnswers = "0";
                    param = new SqlParameter("total_wrong", SqlDbType.Int) {Value = int.Parse(userTotalwrongAnswers)};
                    cmd.Parameters.Add(param);

                    if (userId.ToLower() == "null") userId = "";
                    param = new SqlParameter("@user_Id", SqlDbType.VarChar) {Value = userId};
                    cmd.Parameters.Add(param);

                    if (userEmail.ToLower() == "null") userEmail = "";
                    param = new SqlParameter("@user_Email", SqlDbType.VarChar) {Value = userEmail};
                    cmd.Parameters.Add(param);

                    if (userAddress.ToLower() == "null") userAddress = "0";
                    param = new SqlParameter("@user_Address", SqlDbType.VarChar) {Value = userAddress};
                    cmd.Parameters.Add(param);

                    if (userCity.ToLower() == "null") userCity = "";
                    param = new SqlParameter("@user_City", SqlDbType.VarChar) {Value = userCity};
                    cmd.Parameters.Add(param);

                    if (userState.ToLower() == "null") userState = "";
                    param = new SqlParameter("@user_State", SqlDbType.VarChar) {Value = userState};
                    cmd.Parameters.Add(param);

                    if (userZipcode.ToLower() == "null") userZipcode = "";
                    param = new SqlParameter("@user_Zipcode", SqlDbType.VarChar) {Value = userZipcode};
                    cmd.Parameters.Add(param);

                    if (userPhone.ToLower() == "null") userPhone = "0";
                    if (userPhone.Length == 0) userPhone = "0";
                    param = new SqlParameter("@user_Phone", SqlDbType.Int) {Value = int.Parse(userPhone)};
                    cmd.Parameters.Add(param);

                    if (userName.ToLower() == "null") userName = "";
                    param = new SqlParameter("@user_name", SqlDbType.VarChar) {Value = userName};
                    cmd.Parameters.Add(param);

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }    
}