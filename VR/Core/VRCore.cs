using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Eitrum.VR {
	/// <summary>
	/// A utlity class for easier workflow in the editor and testing, tracking information
	/// </summary>
	[AddComponentMenu("Eitrum/VR/VRCore")]
	public class VRCore : EiComponent {

		#region Variables

		private static List<XRNode> trackedNodes = new List<XRNode>();

		#endregion

		#region Core

		private void Awake() {
			InputTracking.trackingAcquired += OnTrackingAcquired;
			InputTracking.trackingLost += OnTrackingLost;

			List<XRNodeState> nodeStates = new List<XRNodeState>();
			InputTracking.GetNodeStates(nodeStates);
			for (int i = 0; i < nodeStates.Count; i++) {
				if (nodeStates[i].tracked && !trackedNodes.Contains(nodeStates[i].nodeType)) {
					trackedNodes.Add(nodeStates[i].nodeType);
				}
			}
		}

		#endregion

		#region Tracking

		public static bool HasTracking(XRNode node) {
			return trackedNodes.Contains(node);
		}

		void OnTrackingAcquired(XRNodeState nodeState) {
			trackedNodes.Add(nodeState.nodeType);
		}

		void OnTrackingLost(XRNodeState nodeState) {
			trackedNodes.Remove(nodeState.nodeType);
		}

		#endregion

		#region Recenter

		public static void RecenterTracking() {
			InputTracking.Recenter();
		}

		[ContextMenu("Recenter")]
		public void Recenter() {
			InputTracking.Recenter();
		}

		#endregion
	}
}
