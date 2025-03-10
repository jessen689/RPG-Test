using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	public class CombatUnitBase : MonoBehaviour, ICombatUnit
	{
		[SerializeField] protected Animator animator_;

		#region interface Properties
		public CombatUnitID UnitID { get; private set; }

		public string UnitName { get; private set; }

		public float ActionSpeed { get; private set; }

		public float ActionValue { get; set; }

		public float Health { get; private set; }

		public float MaxHealth { get; private set; }

		public float AttackDmg { get; private set; }

		public bool IsPlayer { get; protected set; }

		public List<Skill> Skills { get; private set; } = new();

		public List<UnitActionID> Actions { get; private set; }
		#endregion

		protected UnitActionID selectedAction;
		protected Skill selectedSkill;

		public event Action<ICombatUnit> OnUnitDead;

		protected const string ANIM_SKILL_STRING_BASE = "CombatUnit_";

		public void AttackTarget(ICombatUnit _unitTarget)
		{
			if (selectedAction == UnitActionID.Attack)
			{
				animator_.Play(ANIM_SKILL_STRING_BASE + "Attack");
				_unitTarget.ReduceHealth(AttackDmg);
			}
			else if (selectedAction == UnitActionID.Skill)
			{
				animator_.Play(ANIM_SKILL_STRING_BASE + selectedSkill.skillData.skillName);
				_unitTarget.ReduceHealth(AttackDmg * selectedSkill.skillData.skillMultiplier);
				selectedSkill.cooldownCount = selectedSkill.skillData.skillCD;
			}
		}

		public void InitializeUnit(UnitData _unitData)
		{
			UnitID = _unitData.unitID;
			UnitName = _unitData.unitName;
			ActionSpeed = _unitData.unitSpeed;
			Health = _unitData.unitHealth;
			MaxHealth = _unitData.unitMaxHealth;
			AttackDmg = _unitData.unitAttack;
			Actions = new(_unitData.unitActions);

			foreach(var s in _unitData.unitSkills)
			{
				Skills.Add(new(GlobalDataRef.Instance.GetSkillData(s)));
			}
		}

		public virtual void ReduceHealth(float _value)
		{
			Health = Mathf.Clamp(Health - _value, 0f, MaxHealth);

			if (Health <= 0f)
			{
				//unit dead
				OnUnitDead?.Invoke(this);
				//SEMENTARA
				gameObject.SetActive(false);
			}
		}

		public void ReduceSkillCD()
		{
			foreach(var skill in Skills)
			{
				if (skill.cooldownCount > 0)
					skill.cooldownCount--;
			}
		}
	}
}
