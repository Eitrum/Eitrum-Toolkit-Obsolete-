using UnityEngine;

namespace Eitrum.Health {
	public class EiDamageOnImpact : EiComponent {

		public EiCombatData damage;

		private void OnCollisionEnter(Collision collision) {
			var damageInterface = collision.collider.GetComponent<EiDamageInterface>();
			if (damageInterface != null) {
				Debug.Log("Hit and dealing damage now");
				damageInterface.Damage(damage);
			}
		}
	}
}