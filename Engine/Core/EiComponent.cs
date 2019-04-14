using Eitrum.Engine.Threading;
using System;
using UnityEngine;

#if EITRUM_NETWORKING
using Eitrum.Networking;
#endif

namespace Eitrum.Engine.Core {
    public class EiComponent : MonoBehaviour, IPreUpdate, IUpdate, ILateUpdate, IFixedUpdate, IThreadedUpdate {
        #region Variables

        [SerializeField]
        [HideInInspector]
        private EiEntity entity;
#if EITRUM_NETWORKING
		[SerializeField]
		[HideInInspector]
		




#pragma warning disable
		private EiNetworkView netView;
#endif

        // Should Not ever be touched by anything!!! Used by core engine for performance
        private EiLLNode<IPreUpdate> preUpdateNode;
        private EiLLNode<IUpdate> updateNode;
        private EiLLNode<ILateUpdate> lateUpdateNode;
        private EiLLNode<IFixedUpdate> fixedUpdateNode;
        private EiLLNode<IThreadedUpdate> threadedUpdateNode;

        #endregion

        #region Properties

        public EiEntity Entity {
            get {
                if (!entity)
                    entity = GetComponent<EiEntity>();
                return entity;
            }
        }

        public bool IsNetworked {
            get {
#if EITRUM_NETWORKING
				return netView != null;
#else
                return false;
#endif
            }
        }

        public static bool GameRunning {
            get {
                return UnityThreading.gameRunning;
            }
        }

#if EITRUM_NETWORKING
		public EiNetworkView NetView { // Ugly name of NetView because unity built in 'NetworkView'
			get {
				if (!netView)
					netView = GetComponent<EiNetworkView> ();
				return netView;
			}
		}
#endif

        public object Target {
            get {
                return this;
            }
        }

        public bool IsNull { get { return this == null; } }

        #endregion

        #region Instantiate EiPrefab

        public static GameObject Instantiate(EiPrefab prefab) {
            return prefab.Instantiate();
        }

        public static GameObject Instantiate(EiPrefab prefab, Vector3 position) {
            return prefab.Instantiate(position);
        }

        public static GameObject Instantiate(EiPrefab prefab, Vector3 position, Quaternion rotation) {
            return prefab.Instantiate(position, rotation);
        }

        public static GameObject Instantiate(EiPrefab prefab, Vector3 position, Quaternion rotation, Transform parent) {
            return prefab.Instantiate(position, rotation, parent);
        }

        public static GameObject Instantiate(EiPrefab prefab, Vector3 position, Quaternion rotation, Vector3 scale) {
            return prefab.Instantiate(position, rotation, scale);
        }

        public static GameObject Instantiate(EiPrefab prefab, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent) {
            return prefab.Instantiate(position, rotation, scale, parent);
        }

        #endregion

        #region Destroy

        public void Destroy() {
            if (entity)
                entity.Destroy();
            else
                MonoBehaviour.Destroy(gameObject);
        }

        public void Destroy(float delay) {
            if (entity)
                entity.Destroy(delay);
            else
                MonoBehaviour.Destroy(gameObject, delay);
        }

        public static void Destroy(EiEntity entity) {
            entity.Destroy();
        }

        public static void Destroy(GameObject gameObject) {
            var entity = gameObject.GetComponent<EiEntity>();
            if (entity) {
                entity.Destroy();
            }
            else {
                MonoBehaviour.Destroy(gameObject);
            }
        }

        public static void Destroy(Transform transform) {
            Destroy(transform.gameObject);
        }

        #endregion

        #region Virtual Update Calls

        public virtual void PreUpdateComponent(float time) {

        }

        public virtual void UpdateComponent(float time) {

        }

        public virtual void LateUpdateComponent(float time) {

        }

        public virtual void FixedUpdateComponent(float time) {

        }

        public virtual void ThreadedUpdateComponent(float time) {

        }

        #endregion

        #region Subscribe/Unsubscribe

        #region Update Timer

        /// <summary>
        /// Subscribes the update timer, store value to be able to unsubscribe.
        /// </summary>
        /// <returns>The update timer.</returns>
        /// <param name="time">Time.</param>
        /// <param name="method">Method.</param>
        protected EiLLNode<UpdateSystem.TimerUpdateData> SubscribeUpdateTimer(float time, Action method) {
            return UpdateSystem.Instance.SubscribeUpdateTimer(this, time, method);
        }

        protected void UnsubscribeUpdateTimer(EiLLNode<UpdateSystem.TimerUpdateData> node) {
            UpdateSystem.Instance.UnsubscribeTimerUpdate(node);
        }

        #endregion

        #region Pre Update

        protected void SubscribePreUpdate() {
            if (preUpdateNode == null)
                preUpdateNode = UpdateSystem.Instance.SubscribePreUpdate(this);
        }

        protected void UnsubscribePreUpdate() {
            if (preUpdateNode != null)
                UpdateSystem.Instance.UnsubscribePreUpdate(preUpdateNode);
            preUpdateNode = null;
        }

        #endregion

        #region Update

        protected void SubscribeUpdate() {
            if (updateNode == null)
                updateNode = UpdateSystem.Instance.SubscribeUpdate(this);
        }

        protected void UnsubscribeUpdate() {
            if (updateNode != null && GameRunning)
                UpdateSystem.Instance.UnsubscribeUpdate(updateNode);
            updateNode = null;
        }

        #endregion

        #region Threaded Update

        protected void SubscribeThreadedUpdate() {
            if (threadedUpdateNode == null)
                threadedUpdateNode = ThreadedUpdateSystem.Instance.Subscribe(this);
        }

        protected void UnsubscribeThreadedUpdate() {
            if (threadedUpdateNode != null)
                ThreadedUpdateSystem.Instance.Unsubscribe(threadedUpdateNode);
            threadedUpdateNode = null;
        }

        #endregion

        #region Late Update

        protected void SubscribeLateUpdate() {
            if (lateUpdateNode == null)
                lateUpdateNode = UpdateSystem.Instance.SubscribeLateUpdate(this);
        }

        protected void UnsubscribeLateUpdate() {
            if (lateUpdateNode != null)
                UpdateSystem.Instance.UnsubscribeLateUpdate(lateUpdateNode);
            lateUpdateNode = null;
        }

        #endregion

        #region Fixed Update

        protected void SubscribeFixedUpdate() {
            if (fixedUpdateNode == null)
                fixedUpdateNode = UpdateSystem.Instance.SubscribeFixedUpdate(this);
        }

        protected void UnsubscribeFixedUpdate() {
            if (fixedUpdateNode != null)
                UpdateSystem.Instance.UnsubscribeFixedUpdate(fixedUpdateNode);
            fixedUpdateNode = null;
        }

        #endregion

        #region Subscribe Update Mode

        protected void Subscribe(UpdateMode mode) {
            switch (mode) {
                case UpdateMode.PreUpdate:
                    SubscribePreUpdate();
                    return;
                case UpdateMode.Update:
                    SubscribeUpdate();
                    return;
                case UpdateMode.LateUpdate:
                    SubscribeLateUpdate();
                    return;
                case UpdateMode.PostUpdate:
                    Debug.LogError("Post Update yet not implemented");
                    return;
                case UpdateMode.FixedUpdate:
                    SubscribeFixedUpdate();
                    return;
                case UpdateMode.ThreadedUpdate:
                    SubscribeThreadedUpdate();
                    return;
            }
        }

        protected void Unsubscribe(UpdateMode mode) {
            switch (mode) {
                case UpdateMode.PreUpdate:
                    UnsubscribePreUpdate();
                    return;
                case UpdateMode.Update:
                    UnsubscribeUpdate();
                    return;
                case UpdateMode.LateUpdate:
                    UnsubscribeLateUpdate();
                    return;
                case UpdateMode.PostUpdate:
                    Debug.LogError("Post Update yet not implemented");
                    return;
                case UpdateMode.FixedUpdate:
                    UnsubscribeFixedUpdate();
                    return;
                case UpdateMode.ThreadedUpdate:
                    UnsubscribeThreadedUpdate();
                    return;
            }
        }

        #endregion

        #region Message System

        protected EiLLNode<MessageSubscriber<T>> Subscribe<T>(Action<T> action) {
            return Message.Subscribe<T>(this, action);
        }

        public static void Unsubscribe<T>(EiLLNode<MessageSubscriber<T>> subscriber) {
            Message.Unsubscribe<T>(subscriber);
        }

        public static void Publish<T>(T message) {
            Message<T>.Publish(message);
        }

        #endregion

        #endregion

        #region Editor

        [ContextMenu("Attach All Components On Entity")]
        public void AttachAllComponentsOnEntity() {
            var objs = GetComponentsInChildren<EiComponent>(true);
            foreach (var obj in objs)
                obj.AttachComponentContextMenu();
        }

        [ContextMenu("Attach Components")]
        private void AttachComponentContextMenu() {
            entity = GetComponentInParent<EiEntity>();
#if EITRUM_NETWORKING
			netView = GetComponent<EiNetworkView> ();
#endif
            AttachComponents();
#if UNITY_EDITOR
            if (gameObject.scene.isLoaded) {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
            else {
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

        protected virtual void AttachComponents() {
        }

        #endregion
    }
}