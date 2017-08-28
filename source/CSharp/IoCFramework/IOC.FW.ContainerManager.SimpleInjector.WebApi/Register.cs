using SimpleInjector;
using System.Web.Http;

namespace IOC.FW.ContainerManager.SimpleInjector.WebApi
{
    public class Register
    {
        public static void RegisterWebApi(Container container)
        {
            var services = GlobalConfiguration.Configuration.Services;
            var controllerTypes = services.GetHttpControllerTypeResolver()
                .GetControllerTypes(services.GetAssembliesResolver());

            foreach (var controllerType in controllerTypes)
            {
                container.Register(controllerType);
            }

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
