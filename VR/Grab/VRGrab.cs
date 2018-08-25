using UnityEngine;
using UnityEngine.XR;

namespace Eitrum.VR {
	[AddComponentMenu("Eitrum/VR/Grab")]
	public class VRGrab : EiComponent {

		#region Variables

		#region Settings

		[Header("Grab settings")]
		[SerializeField]
		private string inputAxis = "Joystick_Axis_11";
		[SerializeField]
		[Tooltip("0 == Unlimited objects")]
		[Range(0, 128)]
		private int maxGrabObjects = 0;
		[SerializeField]
		private LayerMask layerMask;
		[SerializeField]
		[Tooltip("This limits the amount of collisions for grab check down to 32 objects")]
		private bool useOptimizedGrab = false;
		[SerializeField]
		private float grabRadius = 1f;
		[SerializeField]
		private float grabThreshold = 0.05f;
		[SerializeField]
		private float releaseThreshold = 0.05f;

		#endregion

		#region State

		private EiBoolStack canGrab = new EiBoolStack();
		private float grabAmount = 0f;
		private bool isGrabbing = false;
		private float previousStateValue = 0f;

		#endregion

		#region Events

		private EiTrigger onGrab = new EiTrigger();
		private EiTrigger onRelease = new EiTrigger();

		#endregion

		#region Grabbed objects

		private static Collider[] optimizedGrab = new Collider[32];
		private EiLinkedList<EiGrabInterface> grabbedObjects = new EiLinkedList<EiGrabInterface>();

		#endregion

		#endregion

		#region Properties

		public float GrabRadius {
			get {
				return transform.lossyScale.x * grabRadius;
			}
		}

		public bool CanGrab {
			get {
				return canGrab;
			}
		}

		public float GrabValue {
			get {
				return grabAmount;
			}
		}

		#endregion

		#region Core

		private void Awake() {
			SubscribeUpdate();
		}

		public override void UpdateComponent(float time) {
			var newValue = Input.GetAxisRaw(inputAxis);
			if (isGrabbing) {
				if (newValue > grabAmount)
					previousStateValue = newValue;
				else if (newValue < previousStateValue - releaseThreshold)
					Release();
			}
			else {
				if (newValue < grabAmount)
					previousStateValue = newValue;
				else if (newValue > previousStateValue + grabThreshold)
					Grab();
			}

			grabAmount = newValue;

			var iterator = grabbedObjects.GetIterator();
			EiLLNode<EiGrabInterface> node;
			while (iterator.Next(out node)) {
				if (node.Value == null || node.Value.IsNull)
					node.RemoveFromList();
				else
					node.Value.OnGrabUpdate(this, grabAmount, time);
			}
		}

		#endregion

		#region Grab

		/// <summary>
		/// Does a new physics check and grabs everything grabbable nearby
		/// </summary>
		public void Grab() {
			isGrabbing = true;
			onGrab.Trigger();
			if (useOptimizedGrab) {
				var hits = Physics.OverlapSphereNonAlloc(this.transform.position, this.transform.lossyScale.x * grabRadius, optimizedGrab, layerMask, QueryTriggerInteraction.UseGlobal);
				for (int i = 0; i < hits; i++) {
					var grab = optimizedGrab[i].GetComponents<EiGrabInterface>();
					for (int g = 0; g < grab.Length; g++) {
						if (maxGrabObjects == 0 || grabbedObjects.Length < maxGrabObjects) {
							if (grab[g].OnGrab(this))
								grabbedObjects.Add(grab[g]);
							else
								break;
						}
					}
				}
			}
			else {
				var hitObjects = Physics.OverlapSphere(this.transform.position, this.transform.lossyScale.x * grabRadius, layerMask, QueryTriggerInteraction.UseGlobal);
				for (int i = 0; i < hitObjects.Length; i++) {
					var grab = hitObjects[i].GetComponents<EiGrabInterface>();
					for (int g = 0; g < grab.Length; g++) {
						if (maxGrabObjects == 0 || grabbedObjects.Length < maxGrabObjects) {
							if (grab[g].OnGrab(this))
								grabbedObjects.Add(grab[g]);
							else
								break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Grabs an object and adds it to the update list. Only works if the hand is currently grabbing.
		/// </summary>
		/// <param name="grabInterface"></param>
		/// <returns>Returns true if grab succeeded, otherwise returns false.</returns>
		public bool Grab(EiEntity entity) {
			var grab = entity.GetComponents<EiGrabInterface>();
			bool isGrabbed = false;
			for (int i = 0; i < grab.Length; i++) {
				if (Grab(grab[i]))
					isGrabbed = true;
			}
			return isGrabbed;
		}

		/// <summary>
		/// Grabs an object and adds it to the update list. Only works if the hand is currently grabbing.
		/// </summary>
		/// <param name="grabInterface"></param>
		/// <returns>Returns true if grab succeeded, otherwise returns false.</returns>
		public bool Grab(EiGrabInterface grabInterface) {
			if (!isGrabbing || (maxGrabObjects > 0 && grabbedObjects.Length >= maxGrabObjects))
				return false;
			if (grabInterface.OnGrab(this))
				grabbedObjects.Add(grabInterface);
			return true;
		}

		#endregion

		#region Release

		/// <summary>
		/// Releases all objects currently grabbed.
		/// </summary>
		public void Release() {
			isGrabbing = false;
			onRelease.Trigger();

			var iterator = grabbedObjects.GetIterator();
			EiLLNode<EiGrabInterface> node;
			while (iterator.Next(out node)) {
				if (node.Value == null || node.Value.IsNull)
					node.RemoveFromList();
				else
					node.Value.OnRelase(this);
			}
			grabbedObjects.ClearFast();
		}

		/// <summary>
		/// Releases 1 object from the grab list
		/// </summary>
		/// <param name="entity"></param>
		public void Release(EiEntity entity) {
			var grab = entity.GetComponent<EiGrabInterface>();
			if (grab != null)
				Release(grab);
		}

		/// <summary>
		/// Releases 1 object from the grab list
		/// </summary>
		/// <param name="entity"></param>
		public void Release(EiGrabInterface grabInterface) {
			var iterator = grabbedObjects.GetIterator();
			EiLLNode<EiGrabInterface> node;
			while (iterator.Next(out node)) {
				if (node.Value == null || node.Value.IsNull)
					node.RemoveFromList();
				else if (node.Value == grabInterface)
					node.RemoveFromList();
			}
			grabInterface.OnRelase(this);
		}

		#endregion

		#region Editor

#if UNITY_EDITOR

		private void OnDrawGizmos() {
			Gizmos.DrawWireSphere(this.transform.position, GrabRadius);
		}

#endif

		#endregion
	}
}