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

        [SerializeField] private GameObject headbar;

        [SerializeField] private TweenSequencePlayer tweenPlayer;

        private bool isStatic = false;
        private bool canDestroy = false;

        public Tooltip Setup(SO_TooltipData tooltipData)
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

            Vector2 pos = Input.mousePosition;

            float pivotX = pos.x / Screen.width;
            float pivotY = pos.y / Screen.height;

            rt.pivot.Set(pivotX, pivotY);
            this.transform.position = pos;
            tweenPlayer.OnSequenceEnded += TryDestroy;
            this.Show();
            return this;
        }

        public void Show()
        {
            tweenPlayer.TryPlayForward(IEffectPlayer.E_StartAndStopBehaviour.SetAtStart, true);
        }

        public void TryHide()
        {
            if (isStatic) return;
            Hide();
        }
        public void Hide()
        {
            canDestroy = true;
            tweenPlayer.TryPlayReverse(IEffectPlayer.E_StartAndStopBehaviour.Stay, true);
        }

        public void TryDestroy()
        {
            if (!canDestroy)
            {
                if (isStatic)
                    cg.interactable = cg.blocksRaycasts = true;
                canDestroy = true;
                return;
            }
            Destroy(this.gameObject);
        }

        public void SetAsStatic()
        {
            isStatic = true;
            headbar.SetActive(true);
            cg.interactable = cg.blocksRaycasts = true;
        }

        private void ValidateLayoutElement()
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
        }

#if UNITY_EDITOR
        private void Update()
        {
            ValidateLayoutElement();
        }
#endif
    }
}
