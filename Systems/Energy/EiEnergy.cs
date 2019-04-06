using System;
using UnityEngine;

namespace Eitrum.Energy
{
	public class EiEnergy : EiComponent
	{
		#region Variables

		[Header ("Settings")]
		[SerializeField]
		private EiStat maxEnergy = new EiStat (100f, 1f, 1f);
		[SerializeField]
		private EiPropertyEventFloat currentEnergy = new EiPropertyEventFloat (100f);

		#endregion

		#region Properties

		public float MaxEnergy {
			get {
				return maxEnergy.TotalStat;
			}
		}

		public float CurrentEnergy {
			get {
				return currentEnergy.Value;
			}
		}

		public float CurrentEnergyPercentage {
			get {
				return CurrentEnergy / MaxEnergy;
			}
		}

		public float MissingEnergy {
			get {
				return MaxEnergy - CurrentEnergy;
			}
		}

		#endregion

		#region Core

		public bool HasEnergy (float amount)
		{
			return currentEnergy.Value >= amount;
		}

		public bool UseEnergy (float amount)
		{
			if (amount < 0f)
				return false;
			var current = currentEnergy.Value;
			if (current >= amount) {
				currentEnergy.Value = current - amount;
				return true;
			}
			return false;
		}

		public bool RegainEnergy (float amount)
		{
			if (amount < 0f)
				return false;
			var current = currentEnergy.Value;
			if (current < MaxEnergy) {
				currentEnergy.Value = Mathf.Min (current + amount, MaxEnergy);
				return true;
			}
			return false;
		}

		public EiStat GetMaxEnergy ()
		{
			return maxEnergy;
		}

		public EiPropertyEventFloat GetCurrentEnergy ()
		{
			return currentEnergy;
		}

		#endregion

		#region Subscribe

		public void SubscribeOnMaxEnergyChanged (Action<float> method)
		{
			maxEnergy.Subscribe (method);
		}

		public void SubscribeOnMaxEnergyChanged (Action<float> method, bool anyThread)
		{
			maxEnergy.Subscribe (method, anyThread);
		}

		public void SubscribeOnCurrentEnergyChanged (Action<float> method)
		{
			currentEnergy.Subscribe (method);
		}

		public void SubscribeOnCurrentEnergyChanged (Action<float> method, bool anyThread)
		{
			currentEnergy.Subscribe (method, anyThread);
		}

		public void UnsubscribeOnMaxEnergyChanged (Action<float> method)
		{
			maxEnergy.Unsubscribe (method);
		}

		public void UnsubscribeOnCurrentEnergyChanged (Action<float> method)
		{
			currentEnergy.Unsubscribe (method);
		}

		#endregion
	}
}