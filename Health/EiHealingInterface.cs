using System;

namespace Eitrum.Health
{
	public interface EiHealingInterface
	{
		void Heal (float flatHeal);

		void Heal (int healType, float flatHeal);

		void Heal (int healType, float flatHeal, float currentHealthPercentageHeal, float maxHealthPercentageHeal);

		void Heal (int healType, float flatHeal, float currentHealthPercentageHeal, float maxHealthPercentageHeal, EiEntity source);

		/// <summary>
		/// Heal the target with specified combat data, should be a copy of original.
		/// </summary>
		/// <param name="combatData">Combat data.</param>
		void Heal (EiCombatData combatData);

		EiHealth HealthComponent{ get; }
	}
}

