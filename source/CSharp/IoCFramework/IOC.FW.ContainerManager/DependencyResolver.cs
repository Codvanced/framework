using System;
using IOC.FW.Abstraction.Container;
using IOC.FW.Abstraction.Container.Binding;
using IOC.FW.ContainerManager.Binding;
using IOC.FW.Configuration;
using IOC.FW.Abstraction.Repository;

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

            var configuration = ConfigManager.GetConfig();

            foreach (var module in configuration.ContainerManager.Modules)
            {
                var instance = Activator.CreateInstance(
                    module.AssemblyName,
                    module.ClassName
                );
                var instanceModule = (IBinding)instance.Unwrap();

                if (instanceModule is IBinding)
                {
                    instanceModule.SetBinding(adapter);
                }
            }
        }
    }
}