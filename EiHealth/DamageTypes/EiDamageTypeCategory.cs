using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Health
{
	[Serializable]
	public class EiDamageTypeCategory
	{
		#region Variables

		[SerializeField]
		private string categoryName = "";
		[SerializeField]
		private List<EiDamageTypeEntry> damageTypes = new List<EiDamageTypeEntry> ();

		#endregion

		#region Properties

		public string CategoryName {
			get {
				return categoryName;
			}
		}

		public int Length {
			get {
				return damageTypes.Count;
			}
		}

		public EiDamageTypeEntry this [int index] {
			get {
				return damageTypes [index];
			}
		}

		#endregion
	}
}