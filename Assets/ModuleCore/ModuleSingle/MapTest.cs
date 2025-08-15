using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 全局管理器
/// </summary>
public class MapTest : ModuleSingle<MapTest> {
	[Header("地图")]
	/// <summary> 地图宽 </summary>
	public int wide;
	/// <summary> 地图高 </summary>
	public int high;
	/// <summary> 单元大小 </summary>
	public float size;
	/// <summary> 地图类型 </summary>
	public MapType mapType;
	[Header("建筑")]
	/// <summary> 正方形 </summary>
	public Building square;
	/// <summary> 六边形 </summary>
	public Building hexagon;

	protected override void Awake() {
		NoReplace();
		ManagerMap.OnCreate += ManagerMap_OnCreate;
	}
	private void Start() {
		if (mapType == MapType.Square) { ManagerMap.I.CreateMapSquare(wide, high, size); }
		if (mapType == MapType.Hexagon) { ManagerMap.I.CreateMapHexagon(wide, high, size); }
	}
	private void OnDestroy() {
		ManagerMap.OnCreate -= ManagerMap_OnCreate;
	}

	private void ManagerMap_OnCreate() {
		if (mapType == MapType.Square) { BuildingSystem.I.Settings(square); }
		if (mapType == MapType.Hexagon) { BuildingSystem.I.Settings(hexagon); }
	}
}
