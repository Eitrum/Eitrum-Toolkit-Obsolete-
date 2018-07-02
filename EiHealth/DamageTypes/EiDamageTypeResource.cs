using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Health
{
	public class EiDamageTypeResource : EiComponentSingleton<EiDamageTypeResource>
	{
		#region Singleton

		public override bool KeepInResources ()
		{
			return true;
		}

		public override void SingletonCreation ()
		{
			
		}

		#endregion

		[SerializeField]
		#pragma warning disable
		private int uniqueIdGenerator = 0;
		[SerializeField]
		private List<EiDamageTypeCategory> damageCategories = new List<EiDamageTypeCategory> ();

		public int _Length {
			get {
				return damageCategories.Count;
			}
		}

		public EiDamageTypeCategory this [int index] {
			get {
				return damageCategories [index];
			}
		}
	}

}