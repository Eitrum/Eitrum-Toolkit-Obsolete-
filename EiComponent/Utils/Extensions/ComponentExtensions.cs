using System;
using UnityEngine;

namespace Eitrum
{
	public static class ComponentExtensions
	{
		public static T GetOrAddComponent<T> (this Component component) where T : Component
		{
			if (!component) {
				throw new NullReferenceException ("Component does not exist");
			}
			var comp = component.GetComponent<T> ();
			if (comp == null)
				comp = component.gameObject.AddComponent<T> ();
			return comp;
		}

		public static T AddComponent<T> (this Component component) where T : Component
		{
			if (!component) {
				throw new NullReferenceException ("Component does not exist");
			}
			return component.gameObject.AddComponent<T> ();
		}
	}
}

