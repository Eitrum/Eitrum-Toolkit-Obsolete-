using System;

namespace Eitrum.Networking
{
	public class EiNetworkRoom : EiCore
	{
		#region Variables

		private string roomName = "";
		private int currentPlayers = 0;
		private int maxPlayers = 0;
		private int password = 0;
		private bool isVisible = true;
		private bool isOpen = true;
		private string[] customRoomProperties = new string[0];

		#endregion

		#region Properties

		public string RoomName
		{
			get
			{
				return roomName;
			}
		}

		public bool CanJoin
		{
			get
			{
				return isOpen && (maxPlayers == 0 || (maxPlayers - currentPlayers > 0));
			}
		}

		public bool IsPasswordProtected
		{
			get
			{
				return password != 0;
			}
		}

		public int CurrentPlayers
		{
			get
			{
				return currentPlayers;
			}
		}

		public int MaxPlayers
		{
			get
			{
				return maxPlayers;
			}
		}

		public bool IsVisible
		{
			get
			{
				return isVisible;
			}
		}

		public bool IsOpen
		{
			get
			{
				return isOpen;
			}
		}

		public int CustomRoomPropertyLength
		{
			get
			{
				return customRoomProperties.Length;
			}
		}

		#endregion

		#region Help Methods

		public string GetCustomRoomProperty(int index)
		{
			return customRoomProperties[index];
		}

		#endregion

		#region Constructors

		public EiNetworkRoom(string name) : this(name, EiNetworkRoomOptions.Default)
		{

		}

		public EiNetworkRoom(EiNetworkRoomOptions options) : this(options.roomName, options)
		{

		}

		public EiNetworkRoom(string name, EiNetworkRoomOptions options)
		{
			roomName = name;
			maxPlayers = options.maxPlayers;
			password = options.password;
			isVisible = options.isVisible;
			isOpen = options.isOpen;
			customRoomProperties = options.customRoomProperties;
		}

		#endregion
	}

	public struct EiNetworkRoomOptions
	{
		#region Variables

		public string roomName;
		public int maxPlayers;
		public int password;
		public bool isVisible;
		public bool isOpen;
		public string[] customRoomProperties;

		#endregion

		#region Properties

		public static EiNetworkRoomOptions Default
		{
			get
			{
				return new EiNetworkRoomOptions()
				{
					roomName = "Default",
					maxPlayers = 0,
					password = 0,
					isVisible = true,
					isOpen = true,
					customRoomProperties = new string[0]
				};
			}
		}

		#endregion
	}
}