using UnityEditor;
using StdNounou.Core.Editor;
using UnityEngine;

namespace StdNounou.UI.Editor
{
    [CustomEditor(typeof(UIScreen), editorForChildClasses: true)]
    public class ED_UIScreen : UnityEditor.Editor
    {
        private UIScreen targetScript;

        private void OnEnable()
        {
            targetScript = (UIScreen)target;
        }

        public override void OnInspectorGUI()
        {
            ReadOnlyDraws.EditorScriptDraw(typeof(ED_UIScreen), this);
            base.DrawDefaultInspector();

            if (GUILayout.Button("Search SubScreens"))
            {
                targetScript.ED_SearchSubscreens();
            }

            serializedObject.ApplyModifiedProperties();
        }
    } 
}
