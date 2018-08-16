using System;
using UnityEngine;

namespace Eitrum.Health
{
	[AddComponentMenu ("Eitrum/Health/Shield")]
	public class EiShield : EiComponent
	{
		#region Variables

		[SerializeField]
		protected string shieldName = "Not Defined";

		[Header ("Priority Level Settings")]
		[SerializeField]
		protected int damagePriorityLevel = 50;
		[SerializeField]
		protected int healPriorityLevel = -50;


		[Header ("Shield Settings")]
		[SerializeField]
		protected EiPropertyEventFloat baseMaxShield = new EiPropertyEventFloat (100f);
		[SerializeField]
		protected EiPropertyEventFloat maxShieldMultiplier = new EiPropertyEventFloat (1f);
		[SerializeField]
		protected EiPropertyEventFloat currentShield = new EiPropertyEventFloat (100f);
		[Space (8f)]
		[SerializeField]
		protected bool canRegenShieldByHealthRegen = false;
		[Space (8f)]
		[SerializeField]
		protected EiShieldData shieldData;
		[SerializeField]
		protected bool treatAllDamageTypesAsNone = true;

		[Header ("Components")]
		[SerializeField]
		protected EiHealth healthComponent;

		#endregion

		#region Properties

		public float MaxShield {
			get {
				return baseMaxShield.Value * maxShieldMultiplier.Value;
			}
		}

		public float CurrentShield {
			get {
				return currentShield.Value;
			}
		}

		public float MissingShield {
			get {
				return MaxShield - currentShield.Value;
			}
		}

		public bool HasShield {
			get {
				return currentShield.Value > 0f;
			}
		}

		public int ShieldType {
			get {
				return shieldData.damageType;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
#if EITRUM_ADVANCED_HEALTH
			healthComponent.SubscribeDamagePipeline (damagePriorityLevel, ApplyDamage);
			healthComponent.SubscribeHealingPipeline (-damagePriorityLevel, ApplyHeal);
			currentShield.Subscribe (ShieldClamp);
#else
			Debug.LogWarning("Enable ADVANCED_HEALTH to enable 'EiShield'");
#endif
		}

		void ApplyHeal (EiCombatData heal)
		{
			if (!canRegenShieldByHealthRegen)
				return;
			if (!HasShieldType (heal.DamageType))
				return;

			var amountToHeal = Math.Min (MissingShield, heal.TotalAmount);
			if (amountToHeal < 0f)
				amountToHeal = 0f;
			
			currentShield.Value += amountToHeal;

			heal.Reduce (amountToHeal);
		}

		void ApplyDamage (EiCombatData damage)
		{
			if (HasShieldType (damage.DamageType)) {
				if (HasShield) {
					// calculate armor Loss
					var shieldLoss = shieldData.flatShieldLoss + shieldData.shieldLossByTotalDamagePercentage * damage.TotalRemainingAmount;
					var preCalcDamage = damage.TotalRemainingAmount;
					// Reduce damage
					damage.Reduce (shieldData.flatReduction + (preCalcDamage - preCalcDamage * shieldData.damageMultiplier));
					//fix after reduction armor loss
					shieldLoss += (preCalcDamage - damage.TotalRemainingAmount) * shieldData.shieldLossByDamagePercentage;
					//Check if needed further damage adjustments
					var tempArmor = CurrentShield;
					if (shieldLoss > tempArmor) {
						var percentage = 1f - (tempArmor / shieldLoss);
						currentShield.Value = 0f;
						//do some magic stuff for doing more accurate damage reduction based on armor
						damage.Reduce (UnityEngine.Mathf.Lerp (damage.TotalRemainingAmount, preCalcDamage, percentage));
					} else {
						currentShield.Value -= shieldLoss;
					}
				}
			}
		}

		void ShieldClamp (float value)
		{
			currentShield.SetValue (Math.Max (0f, Math.Min (value, MaxShield)), false);
		}

		#endregion

		#region Add

		public void AddCurrentShield (float amount)
		{
			currentShield.Value += amount;
		}

		public void AddMaxBaseShield (float amount)
		{
			baseMaxShield.Value += amount;
		}

		public void AddMaxShieldMultiplier (float amount)
		{
			maxShieldMultiplier.Value += amount;
		}

		#endregion

		#region Remove

		public void RemoveCurrentShield ()
		{
			currentShield.Value = 0f;
		}

		public void RemoveCurrentShield (float amount)
		{
			currentShield.Value -= amount;
		}

		public void RemoveMaxBaseShield (float amount)
		{
			baseMaxShield.Value -= amount;
		}

		public void RemoveMaxShieldMultiplier (float amount)
		{
			maxShieldMultiplier.Value -= amount;
		}

		#endregion

		#region Set

		public void SetCurrentShield (float value)
		{
			currentShield.Value = value;
		}

		public void SetMaxBaseShield (float value)
		{
			baseMaxShield.Value = value;
		}

		public void SetMaxShieldMultiplier (float value)
		{
			maxShieldMultiplier.Value = value;
		}

		#endregion

		#region Get

		/// <summary>
		/// Gets the base max shield value, should only be used for subscribe on changes.
		/// </summary>
		/// <returns>The base max shield.</returns>
		public EiPropertyEventFloat GetBaseMaxShield ()
		{
			return baseMaxShield;
		}

		/// <summary>
		/// Gets the max shield multiplier value, should only be used for subscribe on changes.
		/// </summary>
		/// <returns>The max shield multiplier.</returns>
		public EiPropertyEventFloat GetMaxShieldMultiplier ()
		{
			return maxShieldMultiplier;
		}

		/// <summary>
		/// Gets the current shield value, should only be used for subscribe on changes.
		/// </summary>
		/// <returns>The current shield.</returns>
		public EiPropertyEventFloat GetCurrentShield ()
		{
			return currentShield;
		}

		#endregion

		#region Helpers

		public bool HasShieldType (int damageType)
		{
			return treatAllDamageTypesAsNone || shieldData.damageType == damageType;
		}

		#endregion

		#if UNITY_EDITOR

		protected override void AttachComponents ()
		{
			healthComponent = GetComponent <EiHealth> ();
			base.AttachComponents ();
		}

		#endif
	}
}