using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC.FW.POP3.Handlers
{
    [Serializable]
    public class DownloadEmailHandlerArgs : EventArgs
    {
        public Message Email { get; set; }
    }
}
