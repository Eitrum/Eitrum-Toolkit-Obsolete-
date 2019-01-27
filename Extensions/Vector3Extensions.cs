using System;
using UnityEngine;

namespace Eitrum
{
	public static class Vector3Extensions
	{
		#region Normalizing clamping

		public static Vector3 Normalized (this Vector3 vec, float minDistance)
		{
			if (vec.magnitude < minDistance)
				return vec;
			return vec.normalized;
		}

		public static Vector3 ClampMagnitude (this Vector3 vec, float maxDistance)
		{
			return Vector3.ClampMagnitude (vec, maxDistance);
		}

		public static Vector3 ClampMagnitudeXZ (this Vector3 vec, float maxDistance)
		{
			var y = vec.y;
			vec.y = 0f;
			vec = Vector3.ClampMagnitude (vec, maxDistance);
			vec.y = y;
			return vec;
		}

		#endregion

		#region Conversion

		public static Vector3 ToVector3_XZ (this Vector3 vec)
		{
			vec.y = 0;
			return vec;
		}

		public static Vector3 ToVector3_XY (this Vector3 vec)
		{
			vec.z = 0;
			return vec;
		}

		public static Vector3 ToVector3_YZ (this Vector3 vec)
		{
			vec.x = 0;
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

		public static Vector2 ToVector2_YZ (this Vector3 vec)
		{
			return new Vector2 (vec.y, vec.z);
		}

		public static Vector3Int ToVector3Int (this Vector3 vec)
		{
			return new Vector3Int (vec.x.RoundInt (), vec.y.RoundInt (), vec.z.RoundInt ());
		}

		#endregion

		#region Snap

		public static Vector3 Snap (this Vector3 vector)
		{
			return new Vector3 (vector.x.Round (), vector.y.Round (), vector.z.Round ());
		}

		public static Vector3 Snap (this Vector3 vector, float snapValue)
		{
			if (snapValue == 0f) {
				return vector;
			}
			return Snap (vector / snapValue) * snapValue;
		}

		#endregion

		#region Scale

		public static Vector3 ScaleReturn (this Vector3 target, Vector3 scale)
		{
			target.Scale (scale);
			return target;
		}

		#endregion
	}
}