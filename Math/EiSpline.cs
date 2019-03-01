using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Mathematics
{
	[Serializable]
	public class EiSpline
	{
		#region Variables

		[SerializeField]
		private bool loop = false;
		[SerializeField]
		private bool freeHandle = false;

		[SerializeField]
		private List<EiBezier> bezierCurves = new List<EiBezier> ();

		#endregion

		#region Properties

		public int CurveCount { get { return bezierCurves.Count; } }

		public EiBezier this [int index]{ get { return bezierCurves [index]; } set { bezierCurves [index] = value; } }

		public Vector3 this [int index, float time]{ get { return bezierCurves [index].Evaluate (time); } }

		public Vector3 this [float time]{ get { return this.Evaluate (time); } }

		public bool IsLooping { get { return loop; } }

		public bool IsFreeHandle{ get { return freeHandle; } }

		public static EiSpline Default {
			get {
				return new EiSpline () {
					bezierCurves = new List<EiBezier> () {
						new EiBezier (
							new Vector3 (0f, 0f, 0f),
							new Vector3 (1f, 0f, 0f),
							new Vector3 (2f, -1f, 0f),
							new Vector3 (3f, 0f, 0f)),
						new EiBezier (
							new Vector3 (3f, 0f, 0f),
							new Vector3 (4f, 1f, 0f),
							new Vector3 (5f, 0f, 0f),
							new Vector3 (6f, 0f, 0f)),
					}
				};
			}
		}

		#endregion

		#region Constructor

		public EiSpline ()
		{
			
		}

		public EiSpline (bool loop)
		{
			this.loop = loop;
			if (loop) {
				// Generate loop
			}
		}

		public EiSpline (EiBezier[] bezierCurves, bool loop) : this (loop)
		{
			this.freeHandle = true;
			this.bezierCurves.AddRange (bezierCurves);
		}

		#endregion

		#region Core

		public Vector3 Evaluate (float t)
		{
			if (loop)
				t %= 1f;
			if (t >= 1f) // returns the end point of the last curve
				return bezierCurves [CurveCount - 1] [3];
			if (t < 0f) // return the start point of the first curve
				return bezierCurves [0] [0];
			var val = t * (float)CurveCount;
			var rest = val % 1f;
			return bezierCurves [(int)val].Evaluate (rest);
		}

		public Vector3 Evaluate (Transform offset, float t)
		{
			if (loop)
				t %= 1f;
			if (t >= 1f) // returns the end point of the last curve
				return offset.position + offset.rotation * bezierCurves [CurveCount - 1] [3].ScaleReturn (offset.lossyScale);
			if (t < 0f) // return the start point of the first curve
				return offset.position + offset.rotation * bezierCurves [0] [0].ScaleReturn (offset.lossyScale);
			var val = t * (float)CurveCount;
			var rest = val % 1f;
			return offset.position + offset.rotation * bezierCurves [(int)val].Evaluate (rest).ScaleReturn (offset.lossyScale);
		}

		#endregion

		#region Help Methods

		public void SetLooping ()
		{
			if (loop)
				return;
			loop = true;
			var firstCurve = this [0];
			var lastCurve = this [CurveCount - 1];
			bezierCurves.Add (new EiBezier (lastCurve [3], lastCurve [3] + (lastCurve [3] - lastCurve [2]), firstCurve [0] + (firstCurve [0] - firstCurve [1]), firstCurve [0]));
		}

		#endregion

		#region Editor

		public void DrawGizmos (float drawScale, Vector3 position, Quaternion rotation)
		{
			for (int i = 0; i < bezierCurves.Count; i++) {
				bezierCurves [i].DrawGizmos (drawScale, position, rotation);
			}
		}

		public void DrawGizmos (float drawScale, Vector3 position, Quaternion rotation, float time)
		{
			for (int i = 0; i < bezierCurves.Count; i++) {
				bezierCurves [i].DrawGizmos (drawScale, position, rotation);
			}
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere (position + rotation * this.Evaluate (time), drawScale / 4f);
			Gizmos.color = Color.white;
		}

		#endregion
	}
}