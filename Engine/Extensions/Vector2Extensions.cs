using System;
using UnityEngine;

namespace Eitrum
{
	public static class Vector2Extensions
	{

		#region 2D rotation

		public static Quaternion ToRotationZ (this Vector2 vec)
		{
			if (vec.sqrMagnitude <= Mathf.Epsilon)
				return Quaternion.identity;
			return Quaternion.Euler (0f, 0f, -Mathf.Atan2 (vec.x, vec.y) * Mathf.Rad2Deg);
		}

		#endregion

		#region Conversion

		public static Vector3 ToVector3_XZ (this Vector2 vec)
		{
			return new Vector3 (vec.x, 0, vec.y);
		}

		public static Vector2Int ToVector2Int (this Vector2 vec)
		{
			return new Vector2Int (vec.x.RoundInt (), vec.y.RoundInt ());
		}

		#endregion

		#region Snap

		public static Vector2 Snap (this Vector2 vector)
		{
			return new Vector2 (vector.x.Round (), vector.y.Round ());
		}

		public static Vector2 Snap (this Vector2 vector, float snapValue)
		{
			if (snapValue <= Mathf.Epsilon) {
				return vector;
			}
			return Snap (vector / snapValue) * snapValue;
		}

		#endregion
	}
}

