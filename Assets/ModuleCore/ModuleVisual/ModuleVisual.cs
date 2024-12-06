using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可视化内容生成模块
/// </summary>
public abstract class ModuleVisual<Data> : MonoBehaviour {
    /// <summary> 必须要初始化 </summary>
    protected abstract void Awake();
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;

    /// <summary> 更新可视化 </summary>
    public abstract void UpdateVisual(Data data);
    /// <summary> 释放可视化内容 </summary>
    public abstract void ReleaseVisual(Data data);

    /// <summary> 创建可视化内容 </summary>
    public void Create<T>(ref T value, Transform original, Transform parent) {
        if (value != null) { return; }
        Transform temp = CreateTransform(original, parent);
        value = temp.GetComponent<T>();
    }
    public Transform CreateTransform(Transform original, Transform parent) {
        Transform temp = Instantiate(original, parent);
        temp.gameObject.SetActive(true);
        return temp;
    }
}
