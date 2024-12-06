using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源模块
/// </summary>
/// <typeparam name="Data">绑定的资源类型</typeparam>
public abstract class ModuleAssets<Data> : MonoBehaviour {
    /// <summary> 必须要初始化 </summary>
    protected abstract void Awake();
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;

    /// <summary> 数据计数 </summary>
    public abstract int Count { get; }
    /// <summary> 数据列表 </summary>
    public abstract List<Data> Datas { get; }

    /// <summary> 添加数据 </summary>
    public abstract void Add(Data data);
    /// <summary> 删除数据 </summary>
    public abstract void Remove(Data data);
    /// <summary> 查询数据 </summary>
    public abstract Data Find(int index);
    /// <summary> 循环列表 </summary>
    public abstract void ForEach(Action<Data> action);
}
