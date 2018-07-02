using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Health
{
	public class EiDamageTypeResource : EiComponent
	{
		[SerializeField]
		private List<string> damageCategories = new List<string> ();

		public int Length {
			get {
				return damageCategories.Count;
			}
		}

		public string this [int index] {
			get {
				return damageCategories [index];
			}
		}
	}
}