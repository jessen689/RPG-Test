using UnityEngine;
using UnityEngine.UI;

namespace RPGTest
{
	public class PopUpItem : MonoBehaviour, IPopUp
	{
		[SerializeField] private PopUpUIID id_;
		[SerializeField] private Image itemImage_;
		[SerializeField] private TMPro.TextMeshProUGUI itemText_;
		[SerializeField] private Button okBtn_;

		public PopUpUIID Id { get { return id_; } }

		private ItemData currItem;

		private void Awake()
		{
			okBtn_.onClick.AddListener(ClosePopUp);
		}

		public void ClosePopUp()
		{
			gameObject.SetActive(false);
		}

		public void OpenPopUp(params object[] parameter)
		{
			currItem = (ItemData)parameter[0];
			itemText_.text = $"You obtained {currItem.itemName}!";
			itemImage_.sprite = currItem.itemSprite;

			gameObject.SetActive(true);
		}
	}
}
