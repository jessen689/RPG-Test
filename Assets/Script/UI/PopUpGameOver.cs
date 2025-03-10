using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPGTest
{
	public class PopUpGameOver : MonoBehaviour, IPopUp
	{
		public PopUpUIID Id { get; } = PopUpUIID.GameOverPopUp;

		[SerializeField] private Button retryBtn;
		[SerializeField] private Button quitBtn;

		private const string EXPLORE_SCENE_STRING = "ExploreScene";

		private void Awake()
		{
			retryBtn.onClick.AddListener(RetryGame);
			quitBtn.onClick.AddListener(QuitGame);
		}

		private void QuitGame()
		{
			Application.Quit();
		}

		private void RetryGame()
		{
			GameEvents.Instance.RetryGame();
			GlobalDataRef.Instance.enemyCombatTarget = null;
			StartCoroutine(DelayRetry());
		}

		public void ClosePopUp()
		{
			gameObject.SetActive(false);
		}

		public void OpenPopUp(params object[] parameter)
		{
			gameObject.SetActive(true);
		}

		private IEnumerator DelayRetry()
		{
			TransitionHandler.Instance.FadeInAnim();
			yield return new WaitForSecondsRealtime(.5f);
			ClosePopUp();
			SceneManager.LoadSceneAsync(EXPLORE_SCENE_STRING);
		}
	}
}
