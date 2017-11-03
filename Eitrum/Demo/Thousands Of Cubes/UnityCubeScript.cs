using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCubeScript : MonoBehaviour
{
	Transform target;
	Vector3 offset = new Vector3 ();

	void Awake ()
	{
		this.target = transform;
		this.offset = target.localPosition;
	}

	void FixedUpdate()
	{
		target.localPosition = offset + new Vector3 (Mathf.Sin (Time.time), Mathf.Cos (Time.time), Mathf.PerlinNoise (Time.time, Mathf.Sin (Time.time)));
	}
}
