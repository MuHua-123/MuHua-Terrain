using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// UI面板
/// </summary>
public abstract class ModuleUIPanel : MonoBehaviour {
    /// <summary> 绑定的页面 </summary>
    public ModuleUIPage ModuleUIPage;
    /// <summary> 可选初始化 </summary>
    protected virtual void Awake() { }
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;

    public abstract VisualElement Element { get; }
}
/// <summary>
/// UI项
/// </summary>
public abstract class UIItem<Data, T> where T : UIItem<Data, T> {
    /// <summary> 选择事件 </summary>
    public static event Action<Data> OnSelect;
    /// <summary> 触发事件 </summary>
    public static void Select(Data data) => OnSelect?.Invoke(data);
    /// <summary> 绑定的数据 </summary>
    public readonly Data value;
    /// <summary> 绑定的元素 </summary>
    public readonly VisualElement element;
    /// <summary> 基础实例 </summary>
    public UIItem(Data value, VisualElement element) {
        this.value = value;
        this.element = element;
        OnSelect += UnitUIPanelItem_OnSelect;
    }
    /// <summary> 触发选择事件 </summary>
    public virtual void Select() => OnSelect?.Invoke(value);
    /// <summary> 侦听选择事件 </summary>
    public virtual void UnitUIPanelItem_OnSelect(Data obj) {
        if (value.Equals(obj)) { SelectState(); }
        else { DefaultState(); }
    }
    /// <summary> 默认状态 </summary>
    public virtual void DefaultState() { }
    /// <summary> 选中状态 </summary>
    public virtual void SelectState() { }
    /// <summary> 释放 </summary>
    public virtual void Release() => OnSelect -= UnitUIPanelItem_OnSelect;
}