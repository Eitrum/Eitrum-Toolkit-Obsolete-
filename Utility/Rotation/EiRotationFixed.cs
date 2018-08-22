using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Utility.Rotation {
	[AddComponentMenu("Eitrum/Utility/Rotation/Fixed Rotation")]
	public class EiRotationFixed : EiComponent {
		#region Variables

		[SerializeField]
		private Vector3 rotation = Vector3.zero;
		[SerializeField]
		private Space rotationSpace = Space.Self;

		#endregion

		#region Core

		private void Awake() {
			SubscribeFixedUpdate();
		}

		private void OnEnable() {
			SubscribeFixedUpdate();
		}

		private void OnDisable() {
			UnsubscribeFixedUpdate();
		}

		public override void FixedUpdateComponent(float time) {
			this.transform.Rotate(rotation * time, rotationSpace);
		}

		#endregion
	}
}