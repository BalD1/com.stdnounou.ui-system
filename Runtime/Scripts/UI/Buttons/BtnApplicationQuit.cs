using UnityEngine;
using UnityEngine.UI;

namespace StdNounou.UI
{
    [RequireComponent(typeof(Button))]
    public class BtnApplicationQuit : MonoBehaviour
    {
        [SerializeField] private Button btn;

        private void Reset()
        {
            btn = this.GetComponent<Button>();
        }

        private void Awake()
        {
            btn.onClick.AddListener(() => Application.Quit());
        }
    } 
}