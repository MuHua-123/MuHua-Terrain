using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua {
    public class UIViewModel : MonoBehaviour {
        public UIDocument document;
        public VisualElement element => document.rootVisualElement;

        public T Q<T>(string name = null, string className = null) where T : VisualElement {
            return element.Q<T>(name, className);
        }
    }
}