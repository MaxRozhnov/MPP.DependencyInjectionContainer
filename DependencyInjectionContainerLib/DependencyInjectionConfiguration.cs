using System;
using System.Collections.Generic;
using DependencyInjectionContainerLib.Utils;

namespace DependencyInjectionContainerLib
{
    public class DependencyInjectionConfiguration
    {
        private readonly Dictionary<Type, List<RegisteredTypeInfo>> _registeredTypes;

        public DependencyInjectionConfiguration()
        {
            _registeredTypes = new Dictionary<Type, List<RegisteredTypeInfo>>();
        }






        private void RegisterNewPair(Type _interface, Type _implementation, LifecycleType _lifecycleType = LifecycleType.InstancePerDependency)
        {
            if (!_implementation.IsInterface && 
                !_implementation.IsAbstract &&
                _interface.IsAssignableFrom(_implementation))
            {
                var registeredType = new RegisteredTypeInfo(_interface, _implementation, _lifecycleType);
            }
        }
    }
}