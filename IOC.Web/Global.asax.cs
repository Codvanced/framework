﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using IOC.Business.Implementation;
using System.Reflection;
using IOC.FW.Core.Factory;
using IOC.FW.Web.MVC.SimpleInjector;

namespace IOC.Web
{
    public class MvcApplication 
        : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = InstanceFactory.RegisterModules(Register.RegisterWebApi);
            var resolve = new SimpleInjectorDependencyResolver(container);
            DependencyResolver.SetResolver(resolve);
        }
    }
}