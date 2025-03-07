using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	public class UIPopUpManager : MonoBehaviour
	{
		public static UIPopUpManager Instance { get; private set; }

		[SerializeField] private List<PopUpReference> popUpReferences = new();
		private Dictionary<PopUpUIID, IPopUp> cachedPopUps = new();

		[Serializable] private struct PopUpReference
		{
			public PopUpUIID id;
			public GameObject popUpPrefabs;
		}

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

		public void OpenPopUp(PopUpUIID _id, params object[] _parameters)
		{
			if (!cachedPopUps.ContainsKey(_id))
			{
				foreach(var popUp in popUpReferences)
				{
					if(popUp.id == _id)
					{
						if(Instantiate(popUp.popUpPrefabs).TryGetComponent<IPopUp>(out var cachedPopUp))
						{
							cachedPopUps.Add(_id, cachedPopUp);
							break;
						}
					}
				}
			}

			cachedPopUps[_id].OpenPopUp(_parameters);
		}
	}
}
