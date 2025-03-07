using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	public class ItemManager : MonoBehaviour
	{
		public static ItemManager Instance { get; private set; }

		[SerializeField] private List<ItemData> items = new();

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

		public ItemData GetItem(ItemID _itemId)
		{
			foreach(var item in items)
			{
				if (item.id == _itemId)
					return item;
			}

			Debug.Log($"Item with \"{_itemId}\" id not found... returning \"{items[0].id}\" instead");
			return items[0];
		}

		public ItemData GetRandomItem()
		{
			return items[Random.Range(0, items.Count)];
		}
	}
}
