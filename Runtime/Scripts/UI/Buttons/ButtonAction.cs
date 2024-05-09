using StdNounou.Core;
using UnityEngine;
using UnityEngine.UI;

namespace StdNounou.UI
{
    public abstract class ButtonAction : MonoBehaviourEventsHandler
    {
        [SerializeField] private Button targetButton;

        private void Reset()
        {
            targetButton = this.GetComponent<Button>();
        }

        protected override void EventsSubscriber()
        {
            targetButton.onClick.AddListener(OnClick);
        }

        protected override void EventsUnSubscriber()
        {
            targetButton.onClick.RemoveListener(OnClick);
        }

        protected abstract void OnClick();
    }
}
