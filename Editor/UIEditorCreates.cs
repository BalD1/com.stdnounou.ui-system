using System;
using TMPro;
using StdNounou.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace StdNounou.UI.Editor
{
    public static class UIEditorCreates
    {
        [MenuItem("GameObject/UI/Custom TMP button", priority = 10)]
        public static GameObject CreateCustomTMPButton(MenuCommand menuCommand)
            => CreateCustomTMPButton(menuCommand.context as GameObject);
        public static GameObject CreateCustomTMPButton(GameObject parent)
        {
            return CreatePrefab("UIButton", parent, ManualButtonCreation);
        }

        public static GameObject ManualButtonCreation(GameObject parent)
        {
            GameObject button = new GameObject
            {
                name = "Button",
                layer = 5
            };
            GameObjectUtility.SetParentAndAlign(button, parent);
            Image btnImg = button.AddComponent<Image>();
            //SO_SpritesHolder spritesHolder = Resources.Load<SO_SpritesHolder>("Assets/BaseUnitySpritesHolder");
            //btnImg.sprite = spritesHolder.GetAsset("UISprite");
            btnImg.type = Image.Type.Sliced;
            button.AddComponent<Button>();
            button.AddComponent<ButtonsBaseTween>();
            RectTransform buttonRt = button.GetComponent<RectTransform>();
            buttonRt.sizeDelta = new Vector2(160, 30);

            GameObject btnText = new GameObject()
            {
                name = "Text",
                layer = 5
            };

            TextMeshProUGUI txtComp = btnText.AddComponent<TextMeshProUGUI>();
            txtComp.text = "Button";
            txtComp.color = Color.black;
            txtComp.alignment = TextAlignmentOptions.Center;
            txtComp.fontSize = 24;
            //txtComp.autoSizeTextContainer = true;

            RectTransform btnTextRT = btnText.GetComponent<RectTransform>();
            GameObjectUtility.SetParentAndAlign(btnText, button);
            btnTextRT.pivot = new Vector2(.5f, .5f);
            btnTextRT.anchorMin = new Vector2(0, 0);
            btnTextRT.anchorMax = new Vector2(1, 1);

            btnTextRT.SetOffset(0, 0, 0, 0);

            Selection.SetActiveObjectWithContext(button, button);

            Undo.RegisterCreatedObjectUndo(button, "Create Button");
            Undo.RegisterCreatedObjectUndo(btnText, "Create Button");
            EditorApplication.RepaintHierarchyWindow();

            return button;
        }

        [MenuItem("GameObject/UI/Tabs Handler", priority = 10)]
        public static GameObject CreateTabsHandler(MenuCommand menuCommand)
            => CreateTabsHandler(menuCommand.context as GameObject);
        public static GameObject CreateTabsHandler(GameObject parent)
        {
            return CreatePrefab("UITabsHandler", parent as GameObject, null);
        }

        [MenuItem("GameObject/UI/UITab", priority = 10)]
        public static GameObject CreateUITabButton(MenuCommand menuCommand)
            => CreateUITabButton(menuCommand.context as GameObject);
        public static GameObject CreateUITabButton(GameObject parent)
        {
            return CreatePrefab("UITabButton", parent, null);
        }

        [MenuItem("GameObject/UI/UITabElement", priority = 10)]
        public static GameObject CreateUITabElement(MenuCommand menuCommand)
            => CreateUITabElement(menuCommand.context as GameObject);
        public static GameObject CreateUITabElement(GameObject parent)
        {
            return CreatePrefab("UITabElement", parent, null);
        }

        [MenuItem("GameObject/UI/UISubScreen", priority = 10)]
        public static GameObject CreateUISubScreen(MenuCommand menuCommand)
            => CreateUISubScreen(menuCommand.context as GameObject);
        public static GameObject CreateUISubScreen(GameObject parent)
        {
            GameObject screen = new GameObject
            {
                name = "UISubScreen",
                layer = 5,
            };
            GameObjectUtility.SetParentAndAlign(screen, parent);
            screen.AddComponent<RectTransform>();
            screen.AddComponent<UISubScreen>();

            Selection.SetActiveObjectWithContext(screen, screen);
            Undo.RegisterCreatedObjectUndo(screen, "Create SubScreen");
            EditorApplication.RepaintHierarchyWindow();

            return screen;
        }

        [MenuItem("GameObject/UI/UIRootScreen", priority = 10)]
        public static GameObject CreateUIRootScreen(MenuCommand menuCommand)
    => CreateUIRootScreen(menuCommand.context as GameObject);
        public static GameObject CreateUIRootScreen(GameObject parent)
        {
            GameObject screen = new GameObject
            {
                name = "UIRootScreen",
                layer = 5,
            };
            GameObjectUtility.SetParentAndAlign(screen, parent);
            screen.AddComponent<RectTransform>();
            screen.AddComponent<UIRootScreen>();

            Selection.SetActiveObjectWithContext(screen, screen);
            Undo.RegisterCreatedObjectUndo(screen, "Create RootScreen");
            EditorApplication.RepaintHierarchyWindow();

            return screen;
        }

        private static GameObject CreatePrefab(string objID, GameObject parent, Func<GameObject, GameObject> onPrefabNullFunc)
        {
            SO_PrefabsHolder prefabs = ResourcesObjectLoader.GetUIPrefabsHolder();
            if (prefabs == null)
            {
                return onPrefabNullFunc?.Invoke(parent);
            }

            GameObject instance = prefabs.CreateNewGameObject(objID, true);
            if (instance == null) return onPrefabNullFunc?.Invoke(parent);

            GameObjectUtility.SetParentAndAlign(instance, parent);
            Selection.SetActiveObjectWithContext(instance, instance);
            Undo.RegisterCreatedObjectUndo(instance, "Create " + objID);
            EditorApplication.RepaintHierarchyWindow();
            return instance;
        }

    } 
}