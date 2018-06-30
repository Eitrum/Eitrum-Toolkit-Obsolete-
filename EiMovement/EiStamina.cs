using System;
using UnityEngine;
using Eitrum.EiNet;

namespace Eitrum.Movement
{
	[AddComponentMenu ("Eitrum/Movement/Stamina")]
	public class EiStamina : EiComponent, EiNetworkObservableInterface
	{
		#region Variables

		[SerializeField]
		protected string staminaComponentName = "default-name";

		[Header ("Max Stamina")]
		[SerializeField]
		protected EiPropertyEventFloat baseMaxStamina = new EiPropertyEventFloat (100f);

		[SerializeField]
		protected EiPropertyEventFloat maxStaminaMultiplier = new EiPropertyEventFloat (1f);

		[Header ("Current Stamina Percentage")]
		[SerializeField]
		protected EiPropertyEventFloat currentStaminaPercentage = new EiPropertyEventFloat (1f);

		[Header ("Other Settings")]
		[SerializeField]
		protected bool canBeDrained = true;

		[SerializeField]
		protected float drainedStaminaRecoveryMinimum = 0.2f;

		[SerializeField]
		protected float drainedStaminaLimit = 0.05f;

		[SerializeField]
		protected bool clampStamina = true;

		[SerializeField]
		protected bool forceStaminaToBeUsed = false;

		protected bool isDrained = false;

		#endregion

		#region Properties

		public virtual string StaminaComponentName {
			get {
				return staminaComponentName;
			}
		}

		public virtual float MaxStamina {
			get {
				return baseMaxStamina.Value * maxStaminaMultiplier.Value;
			}
		}

		public virtual float CurrentStamina {
			get {
				return currentStaminaPercentage.Value * MaxStamina;
			}
			set {
				currentStaminaPercentage.Value = value / MaxStamina;
			}
		}

		public virtual float CurrentStaminaPercentage {
			get {
				return currentStaminaPercentage.Value;
			}
			set {
				currentStaminaPercentage.Value = value;
			}
		}

		public virtual bool ClampStamina {
			set {
				clampStamina = value;
			}
			get {
				return clampStamina;
			}
		}

		#endregion

		#region Core

		public virtual bool UseStamina (float amount)
		{
			return UseStaminaPercentage (amount / MaxStamina);
		}

		public virtual bool UseStamina (float amount, float percentage)
		{
			return UseStaminaPercentage (amount / MaxStamina + percentage);
		}

		public virtual bool UseStaminaPercentage (float percentage)
		{
			
			if (clampStamina) {
				if (!forceStaminaToBeUsed && (percentage > currentStaminaPercentage.Value) || (canBeDrained && isDrained)) {
					if (percentage >= Mathf.Epsilon)
						return false;
				}
				currentStaminaPercentage.Value = Mathf.Clamp01 (currentStaminaPercentage.Value - percentage);
			} else {
				currentStaminaPercentage.Value -= percentage;
			}
			if (currentStaminaPercentage.Value < drainedStaminaLimit)
				isDrained = true;
			return true;
		}

		public virtual void RegainStamina (float amount)
		{
			RegainStaminaPercentage (amount / MaxStamina);
		}

		public virtual void RegainStamina (float amount, float percentage)
		{
			RegainStaminaPercentage (amount / MaxStamina + percentage);
		}

		public virtual void RegainStaminaPercentage (float percentage)
		{
			if (clampStamina) {
				currentStaminaPercentage.Value = Mathf.Clamp01 (currentStaminaPercentage.Value + percentage);
			} else {
				currentStaminaPercentage.Value += percentage;
			}
			if (isDrained && currentStaminaPercentage.Value > drainedStaminaRecoveryMinimum)
				isDrained = false;
		}

		public virtual void SetBaseMaxStamina (float amount)
		{
			baseMaxStamina.Value = amount;
		}

		public virtual void SetMaxStaminaMultiplier (float amount)
		{
			maxStaminaMultiplier.Value = amount;
		}

		#endregion

		#region Subscribe/Unsubscribe

		public virtual EiPropertyEventFloat GetCurrentStaminaPercentage ()
		{
			return currentStaminaPercentage;
		}

		public virtual EiPropertyEventFloat GetBaseMaxStamina ()
		{
			return baseMaxStamina;
		}

		public virtual EiPropertyEventFloat GetMaxStaminaMultiplier ()
		{
			return maxStaminaMultiplier;
		}

		#endregion

		#region EiNetworkObservable implementation

		public void OnNetworkSerialize (EiBuffer buffer, bool isWriting)
		{
			if (isWriting) {
				buffer.Write (baseMaxStamina.Value);
				buffer.Write (maxStaminaMultiplier.Value);
				buffer.Write (currentStaminaPercentage.Value);
			} else {
				baseMaxStamina.Value = buffer.ReadFloat ();
				maxStaminaMultiplier.Value = buffer.ReadFloat ();
				currentStaminaPercentage.Value = buffer.ReadFloat ();
			}
		}

		#endregion
	}
}