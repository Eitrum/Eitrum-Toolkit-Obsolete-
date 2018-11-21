using System;
using UnityEngine;

namespace Eitrum
{
	public static class RaycastExtensions
	{
		#region ToRay

		public static Ray ToRay (this Transform transform)
		{
			return new Ray (transform.position, transform.forward);
		}

		public static Ray ToRay (this Component component)
		{
			return component.transform.ToRay ();
		}

		public static Ray ToRay (this Vector3 position, Vector3 direction)
		{
			return new Ray (position, direction);
		}

		#endregion

		#region Hit

		public static bool Hit (this Ray ray)
		{
			return UnityEngine.Physics.Raycast (ray);
		}

		public static bool Hit (this Ray ray, out RaycastHit hit)
		{
			return UnityEngine.Physics.Raycast (ray, out hit);
		}

		public static bool Hit (this Ray ray, float maxDistance)
		{
			return UnityEngine.Physics.Raycast (ray, maxDistance);
		}

		public static bool Hit (this Ray ray, out RaycastHit hit, float maxDistance)
		{
			return UnityEngine.Physics.Raycast (ray, out hit, maxDistance);
		}

		public static bool Hit (this Ray ray, out RaycastHit hit, float maxDistance, int layerMask)
		{
			return UnityEngine.Physics.Raycast (ray, out hit, maxDistance, layerMask);
		}

		public static bool Hit (this Ray ray, int layerMask)
		{
			return UnityEngine.Physics.Raycast (ray, Mathf.Infinity, layerMask);
		}

		public static bool Hit (this Ray ray, float maxDistance, int layerMask)
		{
			return UnityEngine.Physics.Raycast (ray, maxDistance, layerMask);
		}

		#endregion

		#region Hit Distance

		public static float HitDistance (this Ray ray)
		{
			RaycastHit hit;
			if (UnityEngine.Physics.Raycast (ray, out hit)) {
				return hit.distance;
			}
			return -1f;
		}

		public static float HitDistance (this Ray ray, float maxDistance)
		{
			RaycastHit hit;
			if (UnityEngine.Physics.Raycast (ray, out hit, maxDistance)) {
				return hit.distance;
			}
			return -1f;
		}

		public static float HitDistance (this Ray ray, out RaycastHit hit, float maxDistance)
		{
			if (UnityEngine.Physics.Raycast (ray, out hit, maxDistance)) {
				return hit.distance;
			}
			return -1f;
		}

		public static float HitDistance (this Ray ray, out RaycastHit hit, float maxDistance, int layerMask)
		{
			if (UnityEngine.Physics.Raycast (ray, out hit, maxDistance, layerMask)) {
				return hit.distance;
			}
			return -1f;
		}

		public static float HitDistance (this Ray ray, int layerMask)
		{
			RaycastHit hit;
			if (UnityEngine.Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
				return hit.distance;
			}
			return -1f;
		}

		public static float HitDistance (this Ray ray, float maxDistance, int layerMask)
		{
			RaycastHit hit;
			if (UnityEngine.Physics.Raycast (ray, out hit, maxDistance, layerMask)) {
				return hit.distance;
			}
			return -1f;
		}

		#endregion
	}
}