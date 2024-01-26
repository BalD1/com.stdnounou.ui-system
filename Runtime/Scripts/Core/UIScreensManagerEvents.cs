using System;

namespace StdNounou.UI
{
	public static class UIScreensManagerEvents
	{
		public static event Action OnEnteredUI;
		public static void EnteredUI(this UIScreensManager manager)
			=> OnEnteredUI?.Invoke();

		public static event Action OnExitedUI;
		public static void ExitedUI(this UIScreensManager manager)
			=> OnExitedUI?.Invoke();
	} 
}