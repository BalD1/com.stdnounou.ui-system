using System;

namespace StdNounou.UI
{
	public static class UIScreenEvents
	{
        public static event Action<UIRootScreen> OnRootScreenOpened;
        public static void RootScreenOpened(this UIRootScreen root)
            => OnRootScreenOpened?.Invoke(root);

        public static event Action<UIRootScreen> OnRootScreenWillOpen;
        public static void RootScreenWillOpen(this UIRootScreen root)
            => OnRootScreenWillOpen?.Invoke(root);

        public static event Action<UIRootScreen> OnRootScreenClosed;
        public static void RootScreenClosed(this UIRootScreen root)
            => OnRootScreenClosed?.Invoke(root);

        public static event Action<UIRootScreen> OnRootScreenWillClose;
        public static void RootScreenWillClose(this UIRootScreen root)
            => OnRootScreenWillClose?.Invoke(root);

        public static event Action<UISubScreen> OnSubScreenStateChanged;
        public static void SubScreenStateChanged(this UISubScreen root)
            => OnSubScreenStateChanged?.Invoke(root);

    } 
}