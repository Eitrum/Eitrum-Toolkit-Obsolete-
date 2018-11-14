using UnityEngine;
using Eitrum.Mathematics;

namespace Eitrum.Utility.Rotation {
	[AddComponentMenu("Eitrum/Utility/Rotation/Random Rotation")]
	public class EiRandomRotation : EiComponent {
		#region Variables

		[SerializeField]
		private Vector3 minRotation = Vector3.zero;
		[SerializeField]
		private Vector3 maxRotation = Vector3.zero;
		[SerializeField]
		private Space rotationSpace = Space.Self;

		private Vector3 rotation;

		#endregion

		#region Core

		private void Awake() {
			rotation = new Vector3(
				Mathf.Lerp(minRotation.x, maxRotation.x, EiRandom.Float),
				Mathf.Lerp(minRotation.y, maxRotation.y, EiRandom.Float),
				Mathf.Lerp(minRotation.z, maxRotation.z, EiRandom.Float)
				);
			SubscribeUpdate();
		}

		private void OnEnable() {
			SubscribeUpdate();
		}

		private void OnDisable() {
			UnsubscribeUpdate();
		}

		public override void UpdateComponent(float time) {
			this.transform.Rotate(rotation * time, rotationSpace);
		}

		#endregion
	}
}