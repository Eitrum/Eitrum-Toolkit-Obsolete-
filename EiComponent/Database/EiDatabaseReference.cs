using System;
using UnityEngine;

namespace Eitrum {
    [Serializable]
    public class EiDatabaseReference {

        #region Variables

        [SerializeField]
        private int uniqueIdReference = -1;

        #endregion

        #region Properties

        public int UniqueIdReference {
            get {
                return uniqueIdReference;
            }
        }

        public UnityEngine.Object Object {
            get {
                return EiDatabase.Instance.GetObject(uniqueIdReference);
            }
        }

        #endregion

        #region Instantiate

        public UnityEngine.Object Instantiate() {
            if(uniqueIdReference == -1) {
                return null;
            }
            var obj = EiDatabase.Instance.GetObject(uniqueIdReference);
            if(obj) {
                return MonoBehaviour.Instantiate(obj);
            }
            return null;
        }

        public GameObject InstantiateAsGameObject() {
            return Instantiate() as GameObject;
        }

        #endregion
    }
}