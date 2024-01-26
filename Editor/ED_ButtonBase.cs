using UnityEditor;
using StdNounou.Core.Editor;
using UnityEngine;

namespace StdNounou.UI.Editor
{
    [CustomEditor(typeof(ButtonBase))]
    public class ED_ButtonBase : UnityEditor.Editor
    {
        private ButtonBase targetScript;
        private string textToSet;

        private void OnEnable()
        {
            targetScript = (ButtonBase)target;
        }

        public override void OnInspectorGUI()
        {
            ReadOnlyDraws.EditorScriptDraw(typeof(ED_ButtonBase), this);
            base.DrawDefaultInspector();

            using (var textSetter = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Set Text"))
                    targetScript.SetText(textToSet);
                textToSet = EditorGUILayout.TextField(textToSet);
            }

            serializedObject.ApplyModifiedProperties();
        }
    } 
}