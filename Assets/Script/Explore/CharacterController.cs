using UnityEngine;

namespace RPGTest
{
	public class CharacterController : MonoBehaviour
	{
		[SerializeField] private float moveSpeed_;
		[SerializeField] private Rigidbody2D rb2d_;
		[SerializeField] private SpriteRenderer spriteRenderer_;
		[SerializeField] private Animator animator_;

		private Vector2 moveDirection;
		public bool isFacingLeft { get; private set; } = false;

		private const string MOVE_ANIM_STRING = "Move";

		private void Update()
		{
			//movement input
			moveDirection = Vector2.zero;

			if (Input.GetKey(KeyCode.W))
				moveDirection += Vector2.up;

			if (Input.GetKey(KeyCode.A))
				moveDirection += Vector2.left;

			if (Input.GetKey(KeyCode.S))
				moveDirection += Vector2.down;

			if (Input.GetKey(KeyCode.D))
				moveDirection += Vector2.right;
		}

		private void FixedUpdate()
		{
			//move character
			transform.Translate(moveDirection.normalized * moveSpeed_ * Time.fixedDeltaTime);
			if (moveDirection.normalized.x < 0)
			 	isFacingLeft = spriteRenderer_.flipX = true;
			else if(moveDirection.normalized.x > 0)
				isFacingLeft = spriteRenderer_.flipX = false;

			animator_.SetBool(MOVE_ANIM_STRING, moveDirection != Vector2.zero);
		}
	}
}
