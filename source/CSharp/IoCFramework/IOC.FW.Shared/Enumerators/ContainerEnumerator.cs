using System;

namespace IOC.FW.Shared.Enumerators
{
    /// <summary>
    /// Classe utilizada para armazenar enumeradores de containers
    /// </summary>
    public class ContainerEnumerator
    {
        public enum LifeCycle : byte
        {
            Transient = 0,
            Singleton = 1
        }

        [Obsolete]
        public enum UnresolvedType : byte
        {
            Throw = 0,
            ReturnDefault = 1
        }

        [Obsolete]
        public enum ContainerType : byte
        {
            DryIoc = 0,
            SimpleInjector = 1
        }
    }
}