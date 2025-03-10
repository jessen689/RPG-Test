using System;

namespace RPGTest
{
	public class GameEvents
	{
		// Singleton
		private static readonly GameEvents instance = new GameEvents();
		public static GameEvents Instance { get { return instance; } }

		// Events
		public event Action OnRetryGame;
		public event Action<float, EnemyBehaviour> OnEnterCombat;
		public event Action OnBackToExplore;
		public event Action<PlaceID> OnLoadMap;

		// Methods
		public void RetryGame()
		{
			OnRetryGame?.Invoke();
		}

		public void EnterCombat(float _speedMulti, EnemyBehaviour _target)
		{
			OnEnterCombat?.Invoke(_speedMulti, _target);
		}

		public void BackToExplore()
		{
			OnBackToExplore?.Invoke();
		}

		public void LoadMap(PlaceID _dest)
		{
			OnLoadMap?.Invoke(_dest);
		}
	}
}
