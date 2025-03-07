using UnityEngine;

namespace RPGTest
{
	public class GlobalDataRef : MonoBehaviour
	{
		public static GlobalDataRef Instance { get; private set; }

		public GameObject player;
		public float playerHealth;

		private void Awake()
		{
			//singleton
			if (Instance != null && Instance != this)
				Destroy(gameObject);
			else
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
		}
	}
}
