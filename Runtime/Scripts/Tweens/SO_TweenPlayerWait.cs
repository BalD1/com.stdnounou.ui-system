using System;
using UnityEngine;

namespace StdNounou.UI
{
    [CreateAssetMenu(fileName = "NewTweenDelayPlayer", menuName = "StdNounou/Scriptables/Tweens/Delay Player", order = 100)]
    public class SO_TweenPlayerWait : SO_TweenPlayerBase
    {
        public override void ProcessTween(bool isReversed, TweenSequencePlayer.S_TweenData data, float playbackSpeed, Action OnEnd, GameObject leanTargetObj, out LTDescr tween)
        {
            tween = LeanTween.delayedCall(leanTargetObj, GetFinalDuration(data.Modifiers.DurationModifier, playbackSpeed), OnEnd);
        }

        public override void SetAtEnd(Component target, TweenSequencePlayer.S_Modifiers modifiers)
        {
        }

        public override void SetAtStart(Component target, TweenSequencePlayer.S_Modifiers modifiers)
        {
        }
    }
}