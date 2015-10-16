using System;
using IOC.FW.Configuration;
using IOC.FW.Core.Abstraction.DIContainer;
using IOC.FW.Core.Abstraction.DIContainer.Binding;
using IOC.FW.Core.Implementation.DIContainer.Binding;

namespace IOC.FW.Core.Implementation.DIContainer
{
    public class DependencyResolver
    {
        public static volatile IAdapter Adapter;

        public static void Resolve(IAdapter adapter)
        {
            Adapter = adapter;
            Load();

            if (adapter is IAdapterVerifiable)
                ((IAdapterVerifiable)adapter).Verify();
        }

        private static void Load()
        {
            var fwModule = new DefaultBinding();
            fwModule.SetBinding(Adapter);

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
                            module.SetBinding(Adapter);
                        }
                    }
                }
            }
        }
    }
}