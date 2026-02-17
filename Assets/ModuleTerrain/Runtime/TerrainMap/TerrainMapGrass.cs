using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// 草地 - 地形图
/// </summary>
[CreateAssetMenu(fileName = "TerrainMapGrass", menuName = "MuHua/地形/地形纹理/草地")]
public class TerrainMapGrass : TerrainMap {
	/// <summary> 宽 </summary>
	public int wide = 512;
	/// <summary> 高 </summary>
	public int high = 512;

	[Header("材质")]
	/// <summary> 着色器 </summary>
	public Shader shader;
	/// <summary> 主纹理 </summary>
	public Texture2D MainTex;
	/// <summary> 法线 </summary>
	public Texture2D Normal;
	/// <summary> 偏移 </summary>
	public Vector2 Scale;

	public override Material Get(Texture2D mask, Material material) {
		if (material == null) { material = new Material(shader); }
		material.SetTexture("_Mask", mask);
		material.SetTexture("_MainTex", MainTex);
		material.SetTextureScale("_MainTex", Scale);
		material.SetTexture("_Normal", Normal);
		material.SetTextureScale("_Normal", Scale);
		return material;
	}

	public override Texture2D Get(TerrainMeshData meshData, Texture2D texture = null) {
		if (texture == null) { texture = new Texture2D(this.wide, this.high); }
		int wide = texture.width;
		int high = texture.height;
		// 创建数据数组
		NativeArray<Color32> colors = new NativeArray<Color32>(wide * high, Allocator.TempJob);
		NativeArray<Vector3> normals = new NativeArray<Vector3>(meshData.normals, Allocator.TempJob);
		// 创建并调度并行Job
		ParallelJob job = new ParallelJob {
			wide = wide,
			high = high,
			meshWide = meshData.wide,
			meshHigh = meshData.high,
			meshScale = meshData.scale,
			normals = normals,
			colors = colors,
		};
		// 64是批处理大小
		JobHandle handle = job.Schedule(colors.Length, 64);
		// 等待Job完成
		handle.Complete();
		// 写入纹理
		texture.SetPixels32(colors.ToArray());
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		// 释放内存
		normals.Dispose();
		colors.Dispose();
		return texture;
	}

	public struct ParallelJob : IJobParallelFor {
		/// <summary> 宽 </summary>
		[ReadOnly] public int wide;
		/// <summary> 高 </summary>
		[ReadOnly] public int high;
		/// <summary> 网格宽 </summary>
		[ReadOnly] public int meshWide;
		/// <summary> 网格高 </summary>
		[ReadOnly] public int meshHigh;
		/// <summary> 网格规模 </summary>
		[ReadOnly] public float meshScale;
		/// <summary> 网格法线 </summary>
		[ReadOnly] public NativeArray<Vector3> normals;
		/// <summary> 返回结果 </summary>
		public NativeArray<Color32> colors;

		public void Execute(int index) {
			int x = index % wide;
			int y = index / wide;
			Vector2 vector = new Vector2((float)x / wide, (float)y / high);
			// 网格位置采样
			float sampleX = Mathf.Lerp(0, (meshWide - 1) * meshScale, vector.x);
			float sampleY = Mathf.Lerp(0, (meshHigh - 1) * meshScale, vector.y);
			int meshX = Mathf.RoundToInt(sampleX / meshScale);
			int meshY = Mathf.RoundToInt(sampleY / meshScale);
			int meshIndex = (meshY * meshWide) + meshX;
			// 获取法线
			Vector3 n = normals[meshIndex];
			// 取值
			float alpha = (n.y - 0.8f) * 8;
			colors[index] = new Color(0, 0, 0, Mathf.Clamp01(alpha));
		}
	}
}
