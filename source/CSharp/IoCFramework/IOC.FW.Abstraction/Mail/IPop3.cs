using System.Net.Mail;
using OpenPop.Mime;
using System;

namespace IOC.FW.Abstraction.Mail
{
    public interface IPop3
    {
        Action<MailMessage> DownloadEmailHandler { get; set; }

        void Delete(Message email);
        void DeleteAll();
        void DownloadInbox(string host, int port, bool enableSSL, string username, string password);
        void Reply(Message email, SmtpClient smtp, string complement, bool setupToHTMLBody, string from, string to, string cc);
    }
}