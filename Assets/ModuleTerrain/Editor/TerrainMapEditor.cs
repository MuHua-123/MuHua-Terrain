using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MuHua;

/// <summary>
/// 地形图 - 编辑器
/// </summary>
[CustomEditor(typeof(TerrainMap), true)]
public class TerrainMapEditor : Editor {
	/// <summary> 选中目标 </summary>
	public TerrainMap value;

	public virtual void Awake() => value = target as TerrainMap;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		GUILayout.Space(10); // 增加10像素的空白

		if (GUILayout.Button("清除资源")) { ClearAssets(value); }
	}

	/// <summary> 清除资源 </summary>
	public static void ClearAssets(Object obj) {
		// 获取路径
		string path = AssetDatabase.GetAssetPath(obj);
		// 查找网格
		Object[] list = AssetDatabase.LoadAllAssetsAtPath(path);
		// 删除资源
		for (int i = 0; i < list.Length; i++) {
			if (list[i] == obj) { continue; }
			Undo.DestroyObjectImmediate(list[i]);
		}
		EditorUtility.SetDirty(obj);
		AssetDatabase.SaveAssets();
	}
}
