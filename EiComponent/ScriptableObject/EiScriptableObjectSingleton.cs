using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum
{
	public class EiScriptableObjectSingleton<T> : EiScriptableObject
		where T : EiScriptableObject
	{
		#region Singleton

		private static T instance;

		public static T Instance {
			get {
				if (!instance) {
					instance = Resources.Load<T> (typeof(T).Name);
					if (!instance)
						instance = CreateInstance<T> ();
				}
				ToSingleton (instance)?.OnSingletonCreated ();
				return instance;
			}
			protected set {
				instance = value;
				ToSingleton (instance)?.OnSingletonCreated ();
			}
		}

		#endregion

		#region Conversion

		private static EiScriptableObjectSingleton<T> ToSingleton (T target)
		{
			return target as EiScriptableObjectSingleton<T>;
		}

		#endregion

		#region Core

		protected void AssignInstance (T instance)
		{
			Instance = instance;
		}

		#endregion

		#region On Creation

		protected virtual void OnSingletonCreated ()
		{
			
		}

		#endregion
	}
}