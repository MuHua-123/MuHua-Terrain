using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MuHua;

/// <summary>
/// 输入模块
/// </summary>
public class ModuleInput : ModuleSingle<ModuleInput> {

	/// <summary> 当前输入模式 </summary>
	public static InputMode Current;
	/// <summary> 回退输入模式 </summary>
	public static InputMode BackMode;
	/// <summary> 鼠标指针位置 </summary>
	public static Vector2 mousePosition;
	/// <summary> 转换模式事件 </summary>
	public static event Action<InputMode> OnInputMode;

	private static bool isPointerOverUIObject;// 指针是否在UI上

	/// <summary> 指针是否在UI上 </summary>
	public static bool IsPointerOverUIObject => isPointerOverUIObject;

	/// <summary> 设置输入模式 </summary>
	public static void Settings(InputMode mode) {
		BackMode = Current;
		Current = mode;
		OnInputMode?.Invoke(Current);
	}
	/// <summary> 设置输入模式 </summary>
	public static void Back() {
		Current = BackMode;
		OnInputMode?.Invoke(Current);
	}

	protected override void Awake() => NoReplace();

	private void Update() {
#if UNITY_STANDALONE
		//电脑平台
		isPointerOverUIObject = EventSystem.current.IsPointerOverGameObject();
#elif UNITY_WEBGL
		//WebGL平台
		isPointerOverUIObject = EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID
        //安卓平台
        isPointerOverUIObject = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#elif UNITY_IOS
        //苹果平台
        isPointerOverUIObject = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
	}

}
/// <summary>
/// 输入模式
/// </summary>
public enum InputMode {
	/// <summary> 无 </summary>
	None,
	/// <summary> 服装设计 </summary>
	FashionDesign,
	/// <summary> 配饰设计 </summary>
	OrnamentDesign,
	/// <summary> 服装展示 </summary>
	FashionDisplay,
}