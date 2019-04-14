using Eitrum.Engine.Core;
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
		private int damageType = -1;

		[SerializeField]
		private float flatAmount;
		[SerializeField]
		private float currentHealthPercentage;
		[SerializeField]
		private float maxHealthPercentage;

		[SerializeField]
		private string extra;
		private EiEntity source;
		private EiHealth target;

		private float reducedAmount;
		private bool isCopy = false;

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

		public string Extra {
			get {
				return extra;
			}
			set {
				extra = value;
			}
		}

		public bool HasTarget {
			get {
				return target != null;
			}
		}

		public EiCombatData Copy {
			get {
				return new EiCombatData (this, isCopy);
			}
		}

		public bool IsCopy {
			get {
				return isCopy;
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
			this.extra = "";
			this.source = null;
			this.target = null;
		}

		public EiCombatData (int damageType, float flatAmount)
		{
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = 0f;
			this.maxHealthPercentage = 0f;
			this.extra = "";
			this.source = null;
			this.target = null;
		}

		public EiCombatData (int damageType, float flatAmount, EiEntity source)
		{
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = 0f;
			this.maxHealthPercentage = 0f;
			this.extra = "";
			this.source = source;
			this.target = null;
		}

		public EiCombatData (int damageType, float flatAmount, float currentHealthPercentage, float maxHealthPercentage)
		{
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = currentHealthPercentage;
			this.maxHealthPercentage = maxHealthPercentage;
			this.extra = "";
			this.source = null;
			this.target = null;
		}

		public EiCombatData (int damageType, float flatAmount, float currentHealthPercentage, float maxHealthPercentage, EiEntity source)
		{
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = currentHealthPercentage;
			this.maxHealthPercentage = maxHealthPercentage;
			this.extra = "";
			this.source = source;
			this.target = null;
		}

		public EiCombatData (int damageType, float flatAmount, float currentHealthPercentage, float maxHealthPercentage, string extra, EiEntity source, EiHealth target)
		{
			this.damageType = damageType;
			this.flatAmount = flatAmount;
			this.currentHealthPercentage = currentHealthPercentage;
			this.maxHealthPercentage = maxHealthPercentage;
			this.extra = extra;
			this.source = source;
			this.target = target;
		}

		public EiCombatData (EiCombatData data)
		{
			this.damageType = data.damageType;
			this.flatAmount = data.flatAmount;
			this.currentHealthPercentage = data.currentHealthPercentage;
			this.maxHealthPercentage = data.maxHealthPercentage;
			this.extra = data.extra;
			this.source = data.source;
			this.target = data.target;
		}

		public EiCombatData (EiCombatData data, bool isCopy) : this (data)
		{
			this.isCopy = isCopy;
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
			this.extra = text;
		}

		public void Clear ()
		{
			this.damageType = 0;
			this.flatAmount = 0f;
			currentHealthPercentage = 0f;
			maxHealthPercentage = 0f;
			extra = "";
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