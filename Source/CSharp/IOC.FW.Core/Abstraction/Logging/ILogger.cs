using System;

namespace IOC.FW.Core.Abstraction.Logging
{
    public interface ILogger
    {
        void Debug(object message);
        void Warn(object message);
        void Info(object message);
        void Fatal(object message);
        void Error(object message);

        void Debug(object message, Exception ex);
        void Warn(object message, Exception ex);
        void Info(object message, Exception ex);
        void Fatal(object message, Exception ex);
        void Error(object message, Exception ex);

        string[] GetFilesLogger();
        void Close();
    }
}
