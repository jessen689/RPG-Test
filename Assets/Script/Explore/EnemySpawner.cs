using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGTest
{
	public class EnemySpawner : MonoBehaviour
	{
		[SerializeField] private List<Transform> spawnPoints_ = new();
		[SerializeField] private float spawnArea_;
		[SerializeField] private List<EnemyBehaviour> enemies = new();
		[SerializeField] private float respawnTime_;
		[SerializeField] private float fastestResTime_;

		private Dictionary<EnemyBehaviour, RespawnDetail> respawns = new();
		private Dictionary<EnemyBehaviour, RespawnDetail> aliveEnemies = new();
		private Vector2 tempPos;

		private class RespawnDetail
		{
			public float currSpawnTime;
			public float spawnCounter;
		}

		private void Start()
		{
			InitAllEnemy();
		}

		private void Update()
		{
			if(respawns.Count > 0)
			{
				foreach(var res in respawns.Keys.ToList())
				{
					respawns[res].spawnCounter -= Time.deltaTime;
					if(respawns[res].spawnCounter <= 0)
					{
						tempPos = spawnPoints_[Random.Range(0, spawnPoints_.Count)].position;
						tempPos.x = Random.Range(tempPos.x - spawnArea_, tempPos.x + spawnArea_);
						tempPos.y = Random.Range(tempPos.y - spawnArea_, tempPos.y + spawnArea_);
						res.Spawn(tempPos);
						respawns[res].currSpawnTime = Mathf.Max(respawns[res].currSpawnTime - 1f, fastestResTime_);
						respawns[res].spawnCounter = respawns[res].currSpawnTime;
						aliveEnemies.Add(res, respawns[res]);
						respawns.Remove(res);
					}
				}
			}
		}

		private void InitAllEnemy()
		{
			foreach(var e in enemies)
			{
				e.OnDefeatedInCombat += StartRespawn;
				RespawnDetail enemyRespawn = new();
				enemyRespawn.currSpawnTime = respawnTime_;
				enemyRespawn.spawnCounter = respawnTime_;
				aliveEnemies.Add(e, enemyRespawn);
			}
		}

		private void StartRespawn(EnemyBehaviour _deadEnemy)
		{
			respawns.Add(_deadEnemy, aliveEnemies[_deadEnemy]);
			aliveEnemies.Remove(_deadEnemy);
		}
	}
}
