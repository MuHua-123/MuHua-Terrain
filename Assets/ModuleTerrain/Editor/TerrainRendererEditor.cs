using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MuHua;

/// <summary>
/// 地形渲染器 - 编辑器
/// </summary>
[CustomEditor(typeof(TerrainRenderer), true)]
public class TerrainRendererEditor : Editor {
	/// <summary> 选中目标 </summary>
	public TerrainRenderer value;

	public virtual void Awake() => value = target as TerrainRenderer;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		GUILayout.Space(10); // 增加10像素的空白

		if (GUILayout.Button("生成地形")) { GenerateTerrain(value); }
	}

	/// <summary> 创建地形 </summary>
	public static void GenerateTerrain(TerrainRenderer value) {
		TerrainData terrainData = value.terrainData;
		if (terrainData == null) { return; }
		// 生成网格
		TerrainMesh meshData = GenerateMesh(value, terrainData);
		value.meshFilter.mesh = meshData.mesh;
		// 生成材质
		List<Material> materials = new List<Material>();
		List<TerrainMap> terrainMaps = terrainData.terrainMaps;
		for (int i = 0; i < terrainMaps.Count; i++) {
			Material material = GenerateMap(value, meshData, terrainMaps[i]);
			materials.Add(material);
		}
		value.meshRenderer.materials = materials.ToArray();
		// 保存数据
		EditorUtility.SetDirty(terrainData);
		EditorUtility.SetDirty(value);
		AssetDatabase.SaveAssets();
	}

	/// <summary> 创建地形 </summary>
	private static TerrainMesh GenerateMesh(TerrainRenderer value, TerrainData terrainData) {
		// 查找网格数据
		TerrainMesh meshData = Find<TerrainMesh>(terrainData, value.name);
		if (meshData != null) { Undo.DestroyObjectImmediate(meshData); }
		meshData = terrainData.GenerateMesh();
		meshData.name = value.name;
		AssetDatabase.AddObjectToAsset(meshData, terrainData);
		// 初始网格数据
		Vector3 position = value.transform.position;
		TerrainNoise terrainNoise = terrainData.GenerateNoise();
		meshData.Initial(terrainNoise, position);
		// 生成网格
		Mesh mesh = Find<Mesh>(terrainData, value.name);
		if (mesh != null) { Undo.DestroyObjectImmediate(mesh); }
		mesh = meshData.Get();
		mesh.name = value.name;
		AssetDatabase.AddObjectToAsset(mesh, terrainData);
		EditorUtility.SetDirty(terrainData);
		return meshData;
	}
	/// <summary> 生成遮罩图 </summary>
	private static Material GenerateMap(TerrainRenderer value, TerrainMesh meshData, TerrainMap terrainMap) {
		// 获得遮罩
		Texture2D texture = Find<Texture2D>(terrainMap, value.name);
		if (texture != null) { Undo.DestroyObjectImmediate(texture); }
		texture = terrainMap.Get(meshData, texture);
		texture.name = value.name;
		AssetDatabase.AddObjectToAsset(texture, terrainMap);
		// 获得材质
		Material material = Find<Material>(terrainMap, value.name);
		if (material != null) { Undo.DestroyObjectImmediate(material); }
		material = terrainMap.Get(texture, material);
		material.name = value.name;
		AssetDatabase.AddObjectToAsset(material, terrainMap);
		EditorUtility.SetDirty(terrainMap);
		return material;
	}

	/// <summary> 资源查找 </summary> 
	public static T Find<T>(UnityEngine.Object assetObject, string name) where T : UnityEngine.Object {
		// 获取路径
		string path = AssetDatabase.GetAssetPath(assetObject);
		// 查找网格
		var list = AssetDatabase.LoadAllAssetsAtPath(path).OfType<T>();
		return list.FirstOrDefault(obj => obj.name == name);
	}
	/// <summary> 测试时间 </summary> 
	public static void DebugTime(Action action) {
		float startTime = Time.realtimeSinceStartup;
		action?.Invoke();
		Debug.Log("Time耗时: " + (Time.realtimeSinceStartup - startTime) * 1000 + " ms");
	}
}
