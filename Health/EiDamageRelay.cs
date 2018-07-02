using System;
using UnityEngine;

namespace Eitrum.Health
{
	/// <summary>
	/// Ei damage relay, very useful for adding different damage values on different body parts.
	/// </summary>
	[AddComponentMenu ("Eitrum/Health/Damage Relay")]
	public class EiDamageRelay : EiComponent, EiDamageInterface
	{
		#region Variables

		[Header ("Settings")]
		[SerializeField]
		protected EiPropertyEventFloat flatDamageReduction = new EiPropertyEventFloat (0f);
		[SerializeField]
		protected EiPropertyEventFloat damageMultiplier = new EiPropertyEventFloat (1f);
		[SerializeField]
		protected bool calculateFlatReducationFirst = false;
		[Header ("Components")]
		[SerializeField]
		protected EiHealth damageTarget;

		protected EiTrigger<EiDamage> onHit = new EiTrigger<EiDamage> ();

		#endregion

		#region Properties

		public virtual float FlatDamageReduction {
			get {
				return flatDamageReduction.Value;
			}
			set {
				flatDamageReduction.Value = value;
			}
		}

		public virtual float DamageMultiplier {
			get {
				return damageMultiplier.Value;
			}
			set {
				damageMultiplier.Value = value;
			}
		}

		public virtual bool CalculateFlatReductionFirst {
			get {
				return calculateFlatReducationFirst;
			}set {
				calculateFlatReducationFirst = value;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			var ent = Entity;
		}

		public virtual EiTrigger<EiDamage> GetOnArmorHitTrigger ()
		{
			return onHit;
		}

		#endregion

		#region Add

		public virtual void AddFlatDamageReduction (float amount)
		{
			flatDamageReduction.Value += amount;
		}

		public virtual void AddDamageMultiplier (float amount)
		{
			damageMultiplier.Value += amount;
		}

		#endregion

		#region Remove

		public virtual void RemoveFlatDamageReduction (float amount)
		{
			flatDamageReduction.Value -= amount;
		}

		public virtual void RemoveDamageMultiplier (float amount)
		{
			damageMultiplier.Value -= amount;
		}

		#endregion

		#region Set

		public virtual void SetFlatDamageReduction (float value)
		{
			flatDamageReduction.Value = value;
		}

		public virtual void SetDamageMultiplier (float value)
		{
			damageMultiplier.Value = value;
		}

		#endregion

		#region Get

		public virtual EiPropertyEventFloat GetFlatDamageReduction ()
		{
			return flatDamageReduction;
		}

		public virtual EiPropertyEventFloat GetDamageMultiplier ()
		{
			return damageMultiplier;
		}

		public virtual EiTrigger<EiDamage> GetOnHitTrigger ()
		{
			return onHit;
		}

		#endregion

		#region EiDamageInterface implementation

		public void Damage (EiDamage damage)
		{
			if (damage.Target == null) {
				damage.Target = damageTarget;
			} else {
				if ((damage.Target) != damageTarget) {
					Debug.LogWarning ("The damage target is different to what it hit\n" + Entity.EntityName);
				}
			}
			onHit.Trigger (damage);
			damage.ReduceDamage (flatDamageReduction.Value, damageMultiplier.Value, calculateFlatReducationFirst);
			damageTarget.Damage (damage);
		}

		#endregion

		#if UNITY_EDITOR

		protected override void AttachComponents ()
		{
			damageTarget = GetComponentInParent<EiHealth> ();
			base.AttachComponents ();
		}

		#endif
	}
}