using System;
using System.Collections.Generic;

namespace Eitrum.Health
{
	public class EiDamage : EiPoolableObject<EiDamage>
	{
		#region Variables

		private int damageType = 0;
		private float damage = 0f;

		private EiHealth target;
		/// <summary>
		/// The data used to recalculate from scratch.
		/// </summary>
		public EiCombatData data;

		private bool safetyBreak = false;
		private int iteration = 0;

		#endregion

		#region Properties

		public float Damage {
			get {
				return damage;
			}
		}

		public float Heal {
			get {
				return -damage;
			}
		}

		public int DamageType {
			get {
				return damageType;
			}
		}

		public bool Break {
			get {
				return safetyBreak;
			}
			set {
				safetyBreak = value;
			}
		}

		public EiHealth Target {
			get {
				return target;
			}
			set {
				target = value;
			}
		}

		public EiEntity Source {
			get {
				return data.source;
			}
			set {
				data.source = value;
			}
		}

		public int Iteration {
			get {
				return iteration;
			}
			set {
				iteration = value;
				if (iteration > 5)
					safetyBreak = true;
			}
		}

		#endregion

		#region Poolable Object

		protected override void OnDispose ()
		{
			damageType = 0;
			damage = 0f;
			safetyBreak = true;
			data.Clear ();
		}

		#endregion

		#region Set

		public EiDamage ConfigDamage (int damageType, float damage)
		{
			this.damageType = damageType;
			this.damage = damage;
			return this;
		}

		public EiDamage ConfigDamage (EiCombatData data)
		{
			this.damageType = data.damageType;
			this.data = data;
			return this;
		}

		public EiDamage ConfigDamage (EiCombatData data, EiHealth target)
		{
			this.target = target;
			this.damage = data.flatAmount + target.CurrentHealth * data.currentHealthPercentage + target.MaxHealth * data.maxHealthPercentage;
			return ConfigDamage (data);
		}

		public EiDamage ConfigHealing (int damageType, float heal)
		{
			this.damageType = damageType;
			this.damage = -heal;
			return this;
		}

		public EiDamage ConfigHealing (EiCombatData data)
		{
			data.Convert ();
			this.damageType = data.damageType;
			this.data = data;
			return this;
		}

		public EiDamage ConfigHealing (EiCombatData data, EiHealth target)
		{
			data.Convert ();
			this.damageType = data.damageType;
			this.data = data;
			this.target = target;
			this.damage = data.flatAmount + target.CurrentHealth * data.currentHealthPercentage + target.MaxHealth * data.maxHealthPercentage;
			return this;
		}

		#endregion

		#region Core

		public EiDamage AddDamage (float damage)
		{
			this.damage += damage;
			return this;
		}

		public EiDamage AddHealing (float heal)
		{
			this.damage -= heal;
			return this;
		}

		public EiDamage Multiply (float multiplier)
		{
			damage *= multiplier;
			return this;
		}

		public float GetDamage ()
		{
			return damage;
		}

		public float GetHeal ()
		{
			return -damage;
		}

		public int GetDamageType ()
		{
			return damageType;
		}

		public string GetDamageTypeText ()
		{
			return "not-defined";
		}

		public EiDamage SetDamage (float damage)
		{
			this.damage = damage;
			return this;
		}

		public EiDamage SetHealing (float heal)
		{
			this.damage = -heal;
			return this;
		}

		public bool HasDamage ()
		{
			return damage > 0f;
		}

		public bool HasHealing ()
		{
			return damage < 0f;
		}

		public EiDamage RemoveDamage ()
		{
			if (damage > 0f)
				damage = 0f;
			return this;
		}

		public EiDamage RemoveDamage (float amount)
		{
			this.damage -= amount;
			return this;
		}

		public EiDamage RemoveHeal ()
		{
			if (damage < 0f)
				damage = 0f;
			return this;
		}

		public EiDamage RemoveHeal (float amount)
		{
			this.damage += amount;
			return this;
		}

		public EiDamage ReduceDamage (float flat, float percentage, bool calculateFlatFirst = false)
		{
			if (calculateFlatFirst) {
				damage = (damage - flat) * percentage;
			} else {
				damage = (damage * percentage) - flat;
			}
			return this;
		}

		public EiDamage ReduceHealing (float flat, float percentage, bool calculateFlatFirst = false)
		{
			if (calculateFlatFirst) {
				damage = (damage + flat) * percentage;
			} else {
				damage = (damage * percentage) + flat;
			}
			return this;
		}

		/// <summary>
		/// Recalculate this instance. Will remove all previous changes to its original form.
		/// </summary>
		public EiDamage Recalculate ()
		{
			if (target == null)
				return this;

			this.damage = data.flatAmount + target.CurrentHealth * data.currentHealthPercentage + target.MaxHealth * data.maxHealthPercentage;

			return this;
		}

		/// <summary>
		/// Convert this instance from healing to damage or damage to healing.
		/// </summary>
		public EiDamage Convert ()
		{
			damage = -damage;
			return this;
		}

		#endregion
	}
}