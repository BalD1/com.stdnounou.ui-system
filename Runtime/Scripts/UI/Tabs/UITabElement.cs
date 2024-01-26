using UnityEngine;

namespace StdNounou.UI
{
    [RequireComponent(typeof(TweenSequencePlayer))]
    public class UITabElement : MonoBehaviour, IHidableUI
    {
        [field: SerializeField] public TweenSequencePlayer SequencePlayer {  get; private set; }

        public bool IsOpened { get; private set; }

        private void Reset()
        {
            SequencePlayer = this.GetComponent<TweenSequencePlayer>();
        }

        private void Awake()
        {
            if (IsOpened) SequencePlayer.Stop(IEffectPlayer.E_StartAndStopBehaviour.SetAtEnd, true, false);
            else SequencePlayer.Stop(IEffectPlayer.E_StartAndStopBehaviour.SetAtStart, true, false);
        }

        public void OpenCloseFlipFlop(bool playTween)
        {
            if (IsOpened) Close(playTween);
            else Open(playTween);
        }

        public void Open(bool playTween)
        {
            if (IsOpened) return;
            IsOpened = true;
            SequencePlayer.TryPlayForward(IEffectPlayer.E_StartAndStopBehaviour.SetAtStart, true);
        }

        public void Close(bool playTween)
        {
            if (!IsOpened) return;
            IsOpened = false;
            SequencePlayer.TryPlayReverse(IEffectPlayer.E_StartAndStopBehaviour.SetAtEnd, true);
        }
    } 
}