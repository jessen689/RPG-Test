using UnityEngine;

namespace RPGTest
{
	[CreateAssetMenu(fileName = "Item", menuName = "Create Item", order = 0)]
	public class ItemData : ScriptableObject
	{
		public ItemID id;
		public string itemName;
		public Sprite itemSprite;
	}
}