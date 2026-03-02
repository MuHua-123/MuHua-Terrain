using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 地形对象 - 编辑器
/// </summary>
[CustomEditor(typeof(TerrainObject), true)]
public class TerrainObjectEditor : Editor {
	/// <summary> 选中目标 </summary>
	public TerrainObject value;

	public virtual void Awake() => value = target as TerrainObject;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		GUILayout.Space(10); // 增加10像素的空白

		if (GUILayout.Button("生成地形")) { value.Generate(); }
	}

	/// <summary> 生成 </summary>
	private void Generate() {

	}
}
