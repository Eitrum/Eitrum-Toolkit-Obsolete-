using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eitrum;

public class EitrumComponentCubeScript : EiComponent
{
	float time = 0f;

	Vector3 offset = Vector3.zero;
	Vector3 spawnPosition = Vector3.zero;
	Transform target;

	void Awake ()
	{
		target = transform;
		spawnPosition = target.localPosition;
		SubscribeThreadedUpdate ();
		SubscribeFixedUpdate ();
	}

	public override void FixedUpdateComponent (float time)
	{
		target.localPosition = spawnPosition + offset;
	}

	public override void ThreadedUpdateComponent (float time)
	{
		this.time += time;
		offset = new Vector3 (Mathf.Sin (this.time), Mathf.Cos (this.time), Mathf.PerlinNoise (this.time, Mathf.Sin (this.time)));
	}
}
