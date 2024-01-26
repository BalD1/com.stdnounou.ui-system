using System;
using UnityEngine;
using StdNounou.Core;

namespace StdNounou.UI
{
    [CreateAssetMenu(fileName = "New Tween Color Player", menuName = "Scriptable/Tweens/Color Player")]
    public class SO_TweenPlayerColor_SR : SO_TweenPlayerBase
    {
        [field: SerializeField] public Color OriginColor { get; private set; } = Color.white;
        [field: SerializeField] public Color GoalColor { get; private set; } = Color.white;
        [field: SerializeField] public bool AllowAlpha { get; private set; }

        public override void ProcessTween(bool isReversed, TweenSequencePlayer.S_TweenData data, float playbackSpeed, Action OnEnd, GameObject leanTargetObj, out LTDescr tween)
        {
            SpriteRenderer spriteRenderer = data.Target as SpriteRenderer;
            tween = null;

            Color fromColor = Color.white;
            Color toColor = Color.white;

            if (isReversed)
            {
                fromColor = GoalColor * data.Modifiers.GoalModifier;
                toColor = OriginColor * data.Modifiers.OriginModifier;
            }
            else
            {
                fromColor = OriginColor * data.Modifiers.OriginModifier;
                toColor = GoalColor * data.Modifiers.GoalModifier;
            }

            if (spriteRenderer.color == toColor)
            {
                OnEnd?.Invoke();
                return;
            }

            if (AllowAlpha) tween = spriteRenderer.LeanColor(toColor, Duration * data.Modifiers.DurationModifier);
            else
            {
                tween = LeanTween.value(leanTargetObj, fromColor, toColor, GetFinalDuration(data.Modifiers.DurationModifier, playbackSpeed))
                                 .setEase(isReversed ? data.ReversedLeanType : data.ForwardLeanType)
                                 .setOnUpdate((Color c) =>
                                 {
                                     c.a = spriteRenderer.color.a;
                                     spriteRenderer.color = c;
                                 });
            }

            tween.setOnComplete(OnEnd);
        }

        public override void SetAtEnd(Component target, TweenSequencePlayer.S_Modifiers modifiers)
        {
            SpriteRenderer spriteRenderer = target as SpriteRenderer;
            if (spriteRenderer == null) return;

            if (AllowAlpha)
                spriteRenderer.color = GoalColor * modifiers.GoalModifier;
            else
            {
                Color c = GoalColor * modifiers.GoalModifier;
                c.a = spriteRenderer.color.a;
                spriteRenderer.color = c;
            }
        }

        public override void SetAtStart(Component target, TweenSequencePlayer.S_Modifiers modifiers)
        {
            SpriteRenderer spriteRenderer = target as SpriteRenderer;
            if (spriteRenderer == null) return;

            if (AllowAlpha)
                spriteRenderer.color = OriginColor * modifiers.OriginModifier;
            else
            {
                Color c = OriginColor * modifiers.OriginModifier;
                c.a = spriteRenderer.color.a;
                spriteRenderer.color = c;
            }
        }
    } 
}