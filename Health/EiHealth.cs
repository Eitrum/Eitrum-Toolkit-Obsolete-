using System;
using UnityEngine;

namespace Eitrum.Health
{
	[AddComponentMenu("Eitrum/Health/Health")]
	public class EiHealth : EiComponent, EiDamageInterface, EiHealingInterface
	{
		#region Variables

		[Header("Settings")]
		[SerializeField]
		private EiStat maxHealth = new EiStat(100f, 1f, 1f);
		[SerializeField]
		private EiPropertyEventFloat currentHealth = new EiPropertyEventFloat(100f);
		[SerializeField]
		private bool triggerDeathAtZeroLife = true;

		private EiTrigger onDeath = new EiTrigger();
		private EiTrigger<EiEntity> onDeathEntity = new EiTrigger<EiEntity>();

#if EITRUM_ADVANCED_HEALTH

		private EiPriorityList<Action<EiCombatData>> subscribedDamagePipeline = new EiPriorityList<Action<EiCombatData>>();
		private EiPriorityList<Action<EiCombatData>> subscribedHealingPipeline = new EiPriorityList<Action<EiCombatData>>();

#endif

		private bool isDead = false;

		#endregion

		#region Properties

		public float MaxHealth
		{
			get
			{
				return maxHealth.TotalStat;
			}
		}

		public float CurrentHealth
		{
			get
			{
				return currentHealth.Value;
			}
		}

		public virtual float MissingHealth
		{
			get
			{
				return MaxHealth - CurrentHealth;
			}
		}

		public bool IsDead
		{
			get
			{
				return isDead;
			}
		}

		public EiHealth HealthComponent
		{
			get
			{
				return this;
			}
		}

		#endregion

		#region Core

#if EITRUM_ADVANCED_HEALTH
		void Awake(){
			SubscribeDamagePipeline(0, ApplyDamage);
			SubscribeHealingPipeline(0, ApplyHeal);
		}
#endif

		public void ResetHealth()
		{
			currentHealth.Value = MaxHealth;
			isDead = false;
		}

		public void SetHealth(float health)
		{
			var prevHealth = currentHealth.Value;
			if (prevHealth != health)
				currentHealth.Value = Mathf.Clamp(health, 0f, MaxHealth);

			if (triggerDeathAtZeroLife && !isDead && prevHealth > 0f && currentHealth.Value <= 0f)
			{
				onDeath.Trigger();
				onDeathEntity.Trigger(Entity);
			}
		}

		public void Kill()
		{
			if (!isDead)
			{
				SetHealth(0f);
			}
		}

		public EiStat GetMaxHealth()
		{
			return maxHealth;
		}

		public EiPropertyEventFloat GetCurrentHealth()
		{
			return currentHealth;
		}

		#endregion

		#region EiDamageInterface implementation

		public void Damage(float flatDamage)
		{
			DamagePipeline(new EiCombatData(flatDamage));
		}

		public void Damage(int damageType, float flatDamage)
		{
			DamagePipeline(new EiCombatData(damageType, flatDamage));
		}

		public void Damage(int damageType, float flatDamage, float currentHealthPercentageDamage, float maxHealthPercentageDamage)
		{
			DamagePipeline(new EiCombatData(damageType, flatDamage, currentHealthPercentageDamage, maxHealthPercentageDamage));
		}

		public void Damage(int damageType, float flatDamage, float currentHealthPercentageDamage, float maxHealthPercentageDamage, EiEntity source)
		{
			DamagePipeline(new EiCombatData(damageType, flatDamage, currentHealthPercentageDamage, maxHealthPercentageDamage, source));
		}

		/// <summary>
		/// Damage the target with specified combat data, should be a copy of original.
		/// </summary>
		/// <param name="combatData">Combat data.</param>
		public void Damage(EiCombatData combatData)
		{
			DamagePipeline(combatData);
		}

		#endregion

		#region EiHealingInterface implementation

		public void Heal(float flatHeal)
		{
			HealPipeline(new EiCombatData(flatHeal));
		}

		public void Heal(int healType, float flatHeal)
		{
			HealPipeline(new EiCombatData(healType, flatHeal));
		}

		public void Heal(int healType, float flatHeal, float currentHealthPercentageHeal, float maxHealthPercentageHeal)
		{
			HealPipeline(new EiCombatData(healType, flatHeal, currentHealthPercentageHeal, maxHealthPercentageHeal));
		}

		public void Heal(int healType, float flatHeal, float currentHealthPercentageHeal, float maxHealthPercentageHeal, EiEntity source)
		{
			HealPipeline(new EiCombatData(healType, flatHeal, currentHealthPercentageHeal, maxHealthPercentageHeal, source));
		}

		/// <summary>
		/// Heal the target with specified combat data, should be a copy of original.
		/// </summary>
		/// <param name="combatData">Combat data.</param>
		public void Heal(EiCombatData combatData)
		{
			HealPipeline(combatData.Copy);
		}

		#endregion

		#region Run Pipelines

		private void DamagePipeline(EiCombatData combatData)
		{
			if (!combatData.IsCopy)
				combatData = combatData.Copy;
			combatData.ApplyTarget(this);
#if EITRUM_ADVANCED_HEALTH
			for (int i = subscribedDamagePipeline.Count - 1; i >= 0; i--)
			{
				if (!combatData.HasTarget)
					break;
				subscribedDamagePipeline[i].Target(combatData);
			}
#else
			ApplyDamage(combatData);
#endif
		}

		private void HealPipeline(EiCombatData combatData)
		{
			if (!combatData.IsCopy)
				combatData = combatData.Copy;
			combatData.ApplyTarget(this);
#if EITRUM_ADVANCED_HEALTH
			for (int i = subscribedHealingPipeline.Count - 1; i >= 0; i--)
			{
				if (!combatData.HasTarget)
					break;
				subscribedHealingPipeline[i].Target(combatData);
			}
#else
			ApplyHeal(combatData);
#endif
		}

		#endregion

		#region Apply Damage/Healing Implementation

		void ApplyDamage(EiCombatData damage)
		{
			SetHealth(CurrentHealth - damage.TotalAmount);
		}

		void ApplyHeal(EiCombatData heal)
		{
			SetHealth(CurrentHealth + heal.TotalAmount);
		}

		#endregion

		#region Subscribe Pipelines

#if EITRUM_ADVANCED_HEALTH
		public void SubscribeDamagePipeline(int priorityLevel, Action<EiCombatData> target)
		{
			subscribedDamagePipeline.Add(priorityLevel, target);
		}

		public void SubscribeHealingPipeline(int priorityLevel, Action<EiCombatData> target)
		{
			subscribedHealingPipeline.Add(priorityLevel, target);
		}

		public void UnsubscribeDamagePipeline(Action<EiCombatData> target)
		{
			subscribedDamagePipeline.Remove(target);
		}

		public void UnsubscribeHealingPipeline(Action<EiCombatData> target)
		{
			subscribedHealingPipeline.Remove(target);
		}
#endif

		#endregion

		#region Subscribe On Death

		public void SubscribeOnDeath(Action method)
		{
			onDeath.AddAction(method);
		}

		public void SubscribeOnDeath(Action method, bool anyThread)
		{
			onDeath.AddAction(method, anyThread);
		}

		public void SubscribeOnDeath(Action<EiEntity> method)
		{
			onDeathEntity.AddAction(method);
		}

		public void SubscribeOnDeath(Action<EiEntity> method, bool anyThread)
		{
			onDeathEntity.AddAction(method, anyThread);
		}

		public void UnsubscribeOnDeath(Action method)
		{
			onDeath.RemoveAction(method);
		}

		public void UnsubscribeOnDeath(Action<EiEntity> method)
		{
			onDeathEntity.RemoveAction(method);
		}

		#endregion
	}
}