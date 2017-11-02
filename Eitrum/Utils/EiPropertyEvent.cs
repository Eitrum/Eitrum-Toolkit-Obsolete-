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

		protected Action<T> onChanged;
		protected Action<T> onChangedThreaded;
		protected bool hasChanged = false;

		#endregion

		#region Properties

		public virtual T Value {
			set {
				lock (this) {
					if (this.value.Equals (value))
						return;
					this.value = value;
					if (onChanged != null) {
						if (Thread.CurrentThread == EiUnityThreading.MainThread)
							onChanged (value);
						else {
							if (!hasChanged) {
								hasChanged = EiUpdateSystem.AddUnityThreadCallbackToQueue (this);
							}
						}
					}
					if (onChangedThreaded != null)
						onChangedThreaded (value);
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
				if (onChanged != null)
					onChanged (value);
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
		/// Subscribe with the specified method.
		/// </summary>
		/// <param name="method">Method.</param>
		public EiPropertyEvent<T> Subscribe (Action<T> method)
		{
			lock (this)
				onChanged += method;
			return this;
		}

		public EiPropertyEvent<T> SubscribeAndRun (Action<T> method)
		{
			lock (this) {
				onChanged += method;
				method (value);
			}
			return this;
		}

		public EiPropertyEvent<T> Unsubscribe (Action<T> method)
		{
			lock (this)
				if (onChanged != null)
					onChanged -= method;
			return this;
		}

		public EiPropertyEvent<T> SubscribeThreadSafe (Action<T> method)
		{
			lock (this)
				onChangedThreaded += method;
			return this;
		}

		public EiPropertyEvent<T> SubscribeThreadSafeAndRun (Action<T> method)
		{
			lock (this) {
				onChangedThreaded += method;
				method (value);
			}
			return this;
		}

		public EiPropertyEvent<T> UnsubscribeThreadSafe (Action<T> method)
		{
			lock (this)
				if (onChanged != null)
					onChangedThreaded -= method;
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
	}
}