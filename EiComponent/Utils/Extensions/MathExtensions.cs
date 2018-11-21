using System;
using Eitrum.Mathematics;

namespace Eitrum
{
	public static class MathExtensions
	{
		#region Randomness

		public static float Randomness (this float value, float amount)
		{
			return value + EiRandom.Range (-amount, amount);
		}

		public static double Randomness (this double value, double amount)
		{
			return value + EiRandom.Range (-amount, amount);
		}

		public static int Randomness (this int value, int amount)
		{
			return value + EiRandom.Range (-amount, amount);
		}

		#endregion

		#region Round

		public static float Round (this float value)
		{
			return (float)Math.Round ((double)value);
		}

		public static int RoundInt (this float value)
		{
			return (int)Math.Round ((double)value);
		}

		#endregion

		#region Extensions

		public static float Ease (this float time, EaseFunction functionType, EaseType easeType)
		{
			return EiEase.GetEaseFunction (functionType, easeType) (time);
		}

		public static float EaseIn (this float time, EaseFunction functionType)
		{
			return EiEase.GetEaseFunction (functionType, EaseType.In) (time);
		}

		public static float EaseOut (this float time, EaseFunction functionType)
		{
			return EiEase.GetEaseFunction (functionType, EaseType.Out) (time);
		}

		public static float EaseInOut (this float time, EaseFunction functionType)
		{
			return EiEase.GetEaseFunction (functionType, EaseType.InOut) (time);
		}

		#endregion
	}
}