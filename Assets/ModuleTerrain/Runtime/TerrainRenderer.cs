using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形渲染器
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainRenderer : MonoBehaviour {
	/// <summary> 地形数据 </summary>
	public TerrainData terrainData;
	/// <summary> 网格过滤器 </summary>
	public MeshFilter meshFilter;
	/// <summary> 网格渲染器 </summary>
	public MeshRenderer meshRenderer;

	private void Reset() {
		name = Guid.NewGuid().ToString("N");
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
	}
}
