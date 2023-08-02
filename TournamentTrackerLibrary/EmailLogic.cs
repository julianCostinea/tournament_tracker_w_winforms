﻿using System.Collections.Generic;
using System.Net.Mail;

namespace TournamentTrackerLibrary
{
    public static class EmailLogic
    {
        public static void SendEmail(string to, string subject, string body)
        {
            MailAddress fromMailAddress = new MailAddress(GlobalConfig.AppKeyLookup("senderEmail"), GlobalConfig.AppKeyLookup("senderDisplayName"));
            
            MailMessage mail = new MailMessage
            {
                From = fromMailAddress,
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            
            mail.To.Add(to);
            
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(GlobalConfig.AppKeyLookup("senderEmail"), GlobalConfig.AppKeyLookup("senderPassword")),
                EnableSsl = true
            };
            
            client.Send(mail);
        }
    }
}