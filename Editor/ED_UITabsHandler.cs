using UnityEditor;
using UnityEngine;
using StdNounou.Core.Editor;

namespace StdNounou.UI.Editor
{
    [CustomEditor(typeof(UITabsHandler))]
    public class ED_UITabsHandler : UnityEditor.Editor
    {
        private UITabsHandler targetScript;
        private string tabName = "Tab";

        private void OnEnable()
        {
            targetScript = (UITabsHandler)target;
        }

        public override void OnInspectorGUI()
        {
            ReadOnlyDraws.EditorScriptDraw(typeof(ED_UITabsHandler), this);
            base.DrawDefaultInspector();

            GUILayout.Space(5);
            CreateTab();
            if (targetScript.TabButtonsParent != null)
                if (targetScript.TabButtonsParent.childCount > 0) DestroyTab();

            serializedObject.ApplyModifiedProperties();
        }

        private void CreateTab()
        {
            GUILayout.BeginHorizontal();
            tabName = EditorGUILayout.TextField(tabName);
            if (GUILayout.Button("Create Tab"))
            {
                UITabButton btn = UIEditorCreates.CreateUITabButton(targetScript.TabButtonsParent.gameObject).GetComponent<UITabButton>();
                UITabElement element = UIEditorCreates.CreateUITabElement(targetScript.TabElementsParent.gameObject).GetComponent<UITabElement>();

                Undo.RegisterCreatedObjectUndo(btn, "Created tab btn");
                Undo.RegisterCreatedObjectUndo(element, "Created tab element");

                element.gameObject.name = tabName + " Element";

                btn.RelatedButton.SetText(tabName);
                btn.SetElement(element);
            }
            GUILayout.EndHorizontal();
        }

        private void DestroyTab()
        {
            GUILayout.BeginVertical("GroupBox");
            for (int i = 0; i < targetScript.TabButtonsParent.childCount; i++)
            {
                Transform child = targetScript.TabButtonsParent.GetChild(i);

                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
                {
                    Undo.DestroyObjectImmediate(child.GetComponent<UITabButton>().RelatedScreen.gameObject);
                    Undo.DestroyObjectImmediate(child.gameObject);

                    i--;
                }

                GUI.enabled = false;
                EditorGUILayout.ObjectField(child, typeof(UITabButton), true);
                GUI.enabled = true;

                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();
        }
    } 
}