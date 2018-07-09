using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Utility.GravityGun
{
	public class EiGravityGun : EiComponent
	{
		#region Variables

		[Header ("Object Settings")]
		[SerializeField]
		private Vector3 anchor;
		[SerializeField]
		private EiEntity targetObject;
		[SerializeField]
		private Vector3 targetRotation = Vector3.zero;

		[Header ("Speed + Break Settings")]
		[SerializeField]
		private float velocityMultiplier = 4f;
		[SerializeField]
		private float breakForceMultiplier = 4f;

		private Rigidbody targetRigidbody;
		private EiGravityGunForceCalculation forceCalculation;
		private bool isGrabbing = false;
		private bool didHaveGravity = false;

		#endregion

		#region Properties

		public Vector3 AnchorPosition {
			get {
				return this.transform.position + this.transform.rotation * anchor;
			}
		}

		public Quaternion AnchorRotation {
			get {
				return this.transform.rotation * Quaternion.Euler (targetRotation);
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			SubscribeFixedUpdate ();
			GrabEntity (targetObject);
		}

		public void GrabEntity (EiEntity entity)
		{
			if (!entity || !entity.Body)
				return;
			
			if (targetObject)
				ReleaseEntity ();

			targetObject = entity;
			targetRigidbody = entity.Body;
			didHaveGravity = targetRigidbody.useGravity;
			targetRigidbody.useGravity = false;
			forceCalculation = targetObject.AddComponent<EiGravityGunForceCalculation> ();
			isGrabbing = true;
		}

		public void ReleaseEntity ()
		{
			isGrabbing = false;
			if (targetObject) {
				targetObject.UnfreezePhysics ();
				targetObject = null;
			}
			if (targetRigidbody) {
				targetRigidbody.useGravity = didHaveGravity;
				targetRigidbody = null;
			}
			if (forceCalculation) {
				Destroy (forceCalculation);
			}
		}

		public override void FixedUpdateComponent (float time)
		{
			if (!isGrabbing)
				return;
			if (!targetObject) {
				isGrabbing = false;
				return;
			}

			// Force Calculation
			Vector3 difference = AnchorPosition - targetRigidbody.transform.position;
			float distance = difference.magnitude;
			var mass = targetRigidbody.mass;
			float forceAmount = distance * mass * velocityMultiplier;

			if (forceCalculation.force / time > mass * 10f * breakForceMultiplier) {
				#if UNITY_EDITOR
				if (debug) {
					Debug.Log ("Break object connection at force: " + forceCalculation.force / time);
				}
				#endif
				ReleaseEntity ();
				return;
			}
			var targetForce = difference * (forceAmount / time);
			var currentVelocity = targetRigidbody.velocity / time;
			targetRigidbody.AddForce (targetForce - currentVelocity);

			targetRigidbody.angularVelocity = Vector3.Slerp (targetRigidbody.angularVelocity, Vector3.zero, time * velocityMultiplier);
			targetRigidbody.transform.rotation = Quaternion.Slerp (targetRigidbody.transform.rotation, AnchorRotation, time * velocityMultiplier);
		}

		#endregion

		#if UNITY_EDITOR
		[Header ("Editor Only")]
		public bool debug = false;

		void OnDrawGizmos ()
		{
			if (!debug)
				return;
			Gizmos.DrawWireSphere (AnchorPosition, 0.02f);
			if (targetObject) {
				targetRigidbody = targetObject.Body;
			}
			if (targetRigidbody)
				Gizmos.DrawWireMesh (targetRigidbody.GetComponentInChildren<MeshFilter> ().sharedMesh, AnchorPosition, AnchorRotation, targetRigidbody.transform.lossyScale);
		}

		#endif
	}

	#region Other Component

	public class EiGravityGunForceCalculation : EiComponent
	{
		[Readonly]
		public float force = 0f;

		void OnCollisionStay (Collision collision)
		{
			force = collision.impulse.magnitude;
		}
	}

	#endregion
}