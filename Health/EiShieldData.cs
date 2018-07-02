using System;
using UnityEngine;

namespace Eitrum.Health
{
	[Serializable]
	public class EiShieldData
	{
		[Header ("Damage Reduction")]
		public string damageStringType = "Not Defined";
		public int damageType = 0;
		public float flatReduction = 0f;
		public float damageMultiplier = 1f;

		[Header ("Shield Loss")]
		public float flatShieldLoss = 0;
		public float shieldLossByDamagePercentage = 1f;
		public float shieldLossByTotalDamagePercentage = 0f;
	}
}

