using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum {
    public class EiScriptableObjectSingleton<T> : EiScriptableObject
        where T : EiScriptableObjectSingleton<T> {

        #region Singleton

        private static T instance;

        public static T Instance {
            get {
                if (!instance) {
                    instance = Resources.Load<T>(typeof(T).Name);
                    if (!instance) {
                        instance = CreateInstance<T>();
                    }
                    else if (!instance.KeepInResources) {
                        instance = Instantiate<T>(instance);
                    }
                }
                instance?.OnSingletonCreated();
                return instance;
            }
            protected set {
                if (!value.AllowAssignSingleton) {
                    Debug.LogErrorFormat("Assigning singleton instance of type '{0}' is not allowed", typeof(T).Name);
                    return;
                }
                if (instance == value) {
                    Debug.LogWarningFormat("Assigning singleton instance of type '{0}' when it is already assigned", typeof(T).Name);
                    return;
                }
                instance?.OnSingletonDestroyed();
                instance = value;
                instance?.OnSingletonCreated();
            }
        }

        #endregion

        #region Properties

        public static bool HasInstance { get { return instance != null; } }

        protected virtual bool AllowAssignSingleton { get { return false; } }

        protected virtual bool KeepInResources { get { return false; } }
        
        #endregion

        #region Core

        protected void AssignInstance(T instance) {
            Instance = instance;
        }

        #endregion

        #region On Creation

        protected virtual void OnSingletonCreated() {

        }

        protected virtual void OnSingletonDestroyed() {

        }

        #endregion
    }
}