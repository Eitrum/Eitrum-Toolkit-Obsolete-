using System;
using UnityEngine;
using UnityEngine.Events;
using Eitrum.Health;

namespace Eitrum.Utility.Trigger
{
	[AddComponentMenu ("Eitrum/Utility/Trigger/On Hit Trigger")]
	public class EiOnHitTrigger : EiComponent, EiDamageInterface
	{
		public UnityEventEiEntity onHitTrigger;

		void OnHit (EiEntity source)
		{
			onHitTrigger.Invoke (source);
		}

		#region EiDamageInterface implementation

		public void Damage (float flatDamage)
		{
			OnHit (null);
		}

		public void Damage (int damageType, float flatDamage)
		{
			OnHit (null);
		}

		public void Damage (int damageType, float flatDamage, float currentHealthPercentageDamage, float maxHealthPercentageDamage)
		{
			OnHit (null);
		}

		public void Damage (int damageType, float flatDamage, float currentHealthPercentageDamage, float maxHealthPercentageDamage, EiEntity source)
		{
			OnHit (source);
		}

		public void Damage (EiCombatData combatData)
		{
			OnHit (combatData.SourceEntity);
		}

		public EiHealth HealthComponent {
			get {
				return null;
			}
		}

		#endregion
	}
}

