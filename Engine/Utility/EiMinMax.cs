using System;
using UnityEngine;
using Eitrum.Mathematics;

namespace Eitrum
{
	[Serializable]
	public struct EiMinMaxFloat
	{
		#region Variables

		[SerializeField]
		private float minValue;
		[SerializeField]
		private float maxValue;

		#endregion

		#region Properties

		public float MinValue
		{
			get
			{
				return minValue;
			}
		}

		public float MaxValue
		{
			get
			{
				return maxValue;
			}
		}

		public float Difference
		{
			get
			{
				return maxValue - minValue;
			}
		}

		public float RandomValue
		{
			get
			{
				return EiRandom.Range(minValue, maxValue);
			}
		}

		public static EiMinMaxFloat Default
		{
			get
			{
				return new EiMinMaxFloat(0f, 1f);
			}
		}

		#endregion

		#region Constructors

		public EiMinMaxFloat(float min, float max)
		{
			minValue = min;
			maxValue = max;
		}

		public EiMinMaxFloat(EiMinMaxFloat reference)
		{
			minValue = reference.minValue;
			maxValue = reference.maxValue;
		}

		#endregion

		#region Core

		public float GetValue(float time)
		{
			return Mathf.Lerp(minValue, maxValue, time);
		}

		public float GetRandomValue()
		{
			return RandomValue;
		}

		public float GetRandomValue(EiRandom random)
		{
			return random._Range(minValue, maxValue);
		}

		#endregion
	}
}

