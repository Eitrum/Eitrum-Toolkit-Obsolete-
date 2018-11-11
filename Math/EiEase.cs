using System;
using UnityEngine;

namespace Eitrum
{
	#region Enums

	public enum EaseFunction
	{
		Linear,
		Quad,
		Cubic,
		Quartic,
		Quintic,
		Sin,
		Exponential,
		Circular,
		Bounce,
		Elastic,
		Back
	}

	public enum EaseType
	{
		In,
		Out,
		InOut
	}

	#endregion

	public static class EiEase
	{
		#region Extensions

		public static float Ease (this float time, EaseFunction functionType, EaseType easeType)
		{
			return GetEaseFunction (functionType, easeType) (time);
		}

		public static float EaseIn (this float time, EaseFunction functionType)
		{
			return GetEaseFunction (functionType, EaseType.In) (time);
		}

		public static float EaseOut (this float time, EaseFunction functionType)
		{
			return GetEaseFunction (functionType, EaseType.Out) (time);
		}

		public static float EaseInOut (this float time, EaseFunction functionType)
		{
			return GetEaseFunction (functionType, EaseType.InOut) (time);
		}

		#endregion

		#region Core

		public static AnimationCurve GetAnimationCurve (Func<float, float> easeFunction, int steps = 20)
		{
			AnimationCurve curve = new AnimationCurve ();
			steps = Mathf.Clamp (steps, 4, 100);
			float timePerStep = 1f / (float)steps;
			float timeBetweenSteps = timePerStep / 2f;
			for (int i = 0; i <= steps; i++) {
				float time = timePerStep * (float)i;
				var point = easeFunction (time);
				var before = easeFunction (time - timeBetweenSteps);
				var after = easeFunction (time + timeBetweenSteps);
				curve.AddKey (new Keyframe (time, point, (point - before) / timeBetweenSteps, (after - point) / timeBetweenSteps, 0.5f, 0.5f));
			}
			return curve;
		}

		public static Func<float, float> GetEaseFunction (EaseFunction functionType, EaseType easeType)
		{
			switch (functionType) {
			case EaseFunction.Linear:
				return Linear;
			case EaseFunction.Quad:
				return Quad.Get (easeType);
			case EaseFunction.Cubic:
				return Cubic.Get (easeType);
			case EaseFunction.Quartic:
				return Quartic.Get (easeType);
			case EaseFunction.Quintic:
				return Quintic.Get (easeType);
			case EaseFunction.Sin:
				return Sin.Get (easeType);
			case EaseFunction.Exponential:
				return Exponential.Get (easeType);
			case EaseFunction.Circular:
				return Circular.Get (easeType);
			case EaseFunction.Bounce:
				return Bounce.Get (easeType);
			case EaseFunction.Elastic:
				return Elastic.Get (easeType);
			case EaseFunction.Back:
				return Back.Get (easeType);
			}
			return f => f;
		}

		#endregion

		#region Linear

		/// <summary>
		/// Linear ease the specified time.
		/// DOES NOTHING, REALLY!!! WHY YOU USING IT???
		/// </summary>
		/// <param name="time">Time.</param>
		public static float Linear (float time)
		{
			return time;
		}

		#endregion

		#region Quad

		public static class Quad
		{
			public static float EaseIn (float time)
			{
				return time * time;
			}

			public static float EaseOut (float time)
			{
				return -time * (time - 2f);
			}

			public static float EaseInOut (float time)
			{
				time /= 0.5f;
				if (time < 1f)
					return 0.5f * time * time;
				time -= 1f;
				return -0.5f * (time * (time - 2f) - 1f);
			}

			public static Func<float, float> Get (EaseType easeType)
			{
				switch (easeType) {
				case EaseType.In:
					return EaseIn;
				case EaseType.Out:
					return EaseOut;
				case EaseType.InOut:
					return EaseInOut;
				}
				return f => f;
			}
		}

		#endregion

		#region Cubic

		public class Cubic
		{
			public static float EaseIn (float time)
			{
				return time * time * time;
			}

			public static float EaseOut (float time)
			{
				time--;
				return  (time * time * time + 1f);
			}

			public static float EaseInOut (float time)
			{
				time /= 0.5f;
				if (time < 1f)
					return 0.5f * time * time * time;
				time -= 2f;
				return 0.5f * (time * time * time + 2f);
			}

			public static Func<float, float> Get (EaseType easeType)
			{
				switch (easeType) {
				case EaseType.In:
					return EaseIn;
				case EaseType.Out:
					return EaseOut;
				case EaseType.InOut:
					return EaseInOut;
				}
				return f => f;
			}
		}

		#endregion

		#region Quartic

		public class Quartic
		{
			public static float EaseIn (float time)
			{
				return time * time * time * time;
			}

			public static float EaseOut (float time)
			{
				time--;
				return -(time * time * time * time - 1f);
			}

			public static float EaseInOut (float time)
			{
				time /= 0.5f;
				if (time < 1f)
					return 0.5f * time * time * time * time;
				time -= 2f;
				return -0.5f * (time * time * time * time - 2f);
			}

			public static Func<float, float> Get (EaseType easeType)
			{
				switch (easeType) {
				case EaseType.In:
					return EaseIn;
				case EaseType.Out:
					return EaseOut;
				case EaseType.InOut:
					return EaseInOut;
				}
				return f => f;
			}
		}

		#endregion

		#region Quintic

		public class Quintic
		{
			public static float EaseIn (float time)
			{
				return time * time * time * time * time;
			}

			public static float EaseOut (float time)
			{
				time--;
				return (time * time * time * time * time + 1f);
			}

			public static float EaseInOut (float time)
			{
				time /= 0.5f;
				if (time < 1f)
					return 0.5f * time * time * time * time * time;
				time -= 2f;
				return 0.5f * (time * time * time * time * time + 2f);
			}

			public static Func<float, float> Get (EaseType easeType)
			{
				switch (easeType) {
				case EaseType.In:
					return EaseIn;
				case EaseType.Out:
					return EaseOut;
				case EaseType.InOut:
					return EaseInOut;
				}
				return f => f;
			}
		}

		#endregion

		#region Sin

		public class Sin
		{
			public static float EaseIn (float time)
			{
				return -Mathf.Cos ((time) * (Mathf.PI / 2)) + 1f;
			}

			public static float EaseOut (float time)
			{
				return Mathf.Sin ((time) * (Mathf.PI / 2f));
			}

			public static float EaseInOut (float time)
			{
				return -Mathf.Cos (time * Mathf.PI) / 2f + 0.5f;
			}

			public static Func<float, float> Get (EaseType easeType)
			{
				switch (easeType) {
				case EaseType.In:
					return EaseIn;
				case EaseType.Out:
					return EaseOut;
				case EaseType.InOut:
					return EaseInOut;
				}
				return f => f;
			}
		}

		#endregion

		#region Exponential

		public class Exponential
		{
			public static float EaseIn (float time)
			{
				return Mathf.Pow (2f, 10f * (time - 1f));
			}

			public static float EaseOut (float time)
			{
				return (-Mathf.Pow (2f, -10f * time) + 1f);
			}

			public static float EaseInOut (float time)
			{
				time /= 0.5f;
				if (time < 1f)
					return 0.5f * Mathf.Pow (2f, 10f * (time - 1f));
				time--;
				return 0.5f * (-Mathf.Pow (2f, -10f * time) + 2f);
			}

			public static Func<float, float> Get (EaseType easeType)
			{
				switch (easeType) {
				case EaseType.In:
					return EaseIn;
				case EaseType.Out:
					return EaseOut;
				case EaseType.InOut:
					return EaseInOut;
				}
				return f => f;
			}
		}

		#endregion

		#region Circular

		public class Circular
		{
			public static float EaseIn (float time)
			{
				return -0.5f * (Mathf.Sqrt (1f - time * time) - 1f);
			}

			public static float EaseOut (float time)
			{
				time--;
				return 0.5f * Mathf.Sqrt (1f - time * time);
			}

			public static float EaseInOut (float time)
			{
				time /= 0.5f;
				if (time < 1f)
					return -0.25f * (Mathf.Sqrt (1f - time * time) - 1f);
				time -= 2f;
				return 0.25f * (Mathf.Sqrt (1f - time * time) + 1f);
			}

			public static Func<float, float> Get (EaseType easeType)
			{
				switch (easeType) {
				case EaseType.In:
					return EaseIn;
				case EaseType.Out:
					return EaseOut;
				case EaseType.InOut:
					return EaseInOut;
				}
				return f => f;
			}
		}

		#endregion

		#region Bounce

		public class Bounce
		{
			public static float EaseIn (float time)
			{
				time = (1f - time);
				if (time < (1f / 2.75f)) {
					return 1f - (7.5625f * time * time);
				}
				if (time < (2f / 2.75f)) {
					time -= 1.5f / 2.75f;
					return 1f - (7.5625f * time * time + 0.75f);
				}
				if (time < (2.5f / 2.75f)) {
					time -= 2.25f / 2.75f;
					return 1f - (7.5625f * time * time + 0.9375f);
				}
				time -= 2.625f / 2.75f;
				return 1f - (7.5625f * time * time + 0.984375f);
			}

			public static float EaseOut (float time)
			{
				if (time < (1f / 2.75f)) {
					return 7.5625f * time * time;
				}
				if (time < (2f / 2.75f)) {
					time -= 1.5f / 2.75f;
					return 7.5625f * time * time + 0.75f;
				}
				if (time < (2.5f / 2.75f)) {
					time -= 2.25f / 2.75f;
					return 7.5625f * time * time + 0.9375f;
				}
				time -= 2.625f / 2.75f;
				return 7.5625f * time * time + 0.984375f;
			}

			public static float EaseInOut (float time)
			{
				if (time < 0.5f)
					return EaseIn (2f * time) * 0.5f;
				return 0.5f * EaseOut (2f * time - 1f) + 0.5f;
			}

			public static Func<float, float> Get (EaseType easeType)
			{
				switch (easeType) {
				case EaseType.In:
					return EaseIn;
				case EaseType.Out:
					return EaseOut;
				case EaseType.InOut:
					return EaseInOut;
				}
				return f => f;
			}
		}

		#endregion

		#region Elastic

		public class Elastic
		{
			public static float EaseIn (float time)
			{
				if (time <= 0f)
					return 0f;
				if (time >= 1f)
					return 1f;
				time -= 1f;
				return -Mathf.Pow (2f, 10f * time) * Mathf.Sin ((time - 0.1f) * (2f * Mathf.PI) / 0.4f);
			}

			public static float EaseOut (float time)
			{
				if (time <= 0f)
					return 0f;
				if (time >= 1f)
					return 1f;
				return Mathf.Pow (2f, -10f * time) * Mathf.Sin ((time - 0.1f) * (2f * Mathf.PI) / 0.4f) + 1f;
			}

			public static float EaseInOut (float time)
			{
				time /= 0.5f;
				if (time < 1f) {
					time -= 1f;
					return -0.5f * Mathf.Pow (2f, 10f * time) * Mathf.Sin ((time - 0.1f) * (2f * Mathf.PI) / 0.4f);
				}
				time -= 1f;
				return Mathf.Pow (2f, -10f * time) * Mathf.Sin ((time - 0.1f) * (2f * Mathf.PI) / 0.4f) * 0.5f + 1f;
			}

			public static Func<float, float> Get (EaseType easeType)
			{
				switch (easeType) {
				case EaseType.In:
					return EaseIn;
				case EaseType.Out:
					return EaseOut;
				case EaseType.InOut:
					return EaseInOut;
				}
				return f => f;
			}
		}

		#endregion

		#region Back

		public class Back
		{
			const float s1 = 1.70158f;
			const float s2 = 2.5949095f;

			public static float EaseIn (float time)
			{
				return time * time * ((s1 + 1f) * time - s1);
			}

			public static float EaseOut (float time)
			{
				time -= 1f;
				return time * time * ((s1 + 1f) * time + s1) + 1f;
			}

			public static float EaseInOut (float time)
			{
				time /= 0.5f;
				if (time < 1f)
					return 0.5f * (time * time * ((s2 + 1f) * time - s2));
				time -= 2f;
				return 0.5f * (time * time * ((s2 + 1f) * time + s2) + 2f);
			}

			public static Func<float, float> Get (EaseType easeType)
			{
				switch (easeType) {
				case EaseType.In:
					return EaseIn;
				case EaseType.Out:
					return EaseOut;
				case EaseType.InOut:
					return EaseInOut;
				}
				return f => f;
			}
		}

		#endregion
	}
}