using System;
using System.Collections.Generic;

namespace ArFight.Scripts
{
    public class ServiceLocator
    {
        public static Dictionary<Type, object> Services = new Dictionary<Type, object>();
        public static void Register<T>(T service)
        {
            Services[typeof(T)] = service;
        }

        public static void Clear()
        { 
            Services.Clear();
        }
        
        public static T Get<T>()
        {
            return (T) Services[typeof(T)];
        }
    }
}
