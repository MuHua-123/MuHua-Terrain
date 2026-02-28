using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形噪点
/// </summary>
public class TerrainNoise {
	/// <summary> 频率 </summary>
	public float frequency = 2;
	/// <summary> 振幅 </summary>
	public float amplitude = 0.5f;
	/// <summary> 规模 </summary>
	public float noiseScale = 100f;
	/// <summary> 梯度 </summary>
	public int octaves = 4;
	/// <summary> 增加偏移，防止镜像 </summary>
	public Vector2 offset = new Vector2(100000f, 100000f);
	/// <summary> 种子 </summary>
	public Vector2[] offsets = new Vector2[0];

	public TerrainNoise(int seed, float frequency, float amplitude, float noiseScale, int octaves) {
		this.frequency = frequency;
		this.amplitude = amplitude;
		this.noiseScale = noiseScale;
		this.octaves = octaves;

		offsets = new Vector2[octaves];
		System.Random prng = new System.Random(seed);
		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next(-100000, 100000);
			float offsetY = prng.Next(-100000, 100000);
			offsets[i] = new Vector2(offsetX, offsetY);
		}
	}

	/// <summary> 获取高度 </summary> 
	public float Get(float x, float y) {
		float max = 0;
		float noiseHeight = 0;
		for (int i = 0; i < octaves; i++) {
			noiseHeight += Get(x, y, i, offsets[i]);
			max += Mathf.Pow(amplitude, i);
		}
		return Mathf.InverseLerp(-max, max, noiseHeight);
	}

	/// <summary> 获取高度 </summary>
	private float Get(float x, float y, float p, Vector2 seed) {
		float sampleX = (x + seed.x + offset.x) * Mathf.Pow(frequency, p) / noiseScale;
		float sampleY = (y + seed.y + offset.y) * Mathf.Pow(frequency, p) / noiseScale;
		float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
		return perlinValue * Mathf.Pow(amplitude, p);
	}
}
