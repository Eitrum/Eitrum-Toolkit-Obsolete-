using System;
using UnityEngine.Events;
using UnityEngine;
using Eitrum;

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