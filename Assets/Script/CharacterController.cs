using UnityEngine;

namespace RPGTest
{
	public class CharacterController : MonoBehaviour
	{
		[SerializeField] private float moveSpeed_;
		[SerializeField] private Rigidbody2D rb2d_;
		[SerializeField] private SpriteRenderer spriteRenderer_;

		private Vector2 moveDirection;

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
			transform.Translate(moveDirection.normalized * moveSpeed_ * Time.deltaTime);
			if (moveDirection.normalized.x < 0)
				spriteRenderer_.flipX = true;
			else if(moveDirection.normalized.x > 0)
				spriteRenderer_.flipX = false;
		}
	}
}
