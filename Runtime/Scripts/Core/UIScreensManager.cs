using System.Collections.Generic;
using StdNounou.Core;
using UnityEngine;

namespace StdNounou.UI
{
    public class UIScreensManager : Singleton<UIScreensManager>
    {
        [field: SerializeField] public Canvas MainCanvas {  get; private set; }
        public UIRootScreen CurrentRootScreen { get; private set; }
        [field: SerializeField] public bool OpenMainScreenAtStart { get; private set; }
        [field: SerializeField] public bool OpenMainScreenWhenScreenStackIsEmpty { get; private set; }
        public bool OpenMainScreenIfLastWasMainScreen { get; private set; }

        [field: SerializeField] public UIMainScreen MainScreen { get; private set; }
        public static Vector2 CanvasSize { get; private set; }

        public Stack<UIScreen> ScreensStack { get; private set; }

        protected override void EventsSubscriber()
        {
            UIScreenEvents.OnRootScreenOpened += OpenRootScreen;
            UIScreenEvents.OnRootScreenWillOpen += ReplaceOldRootScreen;

            UIScreenEvents.OnRootScreenClosed += OnRootScreenClosed;
            UIScreenEvents.OnRootScreenWillClose += OnRootScreenClosed;

            UIScreenEvents.OnSubScreenStateChanged += OnSubscreenStateChanged;
        }

        protected override void EventsUnSubscriber()
        {
            UIScreenEvents.OnRootScreenOpened -= OpenRootScreen;
            UIScreenEvents.OnRootScreenWillOpen -= ReplaceOldRootScreen;

            UIScreenEvents.OnRootScreenClosed -= OnRootScreenClosed;
            UIScreenEvents.OnRootScreenWillClose -= OnRootScreenClosed;

            UIScreenEvents.OnSubScreenStateChanged -= OnSubscreenStateChanged;
        }

        protected override void Awake()
        {
            ScreensStack = new Stack<UIScreen>();
            if (MainCanvas == null)
            {
                this.LogError("Main Canvas was not set.");
                MainCanvas = GameObject.FindFirstObjectByType<Canvas>();
            }
            if (MainCanvas != null)
            {
                Rect canvasRect = MainCanvas.GetComponent<RectTransform>().rect;
                CanvasSize = new Vector2(canvasRect.width, canvasRect.height);
            }
            base.Awake();

            if (OpenMainScreenAtStart)
                MainScreen?.Open(false);
        }

        public void CloseYoungest()
        {
            if (!ScreensStack.TryPeek(out UIScreen screen)) return;
            if (OpenMainScreenWhenScreenStackIsEmpty && 
                OpenMainScreenIfLastWasMainScreen && 
                screen == MainScreen) return;
            screen.Close(true);
        }

        private void OpenRootScreen(UIRootScreen newScreen)
        {
            ReplaceOldRootScreen(newScreen);
            if (ScreensStack.TryPeek(out UIScreen result))
            {
                if (result != newScreen) ScreensStack.Push(newScreen);
            }
            else ScreensStack.Push(newScreen);
        }

        private void ReplaceOldRootScreen(UIRootScreen newScreen)
        {
            if (newScreen == null) return;
            if (CurrentRootScreen == newScreen) return;
            UIRootScreen oldRootScreen = CurrentRootScreen;
            CurrentRootScreen = newScreen;
            oldRootScreen?.Close(true);
            ScreensStack.Clear();
        }

        private void OnRootScreenClosed(UIRootScreen screen)
        {
            if (screen != CurrentRootScreen) return;
            bool isMainScreen = screen == MainScreen;

            ScreensStack.Clear();
            CurrentRootScreen = null;
            if (!OpenMainScreenWhenScreenStackIsEmpty) return;
            if (isMainScreen && !OpenMainScreenIfLastWasMainScreen) return;
            MainScreen?.Open(true);
            CurrentRootScreen = MainScreen;
        }

        private void OnSubscreenStateChanged(UISubScreen screen)
        {
            if (screen.IsOpened)
            {
                ScreensStack.Push(screen);
                return;
            }
            if (ScreensStack.Count == 0) return;
            if (ScreensStack.Peek() != screen)
            {
                Stack<UIScreen> tmpScreensQueue = new Stack<UIScreen>();
                do tmpScreensQueue.Push(ScreensStack.Pop());
                while (ScreensStack.Count > 0 && ScreensStack.Peek() != screen);
                if (ScreensStack.Count > 0) ScreensStack.Pop();

                while (tmpScreensQueue.Count > 0)
                    ScreensStack.Push(tmpScreensQueue.Pop());

                return;
            }
            ScreensStack.Pop();
        }

    }
}