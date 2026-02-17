using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形网格
/// </summary>
[CreateAssetMenu(fileName = "TerrainMesh", menuName = "MuHua/地形/地形网格")]
public class TerrainMesh : ScriptableObject {
	/// <summary> 大小 </summary>
	public Vector2Int size = new Vector2Int(250, 250);
	/// <summary> 规模 </summary>
	public float scale = 4;
	/// <summary> 海拔高度</summary>
	public float altitude = 200;
	/// <summary> 原点 </summary>
	public AnimationCurve curve;

	/// <summary> 宽 </summary>
	public int Wide => size.x + 1;
	/// <summary> 高 </summary>
	public int High => size.y + 1;

	/// <summary> 生成网格 </summary>
	public TerrainMeshData Get(TerrainNoise noise, Vector3 origin) {
		TerrainMeshData meshData = new TerrainMeshData(Wide, High, scale);
		// 构建数据
		for (int z = 0; z < High; z++)
			for (int x = 0; x < Wide; x++)
				AddVertex(meshData, noise, origin, x, z);
		// 生成网格
		return meshData;
	}

	/// <summary> 添加顶点 </summary> 
	private void AddVertex(TerrainMeshData meshData, TerrainNoise noise, Vector3 origin, int x, int z) {
		// 配置顶点
		float posX = x * scale;
		float posZ = z * scale;
		// 配置高度
		float sampleX = origin.x + posX;
		float sampleY = origin.z + posZ;
		float noiseHeight = noise.Get(sampleX, sampleY);
		float intensity = curve.Evaluate(noiseHeight);
		float posY = Mathf.Lerp(0, altitude, intensity * noiseHeight);
		Vector3 vector = new Vector3(posX, posY, posZ);
		// 展开UV
		float uvX = x / (float)(Wide - 1);
		float uvY = z / (float)(High - 1);
		Vector2 uv = new Vector2(uvX, uvY);
		meshData.AddVertex(vector, uv);
		// 构建三角形
		if (x < Wide - 1 && z < High - 1) { meshData.AddTriangle(Wide); }
		meshData.vertexIndex++;
	}
}
