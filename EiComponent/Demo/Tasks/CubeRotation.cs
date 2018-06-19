using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eitrum;

public class CubeRotation : EiComponent
{

	void Awake ()
	{
		SubscribeUpdate ();
	}

	public override void UpdateComponent (float time)
	{
		this.transform.Rotate (new Vector3 (45, 0) * time);
	}
}
