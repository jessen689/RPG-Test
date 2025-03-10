using UnityEngine;

namespace RPGTest
{
	[CreateAssetMenu(fileName = "Skill", menuName = "Create Skill", order = 1)]
	public class SkillData : ScriptableObject
	{
		public SkillID skillID;
		public string skillName;
		public float skillMultiplier;
		public int skillCD;
	}
}
