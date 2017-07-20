

namespace BevTree
{
	public enum Breakpoint
	{
		None = 1 << 1,
		OnOpen = 1 << 2,
		OnClose = 1 << 3,
		OnSuccess = 1 << 4,
		OnFailure = 1 << 5,
		//OnEnter = 1 << 6,
		//OnExit = 1 << 7
	}
}