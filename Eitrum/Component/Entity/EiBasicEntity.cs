using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Eitrum
{
	public class EiBasicEntity : EiComponent
	{
		#region Variables

		[Header ("Entity Settings")]
		public string entityName = "default-entity";

		[Header ("Entity Components")]
		public Rigidbody body;
		public Collider collision;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the rigidbody, if non exists, add a rigidbody.
		/// </summary>
		/// <value>The body.</value>
		public virtual Rigidbody Body {
			get {
				if (body == null) {
					throw new System.NullReferenceException ("Rigidbody Not Attached");
				}
				return body;
			}
		}

		/// <summary>
		/// Gets the collider.
		/// </summary>
		/// <value>The collider.</value>
		public virtual Collider Collision {
			get {
				if (collision == null) {
					throw new System.NullReferenceException ("Collision Not Attached");
				}
				return collision;
			}
		}

		#endregion

		[ContextMenu ("Load All Entity Settings")]
		public void LoadEntity ()
		{
			if (body == null)
				body = GetComponent<Rigidbody> ();
			if (collision == null)
				collision = GetComponent<Collider> ();
		}
	}
}