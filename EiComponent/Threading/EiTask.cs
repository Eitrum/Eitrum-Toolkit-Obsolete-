using System;
using System.Threading;

namespace Eitrum
{
	public class EiTask : EiCore, EiUnityThreadCallbackInterface
	{
		#region Variables

		Action task;
		Action onAnyThreadTrigger;
		Action onUnityThreadTrigger;

		#endregion

		#region Constructors

		public EiTask(Action action)
		{
			task = action;
			task.BeginInvoke(Callback, null);
		}

		public EiTask(Action action, Action onCompleteCallback)
		{
			onUnityThreadTrigger += onCompleteCallback;
			task = action;
			task.BeginInvoke(Callback, null);
		}

		public EiTask(Action action, Action onCompleteCallback, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += onCompleteCallback;
			else
				onUnityThreadTrigger += onCompleteCallback;
			task = action;
			task.BeginInvoke(Callback, null);
		}

		public EiTask(Action action, Action onCompleteCallback, Action onCompleteAnyThread)
		{
			onUnityThreadTrigger += onCompleteCallback;
			onAnyThreadTrigger += onCompleteAnyThread;
			task = action;
			task.BeginInvoke(Callback, null);
		}

		#endregion

		#region OnComplete callbacks

		public void Subscribe(Action action)
		{
			onUnityThreadTrigger += action;
		}

		public void Subscribe(Action action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
		}

		public void Unsubscribe(Action action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		#endregion

		#region Callbacks

		void Callback(IAsyncResult ar)
		{
			try
			{
				task.EndInvoke(ar);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (onAnyThreadTrigger != null)
					onAnyThreadTrigger();
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (this.onUnityThreadTrigger != null)
				{
					while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
						;
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
		}

		public void UnityThreadOnChangeOnly()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger();
		}

		#endregion

		#region Static creation

		public static EiTask Run(Action action)
		{
			return new EiTask(action);
		}

		public static EiTask Run(Action action, Action onCompleteCallback, Action onCompleteUnityThreadCallback)
		{
			return new EiTask(action, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		public static EiTask<T> Run<T>(Func<T> func, Action<T> onCompleteCallback, Action<T> onCompleteUnityThreadCallback)
		{
			return new EiTask<T>(func, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		public static EiTask<T1, TResult> Run<T1, TResult>(Func<T1, TResult> func, T1 value, Action<TResult> onCompleteCallback, Action<TResult> onCompleteUnityThreadCallback)
		{
			return new EiTask<T1, TResult>(func, value, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		public static EiTask<T1, T2, TResult> Run<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 value, T2 value2, Action<TResult> onCompleteCallback, Action<TResult> onCompleteUnityThreadCallback)
		{
			return new EiTask<T1, T2, TResult>(func, value, value2, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		public static EiTask<T1, T2, T3, TResult> Run<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 value, T2 value2, T3 value3, Action<TResult> onCompleteCallback, Action<TResult> onCompleteUnityThreadCallback)
		{
			return new EiTask<T1, T2, T3, TResult>(func, value, value2, value3, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		public static EiTask<T1, T2, T3, T4, TResult> Run<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 value, T2 value2, T3 value3, T4 value4, Action<TResult> onCompleteCallback, Action<TResult> onCompleteUnityThreadCallback)
		{
			return new EiTask<T1, T2, T3, T4, TResult>(func, value, value2, value3, value4, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		#endregion

		#region Static Creation For Unity Threaded Only

		public static void RunUnityTask(Action method)
		{
			if (EiUnityThreading.MainThread == Thread.CurrentThread)
			{
				method();
			}
			else
			{
				new EiUnityTask(method);
			}
		}

		public static void RunUnityTask<T>(Action<T> method, T item)
		{
			if (EiUnityThreading.MainThread == Thread.CurrentThread)
			{
				method(item);
			}
			else
			{
				new EiUnityTask<T>(item, method);
			}
		}

		public static void RunUnityTask<T1, T2>(Action<T1, T2> method, T1 item1, T2 item2)
		{
			if (EiUnityThreading.MainThread == Thread.CurrentThread)
			{
				method(item1, item2);
			}
			else
			{
				new EiUnityTask<T1, T2>(item1, item2, method);
			}
		}

		public static void RunUnityTask<T1, T2, T3>(Action<T1, T2, T3> method, T1 item1, T2 item2, T3 item3)
		{
			if (EiUnityThreading.MainThread == Thread.CurrentThread)
			{
				method(item1, item2, item3);
			}
			else
			{
				new EiUnityTask<T1, T2, T3>(item1, item2, item3, method);
			}
		}

		public static void RunUnityTask<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, T1 item1, T2 item2, T3 item3, T4 item4)
		{
			if (EiUnityThreading.MainThread == Thread.CurrentThread)
			{
				method(item1, item2, item3, item4);
			}
			else
			{
				new EiUnityTask<T1, T2, T3, T4>(item1, item2, item3, item4, method);
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

			public EiUnityTask(Action method)
			{
				this.method = method;
				secondThread = Nothing;
				secondThread.BeginInvoke(Callback, null);
			}

			static void Nothing()
			{

			}

			void Callback(IAsyncResult ar)
			{
				secondThread.EndInvoke(ar);
				while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
					;
			}

			#endregion

			#region EiUnityThreadCallbackInterface implementation

			public void UnityThreadOnChangeOnly()
			{
				method();
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

			public EiUnityTask(T item, Action<T> method)
			{
				this.method = method;
				this.item = item;
				secondThread = Nothing;
				secondThread.BeginInvoke(Callback, null);
			}

			static void Nothing()
			{

			}

			void Callback(IAsyncResult ar)
			{
				secondThread.EndInvoke(ar);
				while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
					;
			}

			#endregion

			#region EiUnityThreadCallbackInterface implementation

			public void UnityThreadOnChangeOnly()
			{
				method(item);
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

			public EiUnityTask(T1 item1, T2 item2, Action<T1, T2> method)
			{
				this.method = method;
				this.item1 = item1;
				this.item2 = item2;
				secondThread = Nothing;
				secondThread.BeginInvoke(Callback, null);
			}

			static void Nothing()
			{

			}

			void Callback(IAsyncResult ar)
			{
				secondThread.EndInvoke(ar);
				while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
					;
			}

			#endregion

			#region EiUnityThreadCallbackInterface implementation

			public void UnityThreadOnChangeOnly()
			{
				method(item1, item2);
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

			public EiUnityTask(T1 item1, T2 item2, T3 item3, Action<T1, T2, T3> method)
			{
				this.method = method;
				this.item1 = item1;
				this.item2 = item2;
				this.item3 = item3;
				secondThread = Nothing;
				secondThread.BeginInvoke(Callback, null);
			}

			static void Nothing()
			{

			}

			void Callback(IAsyncResult ar)
			{
				secondThread.EndInvoke(ar);
				while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
					;
			}

			#endregion

			#region EiUnityThreadCallbackInterface implementation

			public void UnityThreadOnChangeOnly()
			{
				method(item1, item2, item3);
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

			public EiUnityTask(T1 item1, T2 item2, T3 item3, T4 item4, Action<T1, T2, T3, T4> method)
			{
				this.method = method;
				this.item1 = item1;
				this.item2 = item2;
				this.item3 = item3;
				this.item4 = item4;
				secondThread = Nothing;
				secondThread.BeginInvoke(Callback, null);
			}

			static void Nothing()
			{

			}

			void Callback(IAsyncResult ar)
			{
				secondThread.EndInvoke(ar);
				while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
					;
			}

			#endregion

			#region EiUnityThreadCallbackInterface implementation

			public void UnityThreadOnChangeOnly()
			{
				method(item1, item2, item3, item4);
			}

			#endregion
		}

		#endregion
	}

	public class EiTask<TResult> : EiCore, EiUnityThreadCallbackInterface
	{
		#region Variables

		protected TResult result;
		protected Func<TResult> task;

		protected Action<TResult> onAnyThreadTrigger;
		protected Action<TResult> onUnityThreadTrigger;

		#endregion

		#region Constructors

		public EiTask(Func<TResult> action)
		{
			task = action;
			task.BeginInvoke(Callback, null);
		}

		public EiTask(Func<TResult> action, Action<TResult> onCompleteCallback, Action<TResult> onCompleteUnityThreadCallback)
		{
			this.onAnyThreadTrigger = onCompleteCallback;
			this.onUnityThreadTrigger = onCompleteUnityThreadCallback;
			task = action;
			task.BeginInvoke(Callback, null);
		}

		#endregion

		#region OnComplete callbacks

		public void Subscribe(Action<TResult> action)
		{
			onUnityThreadTrigger += action;
		}

		public void Subscribe(Action<TResult> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
		}

		public void Unsubscribe(Action<TResult> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		#endregion

		#region Callbacks

		void Callback(IAsyncResult ar)
		{
			try
			{
				result = task.EndInvoke(ar);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (onAnyThreadTrigger != null)
					onAnyThreadTrigger(result);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (this.onUnityThreadTrigger != null)
				{
					while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
						;
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
		}

		public void UnityThreadOnChangeOnly()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger(result);
		}

		#endregion

		#region Static

		public static EiTask<TResult> Run(Func<TResult> func, Action<TResult> onCompleteCallback, Action<TResult> onCompleteUnityThreadCallback)
		{
			return new EiTask<TResult>(func, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		#endregion
	}

	public class EiTask<T1, TResult> : EiCore, EiUnityThreadCallbackInterface
	{
		#region Variables

		protected TResult result;
		protected Func<T1, TResult> task;

		protected Action<TResult> onAnyThreadTrigger;
		protected Action<TResult> onUnityThreadTrigger;

		#endregion

		#region Constructors

		public EiTask(Func<T1, TResult> func, T1 value, Action<TResult> onCompleteCallback)
		{
			onUnityThreadTrigger += onCompleteCallback;
			task = func;
			task.BeginInvoke(value, Callback, null);
		}

		public EiTask(Func<T1, TResult> func, T1 value, Action<TResult> onCompleteCallback, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += onCompleteCallback;
			else
				onUnityThreadTrigger += onCompleteCallback;
			task = func;
			task.BeginInvoke(value, Callback, null);
		}

		public EiTask(Func<T1, TResult> func, T1 value, Action<TResult> onCompleteCallback, Action<TResult> onCompleteAnyThread)
		{
			onUnityThreadTrigger += onCompleteCallback;
			onAnyThreadTrigger += onCompleteAnyThread;
			task = func;
			task.BeginInvoke(value, Callback, null);
		}

		#endregion

		#region OnComplete callbacks

		public void Subscribe(Action<TResult> action)
		{
			onUnityThreadTrigger += action;
		}

		public void Subscribe(Action<TResult> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
		}

		public void Unsubscribe(Action<TResult> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		#endregion

		#region Callbacks

		void Callback(IAsyncResult ar)
		{
			try
			{
				result = task.EndInvoke(ar);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (onAnyThreadTrigger != null)
					onAnyThreadTrigger(result);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (this.onUnityThreadTrigger != null)
				{
					while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
						;
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
		}

		public void UnityThreadOnChangeOnly()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger(result);
		}

		#endregion

		#region Static

		public static EiTask<T1, TResult> Run(Func<T1, TResult> func, T1 value, Action<TResult> onCompleteCallback, Action<TResult> onCompleteUnityThreadCallback)
		{
			return new EiTask<T1, TResult>(func, value, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		#endregion
	}

	public class EiTask<T1, T2, TResult> : EiCore, EiUnityThreadCallbackInterface
	{
		#region Variables

		protected TResult result;
		protected Func<T1, T2, TResult> task;

		protected Action<TResult> onAnyThreadTrigger;
		protected Action<TResult> onUnityThreadTrigger;

		#endregion

		#region Constructors

		public EiTask(Func<T1, T2, TResult> func, T1 value, T2 value2, Action<TResult> onCompleteCallback)
		{
			onUnityThreadTrigger += onCompleteCallback;
			task = func;
			task.BeginInvoke(value, value2, Callback, null);
		}

		public EiTask(Func<T1, T2, TResult> func, T1 value, T2 value2, Action<TResult> onCompleteCallback, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += onCompleteCallback;
			else
				onUnityThreadTrigger += onCompleteCallback;
			task = func;
			task.BeginInvoke(value, value2, Callback, null);
		}

		public EiTask(Func<T1, T2, TResult> func, T1 value, T2 value2, Action<TResult> onCompleteCallback, Action<TResult> onCompleteAnyThread)
		{
			onUnityThreadTrigger += onCompleteCallback;
			onAnyThreadTrigger += onCompleteAnyThread;
			task = func;
			task.BeginInvoke(value, value2, Callback, null);
		}

		#endregion

		#region OnComplete callbacks

		public void Subscribe(Action<TResult> action)
		{
			onUnityThreadTrigger += action;
		}

		public void Subscribe(Action<TResult> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
		}

		public void Unsubscribe(Action<TResult> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		#endregion

		#region Callbacks

		void Callback(IAsyncResult ar)
		{
			try
			{
				result = task.EndInvoke(ar);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (onAnyThreadTrigger != null)
					onAnyThreadTrigger(result);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (this.onUnityThreadTrigger != null)
				{
					while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
						;
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
		}

		public void UnityThreadOnChangeOnly()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger(result);
		}

		#endregion

		#region Static

		public static EiTask<T1, T2, TResult> Run(Func<T1, T2, TResult> func, T1 value, T2 value2, Action<TResult> onCompleteCallback, Action<TResult> onCompleteUnityThreadCallback)
		{
			return new EiTask<T1, T2, TResult>(func, value, value2, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		#endregion
	}

	public class EiTask<T1, T2, T3, TResult> : EiCore, EiUnityThreadCallbackInterface
	{
		#region Variables

		protected TResult result;
		protected Func<T1, T2, T3, TResult> task;

		protected Action<TResult> onAnyThreadTrigger;
		protected Action<TResult> onUnityThreadTrigger;

		#endregion

		#region Constructors

		public EiTask(Func<T1, T2, T3, TResult> func, T1 value, T2 value2, T3 value3, Action<TResult> onCompleteCallback)
		{
			onUnityThreadTrigger += onCompleteCallback;
			task = func;
			task.BeginInvoke(value, value2, value3, Callback, null);
		}

		public EiTask(Func<T1, T2, T3, TResult> func, T1 value, T2 value2, T3 value3, Action<TResult> onCompleteCallback, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += onCompleteCallback;
			else
				onUnityThreadTrigger += onCompleteCallback;
			task = func;
			task.BeginInvoke(value, value2, value3, Callback, null);
		}

		public EiTask(Func<T1, T2, T3, TResult> func, T1 value, T2 value2, T3 value3, Action<TResult> onCompleteCallback, Action<TResult> onCompleteAnyThread)
		{
			onUnityThreadTrigger += onCompleteCallback;
			onAnyThreadTrigger += onCompleteAnyThread;
			task = func;
			task.BeginInvoke(value, value2, value3, Callback, null);
		}

		#endregion

		#region OnComplete callbacks

		public void Subscribe(Action<TResult> action)
		{
			onUnityThreadTrigger += action;
		}

		public void Subscribe(Action<TResult> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
		}

		public void Unsubscribe(Action<TResult> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		#endregion

		#region Callbacks

		void Callback(IAsyncResult ar)
		{
			try
			{
				result = task.EndInvoke(ar);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (onAnyThreadTrigger != null)
					onAnyThreadTrigger(result);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (this.onUnityThreadTrigger != null)
				{
					while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
						;
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
		}

		public void UnityThreadOnChangeOnly()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger(result);
		}

		#endregion

		#region Static

		public static EiTask<T1, T2, T3, TResult> Run(Func<T1, T2, T3, TResult> func, T1 value, T2 value2, T3 value3, Action<TResult> onCompleteCallback, Action<TResult> onCompleteUnityThreadCallback)
		{
			return new EiTask<T1, T2, T3, TResult>(func, value, value2, value3, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		#endregion
	}

	public class EiTask<T1, T2, T3, T4, TResult> : EiCore, EiUnityThreadCallbackInterface
	{
		#region Variables

		protected TResult result;
		protected Func<T1, T2, T3, T4, TResult> task;

		protected Action<TResult> onAnyThreadTrigger;
		protected Action<TResult> onUnityThreadTrigger;

		#endregion

		#region Constructors

		public EiTask(Func<T1, T2, T3, T4, TResult> func, T1 value, T2 value2, T3 value3, T4 value4, Action<TResult> onCompleteCallback)
		{
			onUnityThreadTrigger += onCompleteCallback;
			task = func;
			task.BeginInvoke(value, value2, value3, value4, Callback, null);
		}

		public EiTask(Func<T1, T2, T3, T4, TResult> func, T1 value, T2 value2, T3 value3, T4 value4, Action<TResult> onCompleteCallback, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += onCompleteCallback;
			else
				onUnityThreadTrigger += onCompleteCallback;
			task = func;
			task.BeginInvoke(value, value2, value3, value4, Callback, null);
		}

		public EiTask(Func<T1, T2, T3, T4, TResult> func, T1 value, T2 value2, T3 value3, T4 value4, Action<TResult> onCompleteCallback, Action<TResult> onCompleteAnyThread)
		{
			onUnityThreadTrigger += onCompleteCallback;
			onAnyThreadTrigger += onCompleteAnyThread;
			task = func;
			task.BeginInvoke(value, value2, value3, value4, Callback, null);
		}

		#endregion

		#region OnComplete callbacks

		public void Subscribe(Action<TResult> action)
		{
			onUnityThreadTrigger += action;
		}

		public void Subscribe(Action<TResult> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
		}

		public void Unsubscribe(Action<TResult> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		#endregion

		#region Callbacks

		void Callback(IAsyncResult ar)
		{
			try
			{
				result = task.EndInvoke(ar);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (onAnyThreadTrigger != null)
					onAnyThreadTrigger(result);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			try
			{
				if (this.onUnityThreadTrigger != null)
				{
					while (!EiUpdateSystem.AddUnityThreadCallbackToQueue(this))
						;
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
		}

		public void UnityThreadOnChangeOnly()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger(result);
		}

		#endregion

		#region Static

		public static EiTask<T1, T2, T3, T4, TResult> Run(Func<T1, T2, T3, T4, TResult> func, T1 value, T2 value2, T3 value3, T4 value4, Action<TResult> onCompleteCallback, Action<TResult> onCompleteUnityThreadCallback)
		{
			return new EiTask<T1, T2, T3, T4, TResult>(func, value, value2, value3, value4, onCompleteCallback, onCompleteUnityThreadCallback);
		}

		#endregion
	}

}