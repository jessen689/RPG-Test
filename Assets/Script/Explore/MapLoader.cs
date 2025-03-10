using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	public class MapLoader : MonoBehaviour
	{
		[SerializeField] private List<TerrainReference> terrainReferences = new();
		[SerializeField] private CameraFollow cameraFollow_;

		private Dictionary<PlaceID, ITerrainPlace> cachedTerrains = new();

		//public static MapLoader Instance { get; private set; }

		private PlaceID currPlace;

		[Serializable] private struct TerrainReference
		{
			public PlaceID id;
			public GameObject terrainPrefabs;
		}

		private void OnEnable()
		{
			if(GlobalDataRef.Instance.terrainInCombat != null)
			{
				currPlace = GlobalDataRef.Instance.combatPlace;
				cachedTerrains.Add(currPlace, GlobalDataRef.Instance.terrainInCombat.GetComponent<ITerrainPlace>());
				GlobalDataRef.Instance.terrainInCombat.transform.SetParent(transform);
				LoadTerrain(currPlace, true);
			}
			else
				LoadTerrain(currPlace);

			GameEvents.Instance.OnEnterCombat += SaveExploreTerrain;
			GameEvents.Instance.OnLoadMap += LoadTerrain;
		}

		private void OnDisable()
		{
			GameEvents.Instance.OnEnterCombat -= SaveExploreTerrain;
			GameEvents.Instance.OnLoadMap -= LoadTerrain;
		}


		//private void Awake()
		//{
		//	//singleton
		//	if (Instance != null && Instance != this)
		//		Destroy(gameObject);
		//	else
		//	{
		//		Instance = this;
		//		DontDestroyOnLoad(gameObject);
		//	}

		//	//first time load
		//	//currPlace = PlaceID.Town;
		//	//cachedTerrains.Add(currPlace, firstTerrainRef_.GetComponent<ITerrainPlace>());
		//	LoadTerrain(PlaceID.Town);

		//	GameEvents.Instance.OnEnterCombat += PauseExploring;
		//	GameEvents.Instance.OnBackToExplore += () => LoadTerrain(currPlace);
		//}

		public void LoadTerrain(PlaceID _id)
		{
			LoadTerrain(_id, false);
		}

		public void LoadTerrain(PlaceID _id, bool isKeepPlayerPos)
		{
			// Call Transition
			TransitionHandler.Instance.FadeInAnim();

			if (!cachedTerrains.ContainsKey(_id))
			{
				foreach (var terrain in terrainReferences)
				{
					if (terrain.id == _id)
					{
						if (Instantiate(terrain.terrainPrefabs, transform).TryGetComponent<ITerrainPlace>(out var cachedTerrain))
						{
							cachedTerrains.Add(_id, cachedTerrain);
							break;
						}
					}
				}
			}

			HideCurrTerrain();
			cachedTerrains[_id].ShowTerrain(currPlace);
			if(!isKeepPlayerPos)
				cachedTerrains[_id].InitializeTerrain();
			currPlace = _id;

			// Set Camera Restriction
			if (cameraFollow_ == null)
				FindObjectOfType<CameraFollow>();
			cameraFollow_.topLimit = cachedTerrains[currPlace].TopTerrainLimit;
			cameraFollow_.bottomLimit = cachedTerrains[currPlace].BottomTerrainLimit;
			cameraFollow_.leftLimit = cachedTerrains[currPlace].LeftTerrainLimit;
			cameraFollow_.rightLimit = cachedTerrains[currPlace].RightTerrainLimit;
		}

		private void HideCurrTerrain()
		{
			cachedTerrains[currPlace].HideTerrain();
		}

		private void SaveExploreTerrain(float arg1, EnemyBehaviour arg2)
		{
			GlobalDataRef.Instance.combatPlace = currPlace;
			GlobalDataRef.Instance.terrainInCombat = cachedTerrains[currPlace].GetGameObject();
			GlobalDataRef.Instance.terrainInCombat.transform.SetParent(GlobalDataRef.Instance.transform);
			GlobalDataRef.Instance.terrainInCombat.SetActive(false);
		}

		private void PauseExploring(float arg1, EnemyBehaviour arg2)
		{
			HideCurrTerrain();
			var temp = (BaseTerrain)cachedTerrains[currPlace];
			temp.SetSelfLastSpawn(currPlace);
		}
	}
}
