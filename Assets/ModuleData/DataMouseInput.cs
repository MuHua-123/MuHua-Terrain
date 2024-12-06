using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鼠标输入参数
/// </summary>
public class DataMouseInput {
    /// <summary> 鼠标输入参数 </summary>
    public DataMouseInput() { }
    /// <summary> 鼠标滚动量 </summary>
    public float ScrollWheel;
    /// <summary> 视图坐标 </summary>
    public Vector3 ViewPosition;
    /// <summary> 世界坐标 </summary>
    public Vector3 WorldPosition;
    /// <summary> 屏幕坐标 </summary>
    public Vector3 ScreenPosition;
}
