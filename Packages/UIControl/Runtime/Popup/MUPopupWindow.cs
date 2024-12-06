using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MuHua {
    public class MUPopupWindow : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUPopupWindow, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            public UxmlStringAttributeDescription TitleText = new UxmlStringAttributeDescription {
                name = "Title-Text", defaultValue = "标题"
            };
            public UxmlStringAttributeDescription ButtonText = new UxmlStringAttributeDescription {
                name = "Button-Text", defaultValue = "确认"
            };
            public UxmlStringAttributeDescription AssetPath = new UxmlStringAttributeDescription {
                name = "Asset-Path"
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUPopupWindow popup = ve as MUPopupWindow;
                popup.titleText = TitleText.GetValueFromBag(bag, cc);
                popup.buttonText = ButtonText.GetValueFromBag(bag, cc);

                popup.assetPath = AssetPath.GetValueFromBag(bag, cc);
#if UNITY_EDITOR
                VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(popup.assetPath);
                if (asset != null) { popup.ReplaceContent(asset.Instantiate()); }
                else { popup.middle.Clear(); }
#endif
            }
        }
        //布局
        public VisualElement background = new VisualElement();
        public VisualElement top = new VisualElement();
        public VisualElement middle = new VisualElement();
        public VisualElement bottom = new VisualElement();
        //组件
        public Label title = new Label();
        public Button button = new Button();
        //参数
        public Action callback;
        public string assetPath { get; set; }
        public string titleText { get => title.text; set => title.text = value; }
        public string buttonText { get => button.text; set => button.text = value; }

        public MUPopupWindow() {
            //清除原有样式
            title.ClearClassList();
            button.ClearClassList();
            //设置USS类名
            AddToClassList("popup-window");
            background.AddToClassList("popup-window-background");
            top.AddToClassList("popup-window-top");
            middle.AddToClassList("popup-window-middle");
            bottom.AddToClassList("popup-window-bottom");
            title.AddToClassList("unity-text-element");
            title.AddToClassList("popup-window-title");
            button.AddToClassList("unity-text-element");
            button.AddToClassList("popup-window-button");
            //设置层级结构
            hierarchy.Add(background);
            background.Add(top);
            background.Add(middle);
            background.Add(bottom);
            top.Add(title);
            bottom.Add(button);
            //设置事件
            button.RegisterCallback<ClickEvent>((evt) => Close());
        }
        public void Open(Action action) {
            callback = action;
            visible = true;
        }
        public void Close() {
            callback?.Invoke();
            callback = null;
            visible = false;
        }
        public void ReplaceContent(VisualElement element) {
            middle.Clear();
            middle.Add(element);
        }
    }
}