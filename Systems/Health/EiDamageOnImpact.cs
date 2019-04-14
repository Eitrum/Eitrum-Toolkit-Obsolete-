using Eitrum.Engine.Core;
using UnityEngine;

namespace Eitrum.Health {
	public class EiDamageOnImpact : EiComponent {

		public EiCombatData damage;

		public float minimumForceToDealDamage = 0f;

		private void OnCollisionEnter(Collision collision) {
			if (collision.impulse.magnitude < minimumForceToDealDamage)
				return;
			var damageInterface = collision.collider.GetComponent<EiDamageInterface>();
			if (damageInterface != null) {
				var copy = damage.Copy;
				copy.ApplySource(Entity);
				
				damageInterface.Damage(damage);
			}
		}
	}
}