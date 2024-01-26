using UnityEngine;
using StdNounou.Core;

namespace StdNounou.UI
{
	public class UITabsHandler : MonoBehaviourEventsHandler
	{
        [field: SerializeField] public UIScreen ScreenParent {  get; private set; }
        [field: SerializeField] public Transform TabButtonsParent { get; private set; }
        [field: SerializeField] public Transform TabElementsParent { get; private set; }

        public UITabButton[] Tabs { get; private set; }

		private UITabButton currentActiveTab;
        private int currentTabIndex = -1;

        private void Reset()
        {
            TabButtonsParent = this.transform;
            ScreenParent = this.GetComponentInParent<UIScreen>();
        }
        protected override void EventsSubscriber()
        {
            if (ScreenParent != null)
                ScreenParent.OnStateChanged += OnParentStateChanged;
        }

        protected override void EventsUnSubscriber()
        {
            if (ScreenParent != null)
                ScreenParent.OnStateChanged -= OnParentStateChanged;
        }

        protected override void Awake()
        {
            base.Awake();
            SetupArray();
        }

        private void Start()
        {
            SetActiveTab(0, true);
        }

        private void SetupArray()
        {
            Tabs = new UITabButton[TabButtonsParent.transform.childCount];
            for (int i = 0; i < Tabs.Length; i++)
            {
                Tabs[i] = TabButtonsParent.transform.GetChild(i).GetComponent<UITabButton>();
                if (Tabs[i] == null)
                {
                    this.LogError($"Could not get {nameof(UITabButton)} component in child n°{i}.");
                    continue;
                }
                Tabs[i].Setup(i);
                Tabs[i].SetActiveState(false, false);
            }
        }

        public void SetActiveTab(int idx, bool playOpenTween)
        {
            if (idx == currentTabIndex) return;
            if (idx >= Tabs.Length) return;

            currentTabIndex = idx;
            currentActiveTab?.SetActiveState(false, false);
            currentActiveTab = Tabs[idx];
            currentActiveTab.SetActiveState(true, playOpenTween);
        }

        private void OnParentStateChanged(UIScreen screen)
        {
            if (screen.IsOpened)
            {
                SetActiveTab(0, true);
            }
            else
            {
                currentActiveTab?.SetActiveState(false, false);
                currentTabIndex = -1;
            }
        }
    } 
}
