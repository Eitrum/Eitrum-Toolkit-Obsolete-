using System;

namespace Eitrum.Health
{
	[Serializable]
	public struct EiCombatData
	{
		#region Variables

		public string damageStringType;
		public int damageType;

		public float flatAmount;
		public float currentHealthPercentage;
		public float maxHealthPercentage;

		public string comment;
		public EiEntity source;

		#endregion

		#region Constructors

		public EiCombatData (int damageType, float flatAmount)
		{
			damageStringType = "Not Defined";
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = 0f;
			this.maxHealthPercentage = 0f;
			this.comment = "";
			this.source = null;
		}

		public EiCombatData (int damageType, float flatAmount, EiEntity source)
		{
			damageStringType = "Not Defined";
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = 0f;
			this.maxHealthPercentage = 0f;
			this.comment = "";
			this.source = source;
		}

		public EiCombatData (int damageType, float flatAmount, float currentHealthPercentage, float maxHealthPercentage)
		{
			damageStringType = "Not Defined";
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = currentHealthPercentage;
			this.maxHealthPercentage = maxHealthPercentage;
			this.comment = "";
			this.source = null;
		}

		public EiCombatData (int damageType, float flatAmount, float currentHealthPercentage, float maxHealthPercentage, EiEntity source)
		{
			damageStringType = "Not Defined";
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = currentHealthPercentage;
			this.maxHealthPercentage = maxHealthPercentage;
			this.comment = "";
			this.source = source;
		}

		#endregion

		#region Core

		public void ApplySource (EiEntity source)
		{
			this.source = source;
		}

		public void SetComment (string text)
		{
			this.comment = text;
		}

		public void Clear ()
		{
			damageStringType = "Not Defined";
			this.damageType = 0;
			this.flatAmount = 0f;
			currentHealthPercentage = 0f;
			maxHealthPercentage = 0f;
			comment = "";
			source = null;
		}

		/// <summary>
		/// Convert this instance from healing to damage or damage to healing.
		/// </summary>
		public EiCombatData Convert ()
		{
			this.flatAmount = -this.flatAmount;
			return this;
		}

		#endregion
	}
}

