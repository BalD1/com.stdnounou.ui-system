using StdNounou.Core;
using System;
using UnityEngine;

namespace StdNounou.UI
{
    public class ScreenFade : MonoBehaviourEventsHandler
    {
        [SerializeField] private AlphaHandler alphaHandler;

        private LTDescr fadeDescr;

        private void Reset()
        {
            alphaHandler = this.GetComponent<AlphaHandler>();
        }

        protected override void EventsSubscriber()
        {
            ScreenFadeEvents.OnAskHideScreen += HideScreen;
            ScreenFadeEvents.OnAskShowScreen += ShowScreen;
        }

        protected override void EventsUnSubscriber()
        {
            ScreenFadeEvents.OnAskHideScreen -= HideScreen;
            ScreenFadeEvents.OnAskShowScreen += ShowScreen;
        }

        public void HideScreen()
            => HideScreen(1, null);
        public void HideScreen(Action onEnd)
            => HideScreen(1, onEnd);
        public void HideScreen(float duration, Action onEnd)
            => PerformFade(duration, 1, onEnd);

        public void ShowScreen()
            => ShowScreen(1, null);
        public void ShowScreen(Action onEnd)
            => ShowScreen(1, onEnd);
        public void ShowScreen(float duration, Action onEnd)
            => PerformFade(duration, 0, onEnd);

        private void PerformFade(float duration, float alphaGoal, Action onEnd)
        {
            if (alphaGoal == 1) this.StartedHideScreen();
            else onEnd += () => this.EndedShowScreen();

            if (fadeDescr != null)
                LeanTween.cancel(fadeDescr.uniqueId);
            fadeDescr = alphaHandler.LeanAlpha(alphaGoal, duration);
            fadeDescr.optional.onComplete += onEnd;
        }
    } 
}