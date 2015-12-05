using System;

namespace IOC.FW.FTP.Handlers
{
    [Serializable]
    public class ErrorHandlerArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }
}