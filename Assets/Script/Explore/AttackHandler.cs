using UnityEngine;

namespace RPGTest
{
	public class AttackHandler : MonoBehaviour
	{
		[SerializeField] private KeyCode keyToAttack_;
		[SerializeField] private Animator animator_;
		[SerializeField] private float attackRange_;
		[SerializeField] private LayerMask whatIsEnemy_;

		private bool canAttack = true;
		private bool isAttacking = false;

		private const string ATTACK_STRING_ANIM = "Attack";

		private void OnEnable()
		{
			GameEvents.Instance.OnBackToExplore -= () => { isAttacking = false; };
			GameEvents.Instance.OnBackToExplore += () => { isAttacking = false; };
		}

		private void Update()
		{
			if(canAttack && !isAttacking)
			{
				if (Input.GetKeyDown(keyToAttack_))
				{
					isAttacking = true;
					animator_.SetBool(ATTACK_STRING_ANIM, true);
				}
			}
		}

		public void EnableAttack(bool _value)
		{
			canAttack = _value;
		}

		#region Called in Anim event
		private void CheckAttackHit()
		{
			var hit = Physics2D.OverlapCircle(transform.position, attackRange_, whatIsEnemy_);
			if (hit != null)
			{
				if (hit.TryGetComponent(out EnemyBehaviour target))
				{
					if(target.IsFacingLeft == GlobalDataRef.Instance.player.isFacingLeft)
						GameEvents.Instance.EnterCombat(1.5f, target);
					else
						GameEvents.Instance.EnterCombat(1f, target);
				}
			}

			animator_.SetBool(ATTACK_STRING_ANIM, false);
		}

		private void AttackFinished()
		{
			isAttacking = false;
		}
		#endregion
	}
}
