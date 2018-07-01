using System;
using UnityEngine;
using System.Threading;

namespace Eitrum
{
	[Serializable]
	public class EiPropertyEvent<T> : EiCore , EiUnityThreadCallbackInterface
	{
		#region Variables

		[UnityEngine.SerializeField]
		protected T value = default(T);

		protected Action<T> onChangedUnityThread;
		protected Action<T> onChangedAnyThread;
		protected bool hasChanged = false;

		#endregion

		#region Properties

		public virtual T Value {
			set {
				lock (this) {
					this.value = value;
					if (onChangedUnityThread != null) {
						if (Thread.CurrentThread == EiUnityThreading.MainThread)
							onChangedUnityThread (value);
						else {
							if (!hasChanged) {
								hasChanged = EiUpdateSystem.AddUnityThreadCallbackToQueue (this);
							}
						}
					}
					if (onChangedAnyThread != null)
						onChangedAnyThread (value);
				}
			}
			get {
				lock (this)
					return value;
			}
		}

		#endregion

		#region Constructors

		public EiPropertyEvent ()
		{
		}

		public EiPropertyEvent (T value)
		{
			this.value = value;
		}

		#endregion

		#region Helper

		public void UnityThreadOnChangeOnly ()
		{
			lock (this) {
				hasChanged = false;
				if (onChangedUnityThread != null)
					onChangedUnityThread (value);
			}
		}

		#endregion

		#region Setters

		public EiPropertyEvent<T> SetValue (T value)
		{
			lock (this) {
				Value = value;
			}
			return this;
		}

		public EiPropertyEvent<T> SetValue (T value, bool triggerValueChange)
		{
			lock (this) {
				if (triggerValueChange) {
					Value = value;
					return this;
				}
				this.value = value;
			}
			return this;
		}

		#endregion

		#region Subscribe / Unsubscribe

		/// <summary>
		/// Subscribes the method so it can only be called on Unity Main Thread.
		/// </summary>
		/// <returns>The thread safe.</returns>
		/// <param name="method">Method.</param>
		public EiPropertyEvent<T> SubscribeUnityThread (Action<T> method)
		{
			lock (this)
				onChangedUnityThread += method;
			return this;
		}

		/// <summary>
		/// Subscribes the method so it can only be called on Unity Main Thread then runs it once.
		/// </summary>
		/// <returns>The thread safe.</returns>
		/// <param name="method">Method.</param>
		public EiPropertyEvent<T> SubscribeUnityThreadAndRun (Action<T> method)
		{
			lock (this) {
				onChangedUnityThread += method;
				method (value);
			}
			return this;
		}

		/// <summary>
		/// Unsubscribe the specified method from Unity Main Thread updates.
		/// </summary>
		/// <param name="method">Method.</param>
		public EiPropertyEvent<T> UnsubscribeUnityThread (Action<T> method)
		{
			lock (this)
				if (onChangedUnityThread != null)
					onChangedUnityThread -= method;
			return this;
		}

		/// <summary>
		/// Subscribes the method so it can run on any thread.
		/// </summary>
		/// <returns>The thread safe.</returns>
		/// <param name="method">Method.</param>
		public EiPropertyEvent<T> SubscribeAnyThread (Action<T> method)
		{
			lock (this)
				onChangedAnyThread += method;
			return this;
		}

		/// <summary>
		/// Subscribes the method so it can run on any thread and then runs it once.
		/// </summary>
		/// <returns>The thread safe.</returns>
		/// <param name="method">Method.</param>
		public EiPropertyEvent<T> SubscribeAnyThreadAndRun (Action<T> method)
		{
			lock (this) {
				onChangedAnyThread += method;
				method (value);
			}
			return this;
		}

		/// <summary>
		/// Unsubscribes the specified method from being called from any thread.
		/// </summary>
		/// <returns>The thread safe.</returns>
		/// <param name="method">Method.</param>
		public EiPropertyEvent<T> UnsubscribeAnyThread (Action<T> method)
		{
			lock (this)
				if (onChangedUnityThread != null)
					onChangedAnyThread -= method;
			return this;
		}

		#endregion
	}

	[Serializable]
	public class EiPropertyEventFloat : EiPropertyEvent<float>
	{
		private bool clamp01 = false;

		public EiPropertyEventFloat ()
		{
			value = 0f;
		}

		public EiPropertyEventFloat (float value)
		{
			this.value = value;
		}

		public EiPropertyEventFloat (float value, bool clamp01)
		{
			this.value = value;
		}

		public override float Value {
			get {
				return value;
			}
			set {
				if (clamp01)
					base.Value = Mathf.Clamp01 (value);
				else
					base.Value = value;
			}
		}
	}

	[Serializable]
	public class EiPropertyEventVector3 : EiPropertyEvent<Vector3>
	{
	}

	[Serializable]
	public class EiPropertyEventVector2 : EiPropertyEvent<Vector2>
	{
	}

	[Serializable]
	public class EiPropertyEventVector4 : EiPropertyEvent<Vector4>
	{
	}

	[Serializable]
	public class EiPropertyEventQuaternion : EiPropertyEvent<Quaternion>
	{
	}

	[Serializable]
	public class EiPropertyEventInt : EiPropertyEvent<int>
	{
		public EiPropertyEventInt ()
		{
			value = 0;
		}

		public EiPropertyEventInt (int value)
		{
			base.value = value;
		}
	}
}