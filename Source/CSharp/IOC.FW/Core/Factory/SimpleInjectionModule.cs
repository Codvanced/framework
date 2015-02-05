using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;
using System.Collections.Specialized;
using System.Configuration;
using IOC.FW.Core.Abstraction.Factory;
using IOC.FW.Core.Abstraction.Binding;
using IOC.FW.Configuration;
using SimpleInjector.Extensions;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Base;
using IOC.FW.Core.Abstraction.DAO;

namespace IOC.FW.Core.Factory
{
    /// <summary>
    /// Classe responsável pelo Container Simple Injector
    /// </summary>
    public class SimpleInjectionModule
        : IInjectionModule<SimpleInjector.Container>
    {
        /// <summary>
        /// Container de injeção (Simple Injector)
        /// </summary>
        public Container container { get; set; }

        /// <summary>
        /// Constructor padrão, inicializando o container de injeção e o seu Load.
        /// </summary>
        public SimpleInjectionModule()
        {
            this.container = new Container();
            this.Load();
        }

        /// <summary>
        /// Método utilizado para carregar os bindings do SimpleInjector
        /// </summary>
        private void Load()
        {
            var injectionFactory = Configurations.Current.InjectionFactory.Injection;

            if (injectionFactory != null && injectionFactory.Count > 0)
            {
                for (int i = 0; i < injectionFactory.Count; i++)
                {
                    string assembly = injectionFactory[i].Value;
                    string[] assemblies = assembly.Split(',');

                    if (assemblies != null && assemblies.Length >= 1)
                    {
                        var instance = Activator.CreateInstance(assemblies[0].Trim(), assemblies[1].Trim());
                        var module = (IModule)instance.Unwrap();

                        if (module is IModule)
                        {
                            module.SetBinding(this.container);
                        }
                    }
                }
            }

            SetDefaultBindings(this.container);
        }

        /// <summary>
        /// Método destinado a settar os bindings padrões
        /// </summary>
        /// <param name="container">Container de injeção (Simple Injector)</param>
        private void SetDefaultBindings(Container container)
        {
            container.RegisterOpenGeneric(
                typeof(IBaseBusiness<>),
                typeof(BaseBusiness<>),
                Lifestyle.Singleton
            );

            container.RegisterOpenGeneric(
                typeof(IBaseDAO<>),
                typeof(BaseRepository<>),
                Lifestyle.Transient
            );
        }
    }
}
