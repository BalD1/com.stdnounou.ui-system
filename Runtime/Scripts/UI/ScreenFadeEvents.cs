using System;
using StdNounou.Core;

namespace StdNounou.UI
{
    public static class ScreenFadeEvents
    {
        public static event Action<Action> OnAskHideScreen;
        public static void AskHideScreen(this IFadeCaller caller)
        {
            if (OnAskHideScreen == null)
            {
                CustomLogger.LogError(typeof(ScreenFadeEvents), $"{nameof(OnAskHideScreen)} was empty.");
                caller.OnHideScreenEnded();
                return;
            }
            OnAskHideScreen.Invoke(caller.OnHideScreenEnded);
        }
        public static event Action OnStartedHideScreen;
        public static void StartedHideScreen(this ScreenFade screenFade)
            => OnStartedHideScreen?.Invoke();

        public static event Action<Action> OnAskShowScreen;
        public static void AskShowScreen(this IFadeCaller caller)
        {
            if (OnAskShowScreen == null)
            {
                CustomLogger.LogError(typeof(ScreenFadeEvents), $"{nameof(OnAskShowScreen)} was empty.");
                caller.OnShowScreenEnded();
                return;
            }
            OnAskShowScreen?.Invoke(caller.OnShowScreenEnded);
        }

        public static event Action OnEndedShowScreen;
        public static void EndedShowScreen(this ScreenFade screenFade)
            => OnEndedShowScreen?.Invoke();
    } 
}