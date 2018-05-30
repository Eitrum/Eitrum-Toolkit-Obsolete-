using System;

namespace Eitrum.Health
{
	[Serializable]
	public class EiProtectionData
	{
		public string damageStringType = "Not Defined";
		public int damageType = 0;
		public float flatReduction = 0f;
		public float damageMultiplier = 1f;
	}
}