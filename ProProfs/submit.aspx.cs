using System;
using ShermcoYou.DataTypes;

/*
# Variable Name Variable Description Data Type Example (PHP) 
1. result_id                    A unique ID to identify attempt 
2. status_id                    New: Refers to a new attempt.                                                                   
                                Update: Refers to a score update on an existing attempt such as an instructor grading essay or assigning bonus points to update a student's score. 
                                Update existing score with the help of result_id in such a case.
3. quiz_id                      A unique ID which will identify your quiz. INT $_REQUEST['quiz_id'] 
4. quiz_name                    Name of your quiz. VARCHAR $_REQUEST['quiz_name'] 
5. attempt_date                 The date and time stamp of the attempt (UNIX TIMESTAMP)
6. total_marks                  Total number of marks you set for the quiz. Eg: 100. 
7. user_obtained_marks          Total number of marks obtained by the quiz taker. 
8. user_percent_marks           Percentage marks of quiz taker. 
9. user_totalcorrect_answers    Total number of correct answers by quiz taker. 
10. user_totalwrong_answers     Total number of wrong answers/anwer by the quiz taker.
11. user_Id                     Quiz taker's ID
12. user_Email                  Quiz taker's email address.
13. user_Address                Quiz taker's address.
14. user_City                   Quiz taker's city  
15. user_State                  Quiz taker's state
16. user_Zipcode                Quiz taker's zipcode
17. user_Phone                  Quiz taker's phone number
18. user_name                   Quiz taker's name
*/


// http://localhost/proprofs/submit.aspx?result_id=39165507&user_name=TRUITT,LARRY&total_marks=100&attempt_date=1379100594&user_obtained_marks=100&user_percent_marks=100&user_totalcorrect_answers=2&user_totalwrong_answers=0&user_Id=10371&user_Email=&user_Address=&user_City=N%2FA&user_State=N%2FA&user_Zipcode=N/A&user_Phone=&quiz_id=528021&quiz_name=SI+Test+Quiz+1&status=new


public partial class ProProfs_submit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;



        string quizId = Request["quiz_id"];
        string quizName = Request["quiz_name"];
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

        string dbgRcd = "(" + userId + ")   (" + quizName + ") " + userName + "  " + userPercentMarks + " pct  +" + userTotalcorrectAnswers + " -" + userTotalwrongAnswers;
        SqlServer_Impl.LogDebug("ProProfs_submit", dbgRcd);

        //////////////////////////////////////////////////////////////////////////////////
        // A Cludge -- Passed Traing Log ID and Employee ID to PreProfs as single field //
        // Now we need to split them back out                                           //
        // But we gotta pass the TL_UID down the line, so we pass it in zip             //
        //////////////////////////////////////////////////////////////////////////////////
        string[] uidEmp = userId.Split(':');
        userId = uidEmp[0];
        userZipcode = uidEmp[1];

        ///////////////////
        // Clean Up Data //
        ///////////////////
        if (quizId ==  null) quizId = "0";
        if (quizId.Length == 0) quizId = "0";
        if (quizName == null) quizName = "unknown";
        if (userName == null) userName = "unknown";
        if (totalMarks == null) totalMarks = "0";
        if (userObtainedMarks == null) userObtainedMarks = "0";
        if (userPercentMarks == null) userPercentMarks = "0";
        if (userTotalcorrectAnswers == null) userTotalcorrectAnswers = "0";
        if (userTotalwrongAnswers == null) userTotalwrongAnswers = "0";
        if (userId == null) userId = "";
        if (userEmail == null) userEmail = "";
        if (userAddress == null) userAddress = "";
        if (userCity == null) userCity = "";
        if (userState == null) userState = "";
        if (userZipcode == null) userZipcode = "";
        if (userPhone == null) userPhone = "0";
        if (userPhone.Length == 0) userPhone = "0";

        SIU_Training_Quiz testRcd = new SIU_Training_Quiz
        {
            Quiz_ID = int.Parse(quizId),
            Quiz_Name = quizName,
            Attempt_Date = DateTime.Now,
            User_Name = userName,
            Total_Marks = int.Parse(totalMarks),
            User_Marks = int.Parse(userObtainedMarks),
            User_Pct_Marks = int.Parse(userPercentMarks),
            Total_Correct = int.Parse(userTotalcorrectAnswers),
            Total_Wrong = int.Parse(userTotalwrongAnswers),
            User_ID = userId,
            User_Email = userEmail,
            User_Address = userAddress,
            User_City = userCity,
            User_State = userState,
            User_Zipcode = userZipcode,
            User_Phone = int.Parse(userPhone)
        };

        SqlServer_Impl.RecordTest(testRcd);
    }    
}