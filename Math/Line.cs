using System;
using UnityEngine;

namespace Eitrum.Mathematics
{
	public struct Line
	{
		#region Variables

		private Vector3 startReference;
		private Vector3 endReference;
		private Vector3 direction;
		private float length;

		#endregion

		#region Properties

		public Vector3 Direction {
			get {
				return direction;
			}
			set {
				endReference = startReference + direction.normalized;
				RecalculateDirection ();
			}
		}

		public float Length {
			get {
				return length;
			}
		}

		public Vector3 StartReference {
			get {
				return startReference;
			}
			set {
				startReference = value;
				RecalculateDirection ();
			}
		}

		public Vector3 EndReference {
			get {
				return endReference;
			}
			set {
				endReference = value;
				RecalculateDirection ();
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Makes a line with a direction of provided value.
		/// StartReference = (0, 0, 0)
		/// EndReference = (Direction)
		/// </summary>
		/// <param name="direction"></param>
		public Line (Vector3 direction)
		{
			this.startReference = Vector3.zero;
			this.endReference = direction.normalized;
			this.direction = this.endReference;
			length = Vector3.Distance (this.startReference, this.endReference);
		}

		public Line (Vector3 startReference, Vector3 endReference)
		{
			this.startReference = startReference;
			this.endReference = endReference;
			direction = (endReference - startReference).normalized;
			length = Vector3.Distance (this.startReference, this.endReference);
		}

		#endregion

		#region Core

		private void RecalculateDirection ()
		{
			var value = (endReference - startReference);
			if (value.sqrMagnitude < Mathf.Epsilon)
				direction = Vector3.zero;
			else
				direction = value.normalized;
			length = Vector3.Distance (this.startReference, this.endReference);
		}

		public Vector3 GetPointFromReference (float percentage)
		{
			return Vector3.Lerp (startReference, endReference, percentage);
		}

		public Vector3? GetPointAtIntersectionX (float value)
		{
			var dir = Direction;
			if (dir.x == 0)
				return null;

			dir /= Mathf.Abs (dir.x);
			value -= startReference.x;
			Vector3 result = startReference;
			if (dir.x < 0)
				result += dir * -value;
			else
				result += dir * value;

			return result;
		}

		public Vector3? GetPointAtIntersectionY (float value)
		{
			var dir = Direction;
			if (dir.y == 0)
				return null;

			dir /= Mathf.Abs (dir.y);
			value -= startReference.y;
			Vector3 result = startReference;
			if (dir.y < 0)
				result += dir * -value;
			else
				result += dir * value;

			return result;
		}

		public Vector3? GetPointAtIntersectionZ (float value)
		{
			var dir = Direction;
			if (dir.z == 0)
				return null;

			dir /= Mathf.Abs (dir.z);
			value -= startReference.z;
			Vector3 result = startReference;
			if (dir.z < 0)
				result += dir * -value;
			else
				result += dir * value;

			return result;
		}

		#endregion
	}
}