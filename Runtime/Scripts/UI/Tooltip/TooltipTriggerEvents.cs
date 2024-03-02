
using System;

namespace StdNounou.UI
{
    public static class TooltipTriggerEvents
    {
        public static event Action<TooltipTrigger> OnTooltipStartHover;
        public static void OnTooltipStartHover_Call(this TooltipTrigger trigger)
            => OnTooltipStartHover?.Invoke(trigger);
    }
}
