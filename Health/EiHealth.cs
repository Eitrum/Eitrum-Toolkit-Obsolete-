using System;
using UnityEngine;

namespace Eitrum.Health
{
	[AddComponentMenu ("Eitrum/Health/Health")]
	public class EiHealth : EiComponent, EiDamageInterface, EiHealingInterface
	{
		#region Variables

		[Header ("Max Health Settings")]
		[SerializeField]
		protected EiStat maxHealth = new EiStat (100f);

		[SerializeField]
		protected EiPropertyEventFloat currentHealth = new EiPropertyEventFloat (100f);

		[SerializeField]
		protected bool triggerDeathAtZeroLife = true;

		protected EiPriorityList<Action<EiDamage>> subscribedDamagePipeline = new EiPriorityList<Action< EiDamage>> ();
		protected EiPriorityList<Action<EiDamage>> subscribedHealingPipeline = new EiPriorityList<Action< EiDamage>> ();
		protected EiTrigger onDeath = new EiTrigger ();
		protected EiTrigger<EiEntity> onDeathEntity = new EiTrigger<EiEntity> ();

		protected bool isDead = false;

		#endregion

		#region Properties

		public virtual float MaxHealth {
			get {
				return maxHealth.TotalStat;
			}
		}

		public virtual float CurrentHealth {
			get {
				return currentHealth.Value;
			}
		}

		public virtual float MissingHealth {
			get {
				return MaxHealth - CurrentHealth;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			var ent = Entity;
			subscribedDamagePipeline.Add (0, ApplyDamage);
			subscribedHealingPipeline.Add (0, ApplyHeal);
			currentHealth.SubscribeUnityThread (OnHealthChange);
		}

		public virtual void Damage (EiCombatData damageData)
		{
			Damage (EiDamage.NewInstance.ConfigDamage (damageData, this));
		}

		public virtual void Damage (EiDamage damage)
		{
			for (int i = subscribedDamagePipeline.Count - 1; i >= 0; i--) {
				if (damage.Break)
					break;
				if (damage.HasHealing ()) {
					damage.Iteration++;
					Heal (damage);
					return;
				}
				subscribedDamagePipeline [i].Target (damage);
			}
			damage.Dispose ();
		}

		public virtual void Heal (EiCombatData healData)
		{
			Heal (EiDamage.NewInstance.ConfigHealing (healData, this));
		}

		public virtual void Heal (EiDamage heal)
		{
			for (int i = subscribedHealingPipeline.Count - 1; i >= 0; i--) {
				if (heal.Break)
					break;
				if (heal.HasDamage ()) {
					heal.Iteration++;
					Damage (heal);
					return;
				}
				subscribedHealingPipeline [i].Target (heal);
			}
			heal.Dispose ();
		}

		public virtual void Kill ()
		{
			currentHealth.Value = 0f;
			if (triggerDeathAtZeroLife) {
				if (!isDead) {
					onDeath.Trigger ();
					onDeathEntity.Trigger (Entity);
					isDead = true;
				}
			}
		}

		void OnHealthChange (float value)
		{
			if (triggerDeathAtZeroLife) {
				if (value <= 0f && !isDead) {
					onDeath.Trigger ();
					onDeathEntity.Trigger (Entity);
					isDead = true;
				}
			}
		}

		#endregion

		#region Apply Damage/Healing Implementation

		void ApplyDamage (EiDamage damage)
		{
			if (damage.HasDamage ()) {
				var damageToBeDealt = Math.Min (CurrentHealth, damage.Damage);
				currentHealth.Value -= damageToBeDealt;
				damage.RemoveDamage (damageToBeDealt);
			}
		}

		void ApplyHeal (EiDamage heal)
		{
			if (heal.HasHealing ()) {
				var healToBeAdded = Math.Min (MissingHealth, heal.Heal);
				currentHealth.Value += healToBeAdded;
				heal.RemoveHeal (healToBeAdded);
			}
		}

		#endregion

		#region Add

		public void AddBaseMaxHealth (float amount)
		{
			maxHealth.AddBaseValue (amount);
		}

		public void AddMaxHealthMultiplier (float amount)
		{
			maxHealth.AddStatMultiplier (amount);
		}

		public void MultiplierMaxHealth (float amount)
		{
			maxHealth.MultiplyMultiplier (amount);
		}

		#endregion

		#region Remove

		public void RemoveBaseMaxHealth (float amount)
		{
			maxHealth.RemoveBaseValue (amount);
		}

		public void RemoveMaxHealthMultiplier (float amount)
		{
			maxHealth.RemoveStatMultiplier (amount);
		}

		#endregion

		#region Set

		public void SetBaseMaxHealth (float value)
		{
			maxHealth.SetBaseValue (value);
		}

		public void SetMaxHealthMultiplier (float value)
		{
			maxHealth.SetStatMultiplier (value);
		}

		public void SetMaxHealthMultiplierX (float value)
		{
			maxHealth.SetMultiplyMultiplier (value);
		}

		#endregion

		#region Get Variable

		public EiStat GetMaxHealthStat ()
		{
			return maxHealth;
		}

		public EiTrigger<float> GetMaxHealthChangedTrigger ()
		{
			return maxHealth.GetTrigger ();
		}

		public EiPropertyEventFloat GetCurrentHealth ()
		{
			return currentHealth;
		}

		#endregion

		#region Subscribe / Unsubscribe

		public virtual void SubscribeDamagePipeline (int priorityLevel, Action<EiDamage> target)
		{
			subscribedDamagePipeline.Add (priorityLevel, target);
		}

		public virtual void SubscribeHealingPipeline (int priorityLevel, Action<EiDamage> target)
		{
			subscribedHealingPipeline.Add (priorityLevel, target);
		}

		public virtual EiTrigger GetOnDeathTrigger ()
		{
			return onDeath;
		}

		public virtual EiTrigger<EiEntity> GetOnDeathEntityTrigger ()
		{
			return onDeathEntity;
		}

		#endregion
	}
}