using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua {
    public class MUScrollerVertical : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUScrollerVertical, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlIntAttributeDescription MouseWheelScrollSize = new UxmlIntAttributeDescription {
                name = "mouse-wheel-scroll-size",
                defaultValue = 18
            };
            private UxmlFloatAttributeDescription SlidingValue = new UxmlFloatAttributeDescription {
                name = "sliding-value",
                defaultValue = 0
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUScrollerVertical scroller = (MUScrollerVertical)ve;
                scroller.MouseWheelScrollSize = MouseWheelScrollSize.GetValueFromBag(bag, cc);
                scroller.SlidingValue = SlidingValue.GetValueFromBag(bag, cc);
                scroller.ElasticRestoration();
            }
        }
        public event Action<float> SlidingValueChanged;
        public VisualElement dragger = new VisualElement();

        public float MouseWheelScrollSize { get; set; }
        public float SlidingValue { get; set; }

        internal bool isDragger;
        internal float mousePosition;

        internal float ViewportHeight { get => resolvedStyle.height; }
        internal float ContainerHeight { get => dragger.resolvedStyle.height; }
        internal float MaxPosition { get => ComputeMaxPosition(); }
        internal float ComputeMaxPosition() {
            float value = ViewportHeight - ContainerHeight;
            return value >= 0 ? value : -1;
        }

        internal void UpdateDragger(float ratio) {
            dragger.style.height = ratio * resolvedStyle.height;
        }
        internal void UpdateVisualElement(float value, bool callback = false) {
            SlidingValue = Mathf.Clamp(value, 0, 1);
            dragger.transform.position = new Vector3(0, MaxPosition * SlidingValue);
            if (callback) { SlidingValueChanged?.Invoke(SlidingValue); }
        }
        internal void ElasticRestoration() {
            dragger.schedule.Execute(() => { UpdateVisualElement(SlidingValue); }).StartingIn(1);
        }

        public MUScrollerVertical() {
            //设置名称
            dragger.name = "dragger";
            //设置USS类名
            AddToClassList("vertical-scroller");
            dragger.AddToClassList("vertical-scroller-dragger");
            //设置层级结构
            hierarchy.Add(dragger);
            //设置事件
            dragger.RegisterCallback<PointerDownEvent>(DraggerDown);
            dragger.RegisterCallback<PointerMoveEvent>(DraggerDrag);
            dragger.generateVisualContent += DraggerGenerateVisualContent;

            RegisterCallback<PointerDownEvent>(ScrollerDown);
            RegisterCallback<WheelEvent>(ScrollerWheel);
            RegisterCallback<PointerUpEvent>((evt) => ScrollerRelease());
            RegisterCallback<PointerLeaveEvent>((evt) => ScrollerRelease());
        }
        private void DraggerDown(PointerDownEvent evt) {
            isDragger = true;
            mousePosition = evt.position.y - SlidingValue * MaxPosition;
        }
        private void DraggerDrag(PointerMoveEvent evt) {
            if (!isDragger) { return; }
            float offset = evt.position.y - mousePosition;
            float value = offset / MaxPosition;
            UpdateVisualElement(value, true);
            mousePosition = evt.position.y - SlidingValue * MaxPosition;
        }
        private void DraggerGenerateVisualContent(MeshGenerationContext mgc) {
            ElasticRestoration();
        }

        private void ScrollerDown(PointerDownEvent evt) {
            float offset = evt.localPosition.y - dragger.resolvedStyle.height * 0.5f;
            float value = offset / MaxPosition;
            UpdateVisualElement(value, true);
        }
        private void ScrollerWheel(WheelEvent evt) {
            float wheel = Mathf.Clamp(evt.delta.y, -1, 1);
            float current = SlidingValue * MaxPosition;
            float offset = current + wheel * MouseWheelScrollSize;
            float value = offset / MaxPosition;
            UpdateVisualElement(value, true);
        }
        private void ScrollerRelease() {
            isDragger = false;
        }
    }
}
