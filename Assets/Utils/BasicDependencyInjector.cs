using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Gram.Utils
{
    
    // seeing we cannot use any external libraries I created a very very basic one just as an example 
    // for time saving as we dont want to create a full library  we are going to use a singleton, get all bindings from here and 
    // monobehaviours will need to ask for their dependencies instead of a true injection
    public class BasicDependencyInjector
    {
        
        private void SetupBindings() {
            DependencyBinds[] allDependencyBinds = UnityEngine.Object.FindObjectsOfType<DependencyBinds>();
            foreach (DependencyBinds dependencyBinds in allDependencyBinds) {
                dependencyBinds.FillBinds(_objectsByType);
            }
        }
        
        private readonly Dictionary<Type, Object> _objectsByType = new Dictionary<Type, Object>();

        public T GetObjectByType<T>() where T : class {
            Type type = typeof(T);
            if (_objectsByType.ContainsKey(type)) {
                return _objectsByType[type] as T;
            } else {
                Debug.LogError($"injector for type {type.Name} wasnt setup");
                return null;
            }
        }
        
        
        private static BasicDependencyInjector _instance;
        public static BasicDependencyInjector Instance() {
            if (_instance == null) {
                _instance = new BasicDependencyInjector();
            }
            return _instance;
        }

        private BasicDependencyInjector() {
            SetupBindings();
        }

    }
}