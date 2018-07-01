using System;
using UnityEngine;

namespace Eitrum
{
	public class DatabaseReference : PropertyAttribute
	{
		public Type type;

		public DatabaseReference ()
		{

		}

		public DatabaseReference (Type type)
		{
			this.type = type;
		}
	}
}

