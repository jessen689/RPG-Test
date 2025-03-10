using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGTest
{
	public class CombatHealthHandler : MonoBehaviour
	{
		[SerializeField] private Slider healthPrefabs;
		[SerializeField] private RectTransform playerPointParent_;
		[SerializeField] private RectTransform enemyPointParent_;
		[SerializeField] private Vector2 offset_;

		private List<Slider> healthBars = new();
		private List<ICombatUnit> combatUnits = new();

		public void SetCombatUnits()
		{
			for(int p = 0; p< playerPointParent_.childCount; p++)
			{
				combatUnits.Add(playerPointParent_.GetChild(p).GetComponent<ICombatUnit>());
				healthBars.Add(Instantiate(healthPrefabs, playerPointParent_.GetChild(p)));
				healthBars[^1].transform.localPosition = offset_;
				healthBars[^1].value = Mathf.Clamp01(combatUnits[^1].Health / combatUnits[^1].MaxHealth);
			}
			for(int e = 0; e < enemyPointParent_.childCount; e++)
			{
				combatUnits.Add(enemyPointParent_.GetChild(e).GetComponent<ICombatUnit>());
				healthBars.Add(Instantiate(healthPrefabs, enemyPointParent_.GetChild(e)));
				healthBars[^1].transform.localPosition = offset_;
				healthBars[^1].value = Mathf.Clamp01(combatUnits[^1].Health / combatUnits[^1].MaxHealth);
			}
		}

		public void UpdateHealth()
		{
			for(int i = 0; i < combatUnits.Count; i++)
			{
				healthBars[i].value = Mathf.Clamp01(combatUnits[i].Health / combatUnits[i].MaxHealth);
				if (healthBars[i].value == 0)
					healthBars[i].gameObject.SetActive(false);
			}
		}
	}
}
