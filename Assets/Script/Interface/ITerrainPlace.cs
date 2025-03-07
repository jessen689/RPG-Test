namespace RPGTest
{
	public interface ITerrainPlace
	{
		public PlaceID Id { get; }
		public void InitializeTerrain();
		public void ShowTerrain(PlaceID _lastPlace);
		public void HideTerrain();
	}
}
