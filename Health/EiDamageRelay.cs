using System;
using UnityEngine;

namespace Eitrum.Health
{
	/// <summary>
	/// Ei damage relay, very useful for adding different damage values on different body parts.
	/// </summary>
	[AddComponentMenu ("Eitrum/Health/Damage Relay")]
	public class EiDamageRelay : EiComponent, EiDamageInterface
	{
		#region Variables

		[Header ("Settings")]
		[SerializeField]
		private EiPropertyEventFloat flatDamageReduction = new EiPropertyEventFloat (0f);
		[SerializeField]
		private EiPropertyEventFloat damageMultiplier = new EiPropertyEventFloat (1f);
		[Header ("Components")]
		[SerializeField]
		private EiHealth damageTarget;

		private EiTrigger<EiCombatData> onHit = new EiTrigger<EiCombatData> ();

		#endregion

		#region Properties

		public float FlatDamageReduction {
			get {
				return flatDamageReduction.Value;
			}
			set {
				flatDamageReduction.Value = value;
			}
		}

		public float DamageMultiplier {
			get {
				return damageMultiplier.Value;
			}
			set {
				damageMultiplier.Value = value;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			var ent = Entity;
		}

		public EiTrigger<EiCombatData> GetOnHitTrigger ()
		{
			return onHit;
		}

		public EiPropertyEventFloat GetFlatDamageReduction ()
		{
			return flatDamageReduction;
		}

		public EiPropertyEventFloat GetDamageMultiplier ()
		{
			return damageMultiplier;
		}

		#endregion

		#region Flat Damage Reduction

		public void AddFlatDamageReduction (float amount)
		{
			flatDamageReduction.Value += amount;
		}

		public void RemoveFlatDamageReduction (float amount)
		{
			flatDamageReduction.Value -= amount;
		}

		public void SetFlatDamageReduction (float value)
		{
			flatDamageReduction.Value = value;
		}


		#endregion

		#region Damage Multiplier

		public void AddDamageMultiplier (float amount)
		{
			damageMultiplier.Value += amount;
		}


		public void RemoveDamageMultiplier (float amount)
		{
			damageMultiplier.Value -= amount;
		}

		public void SetDamageMultiplier (float value)
		{
			damageMultiplier.Value = value;
		}

		#endregion

		#region EiDamageInterface implementation

		public void Damage (float flatDamage)
		{
			Damage (new EiCombatData (flatDamage));
		}

		public void Damage (int damageType, float flatDamage)
		{
			Damage (new EiCombatData (damageType, flatDamage));
		}

		public void Damage (int damageType, float flatDamage, float currentHealthPercentageDamage, float maxHealthPercentageDamage)
		{
			Damage (new EiCombatData (damageType, flatDamage, currentHealthPercentageDamage, maxHealthPercentageDamage));
		}

		public void Damage (int damageType, float flatDamage, float currentHealthPercentageDamage, float maxHealthPercentageDamage, EiEntity source)
		{
			Damage (new EiCombatData (damageType, flatDamage, currentHealthPercentageDamage, maxHealthPercentageDamage, source));
		}

		/// <summary>
		/// Damage the target with specified combat data, should be a copy of original.
		/// </summary>
		/// <param name="combatData">Combat data.</param>
		public void Damage (EiCombatData combatData)
		{
			combatData.ApplyTarget (damageTarget);
			onHit.Trigger (combatData);

			var flat = combatData.FlatAmount;
			combatData.FlatAmount -= flatDamageReduction.Value - flat * damageMultiplier.Value;
			combatData.CurrentHealthPercentage *= damageMultiplier.Value;
			combatData.MaxHealthPercentage *= damageMultiplier.Value;

			damageTarget.Damage (combatData);
		}

		public EiHealth HealthComponent {
			get {
				return damageTarget;
			}
		}

		#endregion

		#region Subscribe

		public void SubscribeOnHit (Action<EiCombatData> method)
		{
			onHit.AddAction (method);
		}

		public void SubscribeOnHit (Action<EiCombatData> method, bool anyThread)
		{
			onHit.AddAction (method, anyThread);
		}

		public void UnsubscribeOnHit (Action<EiCombatData> method)
		{
			onHit.RemoveAction (method);
		}

		#endregion

		#if UNITY_EDITOR

		protected override void AttachComponents ()
		{
			base.AttachComponents ();
			damageTarget = GetComponentInParent<EiHealth> ();
		}

		#endif
	}
}