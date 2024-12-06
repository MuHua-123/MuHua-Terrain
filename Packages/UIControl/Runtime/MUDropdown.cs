using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua {
    public class MUDropdown : DropdownField {
        public new class UxmlFactory : UxmlFactory<MUDropdown, UxmlTraits> { }
        public new class UxmlTraits : DropdownField.UxmlTraits { }
        public MUDropdown() {
            ClearClassList();
            AddToClassList("dropdown");

            labelElement.ClearClassList();
            labelElement.AddToClassList("unity-text-element");
            labelElement.AddToClassList("dropdown-label");

            VisualElement inputElement = this.Query<VisualElement>(null, "unity-popup-field__input");
            inputElement.ClearClassList();
            inputElement.AddToClassList("dropdown-box");

            VisualElement textElement = this.Query<VisualElement>(null, "unity-base-popup-field__text");
            textElement.ClearClassList();
            textElement.AddToClassList("unity-text-element");
            textElement.AddToClassList("dropdown-text");

            VisualElement arrowElement = this.Query<VisualElement>(null, "unity-base-popup-field__arrow");
            arrowElement.AddToClassList("dropdown-arrow");
        }
    }
}
