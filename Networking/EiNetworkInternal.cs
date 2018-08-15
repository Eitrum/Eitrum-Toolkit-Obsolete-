using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eitrum.Networking.Internal
{
	public class EiNetworkInternal
	{
		#region Variables

		public static EiNetworkRegion currentRegion = EiNetworkRegion.None;
		public static EiNetworkRoom currentRoom = null;
		public static EiNetworkLobby currentLobby = null;
		public static EiNetworkPlayer localPlayer = new EiNetworkPlayer();
		public static EiNetworkRoom[] roomsInLobby = new EiNetworkRoom[0];
		public static float serverTime = 0f;

		#endregion

		#region Bridge Functions

		public static EiNetworkInternalRequest<EiNetworkRegion> requestRegionSwitch;

		#endregion

		#region Region Switching

		public static void RequestRegionSwitch(EiNetworkRegion networkRegion)
		{
			if (currentRegion == networkRegion)
				return;

			if (requestRegionSwitch != null)
				requestRegionSwitch.Request(networkRegion, OnRegionSwitchResponse);
		}

		private static void OnRegionSwitchResponse(EiNetworkInternalResponse<EiNetworkRegion> response)
		{
			if (response.Failed)
			{
				UnityEngine.Debug.LogError(response.ErrorMessage);
				return;
			}
			var newRegion = response.Item;

			if (newRegion == EiNetworkRegion.None)
			{
				UnityEngine.Debug.LogWarning("Could not switch region");
				return;
			}

			currentRegion = newRegion;
		}

		#endregion

		public static void Publish<T>(T message) where T : EiCore
		{
			EiCore.Publish(message);
		}


	}

	public class EiNetworkInternalRequest<T>
	{
		private Action<T> request;
		private Action<EiNetworkInternalResponse<T>> response;

		public void Request(T item, Action<EiNetworkInternalResponse<T>> response)
		{
			this.response = response;
			request(item);
		}

		public void Response(EiNetworkInternalResponse<T> responseMessage)
		{
			if (this.response != null)
				this.response(responseMessage);
		}

		public EiNetworkInternalRequest(Action<T> requestMethod)
		{
			this.request = requestMethod;
		}
	}

	public class EiNetworkInternalResponse<T>
	{
		private T item;
		private bool succeded = false;
		private string errorMessage = "";

		public T Item
		{
			get
			{
				return item;
			}
		}

		public bool Succeded
		{
			get
			{
				return succeded;
			}
		}

		public bool Failed
		{
			get
			{
				return !succeded;
			}
		}

		public string ErrorMessage
		{
			get
			{
				return errorMessage;
			}
		}

		public EiNetworkInternalResponse(T item)
		{
			this.item = item;
			succeded = item != null;
			if (item == null)
				errorMessage = "Returned Item is Null";
		}

		public EiNetworkInternalResponse(string errorMessage)
		{
			item = default(T);
			succeded = false;
			this.errorMessage = errorMessage;
		}
	}
}