using Eitrum.Engine.Core;
using UnityEngine;

namespace Eitrum.Utility.Rotation {
	[AddComponentMenu("Eitrum/Utility/Rotation/Basic Rotation")]
	public class EiRotation : EiComponent {
		#region Variables

		[SerializeField]
		private Vector3 rotation = Vector3.zero;
		[SerializeField]
		private Space rotationSpace = Space.Self;
        [SerializeField]
        private UpdateMode updateMode = UpdateMode.PreUpdate;

        #endregion

        #region Core

		private void OnEnable() {
			Subscribe(updateMode);
		}

		private void OnDisable() {
			Unsubscribe(updateMode);
		}

		public override void UpdateComponent(float time) {
			this.transform.Rotate(rotation * time, rotationSpace);
		}

		#endregion
	}
}