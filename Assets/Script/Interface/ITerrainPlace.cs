using UnityEngine;

namespace RPGTest
{
	public interface ITerrainPlace
	{
		public PlaceID Id { get; }
		public float TopTerrainLimit { get; }
		public float BottomTerrainLimit { get; }
		public float LeftTerrainLimit { get; }
		public float RightTerrainLimit { get; }
		public void InitializeTerrain();
		public void ShowTerrain(PlaceID _lastPlace);
		public void HideTerrain();
		public GameObject GetGameObject();
	}
}
