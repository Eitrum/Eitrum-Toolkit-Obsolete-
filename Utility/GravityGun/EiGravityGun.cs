using System;
using UnityEngine;

namespace Eitrum.Utility.GravityGun
{
	[AddComponentMenu ("Eitrum/Utility/Gravity Gun/Gravity Gun")]
	public class EiGravityGun : EiComponent
	{

		#region Variables

		[Header ("Distance Settings")]
		[SerializeField]
		private float minDistance = 1f;
		[SerializeField]
		private float maxDistance = 5f;

		[Header ("Weight Settings")]
		[SerializeField]
		private float maxWeight = 50f;
		[Header ("Release Settings")]
		[SerializeField]
		private float releaseForce = 50f;
		[SerializeField]
		private bool relativeToMass = true;

		[Header ("Components")]
		[SerializeField]
		private EiGravityCore gravityCore;

		#endregion

		#region Properties

		public float MinDistance {
			get {
				return minDistance;
			}
		}

		public float MaxDistance {
			get {
				return maxDistance;
			}
		}

		public float MaxWeight {
			get {
				return maxWeight;
			}
		}

		public Vector3 AnchorPosition {
			get {
				return gravityCore.AnchorPosition;
			}
		}

		public Quaternion AnchorRotation {
			get {
				return gravityCore.AnchorRotation;
			}
		}

		public bool HasTarget {
			get {
				return gravityCore.Target != null;
			}
		}

		public Vector3 TargetPosition {
			get {
				return HasTarget ? gravityCore.Target.transform.position : this.transform.position;
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			SubscribeUpdate ();
		}

		public override void UpdateComponent (float time)
		{
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				if (HasTarget)
					Release ();
				else
					Fire ();
			}
			if (Input.GetKeyDown (KeyCode.Mouse1)) {
				if (HasTarget) {
					var body = gravityCore.Target.Body;
					Release ();

					body.AddForce (transform.forward * (releaseForce * (relativeToMass ? body.mass : 1f)), ForceMode.Impulse);
				}
			}
		}

		public void Fire ()
		{
			RaycastHit hit;
			if (transform.ToRay ().Hit (out hit, maxDistance)) {
				var entity = hit.collider.GetComponent<EiEntity> ();
				if (entity && entity.Body) {
					if (entity.Body.mass > maxWeight)
						return;
					gravityCore.GrabEntity (entity);
					gravityCore.SetAnchorPosition (new Vector3 (0, 0, Mathf.Clamp (hit.distance, minDistance, maxDistance)));
					gravityCore.SetAnchorRotationRelative (entity.transform.rotation);
				}
			}
		}

		public void Release ()
		{
			gravityCore.ReleaseEntity ();
		}

		#endregion

		#if UNITY_EDITOR

		protected override void AttachComponents ()
		{
			base.AttachComponents ();
			gravityCore = this.GetOrAddComponent<EiGravityCore> ();
		}

		#endif
	}
}

