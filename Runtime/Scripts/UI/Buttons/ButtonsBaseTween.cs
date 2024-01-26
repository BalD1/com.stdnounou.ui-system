using UnityEngine;
using UnityEngine.EventSystems;
using StdNounou.Core;
using static StdNounou.UI.IEffectPlayer;

namespace StdNounou.UI
{
    public class ButtonsBaseTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEffectPlayer
    {
        [SerializeField] private RectTransform rectTransform;

        [Header("Scale Components")]
        [SerializeField] private LeanTweenType tweenType = LeanTweenType.easeInOutBack;
        [SerializeField] private float duration = 0.5f;

        [SerializeField] private Vector2 baseScale;
        [SerializeField] private Vector2 scaleTarget = Vector2.one;

        private LTDescr currentTween;
        private bool isPaused;

        private IEffectPlayer.E_EffectState state = IEffectPlayer.E_EffectState.Stopped;

        private bool isReversed = false;
        private float playbackSpeed = 1;
        private float delta = 0;

        private void Reset()
        {
            rectTransform = this.gameObject.GetComponent<RectTransform>();
            baseScale = rectTransform.localScale;
            scaleTarget = Vector2.one * 1.5f;

            duration = 0.5f;

            tweenType = LeanTweenType.easeInOutBack;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayForward();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayReverse();
        }

        private void TweenScale(Vector3 target, bool reversed)
        {
            isReversed = reversed;
            state = IEffectPlayer.E_EffectState.Playing;
            float finalDuration = CancelCurrentTweenAndGetDuration();

            currentTween = LeanTween.value(rectTransform.gameObject, rectTransform.localScale, target, duration).setOnUpdate((Vector2 val) =>
            {
                rectTransform.localScale = val;

                if (isReversed)
                    delta -= Time.deltaTime;
                else
                    delta += Time.deltaTime;

            }).setOnComplete(() => currentTween = null);
        }

        private float CancelCurrentTweenAndGetDuration()
        {
            if (currentTween == null) return duration;
            Vector2 currentScale = rectTransform.localScale;
            LeanTween.cancel(currentTween.uniqueId);
            rectTransform.localScale = currentScale;
            return delta;
        }

        public bool TryPlayForward(E_StartAndStopBehaviour startBehaviour = E_StartAndStopBehaviour.Stay, bool stopIfPlaying = false)
        {
            PlayForward();
            return true;
        }

        public void PlayForward()
        {
            if (currentTween == null) delta = 0;
            TweenScale(scaleTarget, false);
        }

        public void Pause()
        {
            state = IEffectPlayer.E_EffectState.Paused;
            isPaused = true;
            if (currentTween != null)
                currentTween.pause();
        }

        public void Resume()
        {
            state = IEffectPlayer.E_EffectState.Playing;
            isPaused = false;
            if (currentTween != null)
                currentTween.resume();
        }

        public void Stop(E_StartAndStopBehaviour afterStopState, bool breakLoop, bool callDelegate = false)
        {
            state = IEffectPlayer.E_EffectState.Stopped;
            if (currentTween != null)
                LeanTween.cancel(currentTween.uniqueId);

            switch (afterStopState)
            {
                case IEffectPlayer.E_StartAndStopBehaviour.SetAtStart:
                    rectTransform.localScale = baseScale;
                    break;

                case IEffectPlayer.E_StartAndStopBehaviour.SetAtEnd:
                    rectTransform.localScale = scaleTarget;
                    break;
            }
        }

        public bool IsPlaying()
        {
            return currentTween != null;
        }

        public bool IsPaused()
        {
            return isPaused;
        }

        public bool TryPlayReverse(E_StartAndStopBehaviour startBehaviour = E_StartAndStopBehaviour.Stay, bool stopIfPlaying = false)
        {
            PlayReverse();
            return true;
        }

        public void PlayReverse()
        {
            if (currentTween == null) delta = 1;
            TweenScale(baseScale, true);
        }

        public void SetPlaybackSpeed(float speed)
        {
            playbackSpeed = speed;
            if (currentTween != null)
                currentTween.speed = speed;
        }

        public IEffectPlayer.E_EffectState GetEffectState()
        {
            return state;
        }

        public void SetLoopType(E_LoopType loopType, int count = -1)
        {
            this.LogError("This does not uses tweens loops.");
        }

        public void SetLoopCount(int count)
        {
            this.LogError("This does not uses tweens loops.");
        }
    } 
}
