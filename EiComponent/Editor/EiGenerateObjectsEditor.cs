using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Eitrum.Movement;

namespace Eitrum
{
	public class EiGenerateObjectsEditor : Editor
	{

		[MenuItem ("Eitrum/Generate Simple Character")]
		public static GameObject GenerateSimpleCharacter ()
		{
			var go = new GameObject ("Basic Character");

			// Basic Unity Components
			var rigid = go.AddComponent<Rigidbody> ();
			rigid.constraints = RigidbodyConstraints.FreezeRotation;
			rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			go.AddComponent<AudioSource> ();
			var cap = go.AddComponent<CapsuleCollider> ();
			cap.height = 1.8f;
			cap.radius = 0.3f;
			cap.center = new Vector3 (0, 0.9f);
			// Adding basic entity and input components
			var ent = go.AddComponent<EiEntity> ();
			ent.EntityName = "Basic Character";
			ent.Collider = cap;
			go.AddComponent<EiInput> ();

			//Adding Movemnent
			var movement = go.AddComponent<EiBasicMovement> ();
			var feet = new GameObject ("Feets");
			feet.transform.SetParent (go.transform);
			feet.transform.localPosition = new Vector3 (0, 0.1f);
			movement.SetRayCastDirection (Vector3.down);
			movement.SetRayCastDistance (0.2f);
			movement.SetRayCastTransform (feet.transform);
			go.AddComponent<EiBasicCrouch> ();
			go.AddComponent<EiBasicJump> ();

			// adding rotation and camera
			var rot = go.AddComponent<EiBasicRotation> ();
			// Add Camera As child
			var camObj = new GameObject ("Camera");
			var cam = camObj.AddComponent<Camera> ();
			cam.fieldOfView = 85f;
			cam.nearClipPlane = 0.1f;
			camObj.transform.SetParent (go.transform);
			camObj.transform.localPosition = new Vector3 (0, 1.7f);
			rot.SetNeck (camObj.transform);
			// Generate The mesh Object for simple texture
			var meshObj = GameObject.CreatePrimitive (PrimitiveType.Capsule);
			meshObj.transform.SetParent (go.transform);
			meshObj.transform.localScale = new Vector3 (0.5f, 0.9f, 0.5f);
			meshObj.transform.localPosition = new Vector3 (0f, 0.9f, 0f);
			DestroyImmediate (meshObj.GetComponent<CapsuleCollider> ());


			// Loading All Components
			ent.AttachAllComponentsOnEntity ();
			return go;
		}
	}
}

