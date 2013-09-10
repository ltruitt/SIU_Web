using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Safety_SafetyMeeting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) { }

    [WebMethod(EnableSession = false)]
    public static string MovieStart(string movieId, string pos, string dur)
    {
        string VideoFolder = string.Empty;
        string VideoName = string.Empty;
        int splitIdx = 0;

        splitIdx = movieId.LastIndexOf('/');

        if (splitIdx > 0)
        {
            VideoName = movieId.Substring(splitIdx + 1);
            VideoFolder = movieId.Substring(0, splitIdx);
        }
        else
        {
            VideoName = movieId;
            VideoFolder = "";
        }


        if (VideoName[VideoName.Length - 4] == '.')
            VideoName = VideoName.Substring(0, VideoName.Length - 4);

        SqlServer_Impl.LogVideoWatch(BusinessLayer.UserEmpID, VideoName, VideoFolder, "0", "0");

        return "";
    }





    [WebMethod(EnableSession = false)]
    public static string MovieComplete(string movieId)
    {
        int splitIdx = movieId.LastIndexOf('/');

        string VideoName = movieId.Substring(splitIdx + 1);
        if (VideoName[VideoName.Length - 4] == '.')
            VideoName = VideoName.Substring(0, VideoName.Length - 4);

        string VideoFolder = movieId.Substring(0, splitIdx);

        SqlServer_Impl.LogVideoWatch(BusinessLayer.UserEmpID, VideoName, VideoFolder, "0", "0", true);

        return "";
    }
}