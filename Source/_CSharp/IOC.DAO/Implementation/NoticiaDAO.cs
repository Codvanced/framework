using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOC.Model;
using System.Data.SqlClient;
using IOC.Abstraction.DAO;
using IOC.Interface.DAO;

namespace IOC.DAO.Implementation
{
    public class NoticiaDAO
        : AbstractNoticiaDAO
    {
        public NoticiaDAO()
            : base()
        {
        }

        public override bool Teste(string Titulo)
        {
            var parameters = new Dictionary<string, object>(){
                { "@titulo", Titulo }
            };

            string query = @"SELECT * FROM WT_NOTICIA WHERE DSC_TITULO = @titulo";
            var noticias = this.ExecuteQuery(query, parameters);

            int contador = this.Exec<Noticia, int>(p => { return p.Count(); });

            IList<Noticia> listNoticia = this.SelectAll();

            var whatever = (from obj in listNoticia where obj.IdNoticia == 1 select obj);
            var whatever2 = listNoticia.Where(noticia => noticia.IdNoticia == 1).Select(p => p);

            var whatever3 = (from obj in listNoticia where obj.IdNoticia == 1 select new { id = obj.IdNoticia, name = obj.Autor }).ToList();
            var whatever4 = listNoticia.Where(noticia => noticia.IdNoticia == 1).Select(p => new { id = p.IdNoticia, name = p.Autor }).ToList();

            return
                noticias != null
                && noticias.Any();
        }
    }
}
