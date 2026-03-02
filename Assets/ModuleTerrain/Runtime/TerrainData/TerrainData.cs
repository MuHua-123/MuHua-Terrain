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

	[Header("地形图")]
	/// <summary> 地形图 </summary>
	public List<TerrainMap> terrainMaps;

	/// <summary> 生成噪点 </summary> 
	public TerrainNoise GenerateNoise() {
		return new TerrainNoise(seed, frequency, amplitude, noiseScale, octaves);
	}
	/// <summary> 生成网格 </summary> 
	public TerrainMesh GenerateMesh() {
		TerrainMesh meshData = CreateInstance(typeof(TerrainMesh)) as TerrainMesh;
		meshData.Initial(meshSize.x + 1, meshSize.y + 1, meshScale, meshHeight, meshCurve);
		return meshData;
	}
}
