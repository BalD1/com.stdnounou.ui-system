using UnityEditor;
using StdNounou.Core.Editor;
using UnityEngine;

namespace StdNounou.UI.Editor
{
    [CustomEditor(typeof(UIScreensManager))]
    public class ED_UIScreensManager : UnityEditor.Editor
    {
        private UIScreensManager targetScript;

        private SerializedProperty CurrentRootScreen;
        private SerializedProperty OpenMainScreenIfLastWasMainScreen;

        private void OnEnable()
        {
            targetScript = (UIScreensManager)target;
            CurrentRootScreen = serializedObject.FindPropertyByAutoPropertyName(nameof(CurrentRootScreen));
            OpenMainScreenIfLastWasMainScreen = serializedObject.FindPropertyByAutoPropertyName(nameof(CurrentRootScreen));
        }

        public override void OnInspectorGUI()
        {
            ReadOnlyDraws.EditorScriptDraw(typeof(ED_UIScreensManager), this);
            base.DrawDefaultInspector();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(CurrentRootScreen);
            GUI.enabled = true;

            if (targetScript.OpenMainScreenWhenScreenStackIsEmpty)
                EditorGUILayout.PropertyField(OpenMainScreenIfLastWasMainScreen);

            serializedObject.ApplyModifiedProperties();
        }
    } 
}