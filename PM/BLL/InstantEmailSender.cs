using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
namespace PM.BLL
{
    public class InstantEmailSender
    {
        ReturnPermission RP = new ReturnPermission();
        private bool Send_eMail(string Mail_Subject, string Mail_Body, List<string> TO_Users, List<string> CC_Users)
        {
            string Server_ID = "172.17.2.12";
            int Server_Port = 465;
            string Sender_Display_Name = "PM-Notification";
            string Sender_Account = "lsms@prangroup.com";
            string Sender_Credential = "@dm!n@#sysmail";
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.From = new MailAddress(Sender_Account, Sender_Display_Name);
            foreach (var item in TO_Users)
            {
                mail.To.Add(item);
            }
            foreach (var item in CC_Users)
            {
                mail.CC.Add(item);
            }
            //Common Id
            List<Email_Id> EMAIL_LIST = new List<Email_Id>();
            List<string> GR1 = new List<string>();
            GR1.Add("GR0001");
            EMAIL_LIST = RP.EmailListByGroupType(Group: GR1, TypeS: "ADMIN");
            if (EMAIL_LIST.Count > 0)
            {
                foreach (var item in EMAIL_LIST)
                {
                    mail.CC.Add(item.SMTP);
                }
            }
            //Common Id END

            mail.Subject = Mail_Subject;
            mail.Body = Mail_Body;
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Host = Server_ID;
                smtp.Port = Server_Port;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Sender_Account, Sender_Credential);
                smtp.EnableSsl = false;
                smtp.Timeout = 600000;
            }
            try
            {
                //NEVER_EAT_POISON_Disable_CertificateValidation();
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        static void NEVER_EAT_POISON_Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                )
                {
                    return true;
                };
        }
        public bool Send_Email_User_Open(string StaffName, string LoginId, string LoginPass, string MailId)
        {
            string MailSubject = "New PM User ~ " + StaffName;
            string MailBody = string.Format(@"<p>Welcome, <strong> {0}!</strong></p>
                                            <p>Login ID: <strong>{1}</strong></p>
                                            <p>Default Password: <strong>{2}</strong> </p>
                                            <p>Remember your Login ID and Change your default password after first login.</p>
                                            <p>Application Link- <a href='http://pmc.prangroup.com/pm'>PMC</a> </p>
                                            <br/>
                                            <p>Thank you</p>
                                            <hr/>
                                            <p>This is a system generated email, do not reply to this email.</p>"
                                             , StaffName, LoginId, LoginPass);
            List<string> TO = new List<string>();
            TO.Add(MailId);
            List<string> CC = new List<string>();
            bool IsSend = Send_eMail(MailSubject, MailBody, TO, CC);
            if (IsSend)
            {
                return true;
            }
            else
            {
                return false;
            };
        }
        public bool Send_Email_KSA_Asset_HandOver(string Request_Type, string Transfer_Id)
        {
            var gd = RP.TransferInfo(Transfer_Id);
            List<string> BU1 = new List<string>();
            BU1.Add(gd[21]);
            List<string> GR1 = new List<string>();
            GR1.Add("GR0001");
            var EMAIL_LIST = RP.EmailListByGroup(Group: GR1, BU: BU1, TypeS: "TRNS_RETN");
            if (EMAIL_LIST.Count < 1)
            {
                return false;
            }
            string RT = "Unknown";
            if (Request_Type == "H")
            {
                RT = "Handover";
            }
            if (RT == "R")
            {
                RT = "Return";
            }
            if (RT == "Unknown")
            {
                return false;
            }
            string MailSubject = RT + " ~ " + gd[9];
            string MailBody = string.Format(@"<strong>{0}-{1}</strong> is seeking your <strong>{2}</strong> approval </p><p>User: {3}-{4}</p>
            Link -<a href='http://pmc.prangroup.com/PM/AssetUserManager/TransferWaitingList'>View Request</a> <table border='1'> <tbody> <tr> <th colspan='2' align='center'>Asset Info</th> <th align='center'>User Info</th> <th align='center'>Working Info</th> </tr><tr> <td>Owner:</td><td>{5}</td><td>{6}</td><td>{7}</td></tr><tr> <td>Business:</td><td>{8}</td><td>{9}</td><td>{10}</td></tr><tr> <td>Department:</td><td>{11}</td><td>{12}</td><td>{13}</td></tr><tr> <td>Area:</td><td>{14}</td><td>{15}</td><td>{16}</td></tr><tr> <td>Reason:</td><td colspan='3'> {17} </td></tr></tbody> </table> <hr/> <p>This is a system generated email, do not reply to this email.</p>", gd[8], gd[9], RT,
            gd[15], gd[16],
            gd[12], gd[19], gd[6],
            gd[11], gd[18], gd[5],
            gd[10], gd[17], gd[4],
            gd[20], gd[13], gd[7],
            gd[3]
            );
            List<string> TO = new List<string>();
            List<string> CC = new List<string>();
            var TO_LIST = EMAIL_LIST.Where(t => t.KEEP_IN == "TO");
            foreach (var item in TO_LIST)
            {
                TO.Add(item.SMTP);
            }
            var CC_LIST = EMAIL_LIST.Where(t => t.KEEP_IN == "CC");
            foreach (var item in CC_LIST)
            {
                CC.Add(item.SMTP);
            }
            bool IsSend = Send_eMail(MailSubject, MailBody, TO, CC);
            if (IsSend)
            {
                return true;
            }
            else
            {
                return false;
            };
        }
        public bool Send_Email_KSA_Inspection_Approval(string Area, string inspid, string Request_Type)
        {
            string RT = "Unknown";
            List<string> IAD = new List<string>();
            List<Email_Id> EMAIL_LIST = new List<Email_Id>();
            List<string> GR1 = new List<string>();
            GR1.Add("GR0001");

            if (Request_Type == "INS")
            {
                RT = "Inspection";
                IAD = RP.InspectionApprovalData(id: inspid);
                EMAIL_LIST = RP.EmailListByGroupArea(Group: GR1, TypeS: "INSPECTION", AreA: Area);
                if (EMAIL_LIST.Count < 1)
                {
                    return false;
                }
            }
            else if (Request_Type == "JOB")
            {
                RT = "Job Card";
                IAD = RP.JobApprovalData(id: inspid);
                EMAIL_LIST = RP.EmailListByGroupArea(Group: GR1, TypeS: "JOBCARD", AreA: Area);
                if (EMAIL_LIST.Count < 1)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            string MailSubject = RT + " ~ " + IAD[2];
            string MailBody = string.Empty;
            if (Request_Type == "INS")
            {
                MailBody = string.Format(@"<p>Inspection <strong>{0}</strong> is seeking your approval </p><p>Reason: {1}</p>Link -<a href='http://pmc.prangroup.com/PM/Inspection/InsApproval'>View Inspection</a> <br/><table border='1'><tbody><tr><td>Asset:</td><td>{2}</td></tr><tr><td>User:</td><td>{3}</td></tr><tr><td>Inspection by:</td><td>{4}</td></tr><tr><td>Date of Inspection:</td><td>{5}</td></tr><tr><td>Amount:</td><td style='color:red'>{6}.00</td></tr></tbody></table><hr/> <p>This is a system generated email, do not reply to this email.</p>", IAD[0], IAD[1], IAD[2], IAD[5], IAD[3], IAD[4], IAD[6]);
            }
            else if (Request_Type == "JOB")
            {
                MailBody = string.Format(@"<p>Job Card <strong>{0}</strong> is seeking your approval </p><p>Reason: {1}</p>Link -<a href='http://pmc.prangroup.com/PM/Inspection/JobApproval'>View Job Card</a> <br/><table border='1'><tbody><tr><td>Asset:</td><td>{2}</td></tr><tr><td>User:</td><td>{3}</td></tr><tr><td>Date of Job Card:</td><td>{4}</td></tr><tr><td>Inspection No:</td><td>{5}</td></tr><tr><td>Inspection Amount:</td><td style='color:red'>{6}.00</td></tr><tr><td>Job Card Amount:</td><td style='color:red'>{7}.00</td></tr></tbody></table><hr/> <p>This is a system generated email, do not reply to this email.</p>", IAD[0], IAD[1], IAD[2], IAD[7], IAD[3], IAD[4], IAD[5], IAD[6]);
            }
            else
            {
                return false;
            }

            List<string> TO = new List<string>();
            List<string> CC = new List<string>();
            var TO_LIST = EMAIL_LIST.Where(t => t.KEEP_IN == "TO");
            foreach (var item in TO_LIST)
            {
                TO.Add(item.SMTP);
            }
            var CC_LIST = EMAIL_LIST.Where(t => t.KEEP_IN == "CC");
            foreach (var item in CC_LIST)
            {
                CC.Add(item.SMTP);
            }
            bool IsSend = Send_eMail(MailSubject, MailBody, TO, CC);
            if (IsSend)
            {
                return true;
            }
            else
            {
                return false;
            };
        }
        public bool Send_Email_KSA_Plea_Approval(string Plea, string User, int? Amount, string Reason, string Asset)
        {
            List<Email_Id> EMAIL_LIST = new List<Email_Id>();
            List<string> GR1 = new List<string>();
            GR1.Add("GR0001");
            EMAIL_LIST = RP.EmailListByGroupType(Group: GR1, TypeS: "PLEA");
            if (EMAIL_LIST.Count < 1)
            {
                return false;
            }

            string MailSubject = "Required Approval~ " + Plea;
            string MailBody = string.Format(@"<p>PLEA <strong>{0}</strong> is seeking your approval </p><p>Reason: {1}</p>Link -<a href='http://pmc.prangroup.com/PM/CaseAndPenalty/PleaApproval'>View PLEA</a> <br/><table border='1'><tbody><tr><td>Asset:</td><td>{2}</td></tr><tr><td>User:</td><td>{3}</td></tr><tr><td>Amount:</td><td style='color:red'>{4}.00</td></tr></tbody></table><hr/> <p>This is a system generated email, do not reply to this email.</p>", Plea, Reason, Asset, User, Amount);

            List<string> TO = new List<string>();
            List<string> CC = new List<string>();
            var TO_LIST = EMAIL_LIST.Where(t => t.KEEP_IN == "TO");
            foreach (var item in TO_LIST)
            {
                TO.Add(item.SMTP);
            }
            var CC_LIST = EMAIL_LIST.Where(t => t.KEEP_IN == "CC");
            foreach (var item in CC_LIST)
            {
                CC.Add(item.SMTP);
            }
            bool IsSend = Send_eMail(MailSubject, MailBody, TO, CC);
            if (IsSend)
            {
                return true;
            }
            else
            {
                return false;
            };
        }

        public bool Send_Email_KSA_Inspection_JobCard_Bundle()
        {
            bool rt = false;

            return rt;
        }
    }
}