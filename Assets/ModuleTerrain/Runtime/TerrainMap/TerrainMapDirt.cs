using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 泥地 - 地形图
/// </summary>
[CreateAssetMenu(fileName = "TerrainMapDirt", menuName = "MuHua/地形/地形纹理/泥地")]
public class TerrainMapDirt : TerrainMap {
	[Header("纹理")]
	/// <summary> 宽 </summary>
	public int wide = 512;
	/// <summary> 高 </summary>
	public int high = 512;

	[Header("材质")]
	/// <summary> 着色器 </summary>
	public Shader shader;
	/// <summary> 主纹理 </summary>
	public Texture2D MainTex;
	/// <summary> 法线 </summary>
	public Texture2D Normal;
	/// <summary> 偏移 </summary>
	public Vector2 Scale;

	public override Material Get(Texture2D mask, Material material) {
		if (material == null) { material = new Material(shader); }
		material.SetFloat("_Surface", 0);
		material.SetTexture("_Mask", mask);
		material.SetTexture("_MainTex", MainTex);
		material.SetTextureScale("_MainTex", Scale);
		material.SetTexture("_Normal", Normal);
		material.SetTextureScale("_Normal", Scale);
		return material;
	}

	public override Texture2D Get(TerrainMesh meshData, Texture2D texture = null) {
		if (texture == null) { texture = new Texture2D(this.wide, this.high); }
		int wide = texture.width;
		int high = texture.height;
		Color32[] colors = new Color32[wide * high];
		for (int x = 0; x < wide; x++) {
			for (int y = 0; y < high; y++)
				colors[y * wide + x] = new Color(0, 0, 0, 1);
		}
		texture.SetPixels32(colors);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		return texture;
	}
}
