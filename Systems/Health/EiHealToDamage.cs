using Eitrum.Engine.Core;
using System;
using UnityEngine;

namespace Eitrum.Health
{
	[AddComponentMenu("Eitrum/Health/Heal To Damage")]
	public class EiHealToDamage : EiComponent
	{
		#region Variables

		[SerializeField]
		protected int targetDamageType;

		[SerializeField]
		protected int priorityLevel = 10001;

		[SerializeField]
		protected EiHealth healthComponent;

		protected EiTrigger onHealChange = new EiTrigger();

		#endregion

		#region Properties

		public int TargetDamageType
		{
			get
			{
				return targetDamageType;
			}
			set
			{
				targetDamageType = value;
			}
		}

		#endregion

		#region Core

		void Awake()
		{
#if EITRUM_ADVANCED_HEALTH
			healthComponent.SubscribeHealingPipeline (-priorityLevel, ApplyHeal);
#else
			Debug.LogWarning("Enable ADVANCED_HEALTH to enable 'EiHealToDamage'");
#endif
		}

		void ApplyHeal(EiCombatData combatData)
		{
			if (targetDamageType == -1 || combatData.DamageType == targetDamageType)
			{
				var copy = combatData.Copy;
				combatData.Clear();
				copy.Target.Damage(copy);
				onHealChange.Trigger();
			}
		}

		#endregion
	}
}