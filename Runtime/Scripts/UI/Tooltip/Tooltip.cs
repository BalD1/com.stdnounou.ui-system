using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StdNounou.UI
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI headerField;
        [SerializeField] private TextMeshProUGUI contentField;

        [SerializeField] private LayoutElement layoutElement;

        [SerializeField] private int characterWrapLimit;

        [SerializeField] private RectTransform rt;
        [SerializeField] private CanvasGroup cg;

        [SerializeField] private TweenSequencePlayer tweenPlayer;

        public void SetText(SO_TooltipData tooltipData)
        {
            if (string.IsNullOrEmpty(tooltipData.Header))
                headerField.gameObject.SetActive(false);
            else
            {
                headerField.gameObject.SetActive(true);
                headerField.text = tooltipData.Header;
            }

            contentField.text = tooltipData.Content;
            ValidateLayoutElement();
        }

        public void Show()
        {
            tweenPlayer.TryPlayForward(IEffectPlayer.E_StartAndStopBehaviour.Stay, true);
        }

        public void Hide()
        {
            tweenPlayer.TryPlayReverse(IEffectPlayer.E_StartAndStopBehaviour.Stay, true);
        }

        private void ValidateLayoutElement()
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
        }

        private void Update()
        {
            ValidateLayoutElement();
            SetToMousePosition();
        } 

        private void SetToMousePosition()
        {
            if (cg.alpha == 0) return;
            Vector2 pos = Input.mousePosition;

            float pivotX = pos.x / Screen.width;
            float pivotY = pos.y / Screen.height;

            rt.pivot.Set(pivotX, pivotY);
            this.transform.position = pos;
        }
    }
}
