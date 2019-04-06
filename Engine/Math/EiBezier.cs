using System;
using UnityEngine;

namespace Eitrum.Mathematics
{
	[Serializable]
	public unsafe struct EiBezier
	{
		#region Variables

		public Vector3 startPoint;
		public Vector3 startHandle;

		public Vector3 endHandle;
		public Vector3 endPoint;

		#endregion

		#region Properties

		public Vector3 this [int index] {
			get {
				#if !EITRUM_PERFORMANCE_MODE
				if (index < 0 || index >= 4) {
					throw new IndexOutOfRangeException (string.Format ("Bezier Index has value of {0} while the bounds are between 0-3", index));
				}
				#endif
				fixed(Vector3* p = (&startPoint)) {
					return *(p + index);
				}
			}
			set {
				#if !EITRUM_PERFORMANCE_MODE
				if (index < 0 || index >= 4) {
					throw new IndexOutOfRangeException (string.Format ("Bezier Index has value of {0} while the bounds are between 0-3", index));
				}
				#endif
				fixed(Vector3* p = (&startPoint)) {
					(*(p + index)) = value;
				}
			}
		}

		public int Length {
			get {
				return 4;
			}
		}

		public static EiBezier Default {
			get {
				return new EiBezier (
					new Vector3 (0f, 0f, 0f), 
					new Vector3 (1f, 0f, 0f), 
					new Vector3 (2f, 0f, 0f), 
					new Vector3 (3f, 0f, 0f));
			}
		}

		#endregion

		#region Constructor

		public EiBezier (Vector3 startPoint, Vector3 startHandle, Vector3 endHandle, Vector3 endPoint)
		{
			this.startPoint = startPoint;
			this.startHandle = startHandle;
			this.endHandle = endHandle;
			this.endPoint = endPoint;
		}

		public EiBezier (Transform startPoint, Transform startHandle, Transform endHandle, Transform endPoint)
		{
			this.startPoint = startPoint.position;
			this.startHandle = startHandle.position;
			this.endHandle = endHandle.position;
			this.endPoint = endPoint.position;
		}

		public EiBezier (Transform startPoint, Transform startHandle, Transform endHandle, Transform endPoint, Space space)
		{
			if (space == Space.World) {
				this.startPoint = startPoint.position;
				this.startHandle = startHandle.position;
				this.endHandle = endHandle.position;
				this.endPoint = endPoint.position;
			} else {
				this.startPoint = startPoint.localPosition;
				this.startHandle = startHandle.localPosition;
				this.endHandle = endHandle.localPosition;
				this.endPoint = endPoint.localPosition;
			}
		}

		#endregion

		#region Core

		public Vector3 Evaluate (float t)
		{
			t = Mathf.Clamp01 (t);
			float rt = 1f - t;

			return (rt * rt * rt) * startPoint
			+ (3f * rt * rt * t) * startHandle
			+ (3f * rt * t * t) * endHandle
			+ (t * t * t) * endPoint;
		}

		public Vector3 Evaluate (Transform offset, float t)
		{
			t = Mathf.Clamp01 (t);
			float rt = 1f - t;

			var p = (rt * rt * rt) * startPoint
			        + (3f * rt * rt * t) * startHandle
			        + (3f * rt * t * t) * endHandle
			        + (t * t * t) * endPoint;
			return offset.position + offset.rotation * p.ScaleReturn (offset.lossyScale);
		}

		#endregion

		#region Editor

		public void DrawGizmos (float drawScale, Vector3 position, Quaternion rotation)
		{
			Gizmos.DrawWireSphere (position + rotation * this [0], drawScale / 2f);
			Gizmos.DrawWireSphere (position + rotation * this [3], drawScale / 2f);
			Gizmos.color = Color.grey;
			Gizmos.DrawLine (position + rotation * this [0], position + rotation * this [1]);
			Gizmos.DrawLine (position + rotation * this [2], position + rotation * this [3]);
			Gizmos.DrawWireSphere (position + rotation * this [1], drawScale / 3f);
			Gizmos.DrawWireSphere (position + rotation * this [2], drawScale / 3f);
			Gizmos.color = Color.white;
		}

		public void DrawGizmos (float drawScale, Vector3 position, Quaternion rotation, float time)
		{
			Gizmos.DrawWireSphere (position + rotation * this [0], drawScale / 2f);
			Gizmos.DrawWireSphere (position + rotation * this [3], drawScale / 2f);
			Gizmos.color = Color.grey;
			Gizmos.DrawLine (position + rotation * this [0], position + rotation * this [1]);
			Gizmos.DrawLine (position + rotation * this [2], position + rotation * this [3]);
			Gizmos.DrawWireSphere (position + rotation * this [1], drawScale / 3f);
			Gizmos.DrawWireSphere (position + rotation * this [2], drawScale / 3f);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere (position + rotation * this.Evaluate (time), drawScale / 4f);
			Gizmos.color = Color.white;
		}

		#endregion
	}
}