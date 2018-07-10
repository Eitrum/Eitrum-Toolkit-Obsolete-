using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Eitrum.Utility.LaunchPad
{
	[AddComponentMenu ("Eitrum/Utility/Launch Pad/Launch Pad (Basic)")]
	public class EiLaunchPad : EiComponent
	{
		#region Variables

		[SerializeField]
		private Vector3 direction = Vector3.up;
		[SerializeField]
		private bool setVelocity = true;
		[SerializeField]
		private float launchForce = 50f;
		[SerializeField]
		private bool relativeToMass = true;

		private EiTrigger onLaunch = new EiTrigger ();
		private EiTrigger<EiEntity> onLaunchEntity = new EiTrigger<EiEntity> ();

		#endregion

		#region Properties

		public Vector3 Direction {
			get {
				return this.transform.rotation * direction;
			}
		}

		public Vector3 RelativeDirection {
			get {
				return direction;
			}
		}

		public bool UsesVelocity {
			get {
				return setVelocity;
			}
		}

		public bool UsesForce {
			get {
				return !setVelocity;
			}
		}

		public float LaunchForce {
			get {
				return launchForce * direction.magnitude;
			}
		}

		public bool IsForceRelativeToMass {
			get {
				return relativeToMass;
			}
		}

		#endregion

		#region Core

		public void Launch (EiEntity entity)
		{
			if (!entity || !entity.Body)
				return;
			
			var force = launchForce;
			
			if (setVelocity) {
				entity.Body.velocity = (this.transform.rotation * direction) * force;
			} else {
				if (relativeToMass)
					force *= entity.Body.mass;
				entity.Body.AddForce ((this.transform.rotation * direction) * force, ForceMode.Impulse);
			}
			onLaunch.Trigger ();
			onLaunchEntity.Trigger (entity);
		}

		void OnTriggerEnter (Collider collider)
		{
			var entity = collider.GetComponent<EiEntity> ();
			if (entity) {
				Launch (entity);
			}
		}

		#endregion

		#region Subscribe

		public void SubscribeOnLaunch (Action method)
		{
			onLaunch.AddAction (method);
		}

		public void SubscribeOnLaunch (Action method, bool anyThread)
		{
			onLaunch.AddAction (method, anyThread);
		}

		public void SubscribeOnLaunch (Action<EiEntity> method)
		{
			onLaunchEntity.AddAction (method);
		}

		public void SubscribeOnLaunch (Action<EiEntity> method, bool anyThread)
		{
			onLaunchEntity.AddAction (method, anyThread);
		}

		public void UnsubscribeOnLaunch (Action method)
		{
			onLaunch.RemoveAction (method);
		}

		public void UnsubscribeOnLaunch (Action<EiEntity> method)
		{
			onLaunchEntity.RemoveAction (method);
		}

		#endregion

		#if UNITY_EDITOR

		void OnDrawGizmos ()
		{
			Gizmos.DrawLine (this.transform.position, this.transform.position + this.transform.rotation * direction);
		}

		#endif
	}
}