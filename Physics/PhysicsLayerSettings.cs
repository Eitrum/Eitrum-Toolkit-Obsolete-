using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.PhysicsExtension
{
	[CreateAssetMenu (fileName = "Physics Layer Settings", menuName = "Eitrum/Physics/Physics Layer Settings")]
	public class PhysicsLayerSettings : EiScriptableObject
	{
		[SerializeField] private string[] customLayerName = new string[0];
		[SerializeField] public int[] layerMask = new int[32];

		public string GetLayerName (int index)
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

		[ContextMenu ("Apply Layer Settings")]
		public void ApplyLayerSettings ()
		{
			for (int i = 0; i < 32; i++) {
				var value = layerMask [i];
				for (int j = 0; j < 32 - i; j++) {
					var bo = (value & 1 << j) == 1 << j;
					Physics.IgnoreLayerCollision (i, 31 - j, !bo);
				}
			}
		}

		[ContextMenu ("Load Layer Settings")]
		public void LoadLayerSettings ()
		{
			for (int i = 0; i < 32; i++) {
				var value = 0;
				for (int j = 0; j < 32 - i; j++) {
					value |= Physics.GetIgnoreLayerCollision (i, 31 - j) ? 0 : 1 << j;
				}
				layerMask [i] = value;
			}
		}
	}
}