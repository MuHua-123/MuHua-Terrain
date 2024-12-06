using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua {
    public class MUToggle : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUToggle, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlBoolAttributeDescription Value = new UxmlBoolAttributeDescription {
                name = "Value"
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUToggle toggle = (MUToggle)ve;
                toggle.Value = Value.GetValueFromBag(bag, cc);
            }
        }
        public event Action<bool> OnChange;
        public VisualElement checkmark = new VisualElement();
        /// <summary> 设置值 触发事件 </summary>
        public bool Value {
            get { return value; }
            set { SetValue(value); OnChange?.Invoke(value); }
        }
        /// <summary> 设置值不会触发事件 </summary>
        public void SetValue(bool value) {
            this.value = value;
            if (value) { checkmark.AddToClassList("toggle-checkmark-a"); }
            else { checkmark.RemoveFromClassList("toggle-checkmark-a"); }
        }

        internal bool value;

        public MUToggle() {
            //设置名称
            checkmark.name = "Checkmark";
            //设置USS类名
            AddToClassList("toggle");
            checkmark.AddToClassList("toggle-checkmark");
            //设置层级结构
            hierarchy.Add(checkmark);
            //设置事件
            checkmark.RegisterCallback<ClickEvent>((evt) => Value = !Value);
        }
    }
}
