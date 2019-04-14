using Eitrum.Engine.Core;
using System;

namespace Eitrum.Health
{
	public interface EiDamageInterface
	{
		void Damage (float flatDamage);

		void Damage (int damageType, float flatDamage);

		void Damage (int damageType, float flatDamage, float currentHealthPercentageDamage, float maxHealthPercentageDamage);

		void Damage (int damageType, float flatDamage, float currentHealthPercentageDamage, float maxHealthPercentageDamage, EiEntity source);

		/// <summary>
		/// Damage the target with specified combat data, should be a copy of original.
		/// </summary>
		/// <param name="combatData">Combat data.</param>
		void Damage (EiCombatData combatData);

		EiHealth HealthComponent{ get; }
	}
}

