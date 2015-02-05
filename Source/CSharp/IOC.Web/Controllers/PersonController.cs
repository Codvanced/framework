﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IOC.Business.Implementation;
using IOC.Model;
using IOC.FW.Core.Factory;
using IOC.Abstraction.Business;
using IOC.FW.Web.MVC.Base;

namespace IOC.Web.Controllers
{
    public class PersonController
        : BaseController
    {
        private readonly AbstractPersonBusiness _business;

        public PersonController(AbstractPersonBusiness business)
        {
            this._business = business;
        }

        public JsonResult Index()
        {
            this._business.Insert(
                new Person
                {
                    IdOcupation = 1,
                    PersonName = "Person 4",
                    Gender = "Masculino",
                    Created = DateTime.Now
                },
                new Person
                {
                    IdOcupation = 2,
                    PersonName = "Person 5",
                    Gender = "Feminino",
                    Created = DateTime.Now
                },
                new Person
                {
                    IdOcupation = 3,
                    PersonName = "Person 6",
                    Gender = "Feminino",
                    Created = DateTime.Now
                }
            );

            var update = this._business.SelectSingle(p => p.IdPerson == 2);
            update.Gender = "Feminino";

            this._business.Update(update);
            this._business.Delete(update);

            var person = this._business.SelectAll(p => p.Ocupation);
            return Json(person, JsonRequestBehavior.AllowGet);
        }
    }
}