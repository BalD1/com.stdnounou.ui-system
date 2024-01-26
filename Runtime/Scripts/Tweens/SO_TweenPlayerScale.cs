using StdNounou.Core;
using System;
using UnityEngine;

namespace StdNounou.UI
{
[CreateAssetMenu(fileName = "NewTweenScalePlayer", menuName = "Scriptable/Tweens/Scale Player")]
    public class SO_TweenPlayerScale : SO_TweenPlayerBase
    {
        [field: SerializeField] public Vector2 OriginScale = Vector2.one;
        [field: SerializeField] public Vector2 GoalScale = new Vector2(2, 2);

        public override void ProcessTween(bool isReversed, TweenSequencePlayer.S_TweenData data, float playbackSpeed, Action OnEnd, GameObject leanTargetObj, out LTDescr tween)
        {
            tween = null;

            Vector2 toScale = Vector2.zero;

            if (isReversed) toScale = OriginScale * data.Modifiers.OriginModifier;
            else toScale = GoalScale * data.Modifiers.GoalModifier;
            tween = data.Target.transform.LeanScale(toScale, GetFinalDuration(data.Modifiers.DurationModifier, playbackSpeed))
                                         .setEase(isReversed ? data.ReversedLeanType : data.ForwardLeanType)
                                         .setOnComplete(OnEnd);
        }

        public override void SetAtStart(Component target, TweenSequencePlayer.S_Modifiers modifiers)
        {
            target.transform.SetScale(OriginScale * modifiers.GoalModifier);
        }

        public override void SetAtEnd(Component target, TweenSequencePlayer.S_Modifiers modifiers)
        {
            target.transform.SetScale(GoalScale * modifiers.GoalModifier);
        }
    } 
}