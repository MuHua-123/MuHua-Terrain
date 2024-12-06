using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机模块
/// </summary>
public abstract class ModuleCamera : MonoBehaviour {
    /// <summary> 默认图层遮罩 </summary>
    public static readonly LayerMask DefaultLayerMask = ~(1 << 0) | 1 << 0;
    /// <summary> 必须要初始化 </summary>
    protected abstract void Awake();
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;

    /// <summary> 相机位置 </summary>
    public abstract Vector3 Position { get; set; }
    /// <summary> 相机旋转 </summary>
    public abstract Vector3 EulerAngles { get; set; }
    /// <summary> 相机视野 </summary>
    public abstract float VisualField { get; set; }
    /// <summary> 渲染纹理 </summary>
    public abstract RenderTexture RenderTexture { get; }

    /// <summary> 更新渲染纹理 </summary>
    public abstract void UpdateRenderTexture(int x, int y);
    /// <summary> 屏幕坐标转换视图坐标(0-1) </summary>
    public abstract Vector3 ScreenToViewPosition(Vector3 screenPosition);
    /// <summary> 屏幕坐标转换世界坐标 </summary>
    public abstract Vector3 ScreenToWorldPosition(Vector3 screenPosition);

    /// <summary> 屏幕坐标获取世界对象 </summary>
    public abstract bool ScreenToWorldObject<T>(Vector3 screenPosition, out T value) where T : Object;
    /// <summary> 屏幕坐标获取世界对象 </summary>
    public abstract bool ScreenToWorldObject<T>(Vector3 screenPosition, out T value, LayerMask planeLayerMask) where T : Object;
    /// <summary> 屏幕坐标获取世界对象的父对象 </summary>
    public abstract bool ScreenToWorldObjectParent<T>(Vector3 screenPosition, out T value) where T : Object;
    /// <summary> 屏幕坐标获取世界对象的父对象 </summary>
    public abstract bool ScreenToWorldObjectParent<T>(Vector3 screenPosition, out T value, LayerMask planeLayerMask) where T : Object;
}
