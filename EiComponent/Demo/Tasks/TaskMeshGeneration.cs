using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eitrum;

public class TaskMeshGeneration : EiComponent
{
	[Header ("Press Space To Generate")]
	public MeshFilter meshFilter;
	public int sleepTimeInMs = 1000;
	bool isRunning = false;

	void Awake ()
	{
		SubscribeUpdate ();
	}

	public override void UpdateComponent (float time)
	{
		if (!isRunning && Input.GetKeyDown (KeyCode.Space)) {
			isRunning = true;
			EiTask.Run (GenerateMesh, null, ApplyMesh);
		}
	}

	public Vector3[] GenerateMesh ()
	{
		Vector3[] verticies = new Vector3[4];
		verticies [0] = new Vector3 (-1, 1, 0);
		verticies [1] = new Vector3 (1, 1, 0);
		verticies [2] = new Vector3 (1, -1, 0);
		verticies [3] = new Vector3 (-1, -1, 0);

		System.Threading.Thread.Sleep (sleepTimeInMs);

		return verticies;
	}

	void ApplyMesh (Vector3[] verticies)
	{
		Mesh m = new Mesh ();
		m.name = "Hello Mesh";
		int[] n = new int[6]{ 0, 1, 2, 2, 3, 0 };

		m.vertices = verticies;
		m.triangles = n;
		m.RecalculateNormals ();
		m.RecalculateBounds ();

		if (meshFilter.mesh != null)
			Destroy (meshFilter.mesh);
		meshFilter.mesh = m;
		isRunning = false;
	}
}
