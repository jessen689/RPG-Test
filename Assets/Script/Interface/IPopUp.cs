namespace RPGTest
{
	public interface IPopUp 
	{
		public PopUpUIID Id { get; }
		public void OpenPopUp(params object[] parameter);
		public void ClosePopUp();
	}
}
