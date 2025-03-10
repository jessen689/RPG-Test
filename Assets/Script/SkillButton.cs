using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPGTest
{
	[RequireComponent(typeof(Button))]
	public class SkillButton : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI skillNameText_;
		[SerializeField] private TMPro.TextMeshProUGUI skillMultiText_;

		private Button btn;
		private SkillData skill;
		public event Action<SkillID> OnSelected;

		private void Awake()
		{
			if (btn == null)
				btn = GetComponent<Button>();
			btn.onClick.AddListener(SkillSelected);
		}

		public void SetSkillToBtn(SkillData _value)
		{
			skill = _value;
			skillNameText_.text = skill.skillName;
			skillMultiText_.text = $"Multi : {skill.skillMultiplier}x";
		}

		public void ActivateButton(bool _value)
		{
			if(btn == null)
				btn = GetComponent<Button>();
			btn.interactable = _value;
		}

		private void SkillSelected()
		{
			OnSelected?.Invoke(skill.skillID);
		}
	}
}
