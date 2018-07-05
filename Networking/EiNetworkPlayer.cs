using System;
using UnityEngine;

namespace Eitrum.Networking
{
	public class EiNetworkPlayer : EiCore, EiBufferInterface
	{
		#region Variables

		[SerializeField]
		private string playerName = "";
		[SerializeField]
		private int playerId = -1;

		#endregion

		#region Properties

		public string PlayerName {
			get {
				return playerName;
			}
		}

		public int PlayerID {
			get {
				return playerId;
			}
		}

		public bool IsMine {
			get {
				return false;
			}
		}

		#endregion

		#region EiBufferInterface implementation

		public void WriteTo (EiBuffer buffer)
		{
			buffer.WriteUTF32 (playerName);
			buffer.Write (playerId);
		}

		public void ReadFrom (EiBuffer buffer)
		{
			playerName = buffer.ReadUTF32 ();
			playerId = buffer.ReadInt ();
		}

		#endregion
	}
}