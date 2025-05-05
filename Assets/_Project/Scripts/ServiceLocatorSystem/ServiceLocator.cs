using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.ServiceLocatorSystem
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance { get; private set; }

        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public static void Init()
        {
            if (Instance == null)
                Instance = new ServiceLocator();
        }

        public void Register<T>(T service) where T : IService
        {
            var key = typeof(T);
            if (_services.ContainsKey(key))
            {
                return;
            }

            _services.Add(key, service);
        }

        public void Unregister<T>() where T : IService
        {
            var key = typeof(T);
            if (!_services.ContainsKey(key))
            {
                return;
            }

            _services.Remove(key);
        }

        public T Get<T>() where T : IService
        {
            var key = typeof(T);
            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"{key} not registered service!");
                throw new InvalidOperationException();
            }

            return (T)_services[key];
        }
    }
}