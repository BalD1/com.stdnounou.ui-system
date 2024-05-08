using UnityEngine;
using UnityEngine.EventSystems;

namespace StdNounou.UI
{
    public class TooltipHeadbar : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        [SerializeField] private GameObject tooltip;
        private Vector2 posDiff;

        public void OnBeginDrag(PointerEventData eventData)
        {
            posDiff = (Vector2)tooltip.transform.position - eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            tooltip.transform.position = eventData.position + posDiff;
        }
    }
}
