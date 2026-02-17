using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形网格数据
/// </summary>
public class TerrainMeshData {
	/// <summary> 网格 </summary>
	public Mesh mesh;

	/// <summary> 宽 </summary>
	[HideInInspector] public int wide;
	/// <summary> 高 </summary>
	[HideInInspector] public int high;
	/// <summary> 规模 </summary>
	[HideInInspector] public float scale;
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
	/// <summary> 法线 </summary>
	[HideInInspector] public Vector3[] normals;

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
}
