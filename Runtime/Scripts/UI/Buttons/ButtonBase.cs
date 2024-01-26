using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace StdNounou.UI
{
	public class ButtonBase : MonoBehaviour
	{
		[field: SerializeField] public Button ButtonTarget { get; private set; }
		[SerializeField] private TextMeshProUGUI buttonText;

        private void Reset()
        {
            ButtonTarget = this.GetComponent<Button>();
			buttonText = this.GetComponentInChildren<TextMeshProUGUI>();
        }

		public void SetText(string text)
		{
#if UNITY_EDITOR
			Undo.RegisterCompleteObjectUndo(this.gameObject, "Rename Button Name");
            this.gameObject.name = text + " Btn";
#endif
			this.buttonText.text = text;
#if UNITY_EDITOR
			EditorUtility.SetDirty(this.buttonText);
#endif
		}

		public void AddListenerToOnClick(UnityAction listener)
			=> ButtonTarget.onClick.AddListener(listener);
		public void RemoveListenerToOnClick(UnityAction listener)
			=> ButtonTarget.onClick.RemoveListener(listener);
	} 
}
