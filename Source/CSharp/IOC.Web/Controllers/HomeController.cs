using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleInjector;
using IOC.Model;
using IOC.Abstraction.Business;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Factory;
using IOC.Validation;
//TODO: Refatorar o framework para aplicação a convenção de código proposta
namespace IOC.Web.Controllers
{
    public class HomeController
        : Controller
    {
        private readonly AbstractNoticiaBusiness _business;

        public HomeController(AbstractNoticiaBusiness business)
        {
            this._business = business;
        }

        //[HttpGet, OutputCache(Duration=10)]
        [HttpGet]
        public ActionResult Index()
        {
            IList<Noticia> allNews = this._business.SelectAll();
            return View(allNews);
        }

        [HttpGet]
        public ActionResult Item(int? id)
        {
            Noticia noticia = this._business.SelectSingle(i => i.IdNoticia == id);
            return View(noticia);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var noticiaDelete = _business.SelectSingle(noticia => noticia.IdNoticia == id);
            _business.Delete(noticiaDelete);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[OutputCache(Duration = 10)]
        public ActionResult Item(
            int? id,
            string titulo,
            string descricao,
            DateTime? data,
            string autor
        )
        {
            
            Noticia noticia = new Noticia();
            noticia.Titulo = titulo;
            noticia.Autor = autor;
            noticia.Descricao = descricao;

            if (data.HasValue)
                noticia.DataNoticia = data.Value;

            if (id.HasValue && id.Value > 0)
                noticia.IdNoticia = id.Value;

            NoticiaValidation validation = new NoticiaValidation();
            var validateResult = validation.Validate(noticia);

            if (validateResult.IsValid)
            {
                var teste = _business.TitleAlreadyExists(titulo, id);

                if (!_business.TitleAlreadyExists(titulo, id))
                {
                    if (id.HasValue && id.Value > 0)
                    {
                        var tempNoticia = this._business.SelectSingle(i => i.IdNoticia == id.Value);

                        if (tempNoticia != null)
                        {
                            tempNoticia.Titulo = noticia.Titulo;
                            tempNoticia.Autor = noticia.Autor;
                            tempNoticia.DataAlteracao = DateTime.Now;
                            tempNoticia.Descricao = noticia.Descricao;
                            this._business.Update(tempNoticia);
                        }
                        else
                        {
                            noticia.DataAlteracao = DateTime.Now;
                            this._business.Insert(noticia);
                        }
                    }
                    else
                    {
                        noticia.DataCadastro = DateTime.Now;
                        this._business.Insert(noticia);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    this.ModelState.AddModelError(
                        "TitleExists",
                        "Já existe uma notícia com o título escolhido."
                    );
                }
            }

            foreach (var erro in validateResult.Errors)
            {
                this.ModelState.AddModelError(
                    erro.PropertyName,
                    erro.ErrorMessage
                );
            }

            return View();
        }
    }
}