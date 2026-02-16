using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形图
/// </summary>
public abstract class TerrainMap : ScriptableObject {
	/// <summary> 宽 </summary>
	public int wide = 512;
	/// <summary> 高 </summary>
	public int high = 512;

	/// <summary> 获取纹理 </summary> 
	public abstract Texture2D Get(TerrainMeshData meshData);
	/// <summary> 获取纹理 </summary> 
	public abstract Texture2D Get(Texture2D texture, TerrainMeshData meshData);
}
