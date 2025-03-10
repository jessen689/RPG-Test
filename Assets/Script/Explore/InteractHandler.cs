using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	public class InteractHandler : MonoBehaviour
	{
		[SerializeField] private KeyCode keyButtonToInteract_;

		private List<IInteractable> interactables = new();
		private IInteractable currTarget;

		private void Update()
		{
			if (Input.GetKeyDown(keyButtonToInteract_) && currTarget != null)
			{
				currTarget.Interact();
			}
		}

		private void UpdateInteractTarget(IInteractable _removedTarget = null)
		{
			currTarget = null;
			if (interactables.Count > 0)
			{
				for(int i = 0; i < interactables.Count; i++)
				{
					if(currTarget == null && interactables[i].CanInteract)
					{
						interactables[i].SetAsTarget();
						currTarget = interactables[i];
					}
					else
						interactables[i].SetNonTarget();
				}
			}
			else if(_removedTarget != null)
			{
				_removedTarget.SetNonTarget();
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.TryGetComponent(out IInteractable target))
			{
				interactables.Add(target);
				UpdateInteractTarget();
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if(collision.TryGetComponent(out IInteractable target))
			{
				if (interactables.Contains(target))
				{
					interactables.Remove(target);
					UpdateInteractTarget(target);
				}
			}
		}
	}
}
