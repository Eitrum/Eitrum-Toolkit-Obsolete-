using System;
using UnityEngine.Events;
using UnityEngine;
using Eitrum;
using Eitrum.Engine.Core;

namespace Eitrum.Engine.Utility
{
	[Serializable]
	public class UnityEventEiEntity : UnityEvent<EiEntity>
	{
	}

	[Serializable]
	public class UnityEventCollider : UnityEvent<Collider>
	{
	}

	[Serializable]
	public class UnityEventInt : UnityEvent<int>
	{
	}

	[Serializable]
	public class UnityEventString : UnityEvent<string>
	{
	}

	[Serializable]
	public class UnityEventFloat : UnityEvent<float>
	{
	}

	[Serializable]
	public class UnityEventVector3 : UnityEvent<Vector3>
	{
	}

	[Serializable]
	public class UnityEventVector2 : UnityEvent<Vector2>
	{
	}

	[Serializable]
	public class UnityEventQuaternion : UnityEvent<Quaternion>
	{
	}
}