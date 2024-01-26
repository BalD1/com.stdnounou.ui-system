using System;
using UnityEngine;

namespace StdNounou.UI
{
    public abstract class SO_TweenPlayerBase : ScriptableObject
    {
        [field: SerializeField] public float Duration { get; private set; } = 1f;
        public abstract void ProcessTween(bool isReversed, TweenSequencePlayer.S_TweenData data, float playbackSpeed, Action OnEnd, GameObject leanTargetObj, out LTDescr tween);

        public abstract void SetAtStart(Component target, TweenSequencePlayer.S_Modifiers modifiers);
        public abstract void SetAtEnd(Component target, TweenSequencePlayer.S_Modifiers modifiers);

        public virtual float GetFinalDuration(float modifier, float playbackSpeed)
            => Duration * modifier / playbackSpeed;
    } 
}