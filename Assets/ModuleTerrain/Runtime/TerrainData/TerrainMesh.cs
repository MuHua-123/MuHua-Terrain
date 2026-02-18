using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形网格
/// </summary>
public class TerrainMesh : ScriptableObject {
	/// <summary> 网格 </summary>
	public Mesh mesh;
	/// <summary> 宽 </summary>
	public int wide;
	/// <summary> 高 </summary>
	public int high;
	/// <summary> 规模 </summary>
	public float scale;
	/// <summary> 高度 </summary>
	public float height = 200;
	/// <summary> 原点 </summary>
	public Vector3 origin;
	/// <summary> 曲线 </summary>
	public AnimationCurve curve;
	/// <summary> 顶点索引 </summary>
	[HideInInspector] public int vertexIndex;
	/// <summary> 三角形索引 </summary>
	[HideInInspector] public int triangleIndex;
	/// <summary> 三角形 </summary>
	[HideInInspector] public int[] triangles;
	/// <summary> UV </summary>
	[HideInInspector] public Vector2[] uvs;
	/// <summary> 顶点 </summary>
	[HideInInspector] public Vector3[] vertices;

	/// <summary> 初始网格 </summary>
	public void Initial(int wide, int high, float scale, float height, AnimationCurve curve) {
		this.wide = wide;
		this.high = high;
		this.scale = scale;
		this.height = height;
		this.curve = curve;
	}
	/// <summary> 初始网格 </summary>
	public void Initial(TerrainNoise noise, Vector3 origin) {
		this.origin = origin;
		// 初始索引
		vertexIndex = 0;
		triangleIndex = 0;
		// 网格数组
		vertices = new Vector3[wide * high];
		uvs = new Vector2[wide * high];
		triangles = new int[(wide - 1) * (high - 1) * 6];
		// 构建数据
		for (int z = 0; z < high; z++)
			for (int x = 0; x < wide; x++)
				AddVertex(noise, origin, x, z);
	}
	/// <summary> 获取网格 </summary> 
	public Mesh Get() {
		// 生成网格
		mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		return mesh;
	}

	/// <summary> 添加顶点 </summary> 
	private void AddVertex(TerrainNoise noise, Vector3 origin, int x, int z) {
		// 配置顶点
		float posX = x * scale;
		float posZ = z * scale;
		// 配置高度
		float sampleX = origin.x + posX;
		float sampleY = origin.z + posZ;
		float noiseHeight = noise.Get(sampleX, sampleY);
		float intensity = curve.Evaluate(noiseHeight);
		float posY = Mathf.Lerp(0, height, intensity * noiseHeight);
		vertices[vertexIndex] = new Vector3(posX, posY, posZ);
		// 展开UV
		float uvX = x / (float)(wide - 1);
		float uvY = z / (float)(high - 1);
		uvs[vertexIndex] = new Vector2(uvX, uvY);
		// 构建三角形
		if (x < wide - 1 && z < high - 1) { AddTriangle(wide); }
		vertexIndex++;
	}
	/// <summary> 添加三角形 </summary> 
	private void AddTriangle(int wide) {
		int a = vertexIndex;
		int b = vertexIndex + 1;
		int c = vertexIndex + wide;
		int d = vertexIndex + wide + 1;
		AddTriangle(a, c, d);
		AddTriangle(a, d, b);
	}
	/// <summary> 添加三角形 </summary> 
	private void AddTriangle(int a, int b, int c) {
		triangles[triangleIndex] = a;
		triangles[triangleIndex + 1] = b;
		triangles[triangleIndex + 2] = c;
		triangleIndex += 3;
	}
}
