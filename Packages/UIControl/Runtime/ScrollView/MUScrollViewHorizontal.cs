using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MuHua {
    public class MUScrollViewHorizontal : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUScrollViewHorizontal, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlIntAttributeDescription MouseWheelScrollSize = new UxmlIntAttributeDescription {
                name = "mouse-wheel-scroll-size",
                defaultValue = 18
            };
            private UxmlFloatAttributeDescription SlidingValue = new UxmlFloatAttributeDescription {
                name = "sliding-value",
                defaultValue = 0
            };
            public UxmlIntAttributeDescription Count = new UxmlIntAttributeDescription {
                name = "count"
            };
            public UxmlStringAttributeDescription AssetPath = new UxmlStringAttributeDescription {
                name = "asset-path"
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUScrollViewHorizontal scrollView = (MUScrollViewHorizontal)ve;
                scrollView.MouseWheelScrollSize = MouseWheelScrollSize.GetValueFromBag(bag, cc);
                scrollView.SlidingValue = SlidingValue.GetValueFromBag(bag, cc);
                scrollView.AssetPath = AssetPath.GetValueFromBag(bag, cc);
#if UNITY_EDITOR
                VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(scrollView.AssetPath);
                if (asset == null) { return; }
                scrollView.ClearContainer();
                int count = Count.GetValueFromBag(bag, cc);
                for (int i = 0; i < count; i++) { scrollView.AddContainer(asset); }
#endif
                scrollView.scroller.MouseWheelScrollSize = scrollView.MouseWheelScrollSize;
                scrollView.ContainerRelease();
            }
        }
        public MUScrollerHorizontal scroller = new MUScrollerHorizontal();
        public VisualElement viewport = new VisualElement();
        public VisualElement container = new VisualElement();

        public string AssetPath { get; set; }
        public float MouseWheelScrollSize { get; set; }
        public float SlidingValue { get; set; }

        public void AddContainer(VisualTreeAsset asset) {
            AddContainer(asset.Instantiate());
        }
        public void AddContainer(VisualElement element) {
            container.Add(element);
        }
        public void ClearContainer() {
            container.Clear();
        }

        internal bool isDragger;
        internal float mousePosition;

        internal float ViewportWidth { get => viewport.resolvedStyle.width; }
        internal float ContainerWidth { get => container.resolvedStyle.width; }
        internal float MaxPosition { get => ComputeMaxPosition(); }
        internal float ComputeMaxPosition() {
            float value = ViewportWidth - ContainerWidth;
            return value <= 0 ? value : -1;
        }

        internal void UpdateVisualElement(float value) {
            SlidingValue = value;
            container.transform.position = new Vector3(MaxPosition * SlidingValue, 0);
            scroller.UpdateVisualElement(SlidingValue);
        }

        public MUScrollViewHorizontal() {
            //设置名称
            viewport.name = "viewport";
            container.name = "container";
            //设置USS类名
            AddToClassList("horizontal-scroll-view");
            viewport.AddToClassList("horizontal-scroll-view-viewport");
            container.AddToClassList("horizontal-scroll-view-container");

            scroller.ClearClassList();
            scroller.AddToClassList("horizontal-scroll-view-scroller");

            VisualElement dragger = scroller.dragger;
            dragger.ClearClassList();
            dragger.AddToClassList("horizontal-scroll-view-dragger");
            //设置层级结构
            hierarchy.Add(viewport);
            hierarchy.Add(scroller);
            viewport.Add(container);
            //设置事件
            scroller.SlidingValueChanged += UpdateVisualElement;
            container.RegisterCallback<PointerDownEvent>(ContainerDown);
            container.RegisterCallback<PointerMoveEvent>(ContainerDrag);
            container.RegisterCallback<PointerUpEvent>((evt) => ContainerRelease());
            container.RegisterCallback<PointerLeaveEvent>((evt) => ContainerRelease());
            container.RegisterCallback<WheelEvent>(ContainerWheel);
            container.generateVisualContent += ContainerGenerateVisualContent;
        }
        private void ContainerDown(PointerDownEvent evt) {
            isDragger = true;
            mousePosition = evt.position.x - SlidingValue * MaxPosition;
        }
        private void ContainerDrag(PointerMoveEvent evt) {
            if (!isDragger) { return; }
            float offset = evt.position.x - mousePosition;
            float value = offset / MaxPosition;
            UpdateVisualElement(value);
            mousePosition = evt.position.x - SlidingValue * MaxPosition;
        }
        private void ContainerRelease() {
            isDragger = false;
            float value = Mathf.Clamp01(SlidingValue);
            container.schedule.Execute(() => { UpdateVisualElement(value); }).StartingIn(1);
        }
        private void ContainerWheel(WheelEvent evt) {
            float wheel = Mathf.Clamp(evt.delta.y, -1, 1);
            float current = SlidingValue * MaxPosition;
            float offset = current - wheel * MouseWheelScrollSize;
            float value = offset / MaxPosition;
            UpdateVisualElement(value);
            ContainerRelease();
        }
        private void ContainerGenerateVisualContent(MeshGenerationContext mgc) {
            float ratio = Mathf.Clamp01(ViewportWidth / ContainerWidth);
            scroller.UpdateDragger(ratio);
            ContainerRelease();
        }
    }
}
