using UnityEngine;

namespace StdNounou.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform loadingPrompt;
        [SerializeField] private RectTransform loadingCompletedScreen;

        public void OnLoadingCompleted()
        {
            loadingPrompt.gameObject.SetActive(false);
            loadingCompletedScreen.gameObject.SetActive(true);
        }
    } 
}
