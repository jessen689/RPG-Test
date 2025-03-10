using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	[CreateAssetMenu(fileName = "Unit", menuName = "Create Unit", order = 0)]
	public class UnitData : ScriptableObject
	{
		public CombatUnitID unitID;
		public string unitName;
		public float unitHealth;
		public float unitMaxHealth;
		public float unitAttack;
		public float unitSpeed;
		public List<SkillID> unitSkills;
		public List<UnitActionID> unitActions;
		public GameObject unitPrefabs;
	}
}
