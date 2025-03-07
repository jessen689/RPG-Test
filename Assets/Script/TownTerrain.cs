using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	public class TownTerrain : MonoBehaviour, ITerrainPlace
	{
		[SerializeField] private PlaceID id_;
		[SerializeField] private List<SpawnPoint> spawnPoints = new();

		public PlaceID Id { get { return id_; } }

		private PlaceID prevPlace;

		[Serializable] private struct SpawnPoint
		{
			public PlaceID id;
			public Vector2 spawnPos;
		}

		public void HideTerrain()
		{
			gameObject.SetActive(false);
		}

		public void InitializeTerrain()
		{
			transform.position = Vector3.zero;
			foreach(var spawn in spawnPoints)
			{
				if (spawn.id == prevPlace)
				{
					GlobalDataRef.Instance.player.transform.position = spawn.spawnPos;
					break;
				}
			}
		}

		public void ShowTerrain(PlaceID _lastPlace)
		{
			gameObject.SetActive(true);
			prevPlace = _lastPlace;
		}
	}
}
