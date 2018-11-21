using System;
using UnityEngine;

namespace Eitrum
{
	public class EiScriptableObject : ScriptableObject, EiBaseInterface
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