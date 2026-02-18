using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形数据
/// </summary>
[CreateAssetMenu(fileName = "TerrainData", menuName = "MuHua/地形/地形数据")]
public class TerrainData : ScriptableObject {

	[Header("噪点")]
	/// <summary> 种子 </summary>
	public int seed;
	/// <summary> 噪点规模 </summary>
	public float noiseScale = 300f;
	/// <summary> 频率 </summary>
	public float frequency = 4;
	/// <summary> 振幅 </summary>
	public float amplitude = 0.1f;
	/// <summary> 梯度 </summary>
	public int octaves = 4;

	[Header("网格")]
	/// <summary> 大小 </summary>
	public Vector2Int meshSize = new Vector2Int(250, 250);
	/// <summary> 规模 </summary>
	public float meshScale = 4;
	/// <summary> 高度</summary>
	public float meshHeight = 200;
	/// <summary> 曲线 </summary>
	public AnimationCurve meshCurve;
	/// <summary> 网格 </summary>
	// public TerrainMesh terrainMesh;

	[Header("地形图")]
	/// <summary> 地形图 </summary>
	public List<TerrainMap> terrainMaps;

	/// <summary> 生成噪点 </summary> 
	public TerrainNoise GenerateNoise() {
		return new TerrainNoise(frequency, amplitude, noiseScale, octaves);
	}
	/// <summary> 生成网格 </summary> 
	public TerrainMesh GenerateMesh() {
		TerrainMesh meshData = new TerrainMesh();
		meshData.Initial(meshSize.x + 1, meshSize.y + 1, meshScale, meshHeight, meshCurve);
		return meshData;
	}

	#region 构建网格
	// /// <summary> 宽 </summary>
	// public int MeshWide => meshSize.x + 1;
	// /// <summary> 高 </summary>
	// public int MeshHigh => meshSize.y + 1;
	// /// <summary> 生成网格 </summary>
	// public TerrainMeshData Get(TerrainNoise noise, Vector3 origin) {
	// 	TerrainMeshData meshData = new TerrainMeshData(MeshWide, MeshHigh, meshScale);
	// 	// 构建数据
	// 	for (int z = 0; z < MeshHigh; z++)
	// 		for (int x = 0; x < MeshWide; x++)
	// 			AddVertex(meshData, noise, origin, x, z);
	// 	// 生成网格
	// 	return meshData;
	// }
	// /// <summary> 添加顶点 </summary> 
	// private void AddVertex(TerrainMeshData meshData, TerrainNoise noise, Vector3 origin, int x, int z) {
	// 	// 配置顶点
	// 	float posX = x * meshScale;
	// 	float posZ = z * meshScale;
	// 	// 配置高度
	// 	float sampleX = origin.x + posX;
	// 	float sampleY = origin.z + posZ;
	// 	float noiseHeight = noise.Get(sampleX, sampleY);
	// 	float intensity = meshCurve.Evaluate(noiseHeight);
	// 	float posY = Mathf.Lerp(0, meshHeight, intensity * noiseHeight);
	// 	Vector3 vector = new Vector3(posX, posY, posZ);
	// 	// 展开UV
	// 	float uvX = x / (float)(MeshWide - 1);
	// 	float uvY = z / (float)(MeshHigh - 1);
	// 	Vector2 uv = new Vector2(uvX, uvY);
	// 	meshData.AddVertex(vector, uv);
	// 	// 构建三角形
	// 	if (x < MeshWide - 1 && z < MeshHigh - 1) { meshData.AddTriangle(MeshWide); }
	// 	meshData.vertexIndex++;
	// }
	#endregion
}
