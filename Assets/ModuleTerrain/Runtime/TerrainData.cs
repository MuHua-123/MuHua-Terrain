using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形数据
/// </summary>
[CreateAssetMenu(fileName = "TerrainData", menuName = "MuHua/地形/地形数据")]
public class TerrainData : ScriptableObject {

	[Header("网格")]
	/// <summary> 宽 </summary>
	public int wide = 250;
	/// <summary> 高 </summary>
	public int high = 250;
	/// <summary> 规模</summary>
	public float scale = 4;
	/// <summary> 海拔高度</summary>
	public float altitude = 200;
	/// <summary> 曲线</summary>
	public AnimationCurve curve;

	[Header("噪点")]
	/// <summary> 种子 </summary>
	public int seed;
	/// <summary> 噪点规模 </summary>
	public float noiseScale = 250f;
	/// <summary> 频率 </summary>
	public float frequency = 4;
	/// <summary> 振幅 </summary>
	public float amplitude = 0.1f;
	/// <summary> 梯度 </summary>
	public int octaves = 4;

	/// <summary> 生成噪点 </summary> 
	public TerrainNoise GenerateNoise() {
		return new TerrainNoise(frequency, amplitude, noiseScale, octaves);
	}
	/// <summary> 生成网格 </summary> 
	public TerrainMesh GenerateMesh() {
		return new TerrainMesh(wide, high, scale, altitude, curve);
	}
}
