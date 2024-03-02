using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StdNounou.UI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [field: SerializeField] public SO_TooltipData TooltipData { get; private set; }
        private Tooltip relatedTooltip;

        private LTDescr showDelayTween;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (showDelayTween != null)
                LeanTween.cancel(showDelayTween.uniqueId);
            showDelayTween = LeanTween.delayedCall(TooltipData.DelayBeforeShow, () =>
            {
                this.OnTooltipStartHover_Call();
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (showDelayTween != null)
                LeanTween.cancel(showDelayTween.uniqueId);
            relatedTooltip?.TryHide();
        }

        public void SetRelatedTooltip(Tooltip tooltip)
        {
            this.relatedTooltip = tooltip;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            relatedTooltip.SetAsStatic();
        }
    }
}
