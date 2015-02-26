using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using System.Data.SqlClient;
using IOC.FW.Core.Factory;
using IOC.Abstraction.Business;
using IOC.Abstraction.DAO;

namespace IOC.Business.Implementation
{
    public class NoticiaBusiness 
        : AbstractNoticiaBusiness
    {
        private readonly AbstractNoticiaDAO _dao;

        public NoticiaBusiness(AbstractNoticiaDAO dao)
            : base(dao)
        {
            _dao = dao;
        }

        public override bool TitleAlreadyExists(string Titulo, int? id)
        {
            IList<Noticia> noticiasEncontradas = new List<Noticia>();

            if (id.HasValue && id.Value > 0)
            {
                noticiasEncontradas = this.Select(n =>
                    n.Titulo.Equals(Titulo, StringComparison.OrdinalIgnoreCase)
                    && n.IdNoticia != id.Value
                );
            }
            else
            {
                noticiasEncontradas = this.Select(n =>
                    n.Titulo.Equals(Titulo, StringComparison.OrdinalIgnoreCase)
                );
            }

            return
                noticiasEncontradas != null
                && noticiasEncontradas.Any();
        }

        public override bool Teste(string Titulo)
        {
            return _dao.Teste(Titulo);
        }
    }
}