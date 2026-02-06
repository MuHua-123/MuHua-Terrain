using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MuHua;

/// <summary>
/// 测试柏林噪点 - 编辑器
/// </summary>
[CustomEditor(typeof(TerrainManager), true)]
public class TestTerrainNoiseEditor : Editor {
	/// <summary> 选中目标 </summary>
	public TerrainManager value;

	public virtual void Awake() => value = target as TerrainManager;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		GUILayout.Space(10); // 增加10像素的空白

		if (!Application.isPlaying) { return; }
		if (GUILayout.Button("生成纹理")) { value.Generate(); }
	}
}
