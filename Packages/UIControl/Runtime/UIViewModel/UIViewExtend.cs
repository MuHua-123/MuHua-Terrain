using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua {
    public class UIViewExtend<ViewModel> : MonoBehaviour where ViewModel : UIViewModel {
        public ViewModel view;
        public virtual VisualElement element => view.Q<VisualElement>();

        public T Q<T>(string name = null, string className = null) where T : VisualElement {
            return element.Q<T>(name, className);
        }
    }
}
