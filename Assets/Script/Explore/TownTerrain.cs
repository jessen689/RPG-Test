namespace RPGTest
{
	public class TownTerrain : BaseTerrain
	{
		public override void InitializeTerrain()
		{
			base.InitializeTerrain();

			//restore full health
			GlobalDataRef.Instance.FullHealPlayer();
		}
	}
}
