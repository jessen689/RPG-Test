using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPGTest
{
	public class CombatTargetPicker : MonoBehaviour, IPointerClickHandler
	{
		private ICombatUnit target;

		public event Action<ICombatUnit> OnTargetPicked;

		public void ActivatePicker(bool _isForceClose)
		{
			if (target == null)
			{
				gameObject.SetActive(false);
				return;
			}

			if (!_isForceClose)
				gameObject.SetActive(target.Health > 0);
			else
				gameObject.SetActive(false);
		}

		public void AssignTarget(ICombatUnit _unit)
		{
			target = _unit;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			OnTargetPicked?.Invoke(target);
		}
	}
}
