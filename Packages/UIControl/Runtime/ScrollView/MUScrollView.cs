using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MuHua {
    public class MUScrollView : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUScrollView, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {

        }
        public VisualElement viewport = new VisualElement();
        public VisualElement container = new VisualElement();

        public MUScrollView() {
            //设置名称
            viewport.name = "viewport";
            container.name = "container";
            //设置USS类名
            AddToClassList("scroll-view");
            viewport.AddToClassList("scroll-view-viewport");
            container.AddToClassList("scroll-view-container");
            //设置层级结构
            hierarchy.Add(viewport);
            viewport.Add(container);
        }
    }
}

