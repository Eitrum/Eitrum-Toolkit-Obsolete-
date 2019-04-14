using System;

namespace Eitrum.Networking
{
	public interface EiNetworkPlayer
	{
		#region Properties

		string Name { get; }

		int Id { get; }

		bool IsLocalPlayer{ get; }

        bool Exists { get; }

		#endregion

		#region Methods

		void Subscribe (Action<EiNetworkPlayer> method);

		void Unsubscribe (Action<EiNetworkPlayer> method);

		#endregion
	}
}

namespace Eitrum.Networking.Internal
{
	public sealed class EiNetworkPlayerInternal : EiNetworkPlayer
	{
		#region Variables

		private string playerName = "";
		private int playerId;
		private Action<EiNetworkPlayer> onUpdate;
		private EiNetwork network;

		#endregion

		#region Properties

		public string Name {
			get {
				return playerName;
			}
			set {
				playerName = value;
				onUpdate?.Invoke (this);
			}
		}

		public int Id {
			get {
				return playerId;
			}
			set {
				playerId = value;
				onUpdate?.Invoke (this);
			}
		}

		public bool IsLocalPlayer {
			get {
				return network.LocalPlayer.Id == playerId;
			}
		}

        // TODO: implement exists check
        public bool Exists { get { return true; } }

		#endregion

		#region Constructor

		public EiNetworkPlayerInternal (EiNetwork network, string name, int id)
		{
			this.network = network;
			this.playerName = name;
			this.playerId = id;
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