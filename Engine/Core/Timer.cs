using Eitrum.Engine.Core.Singleton;
using System;
using System.Collections;
using UnityEngine;

namespace Eitrum.Engine.Core {
	public sealed class Timer
	{
		#region Singleton

		public static TimerBehaviour Instance {
			get {
				return TimerBehaviour.Instance;
			}
		}

		#endregion

		#region Once

		// ----------- return Coroutine --------------

		public static Coroutine Once (float time, Action action)
		{
			return Instance._Once (time, action);
		}

		public static Coroutine Once<T0> (float time, Action<T0> action, T0 t0)
		{
			return Instance._Once (time, action, t0);
		}

		public static Coroutine Once<T0, T1> (float time, Action<T0, T1> action, T0 t0, T1 t1)
		{
			return Instance._Once (time, action, t0, t1);
		}

		public static Coroutine Once<T0, T1, T2> (float time, Action<T0, T1, T2> action, T0 t0, T1 t1, T2 t2)
		{
			return Instance._Once (time, action, t0, t1, t2);
		}

		public static Coroutine Once<T0, T1, T2, T3> (float time, Action<T0, T1, T2, T3> action, T0 t0, T1 t1, T2 t2, T3 t3)
		{
			return Instance._Once (time, action, t0, t1, t2, t3);
		}

		// ----------- ref Coroutine --------------

		public static void Once (float time, Action action, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Once (time, action);
		}

		public static void Once<T0> (float time, Action<T0> action, T0 t0, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Once (time, action, t0);
		}

		public static void Once<T0, T1> (float time, Action<T0, T1> action, T0 t0, T1 t1, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Once (time, action, t0, t1);
		}

		public static void Once<T0, T1, T2> (float time, Action<T0, T1, T2> action, T0 t0, T1 t1, T2 t2, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Once (time, action, t0, t1, t2);
		}

		public static void Once<T0, T1, T2, T3> (float time, Action<T0, T1, T2, T3> action, T0 t0, T1 t1, T2 t2, T3 t3, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Once (time, action, t0, t1, t2, t3);
		}

		#endregion

		#region Repeat

		// ----------- return Coroutine --------------

		public static Coroutine Repeat (float stepTime, int itterations, Action action)
		{
			return Instance._Repeat (stepTime, itterations, action);
		}

		public static Coroutine Repeat<T> (float stepTime, int itterations, Action<T> action, T data)
		{
			return Instance._Repeat<T> (stepTime, itterations, action, data);
		}

		public static Coroutine Repeat (float stepTime, int itterations, Action<int> action)
		{
			return Instance._Repeat (stepTime, itterations, action);
		}

		public static Coroutine Repeat<T> (float stepTime, int itterations, Action<int, T> action, T data)
		{
			return Instance._Repeat<T> (stepTime, itterations, action, data);
		}

		// ----------- ref Coroutine --------------

		public static void Repeat (float stepTime, int itterations, Action action, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Repeat (stepTime, itterations, action);
		}

		public static void Repeat<T> (float stepTime, int itterations, Action<T> action, T data, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Repeat<T> (stepTime, itterations, action, data);
		}

		public static void Repeat (float stepTime, int itterations, Action<int> action, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Repeat (stepTime, itterations, action);
		}

		public static void Repeat<T> (float stepTime, int itterations, Action<int, T> action, T data, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Repeat<T> (stepTime, itterations, action, data);
		}

		#endregion

		#region Animate

		// ------------- return Coroutine --------------

		public static Coroutine Animate (float duration, Action<float> operation)
		{
			return Instance._Animate (duration, operation);
		}

		public static Coroutine Animate<T> (float duration, Action<float, T> operation, T animationData)
		{
			return Instance._Animate<T> (duration, operation, animationData);
		}

		public static Coroutine Animate (float duration, Action<float> operation, Action onDone)
		{
			return Instance._Animate (duration, operation, onDone);
		}

		public static Coroutine Animate<T> (float duration, Action<float, T> operation, T animationData, Action onDone)
		{
			return Instance._Animate<T> (duration, operation, animationData, onDone);
		}

		public static Coroutine Animate<T> (float duration, Action<float, T> operation, T animationData, Action<T> onDone)
		{
			return Instance._Animate<T> (duration, operation, animationData, onDone);
		}

		// ------------- ref Coroutine --------------

		public static void Animate (float duration, Action<float> operation, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Animate (duration, operation);
		}

		public static void Animate<T> (float duration, Action<float, T> operation, T animationData, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Animate<T> (duration, operation, animationData);
		}

		public static void Animate (float duration, Action<float> operation, Action onDone, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Animate (duration, operation, onDone);
		}

		public static void Animate<T> (float duration, Action<float, T> operation, T animationData, Action onDone, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Animate<T> (duration, operation, animationData, onDone);
		}

		public static void Animate<T> (float duration, Action<float, T> operation, T animationData, Action<T> onDone, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Animate<T> (duration, operation, animationData, onDone);
		}

		// ------------- return Coroutine ease function --------------

		public static Coroutine Animate (float duration, Action<float> operation, Func<float, float> easeFunction)
		{
			return Instance._Animate (duration, operation, easeFunction);
		}

		public static Coroutine Animate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction)
		{
			return Instance._Animate<T> (duration, operation, animationData, easeFunction);
		}

		public static Coroutine Animate (float duration, Action<float> operation, Func<float, float> easeFunction, Action onDone)
		{
			return Instance._Animate (duration, operation, easeFunction, onDone);
		}

		public static Coroutine Animate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction, Action onDone)
		{
			return Instance._Animate<T> (duration, operation, animationData, easeFunction, onDone);
		}

		public static Coroutine Animate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction, Action<T> onDone)
		{
			return Instance._Animate<T> (duration, operation, animationData, easeFunction, onDone);
		}

		// ------------- ref Coroutine ease function --------------

		public static void Animate (float duration, Action<float> operation, Func<float, float> easeFunction, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Animate (duration, operation, easeFunction);
		}

		public static void Animate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Animate<T> (duration, operation, animationData, easeFunction);
		}

		public static void Animate (float duration, Action<float> operation, Func<float, float> easeFunction, Action onDone, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Animate (duration, operation, easeFunction, onDone);
		}

		public static void Animate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction, Action onDone, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Animate<T> (duration, operation, animationData, easeFunction, onDone);
		}

		public static void Animate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction, Action<T> onDone, ref Coroutine coroutine)
		{
			Stop (coroutine);
			coroutine = Instance._Animate<T> (duration, operation, animationData, easeFunction, onDone);
		}

		#endregion

		#region Stop

		public static void Stop (Coroutine coroutine)
		{
			Instance._Stop (coroutine);
		}

		public static void StopAll ()
		{
			Instance.StopAllCoroutines ();
		}

		#endregion
	}

	public sealed class TimerBehaviour : EiComponentSingleton<TimerBehaviour>
	{
		#region Once

		public Coroutine _Once (float time, Action action)
		{
			return StartCoroutine (EOnce (time, action));
		}

		public Coroutine _Once<T0> (float time, Action<T0> action, T0 t0)
		{
			return StartCoroutine (EOnce (time, action, t0));
		}

		public Coroutine _Once<T0, T1> (float time, Action<T0, T1> action, T0 t0, T1 t1)
		{
			return StartCoroutine (EOnce (time, action, t0, t1));
		}

		public Coroutine _Once<T0, T1, T2> (float time, Action<T0, T1, T2> action, T0 t0, T1 t1, T2 t2)
		{
			return StartCoroutine (EOnce (time, action, t0, t1, t2));
		}

		public Coroutine _Once<T0, T1, T2, T3> (float time, Action<T0, T1, T2, T3> action, T0 t0, T1 t1, T2 t2, T3 t3)
		{
			return StartCoroutine (EOnce (time, action, t0, t1, t2, t3));
		}

		IEnumerator EOnce (float time, Action action)
		{
			yield return new WaitForSeconds (time);
			action ();
		}

		IEnumerator EOnce<T> (float time, Action<T> action, T value)
		{
			yield return new WaitForSeconds (time);
			action (value);
		}

		IEnumerator EOnce<T0, T1> (float time, Action<T0, T1> action, T0 value0, T1 value1)
		{
			yield return new WaitForSeconds (time);
			action (value0, value1);
		}

		IEnumerator EOnce<T0, T1, T2> (float time, Action<T0, T1, T2> action, T0 value0, T1 value1, T2 value2)
		{
			yield return new WaitForSeconds (time);
			action (value0, value1, value2);
		}

		IEnumerator EOnce<T0, T1, T2, T3> (float time, Action<T0, T1, T2, T3> action, T0 value0, T1 value1, T2 value2, T3 value3)
		{
			yield return new WaitForSeconds (time);
			action (value0, value1, value2, value3);
		}

		#endregion

		#region Repeat

		public Coroutine _Repeat (float stepTime, int itterations, Action action)
		{
			return StartCoroutine (ERepeat (stepTime, itterations, action));
		}

		public Coroutine _Repeat<T> (float stepTime, int itterations, Action<T> action, T data)
		{
			return StartCoroutine (ERepeat (stepTime, itterations, action, data));
		}

		public Coroutine _Repeat (float stepTime, int itterations, Action<int> action)
		{
			return StartCoroutine (ERepeat (stepTime, itterations, action));
		}

		public Coroutine _Repeat<T> (float stepTime, int itterations, Action<int, T> action, T data)
		{
			return StartCoroutine (ERepeat (stepTime, itterations, action, data));
		}

		IEnumerator ERepeat (float stepTime, int itterations, Action action)
		{
			var time = new WaitForSeconds (stepTime);
			for (int i = 0; i < itterations; i++) {
				yield return time;
				action ();
			}
		}

		IEnumerator ERepeat<T> (float stepTime, int itterations, Action<T> action, T data)
		{
			var time = new WaitForSeconds (stepTime);
			for (int i = 0; i < itterations; i++) {
				yield return time;
				action (data);
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

		IEnumerator ERepeat<T> (float stepTime, int itterations, Action<int, T> action, T data)
		{
			var time = new WaitForSeconds (stepTime);
			for (int i = 0; i < itterations; i++) {
				yield return time;
				action (i, data);
			}
		}

		#endregion

		#region Animate

		public Coroutine _Animate (float duration, Action<float> operation)
		{
			return StartCoroutine (EAnimate (duration, operation));
		}

		public Coroutine _Animate<T> (float duration, Action<float, T> operation, T animationData)
		{
			return StartCoroutine (EAnimate<T> (duration, operation, animationData));
		}

		public Coroutine _Animate (float duration, Action<float> operation, Action onDone)
		{
			return StartCoroutine (EAnimate (duration, operation, onDone));
		}

		public Coroutine _Animate<T> (float duration, Action<float, T> operation, T animationData, Action onDone)
		{
			return StartCoroutine (EAnimate<T> (duration, operation, animationData, onDone));
		}

		public Coroutine _Animate<T> (float duration, Action<float, T> operation, T animationData, Action<T> onDone)
		{
			return StartCoroutine (EAnimate<T> (duration, operation, animationData, onDone));
		}

		// ------------- ease functions

		public Coroutine _Animate (float duration, Action<float> operation, Func<float, float> easeFunction)
		{
			return StartCoroutine (EAnimate (duration, operation, easeFunction));
		}

		public Coroutine _Animate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction)
		{
			return StartCoroutine (EAnimate<T> (duration, operation, animationData, easeFunction));
		}

		public Coroutine _Animate (float duration, Action<float> operation, Func<float, float> easeFunction, Action onDone)
		{
			return StartCoroutine (EAnimate (duration, operation, easeFunction, onDone));
		}

		public Coroutine _Animate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction, Action onDone)
		{
			return StartCoroutine (EAnimate<T> (duration, operation, animationData, easeFunction, onDone));
		}

		public Coroutine _Animate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction, Action<T> onDone)
		{
			return StartCoroutine (EAnimate<T> (duration, operation, animationData, easeFunction, onDone));
		}

		IEnumerator EAnimate (float duration, Action<float> operation)
		{
			var timer = 0f;
			operation (timer);
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (System.Math.Min (timer, 1f));
			}
		}

		IEnumerator EAnimate<T> (float duration, Action<float, T> operation, T animationData)
		{
			var timer = 0f;
			operation (timer, animationData);
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (System.Math.Min (timer, 1f), animationData);
			}
		}

		IEnumerator EAnimate (float duration, Action<float> operation, Action onDone)
		{
			var timer = 0f;
			operation (timer);
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (System.Math.Min (timer, 1f));
			}
			onDone ();
		}

		IEnumerator EAnimate<T> (float duration, Action<float, T> operation, T animationData, Action onDone)
		{
			var timer = 0f;
			operation (timer, animationData);
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (System.Math.Min (timer, 1f), animationData);
			}
			onDone ();
		}

		IEnumerator EAnimate<T> (float duration, Action<float, T> operation, T animationData, Action<T> onDone)
		{
			var timer = 0f;
			operation (timer, animationData);
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (System.Math.Min (timer, 1f), animationData);
			}
			onDone (animationData);
		}

		//----------- Ease Functions

		IEnumerator EAnimate (float duration, Action<float> operation, Func<float, float> easeFunction)
		{
			var timer = 0f;
			operation (easeFunction (timer));
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (easeFunction (System.Math.Min (timer, 1f)));
			}
		}

		IEnumerator EAnimate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction)
		{
			var timer = 0f;
			operation (easeFunction (timer), animationData);
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (easeFunction (System.Math.Min (timer, 1f)), animationData);
			}
		}

		IEnumerator EAnimate (float duration, Action<float> operation, Func<float, float> easeFunction, Action onDone)
		{
			var timer = 0f;
			operation (easeFunction (timer));
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (easeFunction (System.Math.Min (timer, 1f)));
			}
			onDone ();
		}

		IEnumerator EAnimate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction, Action onDone)
		{
			var timer = 0f;
			operation (easeFunction (timer), animationData);
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (easeFunction (System.Math.Min (timer, 1f)), animationData);
			}
			onDone ();
		}

		IEnumerator EAnimate<T> (float duration, Action<float, T> operation, T animationData, Func<float, float> easeFunction, Action<T> onDone)
		{
			var timer = 0f;
			operation (easeFunction (timer), animationData);
			while (timer < 1f) {
				yield return null;
				timer += Time.unscaledDeltaTime / duration;
				operation (easeFunction (System.Math.Min (timer, 1f)), animationData);
			}
			onDone (animationData);
		}

		#endregion

		#region Stop

		public void _Stop (Coroutine coroutine)
		{
			if (coroutine != null)
				StopCoroutine (coroutine);
		}

		#endregion
	}
}