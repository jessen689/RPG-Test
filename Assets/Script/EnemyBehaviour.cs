using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGTest
{
	public class EnemyBehaviour : MonoBehaviour
	{
		[SerializeField] private EnemyID id_;
		[SerializeField] private float moveSpeed_;
		[SerializeField] private float chaseRange_;
		[SerializeField] private float attackRange_;
		[SerializeField] private LayerMask whatIsPlayer_;
		[SerializeField] private float maxStayDuration_;
		[SerializeField] private float maxPatrolDuration_;
		[SerializeField] private float maxDistanceChase_;
		[SerializeField] private SpriteRenderer spriteRenderer_;
		[SerializeField] private Animator animator_;

		private enum BehaviourState
		{
			Stay,
			Patrol,
			Chase,
			Attack
		}

		private BehaviourState currState;
		private Vector2 moveDirection;
		private bool isStopMoving;
		private bool isRandomDirectionObtained;
		private float durationCounter;

		private const string ATTACK_ANIM_STRING = "Attack";
		private const string MOVE_ANIM_STRING = "Move";

		private void Awake()
		{
			currState = BehaviourState.Stay;
		}

		private void FixedUpdate()
		{
			//set state
			if (Physics2D.OverlapCircle(transform.position, chaseRange_, whatIsPlayer_) && currState != BehaviourState.Attack)
			{
				durationCounter = 0;
				currState = BehaviourState.Chase;
			}

			if (currState == BehaviourState.Chase && Vector2.Distance(transform.position, GlobalDataRef.Instance.player.transform.position) >= maxDistanceChase_)
				currState = BehaviourState.Stay;
			else if (currState == BehaviourState.Chase && Physics2D.OverlapCircle(transform.position, attackRange_, whatIsPlayer_))
				currState = BehaviourState.Attack;

			if(currState == BehaviourState.Stay || currState == BehaviourState.Patrol)
			{
				durationCounter += Time.fixedDeltaTime;
				if (currState == BehaviourState.Stay && durationCounter > maxStayDuration_)
				{
					durationCounter = 0f;
					currState = BehaviourState.Patrol;
				}
				else if (currState == BehaviourState.Patrol && durationCounter > maxPatrolDuration_)
				{
					durationCounter = 0f;
					currState = BehaviourState.Stay;
				}
			}

			//execute state
			if (currState == BehaviourState.Stay)
				StandStill();
			else if (currState == BehaviourState.Patrol)
				Patrol();
			else if (currState == BehaviourState.Chase)
				ChasePlayer();
			else if (currState == BehaviourState.Attack)
				Attack();
		}

		private void Patrol()
		{
			isStopMoving = false;
			if (!isRandomDirectionObtained)
			{
				moveDirection.x = UnityEngine.Random.Range(-1f, 1f);
				moveDirection.y = UnityEngine.Random.Range(-1f, 1f);
				isRandomDirectionObtained = true;
			}
			Move(moveDirection.normalized);
		}

		private void Attack()
		{
			Debug.Log("attacking");
			animator_.SetBool(ATTACK_ANIM_STRING, true);
		}

		private void ChasePlayer()
		{
			isStopMoving = false;
			isRandomDirectionObtained = false;
			moveDirection = GlobalDataRef.Instance.player.transform.position - transform.position;
			Move(moveDirection.normalized);
			Debug.Log("chasing");
		}

		private void Move(Vector2 _finalDirection)
		{
			if (isStopMoving)
				return;

			transform.Translate(moveSpeed_ * Time.fixedDeltaTime * _finalDirection);
			if (moveDirection.normalized.x < 0)
				spriteRenderer_.flipX = true;
			else if (moveDirection.normalized.x > 0)
				spriteRenderer_.flipX = false;
			animator_.SetBool(MOVE_ANIM_STRING, true);
		}

		private void StandStill()
		{
			isStopMoving = true;
			isRandomDirectionObtained = false;
			animator_.SetBool(MOVE_ANIM_STRING, false);
			animator_.SetBool(ATTACK_ANIM_STRING, false);
		}
	}

}
