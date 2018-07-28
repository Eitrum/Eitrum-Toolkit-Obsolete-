using System;
using UnityEngine;

namespace Eitrum.Networking
{
	public class EiNetworkPlayer : EiBufferInterface
	{
		#region Variables

		[SerializeField]
		private string playerName = "";
		[SerializeField]
		private int playerId = -1;

		#endregion

		#region Properties

		public string PlayerName
		{
			get
			{
				return playerName;
			}
		}

		public int PlayerID
		{
			get
			{
				return playerId;
			}
		}

		#region Local Owner Check

		public bool IsMine
		{
			get
			{
				return IsLocal;
			}
		}

		public bool IsLocal
		{
			get
			{
				return EiNetwork.LocalPlayer.PlayerID == PlayerID;
			}
		}

		public bool IsOwner
		{
			get
			{
				return IsLocal;
			}
		}

		#endregion

		#region Static 

		public static EiNetworkPlayer LocalPlayer
		{
			get
			{
				return EiNetwork.LocalPlayer;
			}
		}

		#endregion

		#endregion

		#region Constructors

		public EiNetworkPlayer()
		{
			playerName = "";
			playerId = -1;
		}

		public EiNetworkPlayer(string name)
		{
			playerName = name;
			playerId = -1;
		}

		public EiNetworkPlayer(int id)
		{
			playerName = "";
			playerId = id;
		}

		public EiNetworkPlayer(string name, int id)
		{
			playerName = name;
			playerId = id;
		}

		#endregion

		#region Core

		public void Assign(string name, int id)
		{
			playerName = name;
			playerId = id;
		}

		public void AssignName(string name)
		{
			playerName = name;
		}

		public void AssignId(int id)
		{
			playerId = id;
		}

		#endregion

		#region EiBufferInterface implementation

		public void WriteTo(EiBuffer buffer)
		{
			buffer.WriteUTF32(playerName);
			buffer.Write(playerId);
		}

		public void ReadFrom(EiBuffer buffer)
		{
			playerName = buffer.ReadUTF32();
			playerId = buffer.ReadInt();
		}

		#endregion

		#region Equals Override

		public override bool Equals(object obj)
		{
			var other = obj as EiNetworkPlayer;
			if (other == null)
				return false;
			
			return PlayerID == other.PlayerID;
		}

		public override int GetHashCode()
		{
			return playerId.GetHashCode();
		}

		public static bool operator ==(EiNetworkPlayer player1, EiNetworkPlayer player2)
		{
			if (object.ReferenceEquals(player1, null))
			{
				return object.ReferenceEquals(player2, null);
			}
			return player1.PlayerID == player2.PlayerID;
		}

		public static bool operator !=(EiNetworkPlayer player1, EiNetworkPlayer player2)
		{
			return !(player1 == player2);
		}

		#endregion
	}
}