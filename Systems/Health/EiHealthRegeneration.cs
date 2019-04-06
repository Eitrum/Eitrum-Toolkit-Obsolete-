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
		private EiCombatData healDataPerSecond = default(EiCombatData);

		[Header ("Time Settings")]
		[SerializeField]
		private EiPropertyEventFloat timeBeforeRegenAfterDamageTaken = new EiPropertyEventFloat (5f);
		[SerializeField]
		private EiPropertyEventFloat timeBetweenEachHeal = new EiPropertyEventFloat (0f);

		[Header ("Components")]
		[SerializeField]
		private EiHealth healthComponent;

		private bool useTimeBetweenHealing = false;
		private float lastHealthChange = 0f;
		private float currentTimeToHeal = 0f;
		private float timeLeftToStartHeal = 0f;

		#endregion

		#region Properties

		public EiCombatData HealData {
			get {
				return healDataPerSecond;
			}
		}

		public float HealAmountPerSecond {
			get {
				return healDataPerSecond.FlatAmount +
				healDataPerSecond.CurrentHealthPercentage * healthComponent.CurrentHealth +
				healDataPerSecond.MaxHealthPercentage * healthComponent.MaxHealth;
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
						healthComponent.Heal (healDataPerSecond.TotalAmount * timeBetweenEachHeal.Value);
					}
				} else {
					healthComponent.Heal (healDataPerSecond.TotalAmount * time);
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