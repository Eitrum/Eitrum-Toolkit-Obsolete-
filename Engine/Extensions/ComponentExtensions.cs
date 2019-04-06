using System;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Eitrum
{
	public static class ComponentExtensions
	{
		#region Get Or Add extensions for components

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
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

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static T AddComponent<T> (this Component component) where T : Component
		{
			if (!component) {
				throw new NullReferenceException ("Component does not exist");
			}
			return component.gameObject.AddComponent<T> ();
		}

		#endregion

		#region SetActive

		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static void SetActive (this Component component, bool value)
		{
			component.gameObject.SetActive (value);
		}

		#endregion
	}
}