using UnityEditor;
using UnityEngine;

namespace StdNounou.UI
{
    public class UITabButton : MonoBehaviour
    {
        [field: SerializeField] public  UITabElement RelatedScreen { get; private set; }
        [field: SerializeField] public ButtonBase RelatedButton { get; private set; }
        [field: SerializeField] public UITabsHandler RelatedTabsHandler { get; private set;}

        [SerializeField] private int index;

        private void Reset()
        {
            RelatedButton = this.GetComponent<ButtonBase>();
            RelatedTabsHandler = this.GetComponentInParent<UITabsHandler>();
        }

        private void Awake()
        {
            RelatedButton.AddListenerToOnClick(OnClick);
        }

        public void SetElement(UITabElement screen)
        {
            RelatedScreen = screen;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void Setup(int idx)
        {
            index = idx;
        }

        public void SetActiveState(bool state, bool playTween)
        {
            if (state) RelatedScreen.Open(playTween);
            else RelatedScreen.Close(playTween);
        }

        private void OnClick()
        {
            RelatedTabsHandler.SetActiveTab(index, true);
        }
    } 
}
