using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGTest
{
	public class ExploreManager : MonoBehaviour
	{
		private const string COMBAT_SCENE_NAME = "CombatScene";

		private void OnEnable()
		{
			GameEvents.Instance.OnEnterCombat += EnteringCombatScene;
		}

		private void OnDisable()
		{
			GameEvents.Instance.OnEnterCombat -= EnteringCombatScene;
		}

		private void EnteringCombatScene(float _speed, EnemyBehaviour _target)
		{
			GlobalDataRef.Instance.playerSpeedMulti = _speed;
			GlobalDataRef.Instance.enemyCombatTarget = _target;
			StartCoroutine(DelayCombatScene());
		}

		private IEnumerator DelayCombatScene()
		{
			TransitionHandler.Instance.FadeInAnim();
			yield return new WaitForSecondsRealtime(.5f);
			SceneManager.LoadSceneAsync(COMBAT_SCENE_NAME);
		}
	}
}
