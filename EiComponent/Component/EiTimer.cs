using System;
using System.Collections;
using UnityEngine;

namespace Eitrum
{
	public class EiTimer : EiComponentSingleton<EiTimer>
	{
		public override void SingletonCreation ()
		{
			KeepAlive ();
		}

		#region Static

		public static Coroutine Once (float time, Action action)
		{
			return Instance._Once (time, action);
		}

		public static Coroutine Repeat (float stepTime, int itterations, Action action)
		{
			return Instance._Repeat (stepTime, itterations, action);
		}

		public static Coroutine Repeat (float stepTime, int itterations, Action<int> action)
		{
			return Instance._Repeat (stepTime, itterations, action);
		}

		public static Coroutine Animate (float duration, Action<float> operation)
		{
			return Instance._Animate (duration, operation);
		}

		public static Coroutine Animate (float duration, Action<float> operation, Action onDone)
		{
			return Instance._Animate (duration, operation, onDone);
		}

		public static void Stop (Coroutine coroutine)
		{
			Instance._Stop (coroutine);
		}

		#endregion

		#region Methods

		public Coroutine _Once (float time, Action action)
		{
			return StartCoroutine (EOnce (time, action));
		}

		public Coroutine _Repeat (float stepTime, int itterations, Action action)
		{
			return StartCoroutine (ERepeat (stepTime, itterations, action));
		}

		public Coroutine _Repeat (float stepTime, int itterations, Action<int> action)
		{
			return StartCoroutine (ERepeat (stepTime, itterations, action));
		}

		public Coroutine _Animate (float duration, Action<float> operation)
		{
			return StartCoroutine (EAnimate (duration, operation));
		}

		public Coroutine _Animate (float duration, Action<float> operation, Action onDone)
		{
			return StartCoroutine (EAnimate (duration, operation, onDone));
		}

		public void _Stop (Coroutine coroutine)
		{
			StopCoroutine (coroutine);
		}

		#endregion

		#region Enumerators

		IEnumerator EOnce (float time, Action action)
		{
			yield return new WaitForSeconds (time);
			action ();
		}

		IEnumerator ERepeat (float stepTime, int itterations, Action action)
		{
			var time = new WaitForSeconds (stepTime);
			for (int i = 0; i < itterations; i++) {
				yield return time;
				action ();
			}
		}

		IEnumerator ERepeat (float stepTime, int itterations, Action<int> action)
		{
			var time = new WaitForSeconds (stepTime);
			for (int i = 0; i < itterations; i++) {
				yield return time;
				action (i);
			}
		}

		IEnumerator EAnimate (float duration, Action<float> operation)
		{
			var timer = 0f;
			operation (timer);
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (Math.Min (timer, 1f));
			}
		}

		IEnumerator EAnimate (float duration, Action<float> operation, Action onDone)
		{
			var timer = 0f;
			operation (timer);
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (Math.Min (timer, 1f));
			}
			onDone ();
		}

		#endregion
	}
}

