using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using ShermcoYou.DataTypes;
using MailMessage = System.Net.Mail.MailMessage;


//using System.Net;
//using System.Net.Mail;

//var fromAddress = new MailAddress("from@gmail.com", "From Name");
//var toAddress = new MailAddress("to@example.com", "To Name");
//const string fromPassword = "fromPassword";
//const string subject = "Subject";
//const string body = "Body";

//var smtp = new SmtpClient
//           {
//               Host = "smtp.gmail.com",
//               Port = 587,
//               EnableSsl = true,
//               DeliveryMethod = SmtpDeliveryMethod.Network,
//               UseDefaultCredentials = false,
//               Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
//           };
//using (var message = new MailMessage(fromAddress, toAddress)
//                     {
//                         Subject = subject,
//                         Body = body
//                     })
//{
//    smtp.Send(message);
//}
//MailMessage message = new MailMessage("ltruitt@shermco.com", txtTo, Subject, MailMessage);
// Create mailing addresses 
// that includes a UTF8 character in the display name.
// MailAddress from = new MailAddress("jane@contoso.com",  "Jane " + (char)0xD8+ " Clayton", 
//MailAddress from = new MailAddress("ltruitt@Shermco.com");
//MailAddress to = new MailAddress("JobCompleted@Shermco.com");
//MailAddress cc = new MailAddress("ltruitt@Shermco.com");

// Specify the message content.
//MailMessage message = new MailMessage(from, to);

//message.Body = MailMessage;
//message.BodyEncoding = System.Text.Encoding.UTF8;


// Include some non-ASCII characters in body and subject.
//string someArrows = new string(new char[] {'\u2190', '\u2191', '\u2192', '\u2193'});
//message.Body += Environment.NewLine + someArrows;
//message.Subject = Subject;
//message.SubjectEncoding = System.Text.Encoding.UTF8;


// Set the method that is called back when the send operation ends.
//client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

// The userState can be any object that allows your callback 
// method to identify this send operation.
// For this example, the userToken is a string constant.
//string userState = "test message1";
//client.SendAsync(message, userState);





//string answer = Console.ReadLine();
//// If the user canceled the send, and mail hasn't been sent yet,
//// then cancel the pending operation.
//if (answer.StartsWith("c") && mailSent == false)
//{
//    client.SendAsyncCancel();
//}
// Clean up.
//message.Dispose();





public static class WebMail
{
    public static void NetMail(string txtTo, string Subject, string MailMessage)
    {
        ////////////////////////////////////
        // Use Web Config For Config Info //
        ////////////////////////////////////
        SmtpClient client = new SmtpClient {UseDefaultCredentials = false};

        if ( ! SqlServer_Impl.isProdDB )
        {
            string prefixMessage = "The Following Email was generated on the SiYOU! Development Web Site........" + Environment.NewLine;
            prefixMessage += "The email would have been directed to (" + txtTo + ") if it was processed on the production web site." + Environment.NewLine + Environment.NewLine;
            prefixMessage += MailMessage;

            MailMessage = prefixMessage;
            txtTo = BusinessLayer.UserEmail;
        }


        try
        {
            client.Send(new MailMessage("noreply@shermco.com", txtTo, Subject, MailMessage));
        }
        catch (Exception e)
        {
            SqlServer_Impl.LogDebug("NetMail", e.Message);
        }        
    }

    public static void HtmlMail(string txtTo, string Subject, string MailMessage)
    {
        ////////////////////////////////////
        // Use Web Config For Config Info //
        ////////////////////////////////////
        SmtpClient client = new SmtpClient { UseDefaultCredentials = false };
        MailMessage msgMail = new MailMessage();


        if (!SqlServer_Impl.isProdDB)
        {
            string prefixMessage = "<div style='border: 2 solid black; font-size: 1.3em; font-weight: bold; margin: 10px;'>The Following Email was generated on the SiYOU! Development Web Site........<br/>";
            prefixMessage += "The email would have been directed to (" + txtTo + ") if it was processed on the production web site.</div>";
            prefixMessage += MailMessage;

            MailMessage = prefixMessage;
            txtTo = BusinessLayer.UserEmail;
        }


        string[] toAddressList = txtTo.Split(';');
        foreach (string toaddress in toAddressList)
        {
            if (toaddress.Length > 0)
            {
                msgMail.To.Add(toaddress);
            }
        }


        msgMail.From = new MailAddress("noreply@shermco.com", "Vehicle Inspection Report");
        msgMail.Subject = Subject;
        msgMail.Body = MailMessage;
        msgMail.IsBodyHtml = true;

        try
        {
            client.Send(msgMail);
        }
        catch (Exception e)
        {
            SqlServer_Impl.LogDebug("HtmlMail", e.Message);
        }
    }

    public static void TestEmail(string addressList)
    {
        const string eMailSubject = "SIU EMAIL TEST";
        addressList = "ltruitt@shermco.com, truittjl@texassdc.com, truittjl@tx.rr.com";

        string emailBody = "This is an SIU Web site test message" + Environment.NewLine + Environment.NewLine;

        emailBody += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890" + Environment.NewLine + Environment.NewLine;

        NetMail(addressList, eMailSubject, emailBody);
    }
}