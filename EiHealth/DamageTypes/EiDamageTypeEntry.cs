using System;
using UnityEngine;

namespace Eitrum
{
	[Serializable]
	public class EiDamageTypeEntry
	{
		#region Variables

		[SerializeField]
		private string damageTypeName = "";
		[SerializeField]
		private int uniqueDamageTypeId = 0;

		#endregion

		#region Properties

		public string DamageTypeName {
			get {
				return damageTypeName;
			}
		}

		public int UniqueDamageTypeId {
			get {
				return uniqueDamageTypeId;
			}
		}

		#endregion
	}
}

