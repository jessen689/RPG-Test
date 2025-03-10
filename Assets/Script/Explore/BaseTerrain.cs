using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	public class BaseTerrain : MonoBehaviour, ITerrainPlace
	{
		[SerializeField] private PlaceID id_;
		[SerializeField] private List<SpawnPoint> spawnPoints = new();

		// Camera Limits
		[SerializeField] private float topLimit_;
		[SerializeField] private float bottomLimit_;
		[SerializeField] private float leftLimit_;
		[SerializeField] private float rightLimit_;

		public PlaceID Id { get { return id_; } }

		public float TopTerrainLimit { get { return topLimit_; } }

		public float BottomTerrainLimit { get { return bottomLimit_; } }

		public float LeftTerrainLimit { get { return leftLimit_; } }

		public float RightTerrainLimit { get { return rightLimit_; } }

		private PlaceID prevPlace;

		[Serializable] private class SpawnPoint
		{
			public PlaceID id;
			public Vector2 spawnPos;
		}

		public void HideTerrain()
		{
			gameObject.SetActive(false);
		}

		public virtual void InitializeTerrain()
		{
			transform.position = Vector3.zero;
			foreach (var spawn in spawnPoints)
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

		public void SetSelfLastSpawn(PlaceID _target)
		{
			foreach(var point in spawnPoints)
			{
				if (point.id == _target)
					point.spawnPos = GlobalDataRef.Instance.player.transform.position;
			}
		}

		public GameObject GetGameObject()
		{
			return gameObject;
		}
	}
}
