using System;
using UnityEngine;

namespace Eitrum
{
	[Serializable]
	public class EiStat
	{
		#region Variables

		[SerializeField]
		private float baseStat = 0f;
		[SerializeField]
		private float statMultiplier = 1f;
		[SerializeField]
		private float statMultiplierX = 1f;

		EiTrigger<float> trigger = new EiTrigger<float> ();

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the stat.
		/// Setting the stat will do the following calculation.
		/// baseStat = value / (multiplier * multiplierX);
		/// </summary>
		/// <value>The stat.</value>
		public float TotalStat {
			get {
				return baseStat * statMultiplier * statMultiplierX;
			}
			set {
				baseStat = value / (statMultiplier * statMultiplierX);
			}
		}

		public float BaseStat {
			get {
				return baseStat;
			}
		}

		public float StatMultiplier {
			get {
				return statMultiplier;
			}
		}

		public float StatMultiplierX {
			get {
				return statMultiplierX;
			}
		}

		#endregion

		#region Constructors

		public EiStat ()
		{
			this.baseStat = 0f;
			this.statMultiplier = 1f;
			this.statMultiplierX = 1f;
		}

		public EiStat (float baseStat)
		{
			this.baseStat = baseStat;
			this.statMultiplier = 1f;
			this.statMultiplierX = 1f;
		}

		public EiStat (float baseStat, float statMultiplier, float statMultiplierX)
		{
			this.baseStat = baseStat;
			this.statMultiplier = statMultiplier;
			this.statMultiplierX = statMultiplierX;
		}

		#endregion

		#region Trigger

		public void Trigger ()
		{
			trigger.Trigger (TotalStat);
		}

		public EiTrigger<float> GetTrigger ()
		{
			return trigger;
		}

		public void Subscribe (Action<float> method)
		{
			trigger.AddAction (method);
		}

		public void Subscribe (Action<float> method, bool anyThread)
		{
			trigger.AddAction (method, anyThread);
		}

		public void Unsubscribe (Action<float> method)
		{
			trigger.RemoveAction (method);
		}

		#endregion

		#region Base Stat

		public void AddBaseValue (float amount)
		{
			baseStat += amount;
			trigger.Trigger (TotalStat);
		}

		public void RemoveBaseValue (float amount)
		{
			baseStat -= amount;
			trigger.Trigger (TotalStat);
		}

		public void SetBaseValue (float value)
		{
			baseStat = value;
			trigger.Trigger (TotalStat);
		}

		#endregion

		#region Multiplier

		public void AddStatMultiplier (float amount)
		{
			statMultiplier += amount;
			trigger.Trigger (TotalStat);
		}

		public void RemoveStatMultiplier (float amount)
		{
			statMultiplier -= amount;
			trigger.Trigger (TotalStat);
		}

		public void SetStatMultiplier (float value)
		{
			statMultiplier = value;
			trigger.Trigger (TotalStat);
		}

		#endregion

		#region Multiplier X

		public void MultiplyMultiplier (float value)
		{
			statMultiplierX *= value;
			trigger.Trigger (TotalStat);
		}

		public void SetMultiplyMultiplier (float value)
		{
			statMultiplierX = value;
			trigger.Trigger (TotalStat);
		}

		#endregion
	}
}