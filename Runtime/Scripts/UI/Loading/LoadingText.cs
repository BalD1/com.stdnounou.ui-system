using System.Collections;
using TMPro;
using UnityEngine;

namespace StdNounou.UI
{
    public class LoadingText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp;

        [SerializeField] private string baseText = "Loading";
        [SerializeField] private string finalText = "Loading...";
        private string currentText;

        [SerializeField] private float waitTime = 1;

        private WaitForSeconds waitForSeconds;

        private int currentTextIndex;
        private bool isFinalTextLonger;

        private void Reset()
        {
            tmp = this.GetComponent<TextMeshProUGUI>();
        }

        private void Awake()
        {
            isFinalTextLonger = finalText.Length > baseText.Length;
            string smallestText = isFinalTextLonger ? baseText : finalText;

            currentText = smallestText;
            currentTextIndex = currentText.Length;

            waitForSeconds = new WaitForSeconds(waitTime);
            StartCoroutine(Animation());
        }

        private IEnumerator Animation()
        {
            string fromText = "";
            string toText = "";

            if (isFinalTextLonger)
            {
                fromText = baseText;
                toText = finalText;
            }
            else
            {
                fromText = finalText;
                toText = baseText;
            }

            while(true)
            {
                yield return waitForSeconds;

               if (currentTextIndex >= toText.Length)
               {
                   currentText = fromText;
                   currentTextIndex = currentText.Length;
               }
               else
               {
                   currentText += toText[currentTextIndex];
                   currentTextIndex++;
               }

                tmp.text = currentText;
            }
        }

        public void UpdateWaitTime(float waitTime)
        {
            this.waitTime = waitTime;
            waitForSeconds = new WaitForSeconds(waitTime);
        }
    } 
}
