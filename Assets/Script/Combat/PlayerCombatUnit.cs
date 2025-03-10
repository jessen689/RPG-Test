namespace RPGTest
{
	public class PlayerCombatUnit : CombatUnitBase
	{
		private bool isDefending;

		private void Awake()
		{
			IsPlayer = true;
		}

		public override void ReduceHealth(float _value)
		{
			if (isDefending)
				base.ReduceHealth(_value / 2);
			else
				base.ReduceHealth(_value);
		}

		public void SetSelectedSkill(SkillID _id)
		{
			foreach(var s in Skills)
			{
				if (s.skillData.skillID == _id)
				{
					selectedSkill = s;
					break;
				}
			}
		}

		public void SetSelectedAction(UnitActionID _actionID)
		{
			selectedAction = _actionID;
			isDefending = selectedAction == UnitActionID.Defend;
		}

		public void PlayActionAnim()
		{
			if (selectedAction == UnitActionID.Defend)
				animator_.Play(ANIM_SKILL_STRING_BASE + "Defend");
			else if (selectedAction == UnitActionID.Run)
				animator_.Play(ANIM_SKILL_STRING_BASE + "Run");
		}
	}
}
