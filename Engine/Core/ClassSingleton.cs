using System;

namespace Eitrum.Engine.Core.Singleton {
    //TODO rewrite this shit make it similar to Component Singleton
    public class ClassSingleton<T> where T : ClassSingleton<T> {
        private static T instance;

        public static T Instance {
            get {
                if (instance == null) {
                    instance = Activator.CreateInstance(typeof(T), true) as T;
#if UNITY_EDITOR
                    if (instance == null)
                        throw new Exception(string.Format("Class {0} can't be instantiated due to no default constructor", typeof(T).Name));
#endif
                    instance?.OnSingletonCreated();
                }
                return instance;
            }
            protected set {
                if ((instance != null && !instance.AllowAssignSingleton) || (!(value?.AllowAssignSingleton ?? true))) {
                    UnityEngine.Debug.LogWarning("Can't assign instance of singleton " + typeof(T).Name);
                    return;
                }
                instance?.OnSingletonDestroyed();
                instance = value;
                instance?.OnSingletonCreated();
            }
        }

        public static bool HasInstance => instance != null;

        protected virtual bool AllowAssignSingleton => false;

        protected virtual void OnSingletonCreated() {

        }

        protected virtual void OnSingletonDestroyed() {

        }
    }
}

