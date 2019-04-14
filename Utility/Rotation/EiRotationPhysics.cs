using Eitrum.Engine.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Utility.Rotation {
	[AddComponentMenu("Eitrum/Utility/Rotation/Physics Rotation")]
	public class EiRotationPhysics : EiComponent {
		#region Variables

		[SerializeField]
		private Vector3 rotationForce = Vector3.zero;
		[SerializeField]
		private bool relativeToMass = true;

		private Rigidbody body;

		#endregion

		#region Core

		private void Awake() {
			body = Entity.Body;
		}

		private void OnEnable() {
			SubscribeFixedUpdate();
		}

		private void OnDisable() {
			UnsubscribeFixedUpdate();
		}

		public override void FixedUpdateComponent(float time) {
			var forceToAdd = rotationForce * Mathf.Deg2Rad;
			if (relativeToMass)
				forceToAdd *= body.mass;
			if (body.angularVelocity.sqrMagnitude < forceToAdd.sqrMagnitude)
				body.AddTorque(forceToAdd, ForceMode.Force);
		}

		#endregion

	}
}