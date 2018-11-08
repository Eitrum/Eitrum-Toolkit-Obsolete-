using System;

namespace Eitrum.Networking
{
	public abstract class EiNetworkPlayer
	{
		#region Variables

		protected string playerName = "";
		protected int playerId;
		protected Action<EiNetworkPlayer> onUpdate;
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

		#region Subscribe

		public void Subscribe (Action<EiNetworkPlayer> method)
		{
			onUpdate += method;
		}

		public void Unsubscribe (Action<EiNetworkPlayer> method)
		{
			if (onUpdate != null) {
				onUpdate -= method;
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
				onUpdate?.Invoke (this);
			}
		}

		public new int Id {
			get {
				return playerId;
			}
			set {
				playerId = value;
				onUpdate?.Invoke (this);
			}
		}

		#endregion

		#region Constructor

		public EiNetworkPlayerInternal (EiNetwork network, string name, int id)
		{
			this.network = network;
			this.playerName = name;
			this.playerId = id;
		}

		#endregion
	}
}