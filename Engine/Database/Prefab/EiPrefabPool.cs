using Eitrum.Database;
using Eitrum.Engine.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using Eitrum.Engine.Extensions;

namespace Eitrum {
    [Serializable]
    public class EiPrefabPool : IPrefabPool {
        #region Variables

        [SerializeField]
        private int poolSize = 0;
        [SerializeField]
        private bool keepPoolAlive = true;
        [SerializeField]
        private bool fillAtAwake = false;
        [SerializeField]
        private bool poolableCallbacks = true;

        private bool optimizedCallbacks = false;
        private EiPrefab prefab = null;
        private Queue<GameObject> pooledObjects = new Queue<GameObject>();

        private Transform parentContainer;

        #endregion

        #region Properties

        public bool HasPooling { get { return poolSize > 0; } }

        public int PoolSize { get { return poolSize; } }

        public int PooledAmount { get { return pooledObjects.Count; } }

        public bool KeepPoolAlive { get { return keepPoolAlive; } }

        public bool FillAtAwake { get { return fillAtAwake; } }

        public bool UsePoolCallbacks { get { return poolableCallbacks; } }

        public EiPrefab Prefab { get { return prefab; } }

        public Transform Parent {
            get {
                if (parentContainer == null) {
                    parentContainer = new GameObject(Prefab.ItemName + " Pool").transform;
                    parentContainer.SetActive(false);
                    if (keepPoolAlive)
                        UnityEngine.Object.DontDestroyOnLoad(parentContainer);
                }
                return parentContainer;
            }
        }

        int IPrefabPool.PoolSize { set { poolSize = value; } }

        bool IPrefabPool.KeepPoolAlive { set { keepPoolAlive = value; } }

        bool IPrefabPool.FillAtAwake { set { fillAtAwake = value; } }

        bool IPrefabPool.UsePoolCallbacks { set { poolableCallbacks = value; } }

        public IPrefabPool Edit { get { return this as IPrefabPool; } }

        #endregion

        #region Core

        public void Initialize(EiPrefab prefab) {
            this.prefab = prefab;
            if (fillAtAwake)
                Fill();

            optimizedCallbacks = poolableCallbacks && prefab.GameObject.GetComponent<EiEntity>();
        }

        public GameObject Dequeue(EiInstantiateData data) {
            if (pooledObjects.Count == 0) {
                Edit.LoadPrefab();
                return Dequeue(data);
            }

            var go = pooledObjects.Dequeue();

            var t = go.transform;
            t.SetParent(data.parent, false);
            t.localPosition = data.position;
            t.localRotation = data.rotation;
            if (data.scale.HasValue)
                t.localScale = data.scale.Value;

            if (poolableCallbacks) {
                if (optimizedCallbacks) {
                    var list = go.GetComponent<EiEntity>().Poolables;
                    for (int i = 0, length = list.Length; i < length; i++) {
                        list[i].OnInstantiate();
                    }
                }
                else {
                    var list = go.GetComponentsInChildren<IPoolable>();
                    for (int i = 0, length = list.Length; i < length; i++) {
                        list[i].OnInstantiate();
                    }
                }
            }

            return go;
        }

        public void Enqueue(GameObject go, bool triggerCallbacks = true) {
            if (pooledObjects.Count >= poolSize) {
                UnityEngine.Object.Destroy(go, 0f);
                return;
            }
            go.transform.SetParent(Parent);
            pooledObjects.Enqueue(go);

            if (triggerCallbacks && poolableCallbacks) {
                if (optimizedCallbacks) {
                    var list = go.GetComponent<EiEntity>().Poolables;
                    for (int i = 0, length = list.Length; i < length; i++) {
                        list[i].OnDestroy();
                    }
                }
                else {
                    var list = go.GetComponentsInChildren<IPoolable>();
                    for (int i = 0, length = list.Length; i < length; i++) {
                        list[i].OnDestroy();
                    }
                }
            }
        }

        #endregion

        #region Fill API

        /// <summary>
        /// Fills the pool with objects until its full, 1 object per frame
        /// </summary>
        public void Fill() {
            Fill(poolSize - pooledObjects.Count);
        }

        public void Fill(int count) {
            EiPrefabPoolLoadBalancer.Add(Edit, count);
        }

        public void FastFill() {
            FastFill(poolSize - pooledObjects.Count);
        }

        public void FastFill(int count) {
            var edit = Edit;
            for (int i = 0; i < count; i++) {
                edit.LoadPrefab();
            }
        }

        /// <summary>
        /// Pre loads the pool with objects over a set amount of time
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="time"></param>
        public void Fill(int amount, float time) {
            amount -= pooledObjects.Count;
            Timer.Repeat(time / (float)amount, amount, Edit.LoadPrefab);
        }

        /// <summary>
        /// Clears the pool of any objects not currently in use
        /// </summary>
        public void ClearObjects() {
            parentContainer.Destroy(0f);
            pooledObjects.Clear();
        }

        void IPrefabPool.LoadPrefab() {
            var gameObject = MonoBehaviour.Instantiate(prefab.GameObject, parentContainer);
            if (optimizedCallbacks)
                gameObject.GetComponent<EiEntity>().AssignPoolTarget(this);
            Enqueue(gameObject, false);
        }

        #endregion
    }
}