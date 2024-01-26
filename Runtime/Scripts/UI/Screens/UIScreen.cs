using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StdNounou.Core;

namespace StdNounou.UI
{
    [RequireComponent(typeof(TweenSequencePlayer))]
    public abstract class UIScreen : MonoBehaviourEventsHandler, IHidableUI
    {
        [field: SerializeField] public TweenSequencePlayer SequencePlayer { get; private set; }
        [field: SerializeField] public bool IsOpened { get; private set; }

        public event Action<UIScreen> OnStateChanged;

        [SerializeField] public List<UISubScreen> OpenedSubscreens { get; private set; }
        [SerializeField] protected UISubScreen[] subScreens;

        public virtual void ED_SearchSubscreens()
        {
#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(this.gameObject, "Searched subscreens");
            subScreens = new UISubScreen[0];
            foreach (Transform child in this.transform)
            {
                if (!child.TryGetComponent<UISubScreen>(out UISubScreen subScreen)) continue;
                UISubScreen[] temp = new UISubScreen[subScreens.Length + 1];
                Array.Copy(subScreens, temp, subScreens.Length);
                temp[temp.Length - 1] = subScreen;
                subScreens = temp;
            }
            EditorUtility.SetDirty(this); 
#endif
        }


        private void Reset()
        {
            SequencePlayer = this.GetComponent<TweenSequencePlayer>();
        }

        protected override void EventsSubscriber()
        {
            if (SequencePlayer != null) 
                SequencePlayer.OnSequenceEnded += SequenceEnded;

            foreach (var item in subScreens)
            {
                if (item == null)
                {
                    this.LogError("Subscreen is null.");
                    continue;
                }
                item.OnStateChanged += OnSubscreenStateChanged;
            }
        }

        protected override void EventsUnSubscriber()
        {
            if (SequencePlayer != null)
                SequencePlayer.OnSequenceEnded -= SequenceEnded;

            foreach (var item in subScreens)
            {
                if (item == null)
                {
                    this.LogError("Subscreen is null.");
                    continue;
                }
                item.OnStateChanged -= OnSubscreenStateChanged;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            OpenedSubscreens = new List<UISubScreen>();
        }

        protected virtual void SequenceEnded()
        {
            IsOpened = !IsOpened;
            OnStateChanged?.Invoke(this);
            if (!IsOpened)
                this.gameObject.SetActive(false);
        }

        public void OpenCloseFlipFlop(bool playTween)
        {
            if (IsOpened) Close(playTween);
            else Open(playTween);
        }

        public virtual void Open(bool playTween)
        {
            if (IsOpened) return;
            this.gameObject.SetActive(true);

            if (playTween) TryPlaySequence();
            else SequenceEnded();
        }

        public virtual void Close(bool playTween)
        {
            if (!IsOpened) return;
            while (OpenedSubscreens.Count > 0)
            {
                UISubScreen last = OpenedSubscreens.LastElement();
                last.Close(false, false);
                OpenedSubscreens.Remove(last);
            }
            if (playTween) TryReverseSequence();
            else SequenceEnded();
        }

        protected void TryPlaySequence()
        {
            if (SequencePlayer.IsSequenceNullOrEmpty())
            {
                SequenceEnded();
                return;
            }
            SequencePlayer.TryPlayForward(IEffectPlayer.E_StartAndStopBehaviour.Stay, true);
        }

        protected void TryReverseSequence()
        {
            if (SequencePlayer.IsSequenceNullOrEmpty())
            {
                SequenceEnded();
                return;
            }
            SequencePlayer.TryPlayReverse(IEffectPlayer.E_StartAndStopBehaviour.Stay, true);
        }

        private void OnSubscreenStateChanged(UIScreen screen)
        {
            UISubScreen subScreen = screen as UISubScreen;
            if (subScreen == null) return;
            if (subScreen.IsOpened)
                OpenedSubscreens.Add(subScreen);
            else
                OpenedSubscreens.Remove(subScreen);
        }
    }
}
