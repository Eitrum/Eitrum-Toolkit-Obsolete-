using System;

namespace Eitrum
{
	public class EiTask : EiCore, EiUnityThreadCallbackInterface
	{
		#region Variables

		protected Action task;

		protected Action onComplete;
		protected Action onCompleteUnity;

		#endregion

		#region Constructors

		public EiTask (Action action)
		{
			task = action;
			task.BeginInvoke (Callback, null);
		}

		public EiTask (Action action, Action onCompleteCallback, Action onCompleteUnityThreadCallback)
		{
			this.onComplete = onCompleteCallback;
			this.onCompleteUnity = onCompleteUnityThreadCallback;
			task = action;
			task.BeginInvoke (Callback, null);
		}

		#endregion

		#region OnComplete callbacks

		public void OnComplete (Action action)
		{
			this.onComplete = action;
		}

		public void OnCompleteUnityThread (Action action)
		{
			this.onCompleteUnity = action;
		}

		#endregion

		#region Callbacks

		void Callback (IAsyncResult ar)
		{
			try {
				task.EndInvoke (ar);
			} catch (Exception e) {
				UnityEngine.Debug.LogException (e);
			}
			try {
				if (onComplete != null)
					onComplete ();
			} catch (Exception e) {
				UnityEngine.Debug.LogException (e);
			}
			try {
				if (this.onCompleteUnity != null) {
					while (!EiUpdateSystem.AddUnityThreadCallbackToQueue (this))
						;
				}
			} catch (Exception e) {
				UnityEngine.Debug.LogException (e);
			}
		}

		public void UnityThreadOnChangeOnly ()
		{
			if (onCompleteUnity != null)
				onCompleteUnity ();
		}

		#endregion

		#region Static creation

		public static EiTask Run (Action action)
		{
			return new EiTask (action);
		}

		public static EiTask Run (Action action, Action onCompleteCallback, Action onCompleteUnityThreadCallback)
		{
			return new EiTask (action, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		public static EiTask<T> Run<T> (Func<T> func, Action<T> onCompleteCallback, Action<T> onCompleteUnityThreadCallback)
		{
			return new EiTask<T> (func, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		#endregion
	}

	public class EiTask<T> : EiCore, EiUnityThreadCallbackInterface
	{
		#region Variables

		protected T value;
		protected Func<T> task;

		protected Action<T> onComplete;
		protected Action<T> onCompleteUnity;

		#endregion

		#region Constructors

		public EiTask (Func<T> action)
		{
			task = action;
			task.BeginInvoke (Callback, null);
		}

		public EiTask (Func<T> action, Action<T> onCompleteCallback, Action<T> onCompleteUnityThreadCallback)
		{
			this.onComplete = onCompleteCallback;
			this.onCompleteUnity = onCompleteUnityThreadCallback;
			task = action;
			task.BeginInvoke (Callback, null);
		}

		#endregion

		#region OnComplete callbacks

		public void OnComplete (Action<T> action)
		{
			this.onComplete = action;
		}

		public void OnCompleteUnityThread (Action<T> action)
		{
			this.onCompleteUnity = action;
		}

		#endregion

		#region Callbacks

		void Callback (IAsyncResult ar)
		{
			try {
				value = task.EndInvoke (ar);
			} catch (Exception e) {
				UnityEngine.Debug.LogException (e);
			}
			try {
				if (onComplete != null)
					onComplete (value);
			} catch (Exception e) {
				UnityEngine.Debug.LogException (e);
			}
			try {
				if (this.onCompleteUnity != null) {
					while (!EiUpdateSystem.AddUnityThreadCallbackToQueue (this))
						;
				}
			} catch (Exception e) {
				UnityEngine.Debug.LogException (e);
			}
		}

		public void UnityThreadOnChangeOnly ()
		{
			if (onCompleteUnity != null)
				onCompleteUnity (value);
		}

		#endregion

		#region Static

		public static EiTask<T> Run<T> (Func<T> func, Action<T> onCompleteCallback, Action<T> onCompleteUnityThreadCallback)
		{
			return new EiTask<T> (func, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		#endregion
	}
}