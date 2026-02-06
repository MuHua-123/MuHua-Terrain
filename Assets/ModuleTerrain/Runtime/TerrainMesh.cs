using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形网格
/// </summary>
public class TerrainMesh {
	/// <summary> 大小 </summary>
	public Vector3Int scale;
	/// <summary> 细分 </summary>
	public int details;

	/// <summary> 最大X宽索引 </summary>
	public int maxX;
	/// <summary> 最大Z索引 </summary>
	public int maxZ;
	/// <summary> 宽 </summary>
	public int wide;
	/// <summary> 高 </summary>
	public int high;
	/// <summary> 高 </summary>
	private Vector3 origin;
	/// <summary> 顶点索引 </summary>
	private int vertexIndex;
	/// <summary> 三角形索引 </summary>
	private int triangleIndex;
	/// <summary> 顶点 </summary>
	private Vector3[] vertices;
	/// <summary> UV </summary>
	private Vector2[] uvs;
	/// <summary> 三角形 </summary>
	private int[] triangles;
	/// <summary> 高度图 </summary>
	private float[,] heightMap;

	public TerrainMesh(Vector3Int scale, int details) {
		this.scale = scale;
		this.details = details;
		maxX = scale.x * details + 1;
		maxZ = scale.z * details + 1;
		origin = new Vector3(scale.x, 0, scale.z) * -0.5f;
		// 网格数组
		vertices = new Vector3[maxX * maxZ];
		uvs = new Vector2[maxX * maxZ];
		triangles = new int[(maxX - 1) * (maxZ - 1) * 6];
	}

	/// <summary> 生成网格 </summary>
	public Mesh GenerateMesh(float[,] heightMap) {
		this.heightMap = heightMap;

		vertexIndex = 0;
		triangleIndex = 0;
		Loop(AddVertex);
		return GenerateMesh();
	}
	/// <summary> 生成网格 </summary>
	public Mesh GenerateMesh() {
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		return mesh;
	}

	/// <summary> 循环 </summary>
	private void Loop(Action<int, int> action) {
		for (int z = 0; z < maxZ; z++) {
			for (int x = 0; x < maxX; x++) { action?.Invoke(x, z); }
		}
	}
	/// <summary> 添加顶点 </summary> 
	private void AddVertex(int x, int z) {
		int mapX = x / details;
		int mapY = z / details;

		float posX = x / (float)details + origin.x;
		float posY = heightMap[mapX, mapY] * 2;
		float posZ = z / (float)details + origin.z;
		Vector3 position = new Vector3(posX, posY, posZ);
		vertices[vertexIndex] = position;

		float uvX = x / (float)(maxX - 1);
		float uvY = z / (float)(maxZ - 1);
		uvs[vertexIndex] = new Vector2(uvX, uvY);

		if (x < maxX - 1 && z < maxZ - 1) { AddTriangle(); }

		vertexIndex++;
	}
	/// <summary> 添加三角形 </summary> 
	private void AddTriangle() {
		int a = vertexIndex;
		int b = vertexIndex + 1;
		int c = vertexIndex + maxX;
		int d = vertexIndex + maxX + 1;
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
