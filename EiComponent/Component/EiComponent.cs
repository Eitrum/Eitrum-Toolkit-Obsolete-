using System;
using UnityEngine;
#if EITRUM_NETWORKING
using Eitrum.Networking;
#endif

namespace Eitrum {
	public class EiComponent : MonoBehaviour, EiUpdateInterface {
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
		public EiLLNode<EiUpdateInterface> preUpdateNode;
		public EiLLNode<EiUpdateInterface> updateNode;
		public EiLLNode<EiUpdateInterface> lateUpdateNode;
		public EiLLNode<EiUpdateInterface> fixedUpdateNode;
		public EiLLNode<EiUpdateInterface> threadedUpdateNode;

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

#if EITRUM_NETWORKING
		public EiNetworkView NetView // Ugly name of NetView because unity built in 'NetworkView'
		{
			get {
				if (!netView)
					netView = GetComponent<EiNetworkView>();
				return netView;
			}
		}
#endif

		public EiComponent Component {
			get {
				return this;
			}
		}

		public EiCore Core {
			get {
				return null;
			}
		}

		public bool IsNull {
			get {
#if EITRUM_POOLING
				return this == null || (Entity && entity.IsPooled);
#else
				return this == null;
#endif
			}
		}

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

		public virtual void Destroy() {
			if (entity)
				entity.Destroy();
			else
				MonoBehaviour.Destroy(gameObject);
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
		protected EiLLNode<EiUpdateSystem.TimerUpdateData> SubscribeUpdateTimer(float time, Action method) {
			return EiUpdateSystem.Instance.SubscribeUpdateTimer(this, time, method);
		}

		protected void UnsubscribeUpdateTimer(EiLLNode<EiUpdateSystem.TimerUpdateData> node) {
			EiUpdateSystem.Instance.UnsubscribeTimerUpdate(node);
		}

		#endregion

		#region Pre Update

		protected void SubscribePreUpdate() {
			if (preUpdateNode == null)
				preUpdateNode = EiUpdateSystem.Instance.SubscribePreUpdate(this);
		}

		protected void UnsubscribePreUpdate() {
			if (preUpdateNode != null)
				EiUpdateSystem.Instance.UnsubscribePreUpdate(preUpdateNode);
			preUpdateNode = null;
		}

		#endregion

		#region Update

		protected void SubscribeUpdate() {
			if (updateNode == null)
				updateNode = EiUpdateSystem.Instance.SubscribeUpdate(this);
		}

		protected void UnsubscribeUpdate() {
			if (updateNode != null)
				EiUpdateSystem.Instance.UnsubscribeUpdate(updateNode);
			updateNode = null;
		}

		#endregion

		#region Threaded Update

		protected void SubscribeThreadedUpdate() {
			if (threadedUpdateNode == null)
				threadedUpdateNode = EiThreadedUpdateSystem.Instance.Subscribe(this);
		}

		protected void UnsubscribeThreadedUpdate() {
			if (threadedUpdateNode != null)
				EiThreadedUpdateSystem.Instance.Unsubscribe(threadedUpdateNode);
			threadedUpdateNode = null;
		}

		#endregion

		#region Late Update

		protected void SubscribeLateUpdate() {
			if (lateUpdateNode == null)
				lateUpdateNode = EiUpdateSystem.Instance.SubscribeLateUpdate(this);
		}

		protected void UnsubscribeLateUpdate() {
			if (lateUpdateNode != null)
				EiUpdateSystem.Instance.UnsubscribeLateUpdate(lateUpdateNode);
			lateUpdateNode = null;
		}

		#endregion

		#region Fixed Update

		protected void SubscribeFixedUpdate() {
			if (fixedUpdateNode == null)
				fixedUpdateNode = EiUpdateSystem.Instance.SubscribeFixedUpdate(this);
		}

		protected void UnsubscribeFixedUpdate() {
			if (fixedUpdateNode != null)
				EiUpdateSystem.Instance.UnsubscribeFixedUpdate(fixedUpdateNode);
			fixedUpdateNode = null;
		}

		#endregion

		#region Message System

		protected EiLLNode<EiMessageSubscriber<T>> Subscribe<T>(Action<T> action) {
			return EiMessage.Subscribe<T>(this, action);
		}

		public static EiLLNode<EiMessageSubscriber<T>> Subscribe<T>(EiComponent component, Action<T> action) {
			return EiMessage.Subscribe<T>(component, action);
		}

		public static void Unsubscribe<T>(EiLLNode<EiMessageSubscriber<T>> subscriber) {
			EiMessage.Unsubscribe<T>(subscriber);
		}

		public static void Publish<T>(T message) {
			EiMessage<T>.Publish(message);
		}

		#endregion

		#endregion

		#region Editor
#if UNITY_EDITOR

		[ContextMenu("Attach All Components On Entity")]
		public void AttachAllComponentsOnEntity() {
			var objs = GetComponentsInChildren<EiComponent>(true);
			foreach (var obj in objs)
				obj.AttachComponents();
		}

		[ContextMenu("Attach Components")]
		private void AttachComponentContextMenu() {
			AttachComponents();
		}

		protected virtual void AttachComponents() {
			entity = GetComponentInParent<EiEntity>();
#if EITRUM_NETWORKING
			netView = GetComponent<EiNetworkView>();
#endif
		}
#endif
		#endregion
	}
}