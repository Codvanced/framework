using IOC.FW.Abstraction.Logging;
using log4net;
using log4net.Appender;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IOC.FW.Logging.Log4net
{
    public class Log4NetAdapter :
        ILogger, IDisposable
    {
        private bool disposedValue = false;
        private readonly ILog _log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType
        );

        public Log4NetAdapter()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private string GetExceptionData(object message, Exception ex)
        {
            StringBuilder dataMessage = new StringBuilder();

            if (ex != null && ex.Data != null && ex.Data.Count > 0)
            {
                foreach (DictionaryEntry data in ex.Data)
                {
                    dataMessage.AppendFormat(
                        "{0}: {1}{2}",
                        data.Key,
                        data.Value,
                        Environment.NewLine
                    );
                }
            }

            dataMessage.Append(message);
            return dataMessage.ToString();
        }

        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void Debug(object message, Exception ex)
        {
            var dataMessage = GetExceptionData(message, ex);
            _log.Debug(dataMessage, ex);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Error(object message, Exception ex)
        {
            var dataMessage = GetExceptionData(message, ex);
            _log.Error(dataMessage, ex);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        public void Fatal(object message, Exception ex)
        {
            var dataMessage = GetExceptionData(message, ex);
            _log.Fatal(dataMessage, ex);
        }

        public string[] GetFilesLogger()
        {
            var filename = new List<string>();
            IAppender[] appenders = _log.Logger.Repository.GetAppenders();

            foreach (var appender in appenders)
            {
                Type t = appender.GetType();

                if (t.Equals(typeof(FileAppender)))
                {
                    filename.Add(((FileAppender)appender).File);
                }
                else if (t.Equals(typeof(RollingFileAppender)))
                {
                    filename.Add(((RollingFileAppender)appender).File);
                }
            }

            return filename.ToArray();
        }

        public void Info(object message)
        {
            _log.Info(message);
        }

        public void Info(object message, Exception ex)
        {
            var dataMessage = GetExceptionData(message, ex);
            _log.Info(dataMessage, ex);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void Warn(object message, Exception ex)
        {
            var dataMessage = GetExceptionData(message, ex);
            _log.Warn(dataMessage, ex);
        }

        public void Close()
        {
            LogManager.Shutdown();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
