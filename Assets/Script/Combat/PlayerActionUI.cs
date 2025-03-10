using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGTest
{
	public class PlayerActionUI : MonoBehaviour
	{
		[SerializeField] private Button attackBtn_;
		[SerializeField] private Button skillBtn_;
		[SerializeField] private Button defendBtn_;
		[SerializeField] private Button runBtn_;
		[SerializeField] private GameObject skillWindow_;
		[SerializeField] private Transform skillContent_;
		[SerializeField] private SkillButton skillButtonPrefabs_;

		// playerSkills and skillButtons count must be same/equal
		private List<Skill> playerSkills= new();
		private List<SkillButton> skillButtons = new();

		public event Action<UnitActionID, SkillID> OnActionConfirmed;
		private UnitActionID selectedAction;
		private SkillID selectedSkill;
		private bool isInitialized = false;

		private void OnEnable()
		{
			skillWindow_.SetActive(false);
		}

		private void Awake()
		{
			attackBtn_.onClick.AddListener(() => { ActionSelected(UnitActionID.Attack); });
			skillBtn_.onClick.AddListener(() => { ActionSelected(UnitActionID.Skill); });
			defendBtn_.onClick.AddListener(() => { ActionSelected(UnitActionID.Defend); });
			runBtn_.onClick.AddListener(() => { ActionSelected(UnitActionID.Run); });
		}

		private void ActionSelected(UnitActionID _actionID)
		{
			selectedAction = _actionID;
			if(_actionID != UnitActionID.Skill)
				OnActionConfirmed?.Invoke(selectedAction, selectedSkill);
			skillWindow_.SetActive(selectedAction == UnitActionID.Skill);
		}

		public void SetupAllSkill(List<Skill> _skills)
		{
			if (!isInitialized)
			{
				playerSkills = new(_skills);
				foreach(var ps in playerSkills)
				{
					skillButtons.Add(Instantiate(skillButtonPrefabs_, skillContent_));
					skillButtons[^1].SetSkillToBtn(ps.skillData);
					skillButtons[^1].OnSelected += SkillSelected;
				}
				isInitialized = true;
			}
		}

		private void SkillSelected(SkillID _skill)
		{
			selectedSkill = _skill;
			OnActionConfirmed?.Invoke(selectedAction, selectedSkill);
		}

		public void UpdateSkillCDStat()
		{
			for(int i = 0; i < skillButtons.Count; i++)
			{
				skillButtons[i].ActivateButton(playerSkills[i].cooldownCount <= 0);
			}
		}
	}
}
