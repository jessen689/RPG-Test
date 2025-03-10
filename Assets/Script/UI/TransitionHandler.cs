using UnityEngine;

namespace RPGTest
{
	public class TransitionHandler : MonoBehaviour
	{
		public static TransitionHandler Instance { get; private set; }

		[SerializeField] private Animator animator_;

		private bool isTransitioning = false;

		private const string FADE_IN_STRING = "CanvasTransition_In";
		private const string FADE_OUT_STRING = "CanvasTransition_Out";

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
		}

		public void FadeInAnim()
		{
			if (isTransitioning)
				return;

			animator_.Play(FADE_IN_STRING);
			Time.timeScale = 0f;
			isTransitioning = true;
		}

		public void FadeOutAnim()
		{
			animator_.Play(FADE_OUT_STRING);
		}

		#region Called in Anim event
		private void UnFreezeTime()
		{
			Time.timeScale = 1f;
			isTransitioning = false;
		}
		#endregion
	}
}
