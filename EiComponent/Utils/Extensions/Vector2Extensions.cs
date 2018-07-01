using System;
using UnityEngine;

namespace Eitrum
{
	public static class Vector2Extensions
	{
		public static Vector3 ToVector3_XZ (this Vector2 vec)
		{
			return new Vector3 (vec.x, 0, vec.y);
		}
	}
}

