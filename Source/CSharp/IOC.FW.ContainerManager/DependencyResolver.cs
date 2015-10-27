using System;
using IOC.FW.Core.Abstraction.Container;
using IOC.FW.Core.Abstraction.Container.Binding;
using IOC.FW.ContainerManager.Binding;
using IOC.FW.Configuration;
using IOC.FW.Core.Abstraction.Repository;
using IOC.FW.Shared.Enumerators;
using System.Configuration;

namespace IOC.FW.ContainerManager
{
    public class DependencyResolver
        : IDependencyResolver
    {
        public void Resolve(IAdapter adapter)
        {
            Load(adapter);

            if (adapter is IAdapterVerifiable)
                ((IAdapterVerifiable)adapter).Verify();
        }

        private void Load(IAdapter adapter)
        {
            var fwModule = new DefaultBinding();
            fwModule.SetBinding(adapter);

            if (Configurations.Current != null
                && Configurations.Current.InjectionFactory != null
                && Configurations.Current.InjectionFactory.Injection != null
                && Configurations.Current.InjectionFactory.Injection.Count > 0
            )
            {
                var injectionFactory = Configurations.Current.InjectionFactory.Injection;
                for (int i = 0; i < injectionFactory.Count; i++)
                {
                    string assembly = injectionFactory[i].Value;
                    string[] assemblies = assembly.Split(',');

                    if (assemblies != null && assemblies.Length >= 1)
                    {
                        var instance = Activator.CreateInstance(assemblies[0].Trim(), assemblies[1].Trim());
                        var module = (IBinding)instance.Unwrap();

                        if (module is IBinding)
                        {
                            module.SetBinding(adapter);
                        }
                    }
                }
            }
        }
    }
}