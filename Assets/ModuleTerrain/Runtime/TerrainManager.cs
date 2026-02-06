using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 地形管理器
/// </summary>
public class TerrainManager : ModuleSingle<TerrainManager> {

	[Header("网格")]
	/// <summary> 网格大小 </summary>
	public Vector3Int meshScale = new Vector3Int(1, 1, 1);
	/// <summary> 网格细分 </summary>
	public int details = 2;

	[Header("噪点")]
	/// <summary> 种子 </summary>
	public int seed;
	/// <summary> 偏移X </summary>
	public float offsetX;
	/// <summary> 偏移Y </summary>
	public float offsetY;
	/// <summary> 宽 </summary>
	public int wide = 100;
	/// <summary> 高 </summary>
	public int high = 100;
	/// <summary> 规模 </summary>
	public float noiseScale = 30;
	/// <summary> 梯度 </summary>
	public int octaves = 4;
	/// <summary> 连续性 </summary>
	public float persistance = 0.5f;
	/// <summary> 间隙 </summary>
	public float lacunarity = 1.87f;

	[Header("组件")]
	/// <summary> 网格过滤器 </summary>
	public MeshFilter meshFilter;
	/// <summary> 网格渲染器 </summary>
	public MeshRenderer meshRenderer;

	protected override void Awake() => NoReplace();

	/// <summary> 生成 </summary>
	public void Generate() {
		Vector2 v1 = new Vector2(offsetX, offsetY);
		TerrainNoise perlinNoise = new TerrainNoise(wide, high, noiseScale, octaves, persistance, lacunarity);
		meshRenderer.material.mainTexture = perlinNoise.GenerateTexture(seed, v1);

		TerrainMesh mesh = new TerrainMesh(meshScale, details);
		meshFilter.mesh = mesh.GenerateMesh(perlinNoise.GenerateNoiseMap(seed, v1));
	}
}
