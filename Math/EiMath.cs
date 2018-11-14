using System;
using UnityEngine;

namespace Eitrum.Mathematics
{
	public class EiMath
	{
		public static Vector3 GetClosestPointOnLine (Line line, Vector3 point)
		{
			var p = line.StartReference - point;
			var delta = line.Direction;

			float t = (((-p.x * delta.x) + (-p.y * delta.y) + (-p.z * delta.z)) / (delta.x * delta.x + delta.y * delta.y + delta.z * delta.z));
			t /= line.Length;
			return line.GetPointFromReference (t);
		}

		public static float GetValueFromPointOnLine (Line line, Vector3 point)
		{
			var p = line.StartReference - point;
			var delta = line.Direction;

			float t = (((-p.x * delta.x) + (-p.y * delta.y) + (-p.z * delta.z)) / (delta.x * delta.x + delta.y * delta.y + delta.z * delta.z));
			t /= line.Length;
			return t;
		}

	}
}
