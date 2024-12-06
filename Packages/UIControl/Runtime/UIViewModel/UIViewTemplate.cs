using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua {
    public class UIViewTemplate<Data> {
        public Data value;
        public VisualElement element;
        public virtual void SetValue(VisualTreeAsset asset, Data value) {
            this.value = value;
            this.element = asset.Instantiate();
        }
    }
}
