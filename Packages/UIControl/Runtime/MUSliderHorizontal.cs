using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua {
    public class MUSliderHorizontal : VisualElement {
        public enum RoundDataType {
            保留两位小数 = 0,
            小数 = 1,
            整数 = 2,
        }
        public new class UxmlFactory : UxmlFactory<MUSliderHorizontal, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlStringAttributeDescription Text = new UxmlStringAttributeDescription {
                name = "text"
            };
            private UxmlFloatAttributeDescription MinValue = new UxmlFloatAttributeDescription {
                name = "min-value"
            };
            private UxmlFloatAttributeDescription MaxValue = new UxmlFloatAttributeDescription {
                name = "max-value"
            };
            private UxmlFloatAttributeDescription SlidingValue = new UxmlFloatAttributeDescription {
                name = "sliding-value"
            };
            private UxmlBoolAttributeDescription DisplayInput = new UxmlBoolAttributeDescription {
                name = "display-input"
            };
            private UxmlEnumAttributeDescription<RoundDataType> DataType = new UxmlEnumAttributeDescription<RoundDataType> {
                name = "data-type"
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUSliderHorizontal slider = (MUSliderHorizontal)ve;
                slider.Text = Text.GetValueFromBag(bag, cc);
                slider.MinValue = MinValue.GetValueFromBag(bag, cc);
                slider.MaxValue = MaxValue.GetValueFromBag(bag, cc);
                slider.SlidingValue = SlidingValue.GetValueFromBag(bag, cc);
                slider.DisplayInput = DisplayInput.GetValueFromBag(bag, cc);
                slider.DataType = DataType.GetValueFromBag(bag, cc);
            }
        }
        public event Action<float> SlidingValueChanged;
        public Label labelElement = new Label();
        public MUFloatField floatField = new MUFloatField();
        public VisualElement background = new VisualElement();
        public VisualElement container = new VisualElement();
        public VisualElement tracker = new VisualElement();
        public VisualElement dragger = new VisualElement();

        public string Text {
            get => labelElement.text;
            set => UpdateLabelElement(value);
        }
        public float MinValue {
            get => minValue;
            set { minValue = value; UpdateFloatField(); }
        }
        public float MaxValue {
            get => maxValue;
            set { maxValue = value; UpdateFloatField(); }
        }
        public float SlidingValue {
            get => slidingValue;
            set => UpdateSlidingValue(value);
        }
        public bool DisplayInput {
            get => isDisplayInput;
            set => UpdateFloatField(value);
        }
        public RoundDataType DataType {
            get => dataType;
            set { dataType = value; UpdateFloatField(); }
        }
        public float Value {
            get => UpdateValue();
            set => UpdateValue(value);
        }

        internal float minValue;
        internal float maxValue;
        internal float slidingValue;
        internal bool isDisplayInput;
        internal bool isDragger;
        internal float mousePosition;
        internal float originalPosition;
        public RoundDataType dataType;

        internal float MaxPosition { get => container.resolvedStyle.width; }
        internal float CurrentPosition { get => slidingValue * container.resolvedStyle.width; }

        internal void UpdateLabelElement(string value) {
            bool display = value != "" && value != null;
            labelElement.text = value;
            labelElement.style.display = display ? DisplayStyle.Flex : DisplayStyle.None;
        }
        internal void UpdateSlidingValue(float value) {
            UpdateDragger(value);
            SlidingValueChanged?.Invoke(Value);
        }
        internal void UpdateDragger(float value) {
            slidingValue = value;
            slidingValue = Mathf.Clamp(slidingValue, 0, 1);
            tracker.style.width = CurrentPosition;
            UpdateFloatField();
        }
        internal void UpdateFloatField(ChangeEvent<float> evt) {
            float value = Mathf.Clamp(evt.newValue, MinValue, MaxValue);
            slidingValue = (value - MinValue) / (MaxValue - MinValue);
            tracker.style.width = CurrentPosition;
            SlidingValueChanged?.Invoke(Value);
        }
        internal void UpdateFloatField(bool value) {
            isDisplayInput = value;
            floatField.style.display = isDisplayInput ? DisplayStyle.Flex : DisplayStyle.None;
        }
        internal void UpdateFloatField() {
            floatField.SetValueWithoutNotify(Value);
        }
        internal float UpdateValue() {
            float value = Mathf.Lerp(MinValue, MaxValue, SlidingValue);
            if (dataType == RoundDataType.保留两位小数) { value = (float)Math.Round(value, 2); }
            if (dataType == RoundDataType.整数) { value = Mathf.FloorToInt(value); }
            return Mathf.Clamp(value, MinValue, MaxValue);
        }
        internal void UpdateValue(float value) {
            slidingValue = value / (MaxValue - MinValue);
            slidingValue = Mathf.Clamp(slidingValue, 0, 1);
            tracker.style.width = CurrentPosition;
            UpdateFloatField();
        }

        public MUSliderHorizontal() {
            //设置名称
            labelElement.name = "Label";
            floatField.name = "FloatField";
            background.name = "Background";
            container.name = "Container";
            tracker.name = "Tracker";
            dragger.name = "Dragger";
            //设置USS类名
            AddToClassList("horizontal-slider");
            labelElement.ClearClassList();
            labelElement.AddToClassList("unity-text-element");
            labelElement.AddToClassList("horizontal-slider-label");

            background.AddToClassList("horizontal-slider-background");
            container.AddToClassList("horizontal-slider-container");
            tracker.AddToClassList("horizontal-slider-tracker");
            dragger.AddToClassList("horizontal-slider-dragger");

            floatField.ClearClassList();
            floatField.AddToClassList("horizontal-slider-field");
            floatField.inputElement.ClearClassList();
            floatField.inputElement.AddToClassList("horizontal-slider-field-box");
            floatField.textElement.ClearClassList();
            floatField.textElement.AddToClassList("unity-text-element");
            floatField.textElement.AddToClassList("horizontal-slider-field-text");
            //设置层级结构
            hierarchy.Add(labelElement);
            hierarchy.Add(background);
            hierarchy.Add(floatField);
            background.Add(container);
            container.Add(tracker);
            tracker.Add(dragger);
            //设置事件
            dragger.RegisterCallback<PointerDownEvent>(DraggerDown);
            dragger.RegisterCallback<PointerMoveEvent>(DraggerDrag);
            dragger.RegisterCallback<PointerUpEvent>((evt) => isDragger = false);
            dragger.RegisterCallback<PointerLeaveEvent>((evt) => isDragger = false);

            floatField.RegisterCallback<ChangeEvent<float>>(UpdateFloatField);

            container.RegisterCallback<PointerDownEvent>(ContainerDown);
        }
        private void DraggerDown(PointerDownEvent evt) {
            isDragger = true;
            mousePosition = evt.position.x;
            originalPosition = CurrentPosition;
        }
        private void DraggerDrag(PointerMoveEvent evt) {
            if (!isDragger) { return; }
            float offset = evt.position.x - mousePosition;
            float value = (originalPosition + offset) / MaxPosition;
            UpdateSlidingValue(value);
        }
        private void ContainerDown(PointerDownEvent evt) {
            float value = evt.localPosition.x / MaxPosition;
            UpdateSlidingValue(value);
        }
    }
}
