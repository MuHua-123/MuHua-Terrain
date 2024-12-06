using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua {
    public class MUFloatField : FloatField {
        public new class UxmlFactory : UxmlFactory<MUFloatField, UxmlTraits> { }
        public new class UxmlTraits : FloatField.UxmlTraits { }
        public VisualElement inputElement;
        public VisualElement textElement;
        public MUFloatField() {
            ClearClassList();
            AddToClassList("input-field");

            labelElement.ClearClassList();
            labelElement.AddToClassList("unity-text-element");
            labelElement.AddToClassList("input-field-label");

            inputElement = this.Q<VisualElement>("unity-text-input");
            inputElement.ClearClassList();
            inputElement.AddToClassList("input-field-box");

            textElement = inputElement.Q<VisualElement>("");
            textElement.ClearClassList();
            textElement.AddToClassList("unity-text-element");
            textElement.AddToClassList("input-field-text");
        }
    }
}
