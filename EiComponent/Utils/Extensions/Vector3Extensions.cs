using System;
using UnityEngine;

namespace Eitrum
{
	public static class Vector3Extensions
	{
		public static Vector3 Normalized (this Vector3 vec, float minDistance)
		{
			if (vec.magnitude < minDistance)
				return vec;
			return vec.normalized;
		}

		public static Vector3 ClampMagnitude(this Vector3 vec, float maxDistance)
		{
			return Vector3.ClampMagnitude(vec, maxDistance);
		}

		public static Vector3 ClampMagnitudeXZ (this Vector3 vec, float maxDistance)
		{
			var y = vec.y;
			vec.y = 0f;
			vec = Vector3.ClampMagnitude (vec, maxDistance);
			vec.y = y;
			return vec;
		}

		public static Vector3 ToVector3_XZ (this Vector3 vec)
		{
			vec.y = 0;
			return vec;
		}

		public static Vector2 ToVector2_XY (this Vector3 vec)
		{
			return new Vector2 (vec.x, vec.y);
		}

		public static Vector2 ToVector2_XZ (this Vector3 vec)
		{
			return new Vector2 (vec.x, vec.z);
		}
	}
}

