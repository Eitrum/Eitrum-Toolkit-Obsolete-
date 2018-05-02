using System;
using System.Threading;

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

		#region Static Creation For Unity Threaded Only

		public static void RunUnityTask (Action method)
		{
			if (EiUnityThreading.MainThread == Thread.CurrentThread) {
				method ();
			} else {
				new EiUnityTask (method);
			}
		}

		public static void RunUnityTask<T> (Action<T> method, T item)
		{
			if (EiUnityThreading.MainThread == Thread.CurrentThread) {
				method (item);
			} else {
				new EiUnityTask<T> (item, method);
			}
		}

		public static void RunUnityTask<T1, T2> (Action<T1, T2> method, T1 item1, T2 item2)
		{
			if (EiUnityThreading.MainThread == Thread.CurrentThread) {
				method (item1, item2);
			} else {
				new EiUnityTask<T1, T2> (item1, item2, method);
			}
		}

		public static void RunUnityTask<T1, T2, T3> (Action<T1, T2, T3> method, T1 item1, T2 item2, T3 item3)
		{
			if (EiUnityThreading.MainThread == Thread.CurrentThread) {
				method (item1, item2, item3);
			} else {
				new EiUnityTask<T1, T2, T3> (item1, item2, item3, method);
			}
		}

		public static void RunUnityTask<T1, T2, T3, T4> (Action<T1, T2, T3, T4> method, T1 item1, T2 item2, T3 item3, T4 item4)
		{
			if (EiUnityThreading.MainThread == Thread.CurrentThread) {
				method (item1, item2, item3, item4);
			} else {
				new EiUnityTask<T1, T2, T3, T4> (item1, item2, item3, item4, method);
			}
		}

		#endregion

		#region Unity Task Classes

		class EiUnityTask : EiUnityThreadCallbackInterface
		{
			#region Variables

			Action method;
			Action secondThread;

			#endregion

			#region Core

			public EiUnityTask (Action method)
			{
				this.method = method;
				secondThread = Nothing;
				secondThread.BeginInvoke (Callback, null);
			}

			static void Nothing ()
			{

			}

			void Callback (IAsyncResult ar)
			{
				secondThread.EndInvoke (ar);
				while (!EiUpdateSystem.AddUnityThreadCallbackToQueue (this))
					;
			}

			#endregion

			#region EiUnityThreadCallbackInterface implementation

			public void UnityThreadOnChangeOnly ()
			{
				method ();
			}

			#endregion
		}

		class EiUnityTask<T> : EiUnityThreadCallbackInterface
		{
			#region Variables

			T item;
			Action<T> method;
			Action secondThread;

			#endregion

			#region Core

			public EiUnityTask (T item, Action<T> method)
			{
				this.method = method;
				this.item = item;
				secondThread = Nothing;
				secondThread.BeginInvoke (Callback, null);
			}

			static void Nothing ()
			{

			}

			void Callback (IAsyncResult ar)
			{
				secondThread.EndInvoke (ar);
				while (!EiUpdateSystem.AddUnityThreadCallbackToQueue (this))
					;
			}

			#endregion

			#region EiUnityThreadCallbackInterface implementation

			public void UnityThreadOnChangeOnly ()
			{
				method (item);
			}

			#endregion
		}

		class EiUnityTask<T1, T2> : EiUnityThreadCallbackInterface
		{
			#region Variables

			T1 item1;
			T2 item2;
			Action<T1, T2> method;
			Action secondThread;

			#endregion

			#region Core

			public EiUnityTask (T1 item1, T2 item2, Action<T1, T2> method)
			{
				this.method = method;
				this.item1 = item1;
				this.item2 = item2;
				secondThread = Nothing;
				secondThread.BeginInvoke (Callback, null);
			}

			static void Nothing ()
			{

			}

			void Callback (IAsyncResult ar)
			{
				secondThread.EndInvoke (ar);
				while (!EiUpdateSystem.AddUnityThreadCallbackToQueue (this))
					;
			}

			#endregion

			#region EiUnityThreadCallbackInterface implementation

			public void UnityThreadOnChangeOnly ()
			{
				method (item1, item2);
			}

			#endregion
		}

		class EiUnityTask<T1, T2, T3> : EiUnityThreadCallbackInterface
		{
			#region Variables

			T1 item1;
			T2 item2;
			T3 item3;
			Action<T1, T2, T3> method;
			Action secondThread;

			#endregion

			#region Core

			public EiUnityTask (T1 item1, T2 item2, T3 item3, Action<T1, T2, T3> method)
			{
				this.method = method;
				this.item1 = item1;
				this.item2 = item2;
				this.item3 = item3;
				secondThread = Nothing;
				secondThread.BeginInvoke (Callback, null);
			}

			static void Nothing ()
			{

			}

			void Callback (IAsyncResult ar)
			{
				secondThread.EndInvoke (ar);
				while (!EiUpdateSystem.AddUnityThreadCallbackToQueue (this))
					;
			}

			#endregion

			#region EiUnityThreadCallbackInterface implementation

			public void UnityThreadOnChangeOnly ()
			{
				method (item1, item2, item3);
			}

			#endregion
		}

		class EiUnityTask<T1, T2, T3, T4> : EiUnityThreadCallbackInterface
		{
			#region Variables

			T1 item1;
			T2 item2;
			T3 item3;
			T4 item4;
			Action<T1, T2, T3, T4> method;
			Action secondThread;

			#endregion

			#region Core

			public EiUnityTask (T1 item1, T2 item2, T3 item3, T4 item4, Action<T1, T2, T3, T4> method)
			{
				this.method = method;
				this.item1 = item1;
				this.item2 = item2;
				this.item3 = item3;
				this.item4 = item4;
				secondThread = Nothing;
				secondThread.BeginInvoke (Callback, null);
			}

			static void Nothing ()
			{

			}

			void Callback (IAsyncResult ar)
			{
				secondThread.EndInvoke (ar);
				while (!EiUpdateSystem.AddUnityThreadCallbackToQueue (this))
					;
			}

			#endregion

			#region EiUnityThreadCallbackInterface implementation

			public void UnityThreadOnChangeOnly ()
			{
				method (item1, item2, item3, item4);
			}

			#endregion
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

		public static EiTask<T0> Run<T0> (Func<T0> func, Action<T0> onCompleteCallback, Action<T0> onCompleteUnityThreadCallback)
		{
			return new EiTask<T0> (func, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		#endregion
	}
}