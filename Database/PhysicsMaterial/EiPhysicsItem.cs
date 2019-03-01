using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.PhysicsExtension {
    [CreateAssetMenu(fileName = "Physics Material", menuName = "Eitrum/Physics/Physics Material", order = 0)]
    public class EiPhysicsItem : EiScriptableObject {
        #region Variables

        public PhysicMaterial physicMaterial = null;
        public float materialStrength = 1f;

        #endregion

        #region Properties

        public PhysicMaterial PhysicMaterial {
            get {
                return physicMaterial;
            }
        }

        public float MaterialStrength {
            get {
                return materialStrength;
            }
        }

        #endregion
    }
}