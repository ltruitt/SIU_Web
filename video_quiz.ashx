<%@ WebHandler Language="C#" Class="video_quiz" %>

using System;
using System.Web;

//  http://www.dotnetperls.com/ashx
//  http://captivatedev.com/2009/12/19/storing-adobe-captivate-4-quiz-results-using-asp-net/


public class video_quiz : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}