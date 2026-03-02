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
		// 生成地形渲染器
		GenerateTerrain(value, terrainData);
		// 保存数据
		TerrainDataEditor.SetDirty(terrainData);
		AssetDatabase.SaveAssets();
	}

	/// <summary> 生成地形渲染器 </summary>
	public static void GenerateTerrain(TerrainRenderer value, TerrainData terrainData) {
		value.terrainData = terrainData;
		TerrainMesh meshData = GenerateMesh(value, terrainData);
		value.meshFilter.mesh = meshData.mesh;
		// 生成材质
		List<TerrainMap> terrainMaps = terrainData.terrainMaps;
		var materials = terrainMaps.Select(obj => GenerateMap(value, meshData, obj));
		value.meshRenderer.materials = materials.ToArray();
		EditorUtility.SetDirty(value);
	}
	/// <summary> 创建地形 </summary>
	private static TerrainMesh GenerateMesh(TerrainRenderer value, TerrainData terrainData) {
		// 查找网格数据
		TerrainMesh meshData = GenerateTerrainMesh(value.name, value.transform.position, terrainData);
		// 生成网格
		GenerateMesh(value.name, meshData, terrainData);
		return meshData;
	}
	/// <summary> 生成遮罩图 </summary>
	private static Material GenerateMap(TerrainRenderer value, TerrainMesh meshData, TerrainMap terrainMap) {
		// 获得遮罩
		Texture2D texture = GenerateTexture(value.name, meshData, terrainMap);
		return GenerateMaterial(value.name, texture, terrainMap);
	}

	/// <summary> 生成地形网格 </summary>
	public static TerrainMesh GenerateTerrainMesh(string name, Vector3 position, TerrainData terrainData) {
		// 查找网格数据
		TerrainMesh meshData = Find<TerrainMesh>(terrainData, name);
		if (meshData != null) { Undo.DestroyObjectImmediate(meshData); }
		meshData = terrainData.GenerateMesh();
		meshData.name = name;
		AssetDatabase.AddObjectToAsset(meshData, terrainData);
		// 初始网格数据
		TerrainNoise terrainNoise = terrainData.GenerateNoise();
		meshData.Initial(terrainNoise, position);
		return meshData;
	}
	/// <summary> 生成网格 </summary> 
	public static Mesh GenerateMesh(string name, TerrainMesh meshData, TerrainData terrainData) {
		Mesh mesh = Find<Mesh>(terrainData, name);
		if (mesh != null) { Undo.DestroyObjectImmediate(mesh); }
		mesh = meshData.Get();
		mesh.name = name;
		AssetDatabase.AddObjectToAsset(mesh, terrainData);
		return mesh;
	}
	/// <summary> 生成纹理 </summary> 
	public static Texture2D GenerateTexture(string name, TerrainMesh meshData, TerrainMap terrainMap) {
		Texture2D texture = Find<Texture2D>(terrainMap, name);
		if (texture != null) { Undo.DestroyObjectImmediate(texture); }
		texture = terrainMap.Get(meshData, texture);
		texture.name = name;
		AssetDatabase.AddObjectToAsset(texture, terrainMap);
		return texture;
	}
	/// <summary> 生成材质 </summary> 
	public static Material GenerateMaterial(string name, Texture2D texture, TerrainMap terrainMap) {
		Material material = Find<Material>(terrainMap, name);
		if (material != null) { Undo.DestroyObjectImmediate(material); }
		material = terrainMap.Get(texture, material);
		material.name = name;
		AssetDatabase.AddObjectToAsset(material, terrainMap);
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
