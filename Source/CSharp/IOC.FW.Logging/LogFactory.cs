using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using System.Configuration;

namespace IOC.FW.Logging
{
    /// <summary>
    /// Classe responsável pela criação dos objetos que fazem o log
    /// </summary>
    public static class LogFactory
    {
        /// <summary>
        /// Preenche a connection string no appender de banco de dados
        /// </summary>
        static LogFactory()
        {
            Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            if(hierarchy != null && hierarchy.Configured)
            {
                foreach(IAppender appender in hierarchy.GetAppenders())
                {
                   if(appender is AdoNetAppender)
                   {
                       var adoNetAppender = (AdoNetAppender)appender;
                       string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

                       if (connectionString == null)
                       {
                           connectionString = String.Empty;
                       }

                       adoNetAppender.ConnectionString = connectionString;
                       adoNetAppender.ActivateOptions(); //Refresh AdoNetAppenders Settings
                   }
                }
            }
        }

        /// <summary>
        /// Cria a implementação settando parâmetro logger como a classe de onde foi chamado o método CreateLog()
        /// </summary>
        /// <returns>retorna uma implementação de ILog</returns>
        public static ILog CreateLog()
        {
            return LogManager.GetLogger(new StackFrame(1).GetMethod().DeclaringType);
        }

        /// <summary>
        /// Cria a implementação settando parâmetro logger como o tipo passado
        /// </summary>
        /// <returns>retorna uma implementação de ILog</returns>
        public static ILog CreateLog(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return LogManager.GetLogger(type);
        }
    }
}
