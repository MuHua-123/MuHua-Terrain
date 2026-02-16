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

		if (GUILayout.Button("生成地形")) { GenerateTerrain(); }
		if (GUILayout.Button("生成遮罩")) { GenerateMap(); }
	}

	/// <summary> 创建地形 </summary>
	private void GenerateTerrain() {
		TerrainData terrainData = value.terrainData;
		if (terrainData == null) { return; }
		// 获取路径
		string path = AssetDatabase.GetAssetPath(terrainData);
		// 查找网格
		var meshes = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Mesh>();
		Mesh mesh = meshes.FirstOrDefault(obj => obj.name == value.name);
		// 删除在创建新的
		if (mesh != null) { Undo.DestroyObjectImmediate(mesh); }
		mesh = CreateMesh(terrainData);
		// 初始化网格
		Initial(mesh);
	}
	/// <summary> 初始网格 </summary>
	private void Initial(Mesh mesh) {
		value.meshFilter.mesh = mesh;
		// 保存数据
		EditorUtility.SetDirty(value);
		AssetDatabase.SaveAssets();
	}
	/// <summary> 创建网格 </summary> 
	private Mesh CreateMesh(TerrainData terrainData) {
		// 获取噪点
		TerrainNoise terrainNoise = terrainData.GenerateNoise();
		// 生成网格数据
		Vector3 position = value.transform.position;
		TerrainMesh terrainMesh = terrainData.GenerateMesh();
		TerrainMeshData meshData = terrainMesh.Get(terrainNoise, position);
		// 生成网格
		Mesh mesh = meshData.Get();
		mesh.name = value.name;
		AssetDatabase.AddObjectToAsset(mesh, terrainData);
		// 保存数据
		EditorUtility.SetDirty(terrainData);
		AssetDatabase.SaveAssets();
		return mesh;
	}

	/// <summary> 生成遮罩图 </summary>
	private void GenerateMap() {
		TerrainData terrainData = value.terrainData;
		TerrainMap terrainMap = value.terrainMap;
		if (terrainData == null || terrainMap == null) { return; }

		float startTime = Time.realtimeSinceStartup;

		// 获取路径
		string path = AssetDatabase.GetAssetPath(terrainMap);
		// 查找网格
		var textures = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Texture2D>();
		Texture2D texture = textures.FirstOrDefault(obj => obj.name == value.name);

		Debug.Log("Time耗时: " + (Time.realtimeSinceStartup - startTime) * 1000 + " ms");
		startTime = Time.realtimeSinceStartup;

		// 获取噪点
		TerrainNoise terrainNoise = terrainData.GenerateNoise();
		// 生成网格数据
		Vector3 position = value.transform.position;
		TerrainMesh terrainMesh = terrainData.GenerateMesh();
		TerrainMeshData meshData = terrainMesh.Get(terrainNoise, position);
		meshData.Get();

		Debug.Log("Time耗时: " + (Time.realtimeSinceStartup - startTime) * 1000 + " ms");
		startTime = Time.realtimeSinceStartup;

		// 创建新的或者更新
		if (texture == null) {
			texture = terrainMap.Get(meshData);
			texture.name = value.name;
			AssetDatabase.AddObjectToAsset(texture, terrainMap);
		}
		else {
			terrainMap.Get(texture, meshData);
		}

		Debug.Log("Time耗时: " + (Time.realtimeSinceStartup - startTime) * 1000 + " ms");
		startTime = Time.realtimeSinceStartup;

		// 保存数据
		EditorUtility.SetDirty(terrainMap);
		AssetDatabase.SaveAssets();

		Debug.Log("Time耗时: " + (Time.realtimeSinceStartup - startTime) * 1000 + " ms");
		startTime = Time.realtimeSinceStartup;
	}
	/// <summary> 创建图 </summary>
	// private void CreateMap(Texture2D texture, TerrainData terrainData, TerrainMap terrainMap) {
	// 	// 获取噪点
	// 	TerrainNoise terrainNoise = terrainData.GenerateNoise();
	// 	// 生成网格数据
	// 	Vector3 position = value.transform.position;
	// 	TerrainMesh terrainMesh = terrainData.GenerateMesh();
	// 	TerrainMeshData meshData = terrainMesh.Get(terrainNoise, position);
	// 	meshData.Get();
	// 	// 生成纹理
	// 	if (texture == null) {
	// 		texture = terrainMap.Get(meshData);
	// 		texture.name = value.name;
	// 		AssetDatabase.AddObjectToAsset(texture, terrainMap);
	// 	}
	// 	else {
	// 		terrainMap.Get(texture, meshData);
	// 	}
	// 	// 保存数据
	// 	EditorUtility.SetDirty(terrainMap);
	// 	AssetDatabase.SaveAssets();
	// }
}
