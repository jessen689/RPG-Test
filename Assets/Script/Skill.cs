namespace RPGTest
{
	public class Skill
	{
		public SkillData skillData;
		public int cooldownCount;

		public Skill(SkillData _data)
		{
			skillData = _data;
			cooldownCount = 0;
		}

		public Skill() { }
	}
}
