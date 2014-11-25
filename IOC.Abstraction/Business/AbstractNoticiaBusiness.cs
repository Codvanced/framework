using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.FW.Core;
using IOC.Model;
using IOC.Abstraction.DAO;
using IOC.FW.Core.Base;

namespace IOC.Abstraction.Business
{
    public abstract class AbstractNoticiaBusiness
        : BaseBusiness<Noticia>
    {
        public AbstractNoticiaBusiness(AbstractNoticiaDAO dao)
            : base(dao)
	    { }

        public abstract bool TitleAlreadyExists(string Titulo, int? id = null);
        public abstract bool Teste(string Titulo);
    }
}
