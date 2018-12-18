using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DependencyInjectionContainerLib.Utils;

namespace DependencyInjectionContainerLib
{
    public class DependencyInjectionContainer
    {
        private readonly DependencyInjectionConfiguration _configuration;
        private readonly Stack<Type> _dependenciesStack;
        private readonly Type _currentType;
        private static readonly object _ob = new object();
        
        public DependencyInjectionContainer(DependencyInjectionConfiguration config)
        {
            _configuration = config;
            _dependenciesStack = new Stack<Type>();
               
        }

        public T Resolve<T>() where T : class
        {
            var typeToResolve = typeof(T);
            RegisteredTypeInfo registeredType;
            if (typeToResolve.IsGenericType)
            {
                registeredType = _configuration.GetImplementation(typeToResolve.GetGenericTypeDefinition());
            }
            else
            {
                registeredType = _configuration.GetImplementation(typeToResolve);
            }

            if (registeredType == null)
            {
                throw new Exception("No such type registered");
            }

            return (T) GetInstance(registeredType);
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return (IEnumerable<T>)InstantiateEnumerable(typeof(T));
        }

        private object InstantiateEnumerable(Type type)
        {
            RegisteredTypeInfo registeredType = _configuration.GetImplementation(type);
            if (registeredType != null)
            {
                IList collection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
                IEnumerable<RegisteredTypeInfo> registeredTypes = _configuration.GetAllImplementations(type);
                foreach (RegisteredTypeInfo item in registeredTypes)
                {
                    collection.Add(GetInstance(item));
                }
                return collection;
            }
            else
            {
                throw new Exception("No such type registered");
            }         
        }

        private object GetInstance(RegisteredTypeInfo registeredType)
        {
            if (registeredType.Lifecycle == LifecycleType.Singleton)
            {
                if (registeredType.Instance == null)
                {
                    lock (_ob)
                    {
                        if (registeredType.Instance == null)
                        {
                            registeredType.Instance = Instantiate(registeredType.InterfaceType);
                        }
                    }
                }
                //TODO return a singleton
                return registeredType.Instance;
            }
            else
            {
                //TODO return a new instance
                object createdInst = Instantiate(registeredType.InterfaceType);
                return createdInst;
            }
        }

        private object Instantiate(Type type)
        {
            RegisteredTypeInfo registeredType = _configuration.GetImplementation(type);

            if (!(registeredType == null))
            {
                if (!_dependenciesStack.Contains(registeredType.InterfaceType))
                {
                    _dependenciesStack.Push(registeredType.InterfaceType);
                    Type typeToInstantiate = registeredType.ImplementationType;
                    
                    if (typeToInstantiate.IsGenericTypeDefinition)
                    {
                        typeToInstantiate.MakeGenericType(_currentType.GenericTypeArguments);
                    }
                    
                    ConstructorInfo[] constructors = typeToInstantiate.GetConstructors().OrderByDescending(x => x.GetParameters().Length).ToArray();
                    
                    int currentConstructor = 1;
                    bool createdSuccessfully = false;
                    object result = null;

                    while (!createdSuccessfully && currentConstructor <= constructors.Length)
                    {
                        try
                        {
                            ConstructorInfo constructorInfo = constructors[currentConstructor - 1];
                            object[] constructorParam = GetConstructorParameters(constructorInfo);
                            result = Activator.CreateInstance(typeToInstantiate, constructorParam);
                            createdSuccessfully = true;
                        }
                        catch
                        {
                            createdSuccessfully = false;
                            currentConstructor++;
                        }
                    }

                    _dependenciesStack.Pop();
                    if (createdSuccessfully)
                    {
                        return result;
                    }
                    else
                    {
                        throw new Exception("Could not instantiate type");
                    }
                }
                else
                {
                    throw new Exception("Could not resolve type");   
                }
            }
            else
            {
                //TODO: Throw an exception
                
                throw new Exception("No such type registered");
                
            }
        }

        private object[] GetConstructorParameters(ConstructorInfo constructorInfo)
        {
            ParameterInfo[] parametersInfo = constructorInfo.GetParameters();
            object[] parameters = new object[parametersInfo.Length];
            for (int i = 0; i < parametersInfo.Length; i++)
            {
                parameters[i] = GetInstance(_configuration.GetImplementation(parametersInfo[i].ParameterType));
            }
            return parameters;
        }

    }
}