using Eitrum.Database;
using UnityEngine;

namespace Eitrum {
    [CreateAssetMenu(fileName = "Prefab", menuName = "Eitrum/Database/Prefab", order = 0)]
    public class EiPrefab : EiScriptableObject {
        #region Variables

        [SerializeField] private string itemName = "empty";
        [SerializeField] private GameObject item = null;
        [SerializeField] private string path = "";
        private int id = -1;

#if EITRUM_POOLING
        [SerializeField]
        private EiPrefabPool poolData = new EiPrefabPool();
#endif

        #endregion

        #region Properties

        public GameObject Item {
            get {
                return item;
            }
        }

        public GameObject GameObject {
            get {
                return item;
            }
        }

        public string ItemName {
            get {
                return itemName;
            }
        }

        public string Path {
            get {
                return path;
            }
        }

        public string FullName {
            get {
                return string.Format("{0}/{1}", path, itemName);
            }
        }

        public int Id {
            get {
#if UNITY_EDITOR
                id = itemName.GetDeterministicHashCode();
#endif
                if (id == -1)
                    id = itemName.GetDeterministicHashCode();
                return id;
            }
        }

#if EITRUM_POOLING
        public EiPrefabPool Pool { get { return poolData; } }
#endif

        #endregion

        #region Initilize

        public void Initialize() {
#if EITRUM_POOLING
            poolData.Initialize(this);
#endif
        }

        #endregion

        #region GameObject Instantiate

        public GameObject Instantiate() {
            return Instantiate(new EiInstantiateData());
        }

        public GameObject Instantiate(Transform parent) {
            return Instantiate(new EiInstantiateData(parent));
        }

        public GameObject Instantiate(Vector3 position) {
            return Instantiate(new EiInstantiateData(position));
        }

        public GameObject Instantiate(Vector3 position, Quaternion rotation) {
            return Instantiate(new EiInstantiateData(position, rotation));
        }

        public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent) {
            return Instantiate(new EiInstantiateData(position, rotation, parent));
        }

        public GameObject Instantiate(Vector3 position, Quaternion rotation, Vector3 scale) {
            return Instantiate(new EiInstantiateData(position, rotation, scale, null));
        }

        public GameObject Instantiate(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent) {
            return Instantiate(new EiInstantiateData(position, rotation, scale, parent));
        }

        public GameObject Instantiate(EiInstantiateData data) {
#if EITRUM_POOLING
            return poolData.Dequeue(data);
#else
            var go = MonoBehaviour.Instantiate(GameObject, data.position, data.rotation, data.parent);
            if (data.scale.HasValue) {
                var goScale = go.transform.localScale;
                goScale.Scale(data.scale.Value);
                go.transform.localScale = goScale;
            }
            return go;
#endif
        }

        #endregion

        #region Editor

        [ContextMenu("Apply Name")]
        private void ApplyName() {
            itemName = item?.name ?? "empty";
        }

#if UNITY_EDITOR

        private void Awake() {
            var objs = UnityEditor.Selection.objects;
            if (objs.Length > 0) {
                UnityEditor.Selection.objects = new Object[0];
                for (int i = 0, length = objs.Length; i < length; i++) {
                    objs[i] = GenerateFile(objs[i] as GameObject);
                }
                UnityEditor.Selection.objects = objs;
                DestroyImmediate(this);
            }
        }

        private Object GenerateFile(GameObject gameObject) {
            if (gameObject == null)
                return null;
            var instance = CreateInstance<EiPrefab>();
            instance.name = gameObject.name;
            instance.item = gameObject;
            instance.itemName = instance.name;

            UnityEditor.AssetDatabase.CreateAsset(instance,
                UnityEditor.AssetDatabase.GenerateUniqueAssetPath(
                    UnityEditor.AssetDatabase.GetAssetPath(gameObject).Replace(".prefab", ".asset")));
            return instance;
        }

#endif

        #endregion
    }
}