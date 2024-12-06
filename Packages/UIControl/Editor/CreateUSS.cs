using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using MuHua;

namespace MuHuaEditor.UIControl {
    public class CreateUSS : Editor {
        public static readonly string USS = "Packages/com.muhua.uicontrol/Assets/USS/";
        /// <summary>
        /// 创建自定义控件的USS
        /// </summary>
        /// <param name="ussResourcesPath"></param>
        /// <param name="name"></param>
        private static void USSCreate(string usspath, string name) {
            string selectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string filePath = $"{selectPath}/{name}.uss";
            AssetDatabase.CopyAsset(usspath + ".uss", filePath);
            AssetDatabase.Refresh();
            Object asset = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
            Selection.activeObject = asset;
        }
        [MenuItem("Assets/Create/UI Toolkit/Fonts USS")]
        private static void USSFonts() {
            USSCreate(USS + "Fonts", "Fonts");
        }

        [MenuItem("Assets/Create/UI Toolkit/ScrollView/ScrollerHorizontal USS")]
        private static void USSScrollerHorizontal() {
            USSCreate(USS + "ScrollView/ScrollerHorizontal", "ScrollerHorizontal");
        }
        [MenuItem("Assets/Create/UI Toolkit/ScrollView/ScrollerVertical USS")]
        private static void USSScrollerVertical() {
            USSCreate(USS + "ScrollView/ScrollerVertical", "ScrollerVertical");
        }
        [MenuItem("Assets/Create/UI Toolkit/ScrollView/ScrollView USS")]
        private static void USSScrollView() {
            USSCreate(USS + "ScrollView/ScrollView", "ScrollView");
        }
        [MenuItem("Assets/Create/UI Toolkit/ScrollView/ScrollViewHorizontal USS")]
        private static void USSScrollViewHorizontal() {
            USSCreate(USS + "ScrollView/ScrollViewHorizontal", "ScrollViewHorizontal");
        }
        [MenuItem("Assets/Create/UI Toolkit/ScrollView/ScrollViewVertical USS")]
        private static void USSScrollViewVertical() {
            USSCreate(USS + "ScrollView/ScrollViewVertical", "ScrollViewVertical");
        }

        [MenuItem("Assets/Create/UI Toolkit/Dropdown USS")]
        private static void USSDropdown() {
            USSCreate(USS + "Dropdown", "Dropdown");
        }
        [MenuItem("Assets/Create/UI Toolkit/Foldout USS")]
        private static void USSFoldout() {
            USSCreate(USS + "Foldout", "Foldout");
        }
        [MenuItem("Assets/Create/UI Toolkit/InputField USS")]
        private static void USSInputField() {
            USSCreate(USS + "InputField", "InputField");
        }
        [MenuItem("Assets/Create/UI Toolkit/PopupPrompt USS")]
        private static void USSPopupPrompt() {
            USSCreate(USS + "PopupPrompt", "PopupPrompt");
        }
        [MenuItem("Assets/Create/UI Toolkit/PopupWindow USS")]
        private static void USSPopupWindow() {
            USSCreate(USS + "PopupWindow", "PopupWindow");
        }
        [MenuItem("Assets/Create/UI Toolkit/Slider USS")]
        private static void USSSlider() {
            USSCreate(USS + "Slider", "Slider");
        }
        [MenuItem("Assets/Create/UI Toolkit/Toggle USS")]
        private static void USSToggle() {
            USSCreate(USS + "Toggle", "Toggle");
        }
    }
}
