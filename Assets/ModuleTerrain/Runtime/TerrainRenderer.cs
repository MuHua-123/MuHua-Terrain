using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形渲染器
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class TerrainRenderer : MonoBehaviour {
	/// <summary> 网格过滤器 </summary>
	public MeshFilter meshFilter;
	/// <summary> 地形数据 </summary>
	public TerrainData terrainData;
	/// <summary> 地形图 </summary>
	public TerrainMap terrainMap;

	private void Reset() {
		name = Guid.NewGuid().ToString("N");
		meshFilter = GetComponent<MeshFilter>();
	}
}
