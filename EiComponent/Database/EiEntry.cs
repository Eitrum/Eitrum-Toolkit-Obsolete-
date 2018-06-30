using System;

namespace Eitrum {
    [Serializable]
    public class EiEntry {
        [Readonly]
        public string itemName = "empty";
        public UnityEngine.Object item;
        [Readonly]
        public int uniqueId = -1;
    }
}