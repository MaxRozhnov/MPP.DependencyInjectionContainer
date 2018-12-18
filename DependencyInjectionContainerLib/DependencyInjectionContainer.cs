using System;
using DependencyInjectionContainerLib.Utils;

namespace DependencyInjectionContainerLib
{
    public class DependencyInjectionContainer
    {
        private readonly DependencyInjectionConfiguration _configuration;
        
        public DependencyInjectionContainer(DependencyInjectionConfiguration config)
        {
            _configuration = config;
        }

        private object GetInstance(RegisteredTypeInfo registeredType)
        {
            if (registeredType.Lifecycle == LifecycleType.Singleton)
            {
                //TODO return a singleton
            }
            else
            {
                //TODO return a new instance
            }
        }

        private object Instantiate(Type type)
        {
            
        }

    }
}