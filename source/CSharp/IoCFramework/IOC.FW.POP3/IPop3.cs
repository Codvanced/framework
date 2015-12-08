using System.Net.Mail;
using OpenPop.Mime;

namespace IOC.FW.POP3
{
    public interface IPop3
    {
        event Pop3.DownloadEmailHandler OnDownloading;

        void Delete(Message email);
        void DeleteAll();
        void DownloadInbox(string host, int port, bool enableSSL, string username, string password);
        void Reply(Message email, SmtpClient smtp, string complement, bool setupToHTMLBody, string from, string to, string cc);
    }
}