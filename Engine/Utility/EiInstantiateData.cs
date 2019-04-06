using System;
using UnityEngine;

namespace Eitrum {
    public struct EiInstantiateData {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3? scale;
        public Transform parent;

        public EiInstantiateData(Vector3 position) {
            this.position = position;
            this.rotation = Quaternion.identity;
            this.scale = null;
            this.parent = null;
        }

        public EiInstantiateData(Transform parent) {
            this.position = Vector3.zero;
            this.rotation = Quaternion.identity;
            this.scale = null;
            this.parent = parent;
        }

        public EiInstantiateData(Vector3 position, Quaternion rotation) {
            this.position = position;
            this.rotation = rotation;
            this.scale = null;
            this.parent = null;
        }

        public EiInstantiateData(Vector3 position, Quaternion rotation, Vector3 scale) {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.parent = null;
        }

        public EiInstantiateData(Vector3 position, Quaternion rotation, Transform parent) {
            this.position = position;
            this.rotation = rotation;
            this.scale = null;
            this.parent = parent;
        }

        public EiInstantiateData(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent) {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.parent = parent;
        }
    }
}
