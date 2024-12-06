using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 预制件模块
/// </summary>
public abstract class ModulePrefab<Data> : MonoBehaviour {
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;

    /// <summary> 关联的数据 </summary>
    public abstract Data Value { get; }
    /// <summary> 更新可视化内容 </summary>
    public abstract void UpdateVisual(Data data);
}
