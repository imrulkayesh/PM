using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using PM.Model;
using PM.ModelsV;

namespace PM.Controllers
{
    public class MailSettingController : Controller
    {
        private PMEntities db = new PMEntities();
        public ActionResult Index()
        {
            string sql_1 = @"SELECT LICENSE_ID, LICENSE_NUMBER, LICENSE_HOLDER_EMAIL, REMAINING, ISSUE_DATE, EXPIRE_DATE
                   FROM VW_LIC_EXPIRE_DATE_COUNT
                   WHERE REMAINING IN (45, 30, 7)";
            var LicenseList = db.Database.SqlQuery<LicenseExpireInfo>(sql: sql_1);
            ViewBag.Message = "License List";
            var R = LicenseList.Take(500).ToList();
            return View(R);
        }

        //[HttpPost]
        //public ActionResult SendEmail()
        //{

        //    List<string> toUser = new List<string> { "mis34@mis.prangroup.com" };
        //    List<string> ccUser = new List<string> { "mis93@mis.prangroup.com" };

        //    string Server_ID = "172.17.2.29";
        //    string senderDisplayName = "License Notification";
        //    string senderDisplayEmail = "BankGuarantee@Prangroup.com";
        //    string senderAccount = ""; // Or any account value
        //    string senderCredential = ""; // Or any credential value

        //    MailMessage email = new MailMessage();
        //    email.IsBodyHtml = true;
        //    email.From = new MailAddress(senderDisplayEmail, senderDisplayName);

        //    try
        //    {
        //        foreach (var item in toUser)
        //        {
        //            email.To.Add(item);
        //        }
        //        foreach (var item in ccUser)
        //        {
        //            email.CC.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", new HandleErrorInfo(ex, "ControllerName", "sendMail"));
        //    }

        //    email.Subject = "Mail Subject";
        //    email.Body = "Mail Body";
        //    email.Body += "<br>Thanks & Regards,<br><b>CS | SW | MIS</b> <br/><br/>";
        //    email.Body += "<b style='color: red;font-style: italic;'>This is a system Generated Mail. Please do not reply to it.</b> <br/>";

        //    var mailSend = new System.Net.Mail.SmtpClient
        //    {
        //        DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
        //        Host = Server_ID,
        //        Timeout = 600000
        //    };

        //    try
        //    {
        //        mailSend.Send(email);
        //        return Json(new { success = true, message = "Email sent successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = "Email sending failed. " + ex.Message });
        //    }
        //}

        [HttpPost]
        public ActionResult SendEmail()
        {
            string sql = @"SELECT LICENSE_ID, LICENSE_NUMBER, LICENSE_HOLDER_EMAIL, REMAINING, ISSUE_DATE, EXPIRE_DATE
                   FROM VW_LIC_EXPIRE_DATE_COUNT
                   WHERE REMAINING IN (45, 30, 7)";

            // Execute the query and retrieve the data
            var data = db.Database.SqlQuery<LicenseExpireInfo>(sql).ToList();

            List<string> toUser = new List<string>();
            List<string> ccUser = new List<string>();

            foreach (var item in data)
            {
                if (item.REMAINING == 45 || item.REMAINING == 30 || item.REMAINING == 7)
                {
                    toUser.Add(item.LICENSE_HOLDER_EMAIL); 
                }
                //else if (item.REMAINING == 35 || item.REMAINING == 7)
                //{
                //    ccUser.Add(item.LICENSE_HOLDER_EMAIL); 
                //}
            }

            string senderDisplayName = "License Notification";
            string senderDisplayEmail = "BankGuarantee@Prangroup.com";
            string senderAccount = ""; 
            string senderCredential = ""; 

            // Set up the SMTP client to send the email
            var mailSend = new System.Net.Mail.SmtpClient
            {
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                Host = "172.17.2.29", // SMTP server
                Timeout = 600000
            };

            // Loop through each license and send an email individually
            foreach (var item in data)
            {
                // Prepare the email message
                MailMessage email = new MailMessage();
                email.IsBodyHtml = true;
                email.From = new MailAddress(senderDisplayEmail, senderDisplayName);

                try
                {
                    // Add the recipient for this specific license holder
                    email.To.Add(item.LICENSE_HOLDER_EMAIL);

                    // Add CC recipients (if necessary)
                    foreach (var cc in ccUser)
                    {
                        email.CC.Add(cc);
                    }
                }
                catch (Exception ex)
                {
                    // If an error occurs while adding recipients, return error
                    return View("Error", new HandleErrorInfo(ex, "ControllerName", "SendEmail"));
                }

                // Set up the email subject and body for this specific license holder
                email.Subject = "License Expiry Reminder";
                email.Body = "Dear License Holder,<br><br>";
                email.Body += "This is a reminder that your license will expire on " + item.EXPIRE_DATE?.ToString("dd-MMM-yyyy") + ". License number is: " + item.LICENSE_NUMBER + ".<br>";
                email.Body += "Please take the necessary actions.<br><br>";
                email.Body += "Thanks and Regards,<br><b>PRAN RFL Group</b><br>";
                email.Body += "<b style='color: red; font-style: italic;'>This is a system-generated email. Please do not reply to it.</b><br>";

                // Try sending the email for this specific license holder
                try
                {
                    mailSend.Send(email);
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "swal('Ohh..','Email sending failed for license','error');";
                    return RedirectToAction("Index");
                }
            }

            TempData["msg"] = "swal('Success..','Emails sent successfully.','success');";
            return RedirectToAction("Index");
        }
    }
}