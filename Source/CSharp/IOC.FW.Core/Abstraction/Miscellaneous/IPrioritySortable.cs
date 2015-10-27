using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//TODO: Review the namespace
namespace IOC.FW.Core.Abstraction.Miscellaneous
{
    /// <summary>
    /// Usada para fazer sorts na base por prioridade
    /// </summary>
    public interface IPrioritySortable
    {
        long Priority { get; set; }
    }
}
