using System;
using UnityEngine;

namespace Eitrum {
	public static class TransformExtensions {
		#region SetActive

		public static void SetActive(this Transform transform, bool value) {
			transform.gameObject.SetActive(value);
		}

		public static void SetAllActive(this Transform[] transforms, bool value) {
			for (int i = transforms.Length - 1; i >= 0; i--) {
				transforms[i].gameObject.SetActive(value);
			}
		}

		#endregion

		#region GetComponents

		public static EiEntity GetEntity(this Transform transform) {
			return transform.GetComponent<EiEntity>();
		}

		public static RectTransform GetRectTransform(this Transform transform) {
			return transform as RectTransform;
		}

		#endregion

		#region Destroy

		public static void Destroy(this Transform transform) {
			transform.Destroy(0f);
		}

		public static void Destroy(this Transform transform, float delay) {
			MonoBehaviour.Destroy(transform, delay);
		}

		public static void DestroyAllChildren(this Transform transform) {
			var childCount = transform.childCount;
			for (int i = childCount - 1; i >= 0; i--) {
				transform.GetChild(i).Destroy(0f);
			}
		}

		public static void DestroyAllChildren(this Transform transform, float delay) {
			var childCount = transform.childCount;
			for (int i = childCount - 1; i >= 0; i--) {
				transform.GetChild(i).Destroy(delay);
			}
		}

		public static void DestroyAllChildrenAfter(this Transform transform, int startIndex) {
			var childCount = transform.childCount;
			for (int i = childCount - 1; i >= startIndex; i--) {
				transform.GetChild(i).Destroy(0f);
			}
		}

		public static void DestroyAllChildrenAfter(this Transform transform, int startIndex, float delay) {
			var childCount = transform.childCount;
			for (int i = childCount - 1; i >= startIndex; i--) {
				transform.GetChild(i).Destroy(delay);
			}
		}

		public static void DestroyAllChildrenBefore(this Transform transform, int index) {
			var childCount = Math.Min(transform.childCount, index);
			for (int i = childCount - 1; i >= 0; i--) {
				transform.GetChild(i).Destroy(0f);
			}
		}

		public static void DestroyAllChildrenBefore(this Transform transform, int index, float delay) {
			var childCount = Math.Min(transform.childCount, index);
			for (int i = childCount - 1; i >= 0; i--) {
				transform.GetChild(i).Destroy(delay);
			}
		}

		public static void DestroyAllChildrenBetween(this Transform transform, int startIndex, int endIndex) {
			var childCount = Math.Min(transform.childCount, endIndex);
			for (int i = childCount - 1; i >= startIndex; i--) {
				transform.GetChild(i).Destroy(0f);
			}
		}

		public static void DestroyAllChildrenBetween(this Transform transform, int startIndex, int endIndex, float delay) {
			var childCount = Math.Min(transform.childCount, endIndex);
			for (int i = childCount - 1; i >= startIndex; i--) {
				transform.GetChild(i).Destroy(delay);
			}
		}

		#endregion

		#region RectTransform

		public static Vector3 GetCenter(this RectTransform rect) {
			return rect.rect.center;
		}

		#endregion
	}
}