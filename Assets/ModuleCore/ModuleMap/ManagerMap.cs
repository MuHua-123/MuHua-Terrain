using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 地图 - 管理器
/// </summary>
public class ManagerMap : ModuleSingle<ManagerMap> {

	/// <summary> 地图 </summary>
	public Map map;
	/// <summary> 地图类型 </summary>
	public MapType mapType;
	/// <summary> 地图创建 </summary>
	public static event Action OnCreate;
	/// <summary> 地图更改 </summary>
	// public static event Action<MapUnit> OnChange;

	protected override void Awake() => NoReplace(false);

	public void CreateMapSquare(int wide, int high, float size) {
		mapType = MapType.Square;
		Vector3 originPosition = SquareTool.CenterPoint(wide, high, size);
		map = new MapSquare(wide, high, size, originPosition, true);
		map.Loop(CreateVisualUnit);
		OnCreate?.Invoke();
	}
	public void CreateMapHexagon(int wide, int high, float size) {
		mapType = MapType.Hexagon;
		Vector3 originPosition = HexTool.CenterPoint(wide, high, size);
		map = new MapHexagon(wide, high, size, originPosition);
		map.Loop(CreateVisualUnit);
		OnCreate?.Invoke();
	}
	private void CreateVisualUnit(int x, int y) {
		MapUnit mapUnit = map[x, y];
		mapUnit.mapSpace = new BuildingSpace();
		MapTile mapTile = ModuleVisual.I.GeneratorMapUnit.CreateVisual(null);
		mapTile.Settings(mapUnit);
	}

	/// <summary> 世界坐标转换地图坐标 </summary>
	public static bool TryWorldPosition(Vector3 worldPosition, out Vector3 position) {
		return I.map.TryWorldPosition(worldPosition, out position);
	}
	/// <summary> 世界坐标 </summary>
	public static bool TryWorldPosition(Vector2Int xy, out Vector3 position) {
		return I.map.TryWorldPosition(xy, out position);
	}
	/// <summary> 获取地图单元 </summary>
	public static bool TryMapUnit(Vector3 worldPosition, out MapUnit unit) {
		return I.map.TryMapUnit(worldPosition, out unit);
	}
	/// <summary> 查询地图路径 </summary>
	public static bool FindPath(Vector3 sp, Vector3 ep, out List<Vector3> vectorPath) {
		return I.map.FindPath(sp, ep, out vectorPath);
	}
}
/// <summary>
/// 地图类型
/// </summary>
public enum MapType {
	/// <summary> 正方形 </summary>
	Square,
	/// <summary> 六边形 </summary>
	Hexagon
}