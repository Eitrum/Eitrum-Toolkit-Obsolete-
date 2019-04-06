using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Database {
    [CreateAssetMenu(fileName = "PrefabDatabase", menuName = "Eitrum/Database/Prefab Database", order = 1)]
    public class EiPrefabDatabase : EiScriptableObjectSingleton<EiPrefabDatabase> {

        #region Singleton

        protected override bool KeepInResources => true;

        protected override bool AllowAssignSingleton => true;

        protected override void OnSingletonCreated() {
            for (int i = 0, length = cachedItems.Count; i < length; i++) {
                cachedItems[i].Initialize();
                cachedDictionary.Add(cachedItems[i].Id, cachedItems[i]);
            }
        }

        #endregion

        #region Variables

        [SerializeField]
        private List<EiPrefab> cachedItems = new List<EiPrefab>();

        private Dictionary<int, EiPrefab> cachedDictionary = new Dictionary<int, EiPrefab>();

        #endregion

        #region Properties

        public int Length {
            get {
                return cachedItems.Count;
            }
        }

        /// <summary>
        /// Gets the prefab at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public EiPrefab this[int index] {
            get {
                return cachedItems[index];
            }
        }

        #endregion

        #region Core

        void Awake() {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                return;
            }
#endif
            if (HasInstance) {
                DestroyImmediate(this);
                return;
            }
            AssignInstance(this);
        }

        #endregion

        #region Get

        public EiPrefab GetPrefabById(int id) {
            return cachedDictionary[id];
        }

        public EiPrefab GetPrefabByIndex(int index) {
            return cachedItems[index];
        }

        public EiPrefab GetPrefabByName(string name) {
            return cachedDictionary[name.GetDeterministicHashCode()];
        }

        #endregion

        #region Editor

        [ContextMenu("Add all Prefabs")]
        public void AddAllPrefabs() {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "Adding all prefabs");
            var assetsGUID = UnityEditor.AssetDatabase.FindAssets("t:" + nameof(EiPrefab));
            cachedItems.Clear();
            for (int i = 0, length = assetsGUID.Length; i < length; i++) {
                cachedItems.Add(
                    UnityEditor.AssetDatabase.LoadAssetAtPath<EiPrefab>(
                        UnityEditor.AssetDatabase.GUIDToAssetPath(assetsGUID[i])));
            }
#endif
        }

        [ContextMenu("Clear Prefabs")]
        public void ClearPrefabs() {
            cachedItems.Clear();
            cachedDictionary.Clear();
        }

        #endregion
    }
}