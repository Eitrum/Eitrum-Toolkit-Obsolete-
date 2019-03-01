using UnityEngine;

namespace Eitrum {
	[AddComponentMenu("Eitrum/Core/Initialize")]
	public class EiInitialize : EiComponent {

        [SerializeField] private PhysicsExtension.PhysicsSettings optionalPhysicsSettings;

		void Awake() {
			var updateSystem = EiUpdateSystem.Instance;
			var timer = EiTimer.Instance;
			var threadingFix = EiUnityThreading.Instance;
            optionalPhysicsSettings?.ApplyAllPhysicsSettings();
		}
	}
}