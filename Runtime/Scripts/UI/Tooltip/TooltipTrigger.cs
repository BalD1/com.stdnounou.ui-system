using UnityEngine;
using UnityEngine.EventSystems;

namespace StdNounou.UI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private SO_TooltipData tooltipData;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Instance.Show(tooltipData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.Hide();
        }
    }
}
