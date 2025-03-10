using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGTest
{
	public class TurnBaseHandler : MonoBehaviour
	{
		[SerializeField] private float actionRequirement_;

		private List<ICombatUnit> units = new();
		// Combat time
		private float currentTime = 0f;

		public ICombatUnit InitTurnBase(List<ICombatUnit> _unitsInCombat)
		{
			units = new(_unitsInCombat);

			// Calculate first turn action value based on requirement
			foreach (var unit in units)
			{
				if(unit.UnitID == CombatUnitID.Player)
					unit.ActionValue = actionRequirement_ / (unit.ActionSpeed * GlobalDataRef.Instance.playerSpeedMulti);
				else
					unit.ActionValue = actionRequirement_ / unit.ActionSpeed;
			}
			// Return the first turn
			return NextTurn();
		}

		public ICombatUnit NextTurn()
		{
			// Sort characters by their next action time
			units = units.OrderBy(u => u.ActionValue).ToList();

			// Get the character who acts next
			ICombatUnit activeCharacter = units[0];

			// Fast forward time to the next action
			currentTime = activeCharacter.ActionValue;

			// Schedule the next turn for this character
			activeCharacter.ActionValue += 100f / activeCharacter.ActionSpeed;

			Debug.Log(activeCharacter.UnitID + " takes an action at time: " + currentTime);
			return activeCharacter;
		}

		public void RemoveUnitFromOrder(ICombatUnit _removedUnit)
		{
			units.Remove(_removedUnit);
		}
	}
}
