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
using System.Linq.Expressions;
using IOC.FW.Core;

namespace IOC.Web.Controllers
{
    public class HomeController
        : Controller
    {
        private readonly INewsBusiness _business;

        public HomeController(INewsBusiness business)
        {
            this._business = business;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IList<News> allNews = this._business.SelectAll();
            return View(allNews);
        }

        [HttpGet]
        public ActionResult Item(int? id)
        {
            News news = this._business.SelectSingle(i => i.IdNews == id);
            return View(news);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var newsDelete = _business.SelectSingle(n => n.IdNews == id);
            _business.Delete(newsDelete);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Item(
            int? id,
            string title,
            string description,
            DateTime? date,
            string author
        )
        {
            News news = new News();
            news.Title = title;
            news.Author = author;
            news.NewsDescription = description;

            if (date.HasValue)
                news.NewsDate = date.Value;

            if (id.HasValue && id.Value > 0)
                news.IdNews = id.Value;

            NewsValidation validation = new NewsValidation();
            var validateResult = validation.Validate(news);

            if (validateResult.IsValid)
            {
                if (!_business.TitleAlreadyExists(title, id))
                {
                    if (id.HasValue && id.Value > 0)
                    {
                        var newsUpdated = this._business.SelectSingle(i => i.IdNews == id.Value);

                        if (newsUpdated != null)
                        {
                            newsUpdated.Title = news.Title;
                            newsUpdated.Author = news.Author;
                            newsUpdated.NewsDescription = news.NewsDescription;
                            newsUpdated.NewsDate = date.Value;
                            newsUpdated.Updated = DateTime.Now;

                            this._business.Update(newsUpdated);
                        }
                        else
                        {
                            news.Updated = DateTime.Now;
                            this._business.Insert(news);
                        }
                    }
                    else
                    {
                        news.Created = DateTime.Now;
                        this._business.Insert(news);
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

        public ActionResult TestAxd() 
        {
            return View();
        }
    }
}