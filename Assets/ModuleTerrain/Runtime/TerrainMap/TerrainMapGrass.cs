using System.Collections;
using System.Collections.Generic;
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
		// 构建数据
		Color32[] colors = new Color32[wide * high];
		for (int y = 0; y < high; y++)
			for (int x = 0; x < wide; x++)
				colors[y * wide + x] = Calculate(meshData, x, y);
		// 构建纹理
		texture.SetPixels32(colors);
		texture.Apply(false, true);
		return texture;
	}

	/// <summary> 计算 </summary> 
	private Color Calculate(TerrainMeshData meshData, float x, float y) {
		Vector2 vector = new Vector2(x / wide, y / high);
		Vector3 normals = meshData.Normals(vector);
		float alpha = (normals.y - 0.8f) * 8;
		return new Color(0, 0, 0, Mathf.Clamp01(alpha));
	}
}
