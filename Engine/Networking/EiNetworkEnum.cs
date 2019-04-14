using System;

namespace Eitrum.Networking
{
	public enum EiNetworkType
	{
        None,
		Singleplayer,
		Photon,
		Other
	}

	public enum EiNetworkTarget
	{
		All,
		Others,
		Server
	}

	public enum EiNetworkInstantiateMask
	{
		None = 0,
		Position = 1,
		Rotation = 2,
		Scale = 4,
		Parent = 8,
		Scale3D = 16,

		PositionRotation = Position | Rotation,
		PositionRotationParent = Position | Rotation | Parent,
		PositionRotationScale = Position | Rotation | Scale,
		PositionRotationScaleParent = Position | Rotation | Scale | Parent,
		PositionRotationScale3D = Position | Rotation | Scale3D,
		PositionRotationScale3DParent = Position | Rotation | Scale3D | Parent
	}

	public enum EiNetworkLogLevel
	{
		None,
		Everything,
		ErrorsOnly
	}
}