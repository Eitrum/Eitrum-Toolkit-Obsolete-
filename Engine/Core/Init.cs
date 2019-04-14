using Eitrum.Engine.Threading;
using UnityEngine;

namespace Eitrum.Engine.Core {
    [AddComponentMenu("Eitrum/Core/Init")]
    public class Init : EiComponent {

        [SerializeField] private PhysicsExtension.PhysicsSettings optionalPhysicsSettings = null;

        void Awake() {
            var updateSystem = UpdateSystem.Instance;
            var timer = Timer.Instance;
            var threadingFix = UnityThreading.Instance;
            optionalPhysicsSettings?.ApplyAllPhysicsSettings();
        }
    }
}