using StdNounou.Core;
using System;
using UnityEngine;

namespace StdNounou.UI
{
    [CreateAssetMenu(fileName = "NewTweenAlphaPlayer", menuName = "StdNounou/Scriptables/Tweens/Alpha Player", order = 100)]
    public class SO_TweenPlayerAlpha : SO_TweenPlayerBase
    {
        [field: SerializeField] public float OriginAlpha = 0;
        [field: SerializeField] public float GoalAlpha = 1;

        public override void ProcessTween(bool isReversed, TweenSequencePlayer.S_TweenData data, float playbackSpeed, Action OnEnd, GameObject leanTargetObj, out LTDescr tween)
        {
            AlphaHandler alphaHandler = data.Target as AlphaHandler;
            tween = null;

            float toAlpha = 0;

            if (isReversed) toAlpha = OriginAlpha * data.Modifiers.OriginModifier;
            else toAlpha = GoalAlpha * data.Modifiers.GoalModifier;

            if (alphaHandler.GetAlpha() == toAlpha)
            {
                OnEnd?.Invoke();
                return;
            }

            tween = alphaHandler.LeanAlpha(toAlpha, GetFinalDuration(data.Modifiers.DurationModifier, playbackSpeed))
                                .setEase(isReversed ? data.ReversedLeanType : data.ForwardLeanType);
            tween.optional.onComplete += OnEnd;
        }

        public override void SetAtStart(Component target, TweenSequencePlayer.S_Modifiers modifiers)
        {
            AlphaHandler alphaHandler = target as AlphaHandler;
            if (alphaHandler == null) return;

            alphaHandler.SetAlpha(OriginAlpha * modifiers.OriginModifier);
        }

        public override void SetAtEnd(Component target, TweenSequencePlayer.S_Modifiers modifiers)
        {
            AlphaHandler alphaHandler = target as AlphaHandler;
            if (alphaHandler == null) return;

            alphaHandler.SetAlpha(GoalAlpha * modifiers.GoalModifier);
        }
    }
}