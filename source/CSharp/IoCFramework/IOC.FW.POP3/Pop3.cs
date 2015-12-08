using IOC.FW.POP3.Handlers;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace IOC.FW.POP3
{
    public class Pop3 : IPop3
    {
        public delegate void DownloadEmailHandler(DownloadEmailHandlerArgs args);
        public event DownloadEmailHandler OnDownloading;

        public Pop3()
        {

        }

        public void DownloadInbox(string host, int port, bool enableSSL, string username, string password)
        {
            using (var client = new Pop3Client())
            {
                var mailUids = new List<string>();
                var mailCount = 0;

                client.Connect(host, port, enableSSL);
                client.Authenticate(username, password);
                mailUids = client.GetMessageUids();
                mailCount = mailUids.Count;

                // zero is a not valid index for getting mail messages here, starting at 1
                for (int i = 0; i < mailCount; i++)
                {
                    //mantain the connection active while using it
                    if (i > 0 && i % 100 == 0)
                        client.NoOperation();

                    var uid = mailUids[i];
                    var msg = client.GetMessage(i + 1);

                    if (OnDownloading != null)
                        OnDownloading(new DownloadEmailHandlerArgs
                        {
                            Email = msg
                        });
                }
            }
        }

        public void Reply(Message email, SmtpClient smtp, string complement, bool setupToHTMLBody, string from, string to, string cc)
        {
            var reply = email.ToMailMessage();
            reply.From = new MailAddress(from);
            reply.To.Add(to);
            reply.CC.Add(cc);
            reply.Body = string.Format("{0}\r\n------------------------{1}", complement, reply.Body);
            reply.IsBodyHtml = setupToHTMLBody;
            smtp.Send(reply);
        }

        public void Delete(Message email)
        {
            using (var client = new Pop3Client())
            {
                int messageCount = client.GetMessageCount();

                for (int messageItem = messageCount; messageItem > 0; messageItem--)
                    if (client.GetMessageHeaders(messageItem).MessageId == email.Headers.MessageId)
                        client.DeleteMessage(messageItem);
            }
        }

        public void DeleteAll()
        {
            using (var client = new Pop3Client())
            {
                client.DeleteAllMessages();
            }
        }
    }
}
