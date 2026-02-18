using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;
using Den.Tools;

/// <summary>
/// 地形管理器
/// </summary>
public class TerrainManager : ModuleSingle<TerrainManager> {
	/// <summary> 区块大小 </summary>
	public Vector2Int blockSize;

	protected override void Awake() => NoReplace();

	/// <summary> 生成 </summary>
	public void Generate() {

	}
}
