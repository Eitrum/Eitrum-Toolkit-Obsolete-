using System;
using UnityEngine;
using System.Collections.Generic;

namespace Eitrum
{
	public class EiDatabase : EiComponentSingleton<EiDatabase>
	{
		#region Singleton

		public override bool KeepInResources ()
		{
			return true;
		}

		#endregion

		public List<EiCategory> categories = new List<EiCategory> ();

		public static UnityEngine.Object Instantiate (int category, int entry)
		{
			return MonoBehaviour.Instantiate (Instance.categories [category].entries [entry].targetObject);
		}

		public static UnityEngine.Object Instantiate (EiEntry entry)
		{
			return MonoBehaviour.Instantiate (entry.targetObject);
		}

		public static T Instantiate<T> (int category, int entry) where T : UnityEngine.Object
		{
			return Instantiate (category, entry) as T;
		}

		public static T Instantiate <T> (EiEntry entry) where T : UnityEngine.Object
		{
			return Instantiate (entry) as T;
		}
	}
}