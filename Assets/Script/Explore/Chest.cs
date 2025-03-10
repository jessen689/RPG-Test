using System.Collections;
using UnityEngine;

namespace RPGTest
{
	public class Chest : MonoBehaviour, IInteractable
	{
		[SerializeField] private bool isRandomItem_;
		[SerializeField] private ItemID itemObtain_;
		[SerializeField] private Animator animator_;

		private bool isOpened = false;

		public bool CanInteract { get; set; } = true;

		private const string SELECTED_ANIM_STRING = "Chest_Selected";
		private const string UNSELECTED_ANIM_STRING = "Chest_Default";
		private const string OPEN_ANIM_STRING = "Chest_Open";

		public void Interact()
		{
			if (!CanInteract)
				return;

			CanInteract = false;
			isOpened = true;
			animator_.Play(OPEN_ANIM_STRING);
		}

		public void SetAsTarget()
		{
			if(!isOpened)
				animator_.Play(SELECTED_ANIM_STRING);
		}

		public void SetNonTarget()
		{
			if(!isOpened)
				animator_.Play(UNSELECTED_ANIM_STRING);
		}

		#region Called in Animation Event
		//used in animation event
		private void RemoveGO()
		{
			gameObject.SetActive(false);
		}

		private void OpenItemWindowTrigger()
		{
			if (isRandomItem_)
			{
				Debug.Log($"Get : {ItemManager.Instance.GetRandomItem().name}");
				UIPopUpManager.Instance.OpenPopUp(PopUpUIID.ItemPopUp, ItemManager.Instance.GetRandomItem());
			}
			else
			{
				Debug.Log($"Get : {ItemManager.Instance.GetItem(itemObtain_).name}");
				UIPopUpManager.Instance.OpenPopUp(PopUpUIID.ItemPopUp, ItemManager.Instance.GetItem(itemObtain_));
			}
		}
		#endregion
	}
}
