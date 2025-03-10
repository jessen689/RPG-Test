using UnityEngine;

namespace RPGTest
{
	public class NPC : MonoBehaviour, IInteractable
	{
		[SerializeField] private string name_;
		[SerializeField] private GameObject bubbleBox_;
		[SerializeField] private string npcDialogue_;

		public bool CanInteract { get; set; } = true;

		public void Interact()
		{
			Debug.Log($"{name_} : {npcDialogue_}");
		}

		public void SetAsTarget()
		{
			bubbleBox_.SetActive(true);
		}

		public void SetNonTarget()
		{
			bubbleBox_.SetActive(false);
		}
	}
}
