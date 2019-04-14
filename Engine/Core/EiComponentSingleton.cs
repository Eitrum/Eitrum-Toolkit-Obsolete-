using System;
using UnityEngine;

namespace Eitrum.Engine.Core.Singleton {
    public class EiComponentSingleton<T> : EiComponent where T : EiComponentSingleton<T> {

        #region Singleton

        private static T instance;

        public static T Instance {
            get {
                if (!GameRunning) {
                    Debug.LogErrorFormat("Game is ending, please check callbacks OnDestroy");
                    return null;
                }
                if (instance == null) {
                    try {
                        var obj = Resources.Load<GameObject>(typeof(T).Name);
                        if (obj != null)
                            instance = obj.GetComponent<T>();
                    }
                    finally {

                    }
                    if (instance != null && !instance.KeepInResources)
                        instance = Instantiate(instance.gameObject).GetComponent<T>();

                    if (instance == null)
                        instance = new UnityEngine.GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();

                    instance.OnSingletonCreated();

                    if (instance.KeepAlive) {
                        DontDestroyOnLoad(instance);
                    }
                }
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
                if (instance.KeepAlive) {
                    DontDestroyOnLoad(instance);
                }
            }
        }

        #endregion

        #region Properties

        public static bool HasInstance { get { return instance != null; } }

        protected virtual bool KeepInResources { get { return false; } }

        protected virtual bool AllowAssignSingleton { get { return false; } }

        protected virtual bool KeepAlive {
            get { return false; }
            set {
                if (value) {
                    DontDestroyOnLoad(this.gameObject);
                }
                else {
                    if ((instance.gameObject.hideFlags & HideFlags.DontSave) == HideFlags.DontSave) {
                        // Move object to current scene
                        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(instance.gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
                    }
                }
            }
        }

        #endregion

        #region Methods

        protected void AssignInstance(T targetInstance) {
            Instance = targetInstance;
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

