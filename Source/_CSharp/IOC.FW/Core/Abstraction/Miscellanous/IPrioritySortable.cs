using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC.FW.Core.Abstraction.Miscellanous
{
    /// <summary>
    /// Usada para fazer sorts na base por prioridade
    /// </summary>
    public interface IPrioritySortable
    {
        long Priority { get; set; }
    }
}
