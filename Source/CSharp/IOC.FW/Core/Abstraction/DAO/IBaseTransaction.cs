using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace IOC.FW.Core.Abstraction.DAO
{
    public interface IBaseTransaction
    {
        void SetConnection(DbConnection connection, DbTransaction transaction);

        void SetConnection(string nameOrConnectionString);
    }
}
