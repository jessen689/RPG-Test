using UnityEngine;

namespace RPGTest
{
	public class EnemyCombatUnit : CombatUnitBase
	{
		private UnitActionID prevAction;

		private void Awake()
		{
			IsPlayer = false;
		}

		public void DoAction()
		{
			if (prevAction == UnitActionID.Attack)
			{
				selectedAction = UnitActionID.Skill;
				selectedSkill = GetAvailableSkill();
				if (selectedSkill == null)
					selectedAction = UnitActionID.Attack;
			}
			else
			{
				selectedAction = UnitActionID.Attack;
			}
		}

		private Skill GetAvailableSkill()
		{
			foreach (var s in Skills)
			{
				if (s.cooldownCount <= 0)
				{
					return s;
				}
			}

			Debug.Log("All skill is in Cooldown, returning null...");
			return null;
		}
	}
}
