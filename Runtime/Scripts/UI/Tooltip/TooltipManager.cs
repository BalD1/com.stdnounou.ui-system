using StdNounou.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StdNounou.UI
{
    public class TooltipManager : PersistentSingleton<TooltipManager>
    {
        [SerializeField] private Tooltip tooltipPF;

        [SerializeField] private Canvas targetCanvas;

        protected override void EventsSubscriber()
        {
            base.EventsSubscriber();
            TooltipTriggerEvents.OnTooltipStartHover += CreateTooltip;
        }

        protected override void EventsUnSubscriber()
        {
            base.EventsUnSubscriber();
            TooltipTriggerEvents.OnTooltipStartHover -= CreateTooltip;
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
        }

        protected override void OnSceneUnloaded(Scene scene)
        {
        }

        public void CreateTooltip(TooltipTrigger tooltipTrigger)
        {
            Tooltip result = tooltipPF.Create(targetCanvas.transform).Setup(tooltipTrigger.TooltipData);
            tooltipTrigger.SetRelatedTooltip(result);
        }
    } 
}