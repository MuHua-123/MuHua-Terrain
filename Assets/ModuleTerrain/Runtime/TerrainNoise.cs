using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形柏林噪点
/// </summary>
public class TerrainNoise {
	/// <summary> 宽 </summary>
	public int wide = 100;
	/// <summary> 高 </summary>
	public int high = 100;
	/// <summary> 规模 </summary>
	public float scale = 30;
	/// <summary> 梯度 </summary>
	public int octaves = 4;
	/// <summary> 连续性 </summary>
	public float persistance = 0.5f;
	/// <summary> 间隙 </summary>
	public float lacunarity = 1.87f;

	/// <summary> 一半宽度 </summary>
	private float halfWide;
	/// <summary> 一半高度 </summary>
	private float halfHigh;
	/// <summary> 最小高度 </summary>
	private float minHigh = float.MaxValue;
	/// <summary> 最大高度 </summary>
	private float maxHigh = float.MinValue;
	/// <summary> 梯度采样偏移 </summary>
	private Vector2[] octaveOffsets = new Vector2[0];

	public TerrainNoise(int wide, int high, float scale, int octaves, float persistance, float lacunarity) {
		this.wide = wide;
		this.high = high;
		this.scale = scale;
		this.octaves = octaves;
		this.persistance = persistance;
		this.lacunarity = lacunarity;
	}
	/// <summary> 生成噪点图 </summary> 
	public float[,] GenerateNoiseMap(int seed, Vector2 offset) {
		float[,] noiseMap = new float[wide, high];

		System.Random prng = new System.Random(seed);
		octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next(-100000, 100000) + offset.x;
			float offsetY = prng.Next(-100000, 100000) + offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		if (scale <= 0) { scale = 0.0001f; }

		minHigh = float.MaxValue;
		maxHigh = float.MinValue;

		halfWide = wide / 2f;
		halfHigh = high / 2f;

		Loop((x, y) => noiseMap[x, y] = Generate(x, y));
		Loop((x, y) => noiseMap[x, y] = Mathf.InverseLerp(minHigh, maxHigh, noiseMap[x, y]));
		return noiseMap;
	}
	/// <summary> 生成纹理图 </summary> 
	public Texture2D GenerateTexture(int seed, Vector2 offset) {
		float[,] noiseMap = GenerateNoiseMap(seed, offset);
		// 生成颜色
		Color[] colors = new Color[wide * high];
		Loop((x, y) => colors[y * wide + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]));
		// 生成纹理
		Texture2D texture = new Texture2D(wide, high);
		texture.filterMode = FilterMode.Point;
		texture.SetPixels(colors);
		texture.Apply();
		return texture;
	}
	/// <summary> 循环 </summary>
	public void Loop(Action<int, int> action) {
		for (int y = 0; y < high; y++) {
			for (int x = 0; x < wide; x++) { action?.Invoke(x, y); }
		}
	}

	/// <summary> 生成单个噪点 </summary>  
	private float Generate(int x, int y) {
		float amplitude = 1;
		float frequency = 1;
		float noiseHeight = 0;

		for (int i = 0; i < octaves; i++) {
			float sampleX = (x - halfWide) / scale * frequency + octaveOffsets[i].x;
			float sampleY = (y - halfHigh) / scale * frequency + octaveOffsets[i].y;

			float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
			noiseHeight += perlinValue * amplitude;

			amplitude *= persistance;
			frequency *= lacunarity;
		}

		if (noiseHeight > maxHigh) { maxHigh = noiseHeight; }
		if (noiseHeight < minHigh) { minHigh = noiseHeight; }

		return noiseHeight;
	}
}
