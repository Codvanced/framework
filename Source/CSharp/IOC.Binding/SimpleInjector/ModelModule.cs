using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;
using IOC.Business.Implementation;
using IOC.Interface.Business;
using IOC.Interface.Binding;
using IOC.Interface.Model;
using IOC.Implementation.Model;

namespace IOC.Binding.SimpleInjector
{
    public class ModelModule
        : IModule
    {
        public void SetBinding(Container container)
        {
            //container.Register<PersonAbstract, Person>();
            //container.Register<OcupationAbstract, Ocupation>();
        }
    }
}
