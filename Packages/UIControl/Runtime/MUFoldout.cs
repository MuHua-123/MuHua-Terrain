using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MuHua {
    public class MUFoldout : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUFoldout, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlStringAttributeDescription Text = new UxmlStringAttributeDescription {
                name = "text",
                defaultValue = "标题"
            };
            private UxmlBoolAttributeDescription Active = new UxmlBoolAttributeDescription {
                name = "active",
                defaultValue = false
            };
            public UxmlIntAttributeDescription Count = new UxmlIntAttributeDescription {
                name = "count"
            };
            public UxmlStringAttributeDescription AssetPath = new UxmlStringAttributeDescription {
                name = "asset-path"
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUFoldout foldout = (MUFoldout)ve;
                foldout.Text = Text.GetValueFromBag(bag, cc);
                foldout.Active = Active.GetValueFromBag(bag, cc);
                foldout.AssetPath = AssetPath.GetValueFromBag(bag, cc);
#if UNITY_EDITOR
                VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(foldout.AssetPath);
                if (asset == null) { return; }
                foldout.ClearContainer();
                int count = Count.GetValueFromBag(bag, cc);
                for (int i = 0; i < count; i++) { foldout.AddContainer(asset); }
#endif
            }
        }
        public Label labelElement = new Label();
        public VisualElement title = new VisualElement();
        public VisualElement image = new VisualElement();
        public VisualElement container = new VisualElement();

        public string Text {
            get => labelElement.text;
            set => labelElement.text = value;
        }
        public bool Active {
            get => isActive;
            set => SetActive(value);
        }

        public void AddContainer(VisualTreeAsset asset) {
            AddContainer(asset.Instantiate());
        }
        public void AddContainer(VisualElement element) {
            container.Add(element);
        }
        public void ClearContainer() {
            container.Clear();
        }

        internal bool isActive;
        internal string AssetPath { get; set; }
        internal void SetActive(bool value) {
            isActive = value;
            container.style.display = Active ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public MUFoldout() {
            //设置名称
            title.name = "title";
            image.name = "image";
            labelElement.name = "label";
            container.name = "container";
            //设置USS类名
            AddToClassList("foldout");
            title.AddToClassList("foldout-title");
            image.AddToClassList("foldout-title-image");
            labelElement.AddToClassList("foldout-title-label");
            container.AddToClassList("foldout-container");
            //设置层级结构
            hierarchy.Add(title);
            hierarchy.Add(container);
            title.Add(image);
            title.Add(labelElement);
            //设置事件
            title.RegisterCallback<ClickEvent>((evt) => Active = !Active);
        }
    }
}
