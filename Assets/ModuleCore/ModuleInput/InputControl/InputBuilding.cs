using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MuHua;

/// <summary>
/// 建筑 - 输入器
/// </summary>
public class InputBuilding : InputControl {

	private int index;
	private bool isEnable;

	protected override void ModuleInput_OnInputMode(InputMode mode) {
		isEnable = mode == InputMode.Building;
		if (!isEnable) { BuildingSystem.I.Cancel(); return; }
		index = 0;
		BuildingSystem.I.Settings(AssetsManager.I.FindBuilding(index));
	}

	#region 输入系统
	/// <summary> 鼠标左键 </summary>
	public void OnMouseLeft(InputValue inputValue) {
		if (!isEnable) { return; }
		BuildingSystem.I.Build();
	}
	/// <summary> 鼠标右键 </summary>
	public void OnMouseRight(InputValue inputValue) {
		if (!isEnable) { return; }
		BuildingSystem.I.Rotate(90);
	}
	/// <summary> 鼠标中键 </summary>
	public void OnMouseMiddle(InputValue inputValue) {
		if (!isEnable) { return; }
		index++;
		BuildingSystem.I.Settings(AssetsManager.I.FindBuilding(index));
	}
	/// <summary> Tab </summary>
	public void OnSwitchMode(InputValue inputValue) {
		if (ModuleInput.Current == InputMode.Building) { ModuleInput.Settings(InputMode.PathFind); return; }
		if (ModuleInput.Current == InputMode.PathFind) { ModuleInput.Settings(InputMode.Building); return; }
	}
	#endregion
}
