using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;

namespace IOC.FW.Core.Factory
{
    /// <summary>
    /// Classe responsável pela construção de objetos de dependência;
    /// </summary>
    public sealed class InstanceFactory
    {
        private static volatile Container _container;
        private static volatile Action<Container> _preAction;
        /// <summary>
        /// Encontra o Container e o constrói
        /// </summary>
        /// <returns></returns>
        private static Container GetInjection()
        {
            if (_container == null)
            {
                var module = new SimpleInjectionModule();
                _container = module.container;

                if (_preAction != null)
                    _preAction(_container);

                _container.Verify();
            }

            return _container;
        }

        /// <summary>
        /// Retorna uma implementação de uma abstração
        /// </summary>
        /// <typeparam name="T">Tipo da Abstração que se deseja encontrar a implementação</typeparam>
        /// <returns>Uma abstração preenchida por uma implementação</returns>
        public static T GetImplementation<T>()
            where T : class
        {
            var injection = GetInjection();
            return injection.GetInstance<T>();
        }

        /// <summary>
        /// Registra os módulos de dependências com suas classes concretas
        /// </summary>
        /// <returns>Container preenchido</returns>
        public static Container RegisterModules(Action<Container> preAction = null)
        {
            _preAction = preAction;
            return GetInjection();
        }
    }
}
