using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形网格
/// </summary>
public class TerrainMesh {
	/// <summary> 宽 </summary>
	public readonly int wide;
	/// <summary> 高 </summary>
	public readonly int high;
	/// <summary> 规模 </summary>
	public readonly float scale;
	/// <summary> 海拔高度</summary>
	public readonly float altitude;
	/// <summary> 原点 </summary>
	public readonly AnimationCurve curve;

	public TerrainMesh(int wide, int high, float scale, float altitude, AnimationCurve curve) {
		this.wide = wide + 1;
		this.high = high + 1;
		this.scale = scale;
		this.altitude = altitude;
		this.curve = curve;
	}

	/// <summary> 生成网格 </summary>
	public TerrainMeshData Get(TerrainNoise noise, Vector3 origin) {
		TerrainMeshData meshData = new TerrainMeshData(wide, high, scale);
		// 构建数据
		for (int z = 0; z < high; z++)
			for (int x = 0; x < wide; x++)
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
		float uvX = x / (float)(wide - 1);
		float uvY = z / (float)(high - 1);
		Vector2 uv = new Vector2(uvX, uvY);
		meshData.AddVertex(vector, uv);
		// 构建三角形
		if (x < wide - 1 && z < high - 1) { meshData.AddTriangle(wide); }
		meshData.vertexIndex++;
	}
}
/// <summary>
/// 地形网格数据
/// </summary>
public class TerrainMeshData {
	/// <summary> 宽 </summary>
	public readonly int wide;
	/// <summary> 高 </summary>
	public readonly int high;
	/// <summary> 规模 </summary>
	public readonly float scale;
	/// <summary> 网格 </summary>
	public Mesh mesh;
	/// <summary> 顶点索引 </summary>
	public int vertexIndex;
	/// <summary> 三角形索引 </summary>
	public int triangleIndex;
	/// <summary> 三角形 </summary>
	public int[] triangles;
	/// <summary> UV </summary>
	public Vector2[] uvs;
	/// <summary> 顶点 </summary>
	public Vector3[] vertices;
	/// <summary> 法线 </summary>
	public Vector3[] normals;

	public TerrainMeshData(int wide, int high, float scale) {
		this.wide = wide;
		this.high = high;
		this.scale = scale;
		// 
		vertexIndex = 0;
		triangleIndex = 0;
		// 网格数组
		vertices = new Vector3[wide * high];
		uvs = new Vector2[wide * high];
		triangles = new int[(wide - 1) * (high - 1) * 6];
	}

	/// <summary> 生成网格 </summary>
	public Mesh Get() {
		// 生成网格
		mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		normals = mesh.normals;
		return mesh;
	}
	/// <summary> 添加顶点 </summary> 
	public void AddVertex(Vector3 vector, Vector2 uv) {
		vertices[vertexIndex] = vector;
		uvs[vertexIndex] = uv;
	}
	/// <summary> 添加三角形 </summary> 
	public void AddTriangle(int wide) {
		int a = vertexIndex;
		int b = vertexIndex + 1;
		int c = vertexIndex + wide;
		int d = vertexIndex + wide + 1;
		AddTriangle(a, c, d);
		AddTriangle(a, d, b);
	}
	/// <summary> 添加三角形 </summary> 
	public void AddTriangle(int a, int b, int c) {
		triangles[triangleIndex] = a;
		triangles[triangleIndex + 1] = b;
		triangles[triangleIndex + 2] = c;
		triangleIndex += 3;
	}
	/// <summary> 法线(0-1) </summary> 
	public Vector3 Normals(Vector2 vector) {
		float sampleX = Mathf.Lerp(0, (wide - 1) * scale, vector.x);
		float sampleY = Mathf.Lerp(0, (high - 1) * scale, vector.y);
		int x = Mathf.RoundToInt(sampleX / scale);
		int y = Mathf.RoundToInt(sampleY / scale);
		int index = (Mathf.RoundToInt(y) * wide) + Mathf.RoundToInt(x);
		return mesh.normals[index];
	}
}