using System;
using UnityEngine;

namespace Eitrum {
	public class EiComponentSingleton<T> : EiComponentSingleton where T : EiComponentSingleton {
		protected static T instance;

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
					if (instance != null && !instance.KeepInResources())
						instance = Instantiate(instance.gameObject).GetComponent<T>();

					if (instance == null)
						instance = new UnityEngine.GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();

					instance.SingletonCreation();
				}
				return instance;
			}
		}

		protected void KeepAlive() {
			DontDestroyOnLoad(this.gameObject);
		}
	}

	public class EiComponentSingleton : EiComponent {
		public virtual void SingletonCreation() {

		}

		public virtual bool KeepInResources() {
			return false;
		}
	}
}

