using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGTest
{
	public class CombatManager : MonoBehaviour
	{
		[SerializeField] private TurnBaseHandler turnBaseHandler_;
		[SerializeField] private CombatHealthHandler healthHandler_;
		[SerializeField] private PlayerActionUI playerActionUI_;
		[SerializeField] private Transform playerParent_;
		[SerializeField] private Transform enemyParent_;
		[SerializeField] private CombatTargetPicker[] targetPickers; // For now max 3

		private float speedCounter;
		private CombatState currState;
		private int playerCount;
		private int enemyCount;
		private List<ICombatUnit> allCombatUnit = new();
		private List<EnemyCombatUnit> allEnemies = new();

		// Unit In Action
		private PlayerCombatUnit playerUnit;
		private EnemyCombatUnit enemyUnit;
		private ICombatUnit currActionUnit;

		private const string EXPLORE_SCENE_NAME = "ExploreScene";

		private enum CombatState { Start, PlayerTurn, EnemyTurn, WonBattle, LoseBattle}

		private void OnEnable()
		{
			Debug.Log("IS THIS CALLED TWICE?");
			currState = CombatState.Start;
			SetupCombat();
		}

		private void SetupCombat()
		{
			// Set Unit
			GenerateCombatUnit(GlobalDataRef.Instance.GetCombatantsData());

			// Set Action Speed/Value/Turn
			currActionUnit = turnBaseHandler_.InitTurnBase(allCombatUnit);

			// Advantage/Dis dmg
			if(GlobalDataRef.Instance.playerSpeedMulti < 1f)
			{
				playerUnit.ReduceHealth(allEnemies[0].AttackDmg);
			}
			else if(GlobalDataRef.Instance.playerSpeedMulti > 1f)
			{
				foreach (var unit in allEnemies)
				{
					unit.ReduceHealth(playerUnit.AttackDmg);
				}
			}
			healthHandler_.UpdateHealth();

			// First time setup
			playerActionUI_.OnActionConfirmed += ExecutePlayerAction;

			// Start Turn
			ExecuteTurn();
		}

		private void ExecutePlayerAction(UnitActionID _action, SkillID _skill)
		{
			playerUnit.SetSelectedAction(_action);
			playerUnit.SetSelectedSkill(_skill);
			playerActionUI_.gameObject.SetActive(_action == UnitActionID.Attack || _action == UnitActionID.Skill);
			if (_action == UnitActionID.Defend)
			{
				StartCoroutine(DelayEachAction());
				playerUnit.PlayActionAnim();
			}
			else if (_action == UnitActionID.Run)
			{
				Invoke(nameof(EscapeCombat), 1f);
				playerUnit.PlayActionAnim();
			}
			else
				SetTargetPicker();
		}

		private void ExecuteTurn()
		{
			if (currActionUnit.IsPlayer)
			{
				currState = CombatState.PlayerTurn;
				playerUnit = (PlayerCombatUnit)currActionUnit;
				// Show player action UI
				playerActionUI_.SetupAllSkill(playerUnit.Skills);
				playerActionUI_.UpdateSkillCDStat();
				playerActionUI_.gameObject.SetActive(true);
			}
			else
			{
				currState = CombatState.EnemyTurn;
				enemyUnit = (EnemyCombatUnit)currActionUnit;
				enemyUnit.DoAction();
				enemyUnit.AttackTarget(playerUnit);
				StartCoroutine(DelayEachAction());
			}
		}

		private IEnumerator DelayEachAction()
		{
			playerActionUI_.gameObject.SetActive(false);
			SetTargetPicker(true);
			yield return new WaitForSeconds(1f);
			healthHandler_.UpdateHealth();
			if(currState != CombatState.WonBattle && currState != CombatState.LoseBattle)
			{
				currActionUnit = turnBaseHandler_.NextTurn();
				ExecuteTurn();
			}
		}

		private void GenerateCombatUnit(List<UnitData> _datas)
		{
			// Count to check total alive unit
			playerCount = enemyCount = 0;

			foreach(var unit in _datas)
			{
				GameObject newUnitObj = Instantiate(unit.unitPrefabs);
				if (newUnitObj.TryGetComponent(out PlayerCombatUnit newPlayerUnit))
				{
					allCombatUnit.Add(newPlayerUnit);
					newPlayerUnit.transform.SetParent(playerParent_);
					playerCount++;
					playerUnit = newPlayerUnit;
				}
				else if(newUnitObj.TryGetComponent(out EnemyCombatUnit newEnemyUnit))
				{
					allCombatUnit.Add(newEnemyUnit);
					newEnemyUnit.transform.SetParent(enemyParent_);
					targetPickers[enemyCount].transform.SetParent(newEnemyUnit.transform);
					targetPickers[enemyCount].transform.localPosition = Vector3.zero;
					targetPickers[enemyCount].AssignTarget(newEnemyUnit);
					targetPickers[enemyCount].OnTargetPicked += ExecutePlayerAttack;
					enemyCount++;
					allEnemies.Add(newEnemyUnit);
				}

				allCombatUnit[^1].InitializeUnit(unit);
				allCombatUnit[^1].OnUnitDead += RemoveUnitFromCombat;
			}

			healthHandler_.SetCombatUnits();
		}

		private void SetTargetPicker(bool _forceClose = false)
		{
			foreach (var picker in targetPickers)
			{
				picker.ActivatePicker(_forceClose);
			}
		}

		private void ExecutePlayerAttack(ICombatUnit _target)
		{
			playerUnit.AttackTarget(_target);
			StartCoroutine(DelayEachAction());
		}

		private void RemoveUnitFromCombat(ICombatUnit _unit)
		{
			_unit.OnUnitDead -= RemoveUnitFromCombat;
			if (_unit.IsPlayer)
			{
				playerCount--;
				if(playerCount <= 0)
				{
					// Lose
					currState = CombatState.LoseBattle;

					// Do something when losing
					Debug.Log("LOSE");
					UIPopUpManager.Instance.OpenPopUp(PopUpUIID.GameOverPopUp);
				}
			}
			else
			{
				enemyCount--;
				if (enemyCount <= 0)
				{
					// Win
					currState = CombatState.WonBattle;

					//Do something when winning
					Debug.Log("WIN");
					GlobalDataRef.Instance.enemyCombatTarget.Defeated();
					StartCoroutine(DelayBackExplore());
				}
			}
			
			allCombatUnit.Remove(_unit);
			turnBaseHandler_.RemoveUnitFromOrder(_unit);
		}

		private void EscapeCombat()
		{
			Debug.Log("RUNNING AWAY!");
			StartCoroutine(DelayBackExplore());
		}

		private IEnumerator DelayBackExplore()
		{
			yield return new WaitForSeconds(.5f);
			TransitionHandler.Instance.FadeInAnim();
			yield return new WaitForSecondsRealtime(.5f);
			GameEvents.Instance.BackToExplore();
			SceneManager.LoadSceneAsync(EXPLORE_SCENE_NAME);
		}
	}
}
