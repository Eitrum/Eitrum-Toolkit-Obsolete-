using System;
using UnityEngine;

namespace Eitrum.Health
{
	[Serializable]
	public class EiCombatData
	{
		#region Variables

		[SerializeField]
		[EiDamageType]
		private int damageType;

		[SerializeField]
		private float flatAmount;
		[SerializeField]
		private float currentHealthPercentage;
		[SerializeField]
		private float maxHealthPercentage;

		[SerializeField]
		private string comment;
		private EiEntity source;
		private EiHealth target;

		private float reducedAmount;

		#endregion

		#region Properties

		public float TotalAmount {
			get {
				return flatAmount + target.CurrentHealth * currentHealthPercentage + target.MaxHealth * maxHealthPercentage;
			}
		}

		public float TotalRemainingAmount {
			get {
				return TotalAmount - reducedAmount;
			}
		}

		public int DamageType {
			get {
				return damageType;
			}
			set {
				damageType = value;
			}
		}

		public float FlatAmount {
			get {
				return flatAmount;
			}
			set {
				flatAmount = value;
			}
		}

		public float CurrentHealthPercentage {
			get {
				return currentHealthPercentage;
			}set {
				currentHealthPercentage = value;
			}
		}

		public float MaxHealthPercentage {
			get {
				return maxHealthPercentage;
			}set {
				maxHealthPercentage = value;
			}
		}

		public EiHealth Target {
			get {
				return target;
			}
			set {
				target = value;
			}
		}

		public EiEntity SourceEntity {
			get {
				return source;
			}
			set {
				source = value;
			}
		}

		public string Comment {
			get {
				return comment;
			}
			set {
				comment = value;
			}
		}

		public bool HasTarget {
			get {
				return target != null;
			}
		}

		public EiCombatData Copy {
			get {
				return new EiCombatData (this);
			}
		}

		#endregion

		#region Constructors

		public EiCombatData (float flatAmount)
		{

			this.damageType = -1;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = 0f;
			this.maxHealthPercentage = 0f;
			this.comment = "";
			this.source = null;
			this.target = null;
		}

		public EiCombatData (int damageType, float flatAmount)
		{
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = 0f;
			this.maxHealthPercentage = 0f;
			this.comment = "";
			this.source = null;
			this.target = null;
		}

		public EiCombatData (int damageType, float flatAmount, EiEntity source)
		{
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = 0f;
			this.maxHealthPercentage = 0f;
			this.comment = "";
			this.source = source;
			this.target = null;
		}

		public EiCombatData (int damageType, float flatAmount, float currentHealthPercentage, float maxHealthPercentage)
		{
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = currentHealthPercentage;
			this.maxHealthPercentage = maxHealthPercentage;
			this.comment = "";
			this.source = null;
			this.target = null;
		}

		public EiCombatData (int damageType, float flatAmount, float currentHealthPercentage, float maxHealthPercentage, EiEntity source)
		{
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = currentHealthPercentage;
			this.maxHealthPercentage = maxHealthPercentage;
			this.comment = "";
			this.source = source;
			this.target = null;
		}

		public EiCombatData (int damageType, float flatAmount, float currentHealthPercentage, float maxHealthPercentage, string comment, EiEntity source, EiHealth target)
		{
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = currentHealthPercentage;
			this.maxHealthPercentage = maxHealthPercentage;
			this.comment = comment;
			this.source = source;
			this.target = target;
		}

		public EiCombatData (EiCombatData data)
		{
			this.damageType = data.damageType;
			this.flatAmount = data.flatAmount;
			this.currentHealthPercentage = data.currentHealthPercentage;
			this.maxHealthPercentage = data.maxHealthPercentage;
			this.comment = data.comment;
			this.source = data.source;
			this.target = data.target;
		}

		#endregion

		#region Core

		public void Reduce (float amount)
		{
			reducedAmount += amount;
		}

		public void ApplySource (EiEntity source)
		{
			this.source = source;
		}

		public void ApplyTarget (EiHealth target)
		{
			this.target = target;
		}

		public void SetComment (string text)
		{
			this.comment = text;
		}

		public void Clear ()
		{
			this.damageType = 0;
			this.flatAmount = 0f;
			currentHealthPercentage = 0f;
			maxHealthPercentage = 0f;
			comment = "";
			source = null;
		}

		#endregion

		#region Flat Amount

		public void AddFlatAmount (float amount)
		{
			flatAmount += amount;
		}

		public void RemoveFlatAmount (float amount)
		{
			flatAmount -= amount;
		}

		public void SetFlatAmount (float value)
		{
			flatAmount = value;
		}

		#endregion

		#region Current Health

		public void AddCurrentHealthPercentage (float amount)
		{
			currentHealthPercentage += amount;
		}

		public void RemoveCurrentHealthPercentage (float amount)
		{
			currentHealthPercentage -= amount;
		}

		public void SetCurrentHealthPercentage (float value)
		{
			currentHealthPercentage = value;
		}

		#endregion

		#region Max Health

		public void AddMaxHealthPercentage (float amount)
		{
			maxHealthPercentage += amount;
		}

		public void RemoveMaxHealthPercentage (float amount)
		{
			maxHealthPercentage -= amount;
		}

		public void SetMaxHealthPercentage (float value)
		{
			maxHealthPercentage = value;
		}

		#endregion
	}
}