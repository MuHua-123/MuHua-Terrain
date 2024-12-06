using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua {
    public class MUPopupPrompt : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUPopupPrompt, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            public UxmlStringAttributeDescription LabelText = new UxmlStringAttributeDescription {
                name = "Label-Text", defaultValue = "弹窗内容"
            };
            public UxmlStringAttributeDescription ButtonText = new UxmlStringAttributeDescription {
                name = "Button-Text", defaultValue = "确认"
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUPopupPrompt popupPrompt = ve as MUPopupPrompt;
                popupPrompt.labelText = LabelText.GetValueFromBag(bag, cc);
                popupPrompt.buttonText = ButtonText.GetValueFromBag(bag, cc);
            }
        }
        //布局
        public VisualElement background = new VisualElement();
        public VisualElement content = new VisualElement();
        public VisualElement bottom = new VisualElement();
        //组件
        public Label label = new Label();
        public Button button = new Button();
        //参数
        public Action callback;
        public string labelText { get => label.text; set => label.text = value; }
        public string buttonText { get => button.text; set => button.text = value; }

        public MUPopupPrompt() {
            //清除原有样式
            label.ClearClassList();
            button.ClearClassList();
            //设置USS类名
            AddToClassList("popup-prompt");
            background.AddToClassList("popup-prompt-background");
            content.AddToClassList("popup-prompt-content");
            bottom.AddToClassList("popup-prompt-bottom");
            label.AddToClassList("unity-text-element");
            label.AddToClassList("popup-prompt-label");
            button.AddToClassList("unity-text-element");
            button.AddToClassList("popup-prompt-button");
            //设置层级结构
            hierarchy.Add(background);
            background.Add(content);
            background.Add(bottom);
            content.Add(label);
            bottom.Add(button);
            //设置事件
            button.RegisterCallback<ClickEvent>((evt) => Close());
        }
        public void Open(string text, Action action) {
            labelText = text; callback = action; visible = true;
        }
        public void Close() {
            callback?.Invoke(); callback = null; visible = false;
        }
    }
}