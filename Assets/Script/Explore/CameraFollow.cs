using UnityEngine;

namespace RPGTest
{
	public class CameraFollow : MonoBehaviour
	{
		[SerializeField] private Transform followTarget_;
		[SerializeField] private float followSpeed_;
		[SerializeField] private float smoothValue_;
		[SerializeField] private Vector3 offset_;

		// Limits
		public float topLimit { get; set; }
		public float bottomLimit { get; set; }
		public float leftLimit { get; set; }
		public float rightLimit { get; set; }

		private Vector3 finalPos;
		private Vector3 velocity = Vector3.zero;

		private void Start()
		{
			followTarget_ = GlobalDataRef.Instance.player.transform;
		}

		private void FixedUpdate()
		{
			finalPos = followTarget_.position + offset_;
			finalPos.z = transform.position.z;

			finalPos.x = finalPos.x >= rightLimit ? rightLimit : finalPos.x;
			finalPos.x = finalPos.x <= leftLimit ? leftLimit : finalPos.x;
			finalPos.y = finalPos.y >= topLimit ? topLimit : finalPos.y;
			finalPos.y = finalPos.y <= bottomLimit ? bottomLimit : finalPos.y;

			transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref velocity, smoothValue_);
		}
	}
}
