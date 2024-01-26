using StdNounou.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using static StdNounou.UI.IEffectPlayer;

namespace StdNounou.UI
{
	public class TweenSequencePlayer : MonoBehaviour, IEffectPlayer
	{
        [field: SerializeField] public float PlaybackSpeed { get; private set; } = 1;
        [field: SerializeField] public E_EffectState State { get; private set; } = E_EffectState.Stopped;
        [field: SerializeField] public E_StartAndStopBehaviour AfterStopState { get; private set; } = E_StartAndStopBehaviour.Stay;
        [field: SerializeField] public bool IsReversed { get; private set; }

        [field: SerializeField] public E_LoopType LoopType { get; private set; }
        [field: SerializeField] public int LoopCount { get; private set; } = -1;

        [field: SerializeField] public bool PlayAtStart { get; private set; } = false;

        public event Action OnSequenceEnded;

        [System.Serializable]
        public class S_TweenData
        {
#if UNITY_EDITOR
            [SerializeField] public string editorName;
#endif
            [field: SerializeField] public SO_TweenPlayerBase TweenPlayer { get; private set; }
            [field: SerializeField] public Component Target { get; private set; }
            [field: SerializeField] public LeanTweenType ForwardLeanType { get; private set; } = LeanTweenType.easeInOutSine;
            [field: SerializeField] public LeanTweenType ReversedLeanType { get; private set; } = LeanTweenType.easeInOutSine;
            [field: SerializeField] public S_Modifiers Modifiers { get; private set; } = new S_Modifiers(1, 1, 1);
        }

        [System.Serializable]
        public class S_Modifiers
        {
            public S_Modifiers(float originModifier, float goalModifier, float durationModifier)
            {
                this.OriginModifier = originModifier;
                this.GoalModifier = goalModifier;
                this.DurationModifier = durationModifier;
            }
            [field: SerializeField] public float OriginModifier { get; private set; } = 1;
            [field: SerializeField] public float GoalModifier { get; private set; } = 1;
            [field: SerializeField] public float DurationModifier { get; private set; } = 1;
        }

        [System.Serializable]
        public struct S_TweensDataContainer
        {
#if UNITY_EDITOR
            [SerializeField] private string editorName;
#endif
            [field: SerializeField] public S_TweenData[] TweensData { get; private set; }
        }

        [field: SerializeField] public S_TweensDataContainer[] TweensSequence { get; private set; }

        private S_TweenData[] currentSequence;
        private List<LTDescr> currentTweens = new List<LTDescr>();

        [SerializeField, HideInInspector] private int currentFinishedPlayers;
        [SerializeField, HideInInspector] private int currentTweenIndex;

        private void Awake()
        {
            if (!PlayAtStart) return;

            if (IsReversed) TryPlayReverse();
            else TryPlayForward();
        }

        public virtual bool TryPlayForward(E_StartAndStopBehaviour startBehaviour = E_StartAndStopBehaviour.Stay, bool stopIfPlaying = false)
        {
            return TryBegin(startBehaviour, stopIfPlaying, PlayForward);
        }

        public virtual void PlayForward()
            => StartTween(false);

        public void Pause()
        {
            foreach (var item in currentTweens)
                item.pause();

            State = IEffectPlayer.E_EffectState.Paused;
        }

        public void Resume()
        {
            if (IsPlaying()) return;

            foreach (var item in currentTweens)
                item.resume();
            State = IEffectPlayer.E_EffectState.Playing;
        }

        public virtual void Stop(E_StartAndStopBehaviour afterStopState, bool breakLoop, bool callDelegate = true)
        {
            foreach (var item in currentTweens)
                item.pause();

            State = E_EffectState.Stopped;
            currentTweens.Clear();
            SetState(afterStopState);
            if (callDelegate) OnSequenceEnded?.Invoke();

            if (!breakLoop)
                TryPerformLoop(afterStopState);
        }

        private void TryPerformLoop(E_StartAndStopBehaviour afterStopState)
        {
            if (LoopCount != -1)
            {
                LoopCount--;
                if (LoopCount == 0) return;
            }
            switch (LoopType)
            {
                case E_LoopType.Loop:
                    if (IsReversed) TryPlayReverse(E_StartAndStopBehaviour.SetAtEnd, true);
                    else TryPlayForward(E_StartAndStopBehaviour.SetAtStart, true);
                    break;

                case E_LoopType.PingPong:
                    if (IsReversed) TryPlayForward(E_StartAndStopBehaviour.SetAtStart, true);
                    else TryPlayReverse(E_StartAndStopBehaviour.SetAtEnd, true);
                    break;
            }
        }

        public void SetLoopType(E_LoopType loopType, int count = -1)
        {
            this.LoopType = loopType;
            this.LoopCount = count;
        }
        public void SetLoopCount(int count)
        {
            this.LoopCount = count;
        }

        protected void SetState(E_StartAndStopBehaviour afterStopState)
        {
            switch (afterStopState)
            {
                case E_StartAndStopBehaviour.SetAtStart:
                    for (int i = TweensSequence.Length - 1; i >= 0; i--)
                    {
                        foreach (var data in TweensSequence[i].TweensData)
                            data.TweenPlayer.SetAtStart(data.Target, data.Modifiers);
                    }
                     break;

                case E_StartAndStopBehaviour.SetAtEnd:
                    foreach (var item in TweensSequence)
                    {
                        foreach (var data in item.TweensData)
                            data.TweenPlayer.SetAtEnd(data.Target, data.Modifiers);
                    }
                    break;
            }
        }

        public bool IsPlaying()
            => currentTweens.Count > 0 &&
               State == E_EffectState.Playing;

        public bool IsPaused()
            => State == E_EffectState.Paused;

        public bool TryPlayReverse(E_StartAndStopBehaviour startBehaviour = E_StartAndStopBehaviour.SetAtEnd, bool stopIfPlaying = false)
        {
            return TryBegin(startBehaviour, stopIfPlaying, PlayReverse);
        }

        public virtual void PlayReverse()
            => StartTween(true);

        private bool TryBegin(E_StartAndStopBehaviour startBehaviour, bool stopIfPlaying, Action behaviour)
        {
            if (IsSequenceNullOrEmpty())
            {
                this.LogError("Tween sequence was null or empty.");
                return false;
            }
            if (IsPlaying())
            {
                if (!stopIfPlaying) return false;
                Stop(startBehaviour, true, false);
            }
            else SetState(startBehaviour);

            behaviour?.Invoke();
            return true;
        }

        public void SetPlaybackSpeed(float speed)
        {
            PlaybackSpeed = speed;
            if (currentTweens.Count != currentSequence.Length) return;
            for (int i = 0; i < currentSequence.Length; i++)
            {
                if (currentTweens[i] == null) continue;
                currentTweens[i].setSpeed(currentSequence[i].TweenPlayer.GetFinalDuration(currentSequence[i].Modifiers.DurationModifier, PlaybackSpeed));
            }
        }

        protected virtual void TweenEnded()
        {
            Stop(AfterStopState, false);
        }

        public E_EffectState GetEffectState()
            => State;

        private void StartTween(bool reverse)
        {
            State = E_EffectState.Playing;
            IsReversed = reverse;

            SetupCurrent();
        }

        private void SetupCurrent()
        {
            currentTweenIndex = IsReversed ? TweensSequence.Length : -1;
            PlayNextTween();
        }

        private void OnSingleTweenEnded()
        {
            currentFinishedPlayers++;
            if (currentFinishedPlayers >= currentSequence.Length)
                PlayNextTween();
        }
        protected void PlayNextTween()
        {
            if (TweensSequence == null || TweensSequence.Length == 0) return;

            if (IsReversed)
            {
                currentTweenIndex--;
                if (currentTweenIndex < 0)
                {
                    TweenEnded();
                    return;
                }
            }
            else
            {
                currentTweenIndex++;
                if (currentTweenIndex >= TweensSequence.Length)
                {
                    TweenEnded();
                    return;
                }
            }

            SetCurrentTween();
        }

        private void SetCurrentTween()
        {
            currentFinishedPlayers = 0;
            currentSequence = TweensSequence[currentTweenIndex].TweensData;
            currentTweens.Clear();
            if (currentSequence == null || currentSequence.Length == 0) OnSingleTweenEnded();
            foreach (var data in currentSequence)
            {
                LTDescr tween = null;
                data.TweenPlayer.ProcessTween(IsReversed, data, PlaybackSpeed, OnSingleTweenEnded, this.gameObject, out tween);
                if (tween != null) currentTweens.Add(tween);
            }
        }

        public bool IsSequenceNullOrEmpty()
            => TweensSequence == null || TweensSequence.Length == 0;
    }
} 

