using UnityEditor;
using StdNounou.Core.Editor;
using UnityEngine;

namespace StdNounou.UI.Editor
{
    [CustomEditor(typeof(TweenSequencePlayer))]
    public class ED_TweenSequencePlayer : UnityEditor.Editor
    {
        private TweenSequencePlayer targetScript;

        private bool debugMode;

        private SerializedProperty currentFinishedPlayers;
        private SerializedProperty currentTweenIndex;

        private IEffectPlayer.E_StartAndStopBehaviour stopBhvr;
        private bool stopBreakLoop;
        private bool stopCallDelegate;

        private IEffectPlayer.E_StartAndStopBehaviour reverseBhvr;
        private bool reverseStopIfPlaying;

        private float plbckSpeed;

        private void OnEnable()
        {
            targetScript = (TweenSequencePlayer)target;
            currentFinishedPlayers = serializedObject.FindProperty(nameof(currentFinishedPlayers));
            currentTweenIndex = serializedObject.FindProperty(nameof(currentTweenIndex));
        }

        public override void OnInspectorGUI()
        {
            ReadOnlyDraws.EditorScriptDraw(typeof(ED_TweenSequencePlayer), this);
            base.DrawDefaultInspector();

            debugMode = EditorGUILayout.Toggle("Debug Mode", debugMode);
            DebugVars();
            DebugBtns();

            serializedObject.ApplyModifiedProperties();
        }

        private void DebugVars()
        {
            if (!debugMode) return;
            SimpleDraws.HorizontalLine();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(currentFinishedPlayers);
            EditorGUILayout.PropertyField(currentTweenIndex);
            GUI.enabled = true;
        }

        private void DebugBtns()
        {
            if (!debugMode) return;
            if (!Application.isPlaying) return;
            SimpleDraws.HorizontalLine();
            if (GUILayout.Button("Pause"))
                targetScript.Pause();

            if (GUILayout.Button("Resume"))
                targetScript.Resume();

            using (var stopV = new EditorGUILayout.VerticalScope("GroupBox"))
            {
                stopBhvr = (IEffectPlayer.E_StartAndStopBehaviour)EditorGUILayout.EnumPopup(stopBhvr);
                using (var stopH = new EditorGUILayout.HorizontalScope())
                {
                    stopBreakLoop = EditorGUILayout.Toggle("Break Loop", stopBreakLoop);
                    stopCallDelegate = EditorGUILayout.Toggle("Call delegate", stopCallDelegate);
                }
                if (GUILayout.Button("Stop"))
                    targetScript.Stop(stopBhvr, stopBreakLoop, stopCallDelegate);
            }

            using (var reverseV = new EditorGUILayout.VerticalScope("GroupBox"))
            {
                using (var reverseH = new EditorGUILayout.HorizontalScope())
                {
                    reverseBhvr = (IEffectPlayer.E_StartAndStopBehaviour)EditorGUILayout.EnumPopup(reverseBhvr);
                    reverseStopIfPlaying = EditorGUILayout.Toggle("Call delegate", reverseStopIfPlaying);
                }
                if (GUILayout.Button("TryPlayReverse"))
                    targetScript.TryPlayReverse(reverseBhvr, reverseStopIfPlaying);
            }

            plbckSpeed = EditorGUILayout.FloatField("PlaybackSpeed", plbckSpeed);
            if (GUILayout.Button("SetPlaybackSpeed"))
                targetScript.SetPlaybackSpeed(plbckSpeed);
        }
    } 
}
