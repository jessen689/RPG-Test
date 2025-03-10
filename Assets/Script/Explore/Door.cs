using UnityEngine;

namespace RPGTest
{
	public class Door : MonoBehaviour, IInteractable
	{
		[SerializeField] private PlaceID destID_;
		[SerializeField] private Animator animator_;

		public bool CanInteract { get; set; } = true;

		private bool isOpened = false;

		private const string OPEN_ANIM_STRING = "Door_Open";
		private const string SELECTED_ANIM_STRING = "Door_Select";
		private const string DEFAULT_ANIM_STRING = "Door_Default";

		public void Interact()
		{
			isOpened = true;
			animator_.Play(OPEN_ANIM_STRING);
		}

		public void SetAsTarget()
		{
			if (!isOpened)
				animator_.Play(SELECTED_ANIM_STRING);
		}

		public void SetNonTarget()
		{
			if (!isOpened)
				animator_.Play(DEFAULT_ANIM_STRING);
		}

		#region Called in Anim Event
		private void LoadDestination()
		{
			GameEvents.Instance.LoadMap(destID_);
			isOpened = false;
		}
		#endregion
	}
}
