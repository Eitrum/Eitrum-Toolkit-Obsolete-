using System;
using UnityEngine;

namespace Eitrum
{
	public class EiComponent : MonoBehaviour, EiUpdateInterface
	{
		#region Variables

		// Should Not ever be touched by anything!!! Used by core engine for performance
		public EiLLNode<EiUpdateInterface> preUpdateNode;
		public EiLLNode<EiUpdateInterface> updateNode;
		public EiLLNode<EiUpdateInterface> lateUpdateNode;
		public EiLLNode<EiUpdateInterface> fixedUpdateNode;
		public EiLLNode<EiUpdateInterface> threadedUpdateNode;

		#endregion

		#region Properties

		#endregion

		#region Logging

		private string logTag;
		public static bool IsLogging = false;

		protected void Log (Func<object> func)
		{
			if (IsLogging)
				Log (func ());
		}

		protected void Log (object o)
		{
			if (logTag == null)
				logTag = string.Format ("[{0}] " + this.GetType ().Name);
			Debug.Log (logTag + o);
		}

		protected void LogWarning (Func<object> func)
		{
			if (IsLogging)
				LogWarning (func ());
		}

		protected void LogWarning (object o)
		{
			if (logTag == null)
				logTag = string.Format ("[{0}] " + this.GetType ().Name);
			Debug.LogWarning (logTag + o);
		}

		protected void LogError (Func<object> func)
		{
			if (IsLogging)
				LogError (func ());
		}

		protected void LogError (object o)
		{
			if (logTag == null)
				logTag = string.Format ("[{0}] " + this.GetType ().Name);
			Debug.LogError (logTag + o);
		}

		protected void LogException (Func<Exception> func)
		{
			if (IsLogging)
				LogException (func ());
		}

		protected void LogException (Exception o)
		{
			if (logTag == null)
				logTag = string.Format ("[{0}] " + this.GetType ().Name);
			Debug.LogError (logTag + o.ToString ());
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

		#endregion

		#endregion
	}
}