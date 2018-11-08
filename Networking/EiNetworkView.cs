using System;
using UnityEngine;

namespace Eitrum.Networking
{
	[AddComponentMenu ("Eitrum/Networking/Network View")]
	public class EiNetworkView : EiComponent
	{
		#region Variables

		private int viewId = 0;
		private int ownerId = 0;
		private Eitrum.Networking.Internal.EiNetworkInternal network;

		#endregion

		#region Properties

		public int ViewId {
			get {
				return viewId;
			}
		}

		public int OwnerId {
			get {
				return ownerId;
			}
		}

		public EiNetworkPlayer Owner {
			get {
				return null;
			}
		}

		#endregion

		#region Static Help

		public static EiNetworkView Find (int viewId)
		{
			return null;
		}

		public static void SetViewId (EiNetworkView view, int viewId)
		{
			view.viewId = viewId;
		}

		public static void SetOwnerId (EiNetworkView view, int ownerId)
		{
			view.ownerId = ownerId;
		}

		public static void SetNetwork (EiNetworkView view, Eitrum.Networking.Internal.EiNetworkInternal network)
		{
			view.network = network;
		}

		#endregion
	}
}