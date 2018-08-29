using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum {
	public static class RigidbodyExtensions {
		public static bool IsGrounded(this Rigidbody rigidbody, out RaycastHit hit, float distanceCheck = 0.05f) {
			rigidbody.position += new Vector3(0, 0.01f, 0);
			var isGrounded = rigidbody.SweepTest(Vector3.down, out hit, distanceCheck);
			rigidbody.position -= new Vector3(0, 0.01f, 0);
			return isGrounded;
		}
	}
}
