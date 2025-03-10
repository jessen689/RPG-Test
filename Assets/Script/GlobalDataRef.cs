using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	public class GlobalDataRef : MonoBehaviour
	{
		public static GlobalDataRef Instance { get; private set; }

		// Player
		public CharacterController player;
		public float playerHealth;
		public float playerMaxHealth;
		[SerializeField] private List<CombatUnitID> playerUnits = new();

		[Header("Combat")]
		[SerializeField] private List<SkillData> skillDatas = new();
		[SerializeField] private List<UnitData> unitDatas = new();
		public float playerSpeedMulti;
		public EnemyBehaviour enemyCombatTarget;

		private List<CombatUnitID> currCombatants = new();

		[Header("Terrain")]
		public PlaceID combatPlace;
		public GameObject terrainInCombat;

		private void Awake()
		{
			//singleton
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}
			else
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}

			GameEvents.Instance.OnEnterCombat += (speed, target) => { player.gameObject.SetActive(false); };
			GameEvents.Instance.OnBackToExplore += BackToExplore;
			GameEvents.Instance.OnRetryGame += () => { 
				BackToExplore(); 
				enemyCombatTarget = null;
				Destroy(terrainInCombat);
			};
		}

		private void BackToExplore()
		{
			player.gameObject.SetActive(true);
			currCombatants.Clear();
		}

		public void FullHealPlayer()
		{
			playerHealth = playerMaxHealth;
		}

		public SkillData GetSkillData(SkillID _id)
		{
			foreach(var skill in skillDatas)
			{
				if (skill.skillID == _id)
					return skill;
			}
			Debug.Log($"Skill with id : {_id} not found");
			return skillDatas[0];
		}

		public UnitData GetUnitData(CombatUnitID _id)
		{
			foreach (var unit in unitDatas)
			{
				if (unit.unitID == _id)
					return unit;
			}
			Debug.Log($"Combat Unit with id : {_id} not found");
			return unitDatas[0];
		}

		public List<UnitData> GetCombatantsData()
		{
			currCombatants.AddRange(playerUnits);
			currCombatants.AddRange(enemyCombatTarget.combatGroup_);
			Debug.Log(currCombatants.Count);
			List<UnitData> combatantsData = new();
			foreach(var u in currCombatants)
			{
				combatantsData.Add(GetUnitData(u));
			}
			return combatantsData;
		}
	}
}
