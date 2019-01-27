using System;
using UnityEngine;

namespace Eitrum {
	public static class QuaternionExtensions {
		public static Vector4 ToVector4(this Quaternion quat) {
			return new Vector4(quat.x, quat.y, quat.z, quat.w);
		}

		public static float GetAngle(this Quaternion quaternion) {
			return Quaternion.Angle(Quaternion.identity, quaternion);
		}

		public static Vector3 ToAngularVelocity(this Quaternion quaternion) {
			Vector3 axis = Vector3.zero;
			float angle = 0f;
			quaternion.ToAngleAxis(out angle, out axis);
			return axis * (angle * Mathf.Deg2Rad);
		}
	}
}

