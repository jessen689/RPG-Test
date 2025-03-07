using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	public class MapLoader : MonoBehaviour
	{
		[SerializeField] private List<TerrainReference> terrainReferences = new();
		[SerializeField] private GameObject firstTerrainRef_;

		private Dictionary<PlaceID, ITerrainPlace> cachedTerrains = new();

		public static MapLoader Instance { get; private set; }

		private PlaceID currPlace;

		[Serializable] private struct TerrainReference
		{
			public PlaceID id;
			public GameObject terrainPrefabs;
		}

		private void Awake()
		{
			//singleton
			if (Instance != null && Instance != this)
				Destroy(gameObject);
			else
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}

			//first time load
			currPlace = PlaceID.Town;
			cachedTerrains.Add(currPlace, firstTerrainRef_.GetComponent<ITerrainPlace>());
		}

		public void LoadTerrain(PlaceID _id)
		{
			if (!cachedTerrains.ContainsKey(_id))
			{
				foreach(var terrain in terrainReferences)
				{
					if(terrain.id == _id)
					{
						if(Instantiate(terrain.terrainPrefabs).TryGetComponent<ITerrainPlace>(out var cachedTerrain))
						{
							cachedTerrains.Add(_id, cachedTerrain);
							break;
						}
					}
				}
			}

			cachedTerrains[currPlace].HideTerrain();
			cachedTerrains[_id].ShowTerrain(currPlace);
			cachedTerrains[_id].InitializeTerrain();
			currPlace = _id;
		}
	}
}
