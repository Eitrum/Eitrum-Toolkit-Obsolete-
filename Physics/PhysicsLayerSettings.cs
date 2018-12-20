using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.PhysicsExtension
{
	[CreateAssetMenu (fileName = "Physics Layer Settings", menuName = "Eitrum/Physics/Physics Layer Settings")]
	public class PhysicsLayerSettings : EiScriptableObject
	{
		#region Variables

		[SerializeField] private string[] customLayerName = new string[32];
		[SerializeField] private int[] layerMask = new int[32];

		#endregion

		#region Conversions

		public string LayerToName (int index)
		{
			if (index >= 32 || index < 0) {
				throw new System.IndexOutOfRangeException ("Layer Index is out of bounds: " + index);
			}
			string value = "";
			if (index < customLayerName.Length) {
				value = customLayerName [index];
			}
			if (string.IsNullOrEmpty (value)) {
				value = LayerMask.LayerToName (index);
			}
			return value;
		}

		public int NameToLayer (string name)
		{
			for (int i = 0; i < customLayerName.Length; i++) {
				if (customLayerName [i] == name)
					return i;
			}
			return LayerMask.NameToLayer (name);
		}

		#endregion

		#region Helper

		public void SafetySecureLayerMasks ()
		{
			if (layerMask == null) {
				layerMask = new int[32];
			}
			if (layerMask.Length != 32) {
				System.Array.Resize (ref layerMask, 32);
			}
			if (customLayerName == null) {
				customLayerName = new string[32];
			}
			if (customLayerName.Length != 32) {
				System.Array.Resize (ref customLayerName, 32);
			}
		}

		public bool HasLayer (int layerIndex)
		{
			return LayerToName (layerIndex) != "";
		}

		public bool HasCustomLayerName (int layerIndex)
		{
			if (customLayerName.Length > layerIndex) {
				return customLayerName [layerIndex] != "";
			}
			return false;
		}

            		#endregion

		#region Get / Set Ignore Collision

		public void IgnoreLayerCollision (int layer1, int layer2, bool ignore)
		{
			if (!ignore) {
				layerMask [layer1] &= ~(1 << layer2);
				layerMask [layer2] &= ~(1 << layer1);
			} else {
				layerMask [layer1] |= (1 << layer2);
				layerMask [layer2] |= (1 << layer1);
			}
		}

		public bool GetIgnoreLayerCollision (int layer1, int layer2)
		{
			var val = layerMask [layer1];
			var comp = (1 << layer2);
			return (val & comp) == comp;
		}

		#endregion

		#region Apply / Load

		[ContextMenu ("Apply Layer Settings")]
		public void ApplyLayerSettings ()
		{
			for (int i = 0; i < 32; i++) {
				for (int j = 0; j < 32 - i; j++) {
					Physics.IgnoreLayerCollision (i, j, GetIgnoreLayerCollision (i, j));
				}
			}
		}

		[ContextMenu ("Load Layer Settings")]
		public void LoadLayerSettings ()
		{
			for (int i = 0; i < 32; i++) {
				for (int j = 0; j < 32 - i; j++) {
					IgnoreLayerCollision (i, j, Physics.GetIgnoreLayerCollision (i, j));
				}
			}
		}

		#endregion
	}
}