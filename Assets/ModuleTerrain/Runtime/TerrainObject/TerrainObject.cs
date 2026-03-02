using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形对象
/// </summary>
public class TerrainObject : MonoBehaviour {

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

	[Header("区块")]
	/// <summary> 大小 </summary>
	public Vector2Int blockSize = new Vector2Int(10, 10);
	/// <summary> 规模 </summary>
	public float blockScale = 1;

	[Header("组件")]
	/// <summary> 网格渲染器 </summary>
	public MeshRenderer meshRenderer;

	public void Generate() {
		TerrainNoise noise = new TerrainNoise(seed, frequency, amplitude, noiseScale, octaves);

		int wide = blockSize.x;
		int high = blockSize.y;

		Texture2D texture = new Texture2D(wide, high);
		Color32[] colors = new Color32[wide * high];

		for (int x = 0; x < wide; x++) {
			for (int y = 0; y < high; y++) {
				float sampleX = x * blockScale - transform.position.x;
				float sampleY = y * blockScale - transform.position.z;
				float value = noise.Get(sampleX, sampleY);
				colors[y * wide + x] = Color.Lerp(Color.black, Color.white, value);
			}
		}
		texture.SetPixels32(colors);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.filterMode = FilterMode.Point;
		texture.Apply();

		meshRenderer.material.mainTexture = texture;
	}
}
