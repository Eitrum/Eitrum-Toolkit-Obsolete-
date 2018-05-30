using System;
using UnityEngine;

namespace Eitrum.Health
{
	[AddComponentMenu ("Eitrum/Health/Health Regeneration")]
	public class EiHealthRegeneration : EiComponent
	{
		#region Variables

		[Header ("Settings")]
		[SerializeField]
		protected EiCombatData healDataPerSecond;

		[Header ("Time Settings")]
		[SerializeField]
		protected EiPropertyEventFloat timeBeforeRegenAfterDamageTaken = new EiPropertyEventFloat (5f);
		[SerializeField]
		protected EiPropertyEventFloat timeBetweenEachHeal = new EiPropertyEventFloat (0f);

		[Header ("Components")]
		[SerializeField]
		protected EiHealth healthComponent;

		protected bool useTimeBetweenHealing = false;
		protected float lastHealthChange = 0f;
		protected float currentTimeToHeal = 0f;
		protected float timeLeftToStartHeal = 0f;

		#endregion

		#region Properties

		public EiCombatData HealData {
			get {
				return healDataPerSecond;
			}
		}

		public float HealAmountPerSecond {
			get {
				return healDataPerSecond.flatAmount +
				healDataPerSecond.currentHealthPercentage * healthComponent.CurrentHealth +
				healDataPerSecond.maxHealthPercentage * healthComponent.MaxHealth;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			timeBetweenEachHeal.SubscribeAndRun (TimeBetweenHealSetting);
			SubscribeThreadedUpdate ();
		}

		void TimeBetweenHealSetting (float time)
		{
			useTimeBetweenHealing = time <= 0f;
		}

		void HealthChange (float health)
		{
			if (health < lastHealthChange) {
				timeLeftToStartHeal = timeBeforeRegenAfterDamageTaken.Value;
				currentTimeToHeal = 0f;
			}
			lastHealthChange = health;
		}

		public override void ThreadedUpdateComponent (float time)
		{
			if (timeLeftToStartHeal > 0f) {
				timeLeftToStartHeal -= time;
			} else {
				if (useTimeBetweenHealing) {
					if (currentTimeToHeal > 0)
						currentTimeToHeal -= time;
					while (currentTimeToHeal <= 0f) {
						currentTimeToHeal += timeBetweenEachHeal.Value;
						var heal = EiDamage.NewInstance.ConfigHealing (healDataPerSecond, healthComponent);
						heal.Multiply (timeBetweenEachHeal.Value);
						healthComponent.Heal (heal);
					}
				} else {
					var heal = EiDamage.NewInstance.ConfigHealing (healDataPerSecond, healthComponent);
					heal.Multiply (time);
					healthComponent.Heal (heal);
				}
			}
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