using System;
using System.Diagnostics;

namespace Eitrum
{
	public class EiClass : IBase
	{
		#region Variables

		bool isDestroyed = false;

		// Should Not ever be touched by anything!!! Used by core engine for performance
		public EiLLNode<IPreUpdate> preUpdateNode;
		public EiLLNode<IUpdate> updateNode;
		public EiLLNode<ILateUpdate> lateUpdateNode;
		public EiLLNode<IFixedUpdate> fixedUpdateNode;
		public EiLLNode<IThreadedUpdate> threadedUpdateNode;

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

		public object Target {
			get {
				return this;
			}
		}

		public bool IsNull {
			get {
				return isDestroyed;
			}
		}

		public static bool GameRunning {
			get {
				return EiUnityThreading.gameRunning;
			}
		}

		#endregion

		#region Core

		public static void Destroy (EiClass core)
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
        
		#region Message System

		protected EiLLNode<EiMessageSubscriber<T>> Subscribe<T> (Action<T> action)
		{
			return EiMessage.Subscribe (this, action);
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
	}
}