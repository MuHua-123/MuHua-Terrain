using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地形图
/// </summary>
public abstract class TerrainMap : ScriptableObject {
	/// <summary> 获取材质 </summary> 
	public abstract Material Get(Texture2D mask, Material material);
	/// <summary> 获取纹理 </summary> 
	public abstract Texture2D Get(TerrainMeshData meshData, Texture2D texture = null);
}
