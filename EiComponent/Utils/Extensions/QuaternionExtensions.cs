using System;
using UnityEngine;

namespace Eitrum
{
	public static class QuaternionExtensions
	{
		public static Vector4 ToVector4 (this Quaternion quat)
		{
			return new Vector4 (quat.x, quat.y, quat.z, quat.w);
		}
	}
}

