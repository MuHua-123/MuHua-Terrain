using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 相机模块
/// </summary>
public class ModuleCamera : ModuleSingle<ModuleCamera> {

	/// <summary> 当前相机 </summary>
	public static CameraController CurrentCamera;
	/// <summary> 相机模式事件 </summary>
	public static event Action<CameraMode> OnCameraMode;

	/// <summary> 设置相机模式 </summary>
	public static void Settings(CameraMode mode, bool isReset = true) {
		OnCameraMode?.Invoke(mode);
		if (isReset) { I.ResetCamera(); }
	}

	public List<CameraController> cameras;

	protected override void Awake() {
		NoReplace();
		cameras.ForEach(obj => obj.Initial());
	}

	private void OnDestroy() => cameras.ForEach(obj => obj.Release());

	/// <summary> 重置相机 </summary>
	public void ResetCamera() => cameras.ForEach(obj => obj.ResetCamera());

}
/// <summary>
/// 相机模式
/// </summary>
public enum CameraMode {
	/// <summary> 无 </summary>
	None,
	/// <summary> 移轴相机 </summary>
	Axis,
}