using System;
using UnityEngine;

namespace Eitrum
{
	public class EiCallback : EiCore
	{
		#region Variables

		private EiTrigger onSuccess = new EiTrigger ();
		private EiTrigger onFailed = new EiTrigger ();
		protected bool isDone = false;

		#endregion

		#region Properties

		public bool IsDone {
			get {
				return isDone;
			}
		}

		#endregion

		#region Subscribe

		public void AddOnSuccessAnyThread (Action method)
		{
			onSuccess.AddActionAnyThread (method);
		}

		public void AddOnSuccessUnityThread (Action method)
		{
			onSuccess.AddActionUnityThread (method);
		}

		public void AddOnFailedAnyThread (Action method)
		{
			onFailed.AddActionAnyThread (method);
		}

		public void AddOnFailedUnityThread (Action method)
		{
			onFailed.AddActionUnityThread (method);
		}

		#endregion

		#region Unsubscribe

		public void RemoveOnSuccessAnyThread (Action method)
		{
			onSuccess.RemoveActionAnyThread (method);
		}

		public void RemoveOnSuccessUnityThread (Action method)
		{
			onSuccess.RemoveActionUnityThread (method);
		}

		public void RemoveOnFailedAnyThread (Action method)
		{
			onFailed.RemoveActionAnyThread (method);
		}

		public void RemoveOnFailedUnityThread (Action method)
		{
			onFailed.RemoveActionUnityThread (method);
		}

		#endregion

		#region Utils

		public virtual void Clear ()
		{
			onSuccess.Clear ();
			onFailed.Clear ();
			isDone = false;
		}

		#endregion

		#region Callback

		public void Success ()
		{
			if (!isDone) {
				isDone = true;
				onSuccess.Trigger ();
			} else {
				Debug.LogWarning ("Promise was already done @" + Time.time);
			}
		}

		public void Failed ()
		{
			if (!isDone) {
				isDone = true;
				onFailed.Trigger ();
			} else {
				Debug.LogWarning ("Promise was already done @" + Time.time);
			}
		}

		#endregion
	}

	public class EiCallback<T> : EiCallback
	{
		#region Variables

		private T item;
		private EiTrigger<T> onSuccess = new EiTrigger<T> ();

		#endregion

		#region Properties

		public T Item {
			get {
				return item;
			}
		}

		#endregion

		#region Subscribe

		public void AddOnSuccessAnyThread (Action<T> method)
		{
			onSuccess.AddActionAnyThread (method);
		}

		public void AddOnSuccessUnityThread (Action<T> method)
		{
			onSuccess.AddActionUnityThread (method);
		}

		#endregion

		#region Unsubscribe

		public void RemoveOnSuccessAnyThread (Action<T> method)
		{
			onSuccess.RemoveActionAnyThread (method);
		}

		public void RemoveOnSuccessUnityThread (Action<T> method)
		{
			onSuccess.RemoveActionUnityThread (method);
		}

		#endregion

		#region Utils

		public override void Clear ()
		{
			onSuccess.Clear ();
			item = default(T);
			base.Clear ();
		}

		#endregion

		#region Callback

		public void Success (T item)
		{
			if (!isDone) {
				this.item = item;
				base.Success ();
				onSuccess.Trigger (item);
			} else {
				Debug.LogWarning ("Promise was already done @" + Time.time);
			}
		}

		public new void Failed ()
		{
			if (!isDone) {
				item = default(T);
				base.Failed ();
			} else {
				Debug.LogWarning ("Promise was already done @" + Time.time);
			}
		}

		#endregion
	}
}