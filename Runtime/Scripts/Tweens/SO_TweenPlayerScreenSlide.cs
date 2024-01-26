using System;
using UnityEngine;
using StdNounou.Core;

namespace StdNounou.UI
{
    [CreateAssetMenu(fileName = "NewTweenScreenSlidePlayer", menuName = "Scriptable/Tweens/ScreenSlide Player")]
    public class SO_TweenPlayerScreenSlide : SO_TweenPlayerBase
    {
        [SerializeField] private Vector2 screenSizeMultiplier = Vector2.one;

        [SerializeField] private E_HDirection inHorizontalDir = E_HDirection.None;
        [SerializeField] private E_VDirection inVerticalDir = E_VDirection.None;

        [SerializeField] private E_HDirection outHorizontalDir = E_HDirection.None;
        [SerializeField] private E_VDirection outVerticalDir = E_VDirection.None;

        private enum E_HDirection
        {
            None,
            Left,
            Right,
        }

        private enum E_VDirection
        {
            None,
            Down,
            Up
        }

        public override void ProcessTween(bool isReversed, TweenSequencePlayer.S_TweenData data, float playbackSpeed, Action OnEnd, GameObject leanTargetObj, out LTDescr tween)
        {
            Vector2 startPos = BuildPosition(isReversed, inHorizontalDir, inVerticalDir) * data.Modifiers.OriginModifier * screenSizeMultiplier;
            Vector2 targetPos = BuildPosition(!isReversed, outHorizontalDir, outVerticalDir) * data.Modifiers.GoalModifier * screenSizeMultiplier;
            RectTransform rt = data.Target.GetComponent<RectTransform>();

            if (rt.position.x != startPos.x ||
                rt.position.y != startPos.y)
            {
                ForceSetPos(startPos, rt);
            }

            tween = rt.LeanMove((Vector3)targetPos, GetFinalDuration(data.Modifiers.DurationModifier, playbackSpeed))
                      .setEase(isReversed ? data.ReversedLeanType : data.ForwardLeanType)
                      .setOnComplete(OnEnd);
        }

        public override void SetAtEnd(Component target, TweenSequencePlayer.S_Modifiers modifiers)
        {
            ForceSetPos(BuildPosition(true, inHorizontalDir, inVerticalDir), target.GetComponent<RectTransform>());
        }

        public override void SetAtStart(Component target, TweenSequencePlayer.S_Modifiers modifiers)
        {
            ForceSetPos(BuildPosition(false, outHorizontalDir, outVerticalDir), target.GetComponent<RectTransform>());
        }

        private Vector2 BuildPosition(bool isReversed, E_HDirection horDirection, E_VDirection verDirection)
        {
            Vector2 res = Vector2.zero;
            Vector2 screenSize = UIScreensManager.CanvasSize;
            switch (horDirection)
            {
                case E_HDirection.Left:
                    res.x = isReversed ? 0 : -screenSize.x;
                    break;
                case E_HDirection.Right:
                    res.x = isReversed ? 0 : screenSize.x;
                    break;
            }
            switch (verDirection)
            {
                case E_VDirection.Down:
                    res.y = isReversed ? 0 : -screenSize.y;
                    break;
                case E_VDirection.Up:
                    res.y = isReversed ? 0 : screenSize.y;
                    break;
            }

            return res;
        }

        private void ForceSetPos(Vector2 pos, RectTransform target)
        {
            target.SetLeft(pos.x);
            target.SetRight(pos.x * -1);
            target.SetBottom(pos.y);
            target.SetTop(pos.y * -1);
        }
    } 
}