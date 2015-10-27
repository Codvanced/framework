using IOC.FW.Core.Abstraction.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.FW.Core.Abstraction.Repository
{
    public interface IDependencyResolver
    {
        void Resolve(IAdapter adapter);
    }
}