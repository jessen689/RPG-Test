using UnityEngine;

namespace RPGTest
{
	public class CameraFollow : MonoBehaviour
	{
		[SerializeField] private Transform followTarget_;
		[SerializeField] private float followSpeed_;
		[SerializeField] private float smoothValue_;
		[SerializeField] private Vector3 offset_;

		[Header("Limit")]
		[SerializeField] private float topLimit_;
		[SerializeField] private float bottomLimit_;
		[SerializeField] private float leftLimit_;
		[SerializeField] private float rightLimit_;

		private Vector3 finalPos;
		private Vector3 velocity = Vector3.zero;

		private void FixedUpdate()
		{
			finalPos = followTarget_.position + offset_;
			finalPos.z = transform.position.z;

			finalPos.x = finalPos.x >= rightLimit_ ? rightLimit_ : finalPos.x;
			finalPos.x = finalPos.x <= leftLimit_ ? leftLimit_ : finalPos.x;
			finalPos.y = finalPos.y >= topLimit_ ? topLimit_ : finalPos.y;
			finalPos.y = finalPos.y <= bottomLimit_ ? bottomLimit_ : finalPos.y;

			transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref velocity, smoothValue_);
		}
	}
}
