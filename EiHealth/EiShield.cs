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
		protected bool calculateFlatReductionFirst = true;
		[SerializeField]
		protected bool treatAllDamageTypesAsNormal = true;

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
			healthComponent.SubscribeDamagePipeline (damagePriorityLevel, ApplyDamage);
			healthComponent.SubscribeHealingPipeline (-damagePriorityLevel, ApplyHeal);
			currentShield.SubscribeUnityThread (ShieldClamp);
		}

		void ApplyHeal (EiDamage heal)
		{
			if (!canRegenShieldByHealthRegen)
				return;
			if (!HasShieldType (heal.DamageType))
				return;

			var amountToHeal = Math.Min (MissingShield, heal.Heal);
			if (amountToHeal < 0f)
				amountToHeal = 0f;
			
			currentShield.Value += amountToHeal;

			heal.RemoveHeal (amountToHeal);
		}

		void ApplyDamage (EiDamage damage)
		{
			if (HasShieldType (damage.DamageType)) {
				if (HasShield) {
					// calculate armor Loss
					var shieldLoss = shieldData.flatShieldLoss + shieldData.shieldLossByTotalDamagePercentage * damage.Damage;
					var preCalcDamage = damage.Damage;
					// Reduce damage
					damage.ReduceDamage (shieldData.flatReduction, shieldData.damageMultiplier, calculateFlatReductionFirst);
					//fix after reduction armor loss
					shieldLoss += (preCalcDamage - damage.Damage) * shieldData.shieldLossByDamagePercentage;
					//Check if needed further damage adjustments
					var tempArmor = CurrentShield;
					if (shieldLoss > tempArmor) {
						var percentage = 1f - (tempArmor / shieldLoss);
						currentShield.Value = 0f;
						//do some magic stuff for doing more accurate damage reduction based on armor
						damage.SetDamage (UnityEngine.Mathf.Lerp (damage.Damage, preCalcDamage, percentage));
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

		public virtual void AddCurrentShield (float amount)
		{
			currentShield.Value += amount;
		}

		public virtual void AddMaxBaseShield (float amount)
		{
			baseMaxShield.Value += amount;
		}

		public virtual void AddMaxShieldMultiplier (float amount)
		{
			maxShieldMultiplier.Value += amount;
		}

		#endregion

		#region Remove

		public virtual void RemoveCurrentShield ()
		{
			currentShield.Value = 0f;
		}

		public virtual void RemoveCurrentShield (float amount)
		{
			currentShield.Value -= amount;
		}

		public virtual void RemoveMaxBaseShield (float amount)
		{
			baseMaxShield.Value -= amount;
		}

		public virtual void RemoveMaxShieldMultiplier (float amount)
		{
			maxShieldMultiplier.Value -= amount;
		}

		#endregion

		#region Set

		public virtual void SetCurrentShield (float value)
		{
			currentShield.Value = value;
		}

		public virtual void SetMaxBaseShield (float value)
		{
			baseMaxShield.Value = value;
		}

		public virtual void SetMaxShieldMultiplier (float value)
		{
			maxShieldMultiplier.Value = value;
		}

		#endregion

		#region Get

		/// <summary>
		/// Gets the base max shield value, should only be used for subscribe on changes.
		/// </summary>
		/// <returns>The base max shield.</returns>
		public virtual EiPropertyEventFloat GetBaseMaxShield ()
		{
			return baseMaxShield;
		}

		/// <summary>
		/// Gets the max shield multiplier value, should only be used for subscribe on changes.
		/// </summary>
		/// <returns>The max shield multiplier.</returns>
		public virtual EiPropertyEventFloat GetMaxShieldMultiplier ()
		{
			return maxShieldMultiplier;
		}

		/// <summary>
		/// Gets the current shield value, should only be used for subscribe on changes.
		/// </summary>
		/// <returns>The current shield.</returns>
		public virtual EiPropertyEventFloat GetCurrentShield ()
		{
			return currentShield;
		}

		#endregion

		#region Helpers

		public bool HasShieldType (int damageType)
		{
			return treatAllDamageTypesAsNormal || shieldData.damageType == damageType;
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