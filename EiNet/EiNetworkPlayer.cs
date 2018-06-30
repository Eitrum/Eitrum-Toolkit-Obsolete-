using System;

namespace Eitrum.EiNet
{
	public class EiNetworkPlayer : EiCore, EiBufferInterface
	{
		#region Variables

		public string playerName = "";
		public int playerId = -1;

		#endregion

		#region Properties

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