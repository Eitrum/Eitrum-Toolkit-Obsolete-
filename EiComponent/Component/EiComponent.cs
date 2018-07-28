using System;
using UnityEngine;
using Eitrum.Networking;

namespace Eitrum
{
	public class EiComponent : MonoBehaviour, EiUpdateInterface
	{
		#region Variables

		[SerializeField]
		[HideInInspector]
		private EiEntity entity;
		[SerializeField]
		[HideInInspector]
		#pragma warning disable
		private EiNetworkView networkView;

		// Should Not ever be touched by anything!!! Used by core engine for performance
		public EiLLNode<EiUpdateInterface> preUpdateNode;
		public EiLLNode<EiUpdateInterface> updateNode;
		public EiLLNode<EiUpdateInterface> lateUpdateNode;
		public EiLLNode<EiUpdateInterface> fixedUpdateNode;
		public EiLLNode<EiUpdateInterface> threadedUpdateNode;

		#endregion

		#region Properties

		public virtual EiEntity Entity {
			get {
				if (!entity)
					entity = GetComponent<EiEntity> ();
				return entity;
			}
		}

		public virtual bool IsNetworked {
			get {
				return networkView != null;
			}
		}

		public virtual EiNetworkView NetworkView {
			get {
				if (!networkView)
					networkView = GetComponent<EiNetworkView> ();
				return networkView;
			}
		}

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
				return this == null;
			}
		}

		#endregion

		#region Instantiate

		#region Normal

		public new static T Instantiate<T> (T prefab) where T : UnityEngine.Object
		{
			return UnityEngine.Object.Instantiate<T> (prefab);
		}

		#endregion

		#region Prefab Item

		public static GameObject Instantiate(EiPrefab item)
		{
			return item.Instantiate();
		}

		#endregion

		#region DatabaseItem GameObject

		public static UnityEngine.Object Instantiate (EiDatabaseItem item)
		{
			return item.Instantiate();
		}

		public static UnityEngine.Object Instantiate (EiDatabaseItem item, Transform parent)
		{
			return item.Instantiate(parent);
		}

		public static UnityEngine.Object Instantiate (EiDatabaseItem item, Vector3 position)
		{
			return item.Instantiate(position);
		}

		public static UnityEngine.Object Instantiate (EiDatabaseItem item, Vector3 position, Quaternion rotation)
		{
			return item.Instantiate(position, rotation);
		}

		public static UnityEngine.Object Instantiate (EiDatabaseItem item, Vector3 position, Quaternion rotation, Transform parent)
		{
			return item.Instantiate (position, rotation, parent);
		}

		#endregion

		#region DatabaseItem T

		public static T Instantiate<T> (EiDatabaseItem item) where T : UnityEngine.Object
		{
			return item.Instantiate () as T;
		}

		public static T Instantiate<T> (EiDatabaseItem item, Transform parent) where T : UnityEngine.Object
		{
			return item.Instantiate (parent) as T;
		}

		public static T Instantiate<T> (EiDatabaseItem item, Vector3 position) where T : UnityEngine.Object
		{
			return item.Instantiate (position) as T;
		}

		public static T Instantiate<T> (EiDatabaseItem item, Vector3 position, Quaternion rotation) where T : UnityEngine.Object
		{
			return item.Instantiate (position, rotation) as T;
		}

		public static T Instantiate<T> (EiDatabaseItem item, Vector3 position, Quaternion rotation, Transform parent) where T : UnityEngine.Object
		{
			return item.Instantiate (position, rotation, parent) as T;
		}

		#endregion

		#endregion

		#region Network Instantiate

		public static T NetworkInstantiate<T> (string resourcePath) where T : UnityEngine.Object
		{
			var obj = Resources.Load (resourcePath);
			return Instantiate (obj) as T;
		}

		public static GameObject NetworkInstantiate (EiDatabaseItem item)
		{
			var go = item.GameObject;
			if (!go) {
				Debug.LogErrorFormat ("Item {0} is not of type GameObject", item.ItemName);
				return null;
			}
			return null;
			//Do network shenangians to instantiate
		}

		#endregion

		#region Virtual Update Calls

		public virtual void PreUpdateComponent (float time)
		{

		}

		public virtual void UpdateComponent (float time)
		{

		}

		public virtual void LateUpdateComponent (float time)
		{

		}

		public virtual void FixedUpdateComponent (float time)
		{

		}

		public virtual void ThreadedUpdateComponent (float time)
		{

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
		protected EiLLNode<EiUpdateSystem.TimerUpdateData> SubscribeUpdateTimer (float time, Action method)
		{
			return EiUpdateSystem.Instance.SubscribeUpdateTimer (this, time, method);
		}

		protected void UnsubscribeUpdateTimer (EiLLNode<EiUpdateSystem.TimerUpdateData> node)
		{
			EiUpdateSystem.Instance.UnsubscribeTimerUpdate (node);
		}

		#endregion

		#region Pre Update

		protected void SubscribePreUpdate ()
		{
			if (preUpdateNode == null)
				preUpdateNode = EiUpdateSystem.Instance.SubscribePreUpdate (this);
		}

		protected void UnsubscribePreUpdate ()
		{
			if (preUpdateNode != null)
				EiUpdateSystem.Instance.UnsubscribePreUpdate (preUpdateNode);
			preUpdateNode = null;
		}

		#endregion

		#region Update

		protected void SubscribeUpdate ()
		{
			if (updateNode == null)
				updateNode = EiUpdateSystem.Instance.SubscribeUpdate (this);
		}

		protected void UnsubscribeUpdate ()
		{
			if (updateNode != null)
				EiUpdateSystem.Instance.UnsubscribeUpdate (updateNode);
			updateNode = null;
		}

		#endregion

		#region Threaded Update

		protected void SubscribeThreadedUpdate ()
		{
			if (threadedUpdateNode == null)
				threadedUpdateNode = EiThreadedUpdateSystem.Instance.Subscribe (this);
		}

		protected void UnsubscribeThreadedUpdate ()
		{
			if (threadedUpdateNode != null)
				EiThreadedUpdateSystem.Instance.Unsubscribe (threadedUpdateNode);
			threadedUpdateNode = null;
		}

		#endregion

		#region Late Update

		protected void SubscribeLateUpdate ()
		{
			if (lateUpdateNode == null)
				lateUpdateNode = EiUpdateSystem.Instance.SubscribeLateUpdate (this);
		}

		protected void UnsubscribeLateUpdate ()
		{
			if (lateUpdateNode != null)
				EiUpdateSystem.Instance.UnsubscribeLateUpdate (lateUpdateNode);
			lateUpdateNode = null;
		}

		#endregion

		#region Fixed Update

		protected void SubscribeFixedUpdate ()
		{
			if (fixedUpdateNode == null)
				fixedUpdateNode = EiUpdateSystem.Instance.SubscribeFixedUpdate (this);
		}

		protected void UnsubscribeFixedUpdate ()
		{
			if (fixedUpdateNode != null)
				EiUpdateSystem.Instance.UnsubscribeFixedUpdate (fixedUpdateNode);
			fixedUpdateNode = null;
		}

		#endregion

		#region Message System

		protected EiLLNode<EiMessageSubscriber<T>> Subscribe<T> (Action<T> action)
		{
			return EiMessage.Subscribe<T> (this, action);
		}

		protected void Unsubscribe<T> (EiLLNode<EiMessageSubscriber<T>> subscriber)
		{
			EiMessage.Unsubscribe<T> (subscriber);
		}

		public void Publish<T> (T message) where T : EiCore
		{
			EiMessage<T>.Publish (message);
		}

		#endregion

		#endregion

		#if UNITY_EDITOR

		[ContextMenu ("Attach All Components On Entity")]
		public void AttachAllComponentsOnEntity ()
		{
			var objs = GetComponentsInChildren<EiComponent> (true);
			foreach (var obj in objs)
				obj.AttachComponents ();
		}

		[ContextMenu ("Attach Components")]
		private void AttachComponentContextMenu ()
		{
			AttachComponents ();
		}

		protected virtual void AttachComponents ()
		{
			entity = GetComponentInParent<EiEntity> ();
			networkView = GetComponent<EiNetworkView> ();
		}

		#endif
	}
}