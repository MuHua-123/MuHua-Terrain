using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入模块
/// </summary>
public abstract class ModuleInput : MonoBehaviour {
    /// <summary> 必须要初始化 </summary>
    protected abstract void Awake();
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;

    public abstract Vector2 MousePosition { get; }
}
