using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// UI页面
/// </summary>
public abstract class ModuleUIPage : MonoBehaviour {
    public UIDocument document;
    /// <summary> 根目录文档 </summary>
    public VisualElement root => document.rootVisualElement;
    /// <summary> 必须初始化 </summary>
    protected abstract void Awake();
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;
    /// <summary> 添加UI元素 </summary>
    public void Add(VisualElement child) => root.Add(child);
    /// <summary> 查询UI元素 </summary>
    public T Q<T>(string name = null, string className = null) where T : VisualElement => root.Q<T>(name, className);
}
