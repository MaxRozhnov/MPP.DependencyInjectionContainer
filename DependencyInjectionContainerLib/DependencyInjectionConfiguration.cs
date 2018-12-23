using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Register<TInterface, TImplementation>(LifecycleType lifeCycle = LifecycleType.InstancePerDependency)
        {
            RegisterNewPair(typeof(TInterface), typeof(TImplementation), lifeCycle);
        }

        public void Register(Type tInterface, Type tImplementation, LifecycleType lifeCycle = LifecycleType.InstancePerDependency)
        {
            RegisterNewPair(tInterface, tImplementation, lifeCycle);
        }

        public void RegisterAsSelf<TImplementation>(LifecycleType lifeCycle = LifecycleType.InstancePerDependency) where TImplementation : class
        { 
            RegisterNewPair(typeof(TImplementation), typeof(TImplementation), lifeCycle);
        }

        private void RegisterNewPair(Type _interface, Type _implementation, LifecycleType _lifecycleType = LifecycleType.InstancePerDependency)
        {
            if (!_implementation.IsInterface && 
                !_implementation.IsAbstract &&
                _interface.IsAssignableFrom(_implementation))
            {
                var registeredType = new RegisteredTypeInfo(_interface, _implementation, _lifecycleType);

                if (!_registeredTypes.TryGetValue(_interface, out List<RegisteredTypeInfo> typesAlreadyRegistered))
                {
                    _registeredTypes.Add(_interface, new List<RegisteredTypeInfo>() { registeredType });
                }
                else
                {
                    if (!typesAlreadyRegistered.Contains(registeredType))
                    {
                        typesAlreadyRegistered.Add(registeredType);
                    }
                    else
                    {
                        throw new Exception("Could not register type");
                    }
                }
            }
            else
            {
                throw new Exception("Could not register type"); //TODO: Throw another exception, probably.
            }
        }

        public RegisteredTypeInfo GetImplementation(Type _interface)
        {
            return (_registeredTypes.TryGetValue(_interface, out var list)) ? list.Last() : null;
        }

        public IEnumerable<RegisteredTypeInfo> GetAllImplementations(Type _interface)
        {
            if (_registeredTypes.TryGetValue(_interface, out List<RegisteredTypeInfo> typesAlreadyRegistered))
            {
                return typesAlreadyRegistered;
            }
            else
            {
                return null;
            }
        }
    }
}