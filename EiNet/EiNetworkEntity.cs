using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.EiNet
{
	public class EiNetworkEntity : EiComponent
	{
		#region Variables

		[Tooltip ("This is being set in runtime and should never be changed by anything else than the network engine")]
		[SerializeField]
		protected int networkId = 0;

		[SerializeField]
		protected bool hasLocalAuthority = false;

		[SerializeField]
		protected EiNetInterface[] networkComponents;

		#region Cache

		[Tooltip ("Pre cached entity size (amount of components + each component size)" +
		"\nLeaving it at -1 will cache it correctly in runtime.")]
		[SerializeField]
		protected int cachedPackageSize = -1;

		#endregion

		#endregion

		#region Properties

		public virtual int NetworkId {
			get {
				return networkId;
			}
		}

		public virtual bool HasLocalAuthority {
			get {
				return hasLocalAuthority;
			}
		}

		public virtual int Components {
			get {
				return networkComponents.Length;
			}
		}

		public virtual int EntityPackageSize {
			get {
				if (cachedPackageSize == -1) {
					cachedPackageSize = 0;
					for (int i = 0; i < networkComponents.Length; i++)
						cachedPackageSize += networkComponents [i].NetPackageSize;
				}
				return cachedPackageSize;
			}
		}

		#endregion

		#region Core

		public virtual void Awake ()
		{
			var packageSize = EntityPackageSize;
		}

		public virtual void Start ()
		{
			
		}

		public virtual void WriteTo (EiBuffer buffer)
		{
			buffer.Write (NetworkId);
			buffer.Write (EntityPackageSize);
			var comps = Components;
			for (int i = 0; i < comps; i++) {
				networkComponents [i].NetWriteTo (buffer);
			}
		}

		public virtual void ReadFrom (EiBuffer buffer)
		{
			var packageSize = buffer.ReadInt ();
			if (packageSize != EntityPackageSize) {
				buffer.Skip (packageSize);
			} else {
				var comps = Components;
				for (int i = 0; i < comps; i++) {
					networkComponents [i].NetReadFrom (buffer);
				}
			}
		}

		#endregion

		#region Add/Remove Network Components

		/// <summary>
		/// Adds the network component to the list.
		/// Very heavy operation and should not be used unless needed due to re-alloc of array.
		/// </summary>
		/// <param name="networkComponent">Network component.</param>
		public virtual void AddNetworkComponent (EiNetInterface networkComponent)
		{
			networkComponents = networkComponents.Add (networkComponent);
			cachedPackageSize = -1;
		}

		/// <summary>
		/// Removes the network component from the list.
		/// Very heavy operation and should not be used unless needed due to re-alloc of array.
		/// </summary>
		/// <param name="networkComponent">Network component.</param>
		public virtual void RemoveNetworkComponent (EiNetInterface networkComponent)
		{
			networkComponents = networkComponents.Remove (networkComponent);
			cachedPackageSize = -1;
		}

		#endregion

		#region Helpers

		public virtual void GiveAuthority (EiNetPlayerData targetPlayer)
		{
			hasLocalAuthority = false;

		}

		public virtual void RequestOwnership ()
		{

		}

		#endregion
	}
}