namespace RPGTest
{
	public interface IInteractable
	{
		public bool CanInteract { get; set; }
		public void Interact();
		public void SetAsTarget();
		public void SetNonTarget();
	}
}
