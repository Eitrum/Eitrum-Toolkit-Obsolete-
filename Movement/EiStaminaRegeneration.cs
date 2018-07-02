using System;
using UnityEngine;

namespace Eitrum.Movement
{
	[AddComponentMenu ("Eitrum/Movement/Stamina Regeneration")]
	public class EiStaminaRegeneration : EiComponent
	{
		#region Variables

		[Header ("Stamina Regeneration")]
		[SerializeField]
		protected float baseRegenerationPerSecond = 30f;

		[SerializeField]
		protected float regenerationPerSecondMultiplier = 1f;

		[Header ("Delay Settings")]
		[SerializeField]
		protected float baseDelay = 2f;

		[SerializeField]
		protected float delayMultiplier = 1f;

		[Header ("Other Settings")]
		[SerializeField]
		protected EiStamina staminaComponent;

		protected float waitTime = 0f;

		protected float lastUpdate = 0f;

		#endregion

		#region Properties

		public virtual float RegenerationPerSecond {
			get {
				return baseRegenerationPerSecond * regenerationPerSecondMultiplier;
			}
		}

		public virtual float RegenerationDelay {
			get {
				return baseDelay * delayMultiplier;
			}
		}

		public virtual EiStamina Stamina {
			get {
				return staminaComponent;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			SubscribeFixedUpdate ();
			Stamina.GetCurrentStaminaPercentage ().SubscribeAndRun (CurrentStaminaChanged);
		}

		void CurrentStaminaChanged (float f)
		{
			if (lastUpdate > f)
				waitTime = RegenerationDelay;
			lastUpdate = f;
		}

		public override void FixedUpdateComponent (float time)
		{
			if (waitTime >= 0f)
				waitTime -= time;
			else {
				Stamina.RegainStamina (RegenerationPerSecond * time);
			}
		}

		void OnDestroy ()
		{
			Stamina.GetCurrentStaminaPercentage ().Unsubscribe (CurrentStaminaChanged);
		}

		#endregion

		#region Helper

		[ContextMenu ("Link Stamina Component")]
		protected void LinkStaminaComponent ()
		{
			staminaComponent = GetComponent<EiStamina> ();
		}

		[ContextMenu ("Reset To Default")]
		protected void ResetToDefault ()
		{
			baseRegenerationPerSecond = 30f;
			regenerationPerSecondMultiplier = 1f;
			baseDelay = 2f;
			delayMultiplier = 1f;
		}

		#endregion
	}
}

