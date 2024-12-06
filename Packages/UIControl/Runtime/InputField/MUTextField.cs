using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua {
    public class MUTextField : TextField {
        public new class UxmlFactory : UxmlFactory<MUTextField, UxmlTraits> { }
        public new class UxmlTraits : TextField.UxmlTraits {
            public UxmlStringAttributeDescription DefaultPrompt = new UxmlStringAttributeDescription {
                name = "default-prompt"
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUTextField textField = (MUTextField)ve;
                textField.DefaultPrompt = DefaultPrompt.GetValueFromBag(bag, cc);
                textField.SetDefaultPrompt();
            }
        }
        public string DefaultPrompt { get; set; }

        public VisualElement inputElement => this.Q<VisualElement>("unity-text-input");
        public VisualElement textElement => inputElement.Q<VisualElement>("");

        public MUTextField() {
            ClearClassList();
            AddToClassList("input-field");

            labelElement.ClearClassList();
            labelElement.AddToClassList("unity-text-element");
            labelElement.AddToClassList("input-field-label");

            inputElement.ClearClassList();
            inputElement.AddToClassList("input-field-box");

            textElement.ClearClassList();
            textElement.AddToClassList("unity-text-element");
            textElement.AddToClassList("input-field-text");

            RegisterCallback<FocusInEvent>((evt) => { PrepareInput(); });
            RegisterCallback<FocusOutEvent>((evt) => { SetDefaultPrompt(); });
        }
        public void PrepareInput() {
            textElement.RemoveFromClassList("input-field-text-d");
            if (text != DefaultPrompt) { return; }
            text = "";
        }
        public void SetDefaultPrompt() {
            textElement.RemoveFromClassList("input-field-text-d");
            if (value != "") { return; }
            text = DefaultPrompt;
            textElement.AddToClassList("input-field-text-d");
        }
    }
}