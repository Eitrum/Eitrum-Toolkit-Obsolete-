using System;
using UnityEngine;

namespace Eitrum
{
	public static class Vector4Extensions
	{
		public static Quaternion ToQuaternion (this Vector4 vec)
		{
			return new Quaternion (vec.x, vec.y, vec.z, vec.w);
		}

        public static Vector3 ToVector3(this Vector4 vec)
        {
            return new Vector3(vec.x, vec.y, vec.z);
        }
	}
}

