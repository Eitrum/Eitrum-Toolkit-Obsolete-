using System;
using UnityEngine;

namespace Eitrum.Health
{
	[AddComponentMenu ("Eitrum/Health/Heal To Damage")]
	public class EiHealToDamage : EiComponent
	{
		#region Variables

		[SerializeField]
		protected int targetDamageType;

		[SerializeField]
		protected int priorityLevel = 10001;

		[SerializeField]
		protected EiHealth healthComponent;

		protected EiTrigger onHealChange = new EiTrigger ();

		#endregion

		#region Properties

		public virtual string TargetDamageTypeString {
			get {
				return "Not defined";
			}
		}

		public virtual int TargetDamageType {
			get {
				return targetDamageType;
			}
			set {
				targetDamageType = value;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			healthComponent.SubscribeHealingPipeline (-priorityLevel, ApplyHeal);
		}

		void ApplyHeal (EiDamage damage)
		{
			if (damage.DamageType == targetDamageType) {
				damage.Convert ();
				onHealChange.Trigger ();
			}
		}

		#endregion
	}
}