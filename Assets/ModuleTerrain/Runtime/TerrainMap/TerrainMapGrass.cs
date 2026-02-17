using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// 地形图 - 草地
/// </summary>
[CreateAssetMenu(fileName = "TerrainMapGrass", menuName = "MuHua/地形/地形纹理/草地")]
public class TerrainMapGrass : TerrainMap {

	public override Texture2D Get(TerrainMeshData meshData) {
		Texture2D texture = new Texture2D(wide, high);
		return Get(texture, meshData);
	}

	public override Texture2D Get(Texture2D texture, TerrainMeshData meshData) {
		int wide = texture.width;
		int high = texture.height;
		// 创建数据数组
		NativeArray<float> alphas = new NativeArray<float>(wide * high, Allocator.TempJob);
		NativeArray<Vector3> normals = new NativeArray<Vector3>(meshData.normals, Allocator.TempJob);
		// 创建并调度并行Job
		ParallelJob job = new ParallelJob {
			wide = wide,
			high = high,
			meshWide = meshData.wide,
			meshHigh = meshData.high,
			meshScale = meshData.scale,
			normals = normals,
			alphas = alphas,
		};
		// 64是批处理大小
		JobHandle handle = job.Schedule(alphas.Length, 64);
		// 等待Job完成
		handle.Complete();
		// 获取颜色
		Color32[] colors = new Color32[wide * high];
		for (int i = 0; i < alphas.Length; i++) {
			colors[i] = new Color(0, 0, 0, alphas[i]);
		}
		// 释放内存
		normals.Dispose();
		alphas.Dispose();
		// 构建纹理（确保贴图保持可读，因为我们会把它作为 sub-asset 存储并在后面更新）
		texture.SetPixels32(colors);
		// 不要在这里设置 makeNoLongerReadable = true，否则被保存为 sub-asset 后就无法再次写入
		texture.Apply(updateMipmaps: false, makeNoLongerReadable: false);
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
		public NativeArray<float> alphas;

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
			alphas[index] = Mathf.Clamp01(alpha);
		}
	}
}
