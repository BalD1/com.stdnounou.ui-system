using StdNounou.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StdNounou.UI
{
    public class TooltipManager : PersistentSingleton<TooltipManager>
    {
        [SerializeField] private Tooltip tooltip;

        [SerializeField] private float delayBeforeShow = .5f;

        private LTDescr showDelayTween;

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
        }

        protected override void OnSceneUnloaded(Scene scene)
        {
        }

        public void Show(SO_TooltipData tooltipData)
        {
            if (showDelayTween != null)
                LeanTween.cancel(showDelayTween.uniqueId);
            showDelayTween = LeanTween.delayedCall(delayBeforeShow, () =>
            {
                tooltip.SetText(tooltipData);
                tooltip.Show();
            });
        }

        public void Hide()
        {
            if (showDelayTween != null)
                LeanTween.cancel(showDelayTween.uniqueId);
            tooltip.Hide();
        }
    } 
}