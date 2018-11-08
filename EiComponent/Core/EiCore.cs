using System;
using System.Diagnostics;

namespace Eitrum
{
	public class EiCore : EiPreUpdateInterface, EiUpdateInterface, EiLateUpdateInterface, EiFixedUpdateInterface, EiThreadedUpdateInterface
	{
		#region Variables

		bool isDestroyed = false;

		// Should Not ever be touched by anything!!! Used by core engine for performance
		public EiLLNode<EiPreUpdateInterface> preUpdateNode;
		public EiLLNode<EiUpdateInterface> updateNode;
		public EiLLNode<EiLateUpdateInterface> lateUpdateNode;
		public EiLLNode<EiFixedUpdateInterface> fixedUpdateNode;
		public EiLLNode<EiThreadedUpdateInterface> threadedUpdateNode;

		#endregion

		#region Properties

		public bool IsDestroyed {
			get {
				return isDestroyed;
			}set {
				if (value && !isDestroyed) {
					DestroyThis ();
				}
			}
		}

		public object This {
			get {
				return this;
			}
		}

		public bool IsNull {
			get {
				return isDestroyed;
			}
		}

		#endregion

		#region Core

		public static void Destroy (EiCore core)
		{
			core.DestroyThis ();
		}

		public void DestroyThis ()
		{
			if (isDestroyed == false) {
				isDestroyed = true;
				OnDestroy ();
			}
		}

		protected virtual void OnDestroy ()
		{

		}

		#endregion

		#region EiUpdateInterface implementation

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
			return EiMessage.Subscribe (this, action);
		}

		public static EiLLNode<EiMessageSubscriber<T>> Subscribe<T> (EiCore core, Action<T> action)
		{
			return EiMessage.Subscribe (core, action);
		}

		public static void Unsubscribe<T> (EiLLNode<EiMessageSubscriber<T>> subscriber)
		{
			EiMessage.Unsubscribe (subscriber);
		}

		public static void Publish<T> (T message)
		{
			EiMessage<T>.Publish (message);
		}

		#endregion

		#endregion
	}
}