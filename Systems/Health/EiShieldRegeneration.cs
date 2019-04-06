using System;
using UnityEngine;

namespace Eitrum.Health
{
	[AddComponentMenu ("Eitrum/Health/Shield Regeneration")]
	public class EiShieldRegeneration : EiComponent
	{
		#region Variables

		[Header ("Settings (Per Second)")]
		[SerializeField]
		protected EiPropertyEventFloat flatRegen = new EiPropertyEventFloat (25f);
		[SerializeField]
		protected EiPropertyEventFloat currentPercentageRegen = new EiPropertyEventFloat (0f);
		[SerializeField]
		protected EiPropertyEventFloat maxPercentageRegen = new EiPropertyEventFloat (0f);

		[Header ("Time Settings")]
		[SerializeField]
		protected EiPropertyEventFloat timeBeforeRegenAfterDamageTaken = new EiPropertyEventFloat (5f);
		[SerializeField]
		protected EiPropertyEventFloat timeBetweenEachHeal = new EiPropertyEventFloat (0f);

		[Header ("Components")]
		[SerializeField]
		protected EiShield shieldComponent;

		protected bool useTimeBetweenHealing = false;
		protected float lastShieldChange = 0f;
		protected float currentTimeToShieldRegeneration = 0f;
		protected float timeLeftToStartShieldRegeneration = 0f;

		#endregion

		#region Properties

		public virtual float ShieldRegenerationPerSecond {
			get {
				return flatRegen.Value + currentPercentageRegen.Value * shieldComponent.CurrentShield + maxPercentageRegen.Value * shieldComponent.MaxShield;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			shieldComponent.GetCurrentShield ().Subscribe (ShieldChange);
			timeBetweenEachHeal.SubscribeAndRun (TimeBetweenHealSetting);
			SubscribeThreadedUpdate ();
		}

		void TimeBetweenHealSetting (float time)
		{
			useTimeBetweenHealing = time <= 0f;
		}

		void ShieldChange (float shield)
		{
			if (shield < lastShieldChange) {
				timeLeftToStartShieldRegeneration = timeBeforeRegenAfterDamageTaken.Value;
				currentTimeToShieldRegeneration = 0f;
			}
			lastShieldChange = shield;
		}

		public override void ThreadedUpdateComponent (float time)
		{
			if (timeLeftToStartShieldRegeneration > 0f) {
				timeLeftToStartShieldRegeneration -= time;
			} else {
				if (useTimeBetweenHealing) {
					if (currentTimeToShieldRegeneration > 0)
						currentTimeToShieldRegeneration -= time;
					while (currentTimeToShieldRegeneration <= 0f) {
						currentTimeToShieldRegeneration += timeBetweenEachHeal.Value;
						shieldComponent.AddCurrentShield (ShieldRegenerationPerSecond * timeBetweenEachHeal.Value);
					}
				} else {
					shieldComponent.AddCurrentShield (ShieldRegenerationPerSecond * time);
				}
			}
		}

		#endregion

		#if UNITY_EDITOR

		protected override void AttachComponents ()
		{
			shieldComponent = GetComponent<EiShield> ();
			base.AttachComponents ();
		}

		#endif
	}
}