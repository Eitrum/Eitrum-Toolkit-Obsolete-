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

		public EiCallback AddOnSuccess (Action method)
		{
			onSuccess.AddAction (method);
			return this;
		}

		public EiCallback AddOnSuccess (Action method, bool anyThread)
		{
			onSuccess.AddAction (method, anyThread);
			return this;
		}

		public EiCallback AddOnFailed (Action method)
		{
			onFailed.AddAction (method);
			return this;
		}

		public EiCallback AddOnFailed (Action method, bool anyThread)
		{
			onFailed.AddAction (method, anyThread);
			return this;
		}

		#endregion

		#region Unsubscribe

		public EiCallback RemoveOnSuccess (Action method)
		{
			onSuccess.RemoveAction (method);
			return this;
		}

		public EiCallback RemoveOnFailed (Action method)
		{
			onFailed.RemoveAction (method);
			return this;
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

		public EiCallback<T> AddOnSuccess (Action<T> method)
		{
			onSuccess.AddAction (method);
			return this;
		}

		public EiCallback<T> AddOnSuccess (Action<T> method, bool anyThread)
		{
			onSuccess.AddAction (method, anyThread);
			return this;
		}

		#endregion

		#region Unsubscribe

		public EiCallback<T> RemoveOnSuccess (Action<T> method)
		{
			onSuccess.RemoveAction (method);
			return this;
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