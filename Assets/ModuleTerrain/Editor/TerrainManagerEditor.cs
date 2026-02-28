using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MuHua;

/// <summary>
/// 地形管理器 - 编辑器
/// </summary>
[CustomEditor(typeof(TerrainManager), true)]
public class TerrainManagerEditor : Editor {
	/// <summary> 选中目标 </summary>
	public TerrainManager value;

	public virtual void Awake() => value = target as TerrainManager;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		GUILayout.Space(10); // 增加10像素的空白

		if (GUILayout.Button("生成地形")) { Generate(); }
	}

	/// <summary> 生成 </summary>
	private void Generate() {
		// 清除资源

		// 创建地形
		int wide = value.blockSize.x;
		int high = value.blockSize.y;
		for (int x = 0; x < wide; x++)
			for (int y = 0; y < high; y++)
				Generate(x, y);
		// 保存数据
		EditorUtility.SetDirty(value);
		AssetDatabase.SaveAssets();
	}
	private void Generate(int x, int y) {
		GameObject obj = new GameObject();
		obj.transform.SetParent(value.transform);
		// 设置位置
		Vector2Int blockSize = value.blockSize;
		Vector2Int meshSize = value.terrainData.meshSize;
		float meshScale = value.terrainData.meshScale;
		float ox = meshSize.x * blockSize.x * meshScale * -0.5f;
		float oz = meshSize.y * blockSize.y * meshScale * -0.5f;
		Vector3 origin = new Vector3(ox, 0, oz);
		Vector3 position = origin + new Vector3(x * meshSize.x, 0, y * meshSize.y) * meshScale;
		obj.transform.position = position;
		// 设置数据
		TerrainRenderer terrainRenderer = obj.AddComponent<TerrainRenderer>();
		terrainRenderer.terrainData = value.terrainData;
		TerrainRendererEditor.GenerateTerrain(terrainRenderer);
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
