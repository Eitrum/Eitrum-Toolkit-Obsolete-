using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Eitrum
{
	public class EiScriptableObject<T> : ScriptableObject, EiBaseInterface
	{
		#region EiBaseInterface implementation

		public object Target {
			get {
				return this;
			}
		}

		public bool IsNull {
			get {
				return this == null;
			}
		}

		#endregion
	}
}