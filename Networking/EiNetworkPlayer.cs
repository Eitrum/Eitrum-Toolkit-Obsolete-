using System;

namespace Eitrum.Networking
{
	public abstract class EiNetworkPlayer
	{
		#region Variables

		protected string playerName;
		protected int playerId;
		protected EiNetwork network;

		#endregion

		#region Properties

		public string Name {
			get {
				return playerName;
			}
		}

		public int Id {
			get {
				return playerId;
			}
		}

		public bool IsLocalPlayer {
			get {
				return network.LocalPlayer == this;
			}
		}

		#endregion
	}
}

namespace Eitrum.Networking.Internal
{
	public class EiNetworkPlayerInternal : EiNetworkPlayer
	{
		#region Properties

		public new string Name {
			get {
				return playerName;
			}
			set {
				playerName = value;
			}
		}

		public new int Id {
			get {
				return playerId;
			}
			set {
				playerId = value;
			}
		}

		#endregion

		#region Constructor

		public EiNetworkPlayerInternal (EiNetwork network)
		{
			this.network = network;
		}

		#endregion
	}
}