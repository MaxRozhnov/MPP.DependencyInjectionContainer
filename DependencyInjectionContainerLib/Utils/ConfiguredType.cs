using System;

namespace DependencyInjectionContainerLib.Utils
{
    public class ConfiguredType
    {
        public bool IsSingleton { get; set; }

        public Type GetImplementationInterface { get; }

        public Type GetImplementation { get; }

        public object GetInstance { get; set; }

        public ConfiguredType (Type impl, Type interf, bool isSingleton = false)
        {
            IsSingleton = isSingleton;
            GetImplementation = impl;
            GetImplementationInterface = interf;
            GetInstance = null;
        }
    }
}