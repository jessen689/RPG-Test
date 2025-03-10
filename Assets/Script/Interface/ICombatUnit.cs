using System;
using System.Collections.Generic;

namespace RPGTest
{
	public interface ICombatUnit
	{
		public CombatUnitID UnitID { get; }
		public string UnitName { get; }
		public float ActionSpeed { get; }
		public float ActionValue { get; set; }
		public float Health { get; }
		public float MaxHealth { get; }
		public float AttackDmg { get; }
		public bool IsPlayer { get; }
		public List<Skill> Skills { get; }
		public List<UnitActionID> Actions { get; }
		public event Action<ICombatUnit> OnUnitDead;

		public void InitializeUnit(UnitData _unitData);
		public void AttackTarget(ICombatUnit _unitTarget);
		public void ReduceHealth(float _value);
	}
}
