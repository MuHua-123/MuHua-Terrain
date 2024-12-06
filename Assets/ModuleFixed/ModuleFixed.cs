using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 固定数据物体模块
/// </summary>
public abstract class ModuleFixed : MonoBehaviour {
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;
}
