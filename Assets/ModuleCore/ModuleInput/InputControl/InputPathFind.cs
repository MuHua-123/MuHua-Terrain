using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MuHua;

/// <summary>
/// 寻路 - 输入器
/// </summary>
public class InputPathFind : InputControl {

	public bool isEnable;
	public Vector3 origin;
	public LineRenderer lineRenderer;

	protected override void ModuleInput_OnInputMode(InputMode mode) {
		isEnable = mode == InputMode.PathFind;
		lineRenderer.gameObject.SetActive(isEnable);
		if (!isEnable) { return; }
	}

	#region 输入系统
	/// <summary> 鼠标左键 </summary>
	public void OnMouseLeft(InputValue inputValue) {
		if (!isEnable) { return; }
		if (!RayTool.GetMouseToWorldPosition(out Vector3 mousePosition)) { return; }
		ManagerMap.TryWorldPosition(mousePosition, out origin);
	}
	/// <summary> 鼠标右键 </summary>
	public void OnMouseRight(InputValue inputValue) {
		if (!isEnable) { return; }
		if (!RayTool.GetMouseToWorldPosition(out Vector3 mousePosition)) { return; }
		ManagerMap.FindPath(origin, mousePosition, out List<Vector3> vectorPath);
		lineRenderer.positionCount = vectorPath.Count;
		lineRenderer.SetPositions(vectorPath.ToArray());
	}
	/// <summary> 鼠标中键 </summary>
	public void OnMouseMiddle(InputValue inputValue) {
		if (!isEnable) { return; }
	}
	#endregion
}
