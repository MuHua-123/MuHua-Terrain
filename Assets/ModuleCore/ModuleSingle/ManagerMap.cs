using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 地图 - 管理器
/// </summary>
public class ManagerMap : ModuleSingle<ManagerMap> {
	/// <summary> 单元大小 </summary>
	public float size;
	/// <summary> 地图大小 </summary>
	public Vector2Int widthHeight;
	/// <summary> 单元预制 </summary>
	public Transform prefab;

	private Map map;

	protected override void Awake() => NoReplace();

	private void Start() => CreateMapHexagon();

	private void CreateMapGrid() {
		Vector3 originPosition = GridTool.CenterPoint(widthHeight, size);
		map = new MapGrid(widthHeight.x, widthHeight.y, size, originPosition, true);
		map.Loop(CreateVisualUnit);
	}
	private void CreateMapHexagon() {
		Vector3 originPosition = HexTool.CenterPoint(widthHeight, size);
		map = new MapHexagon(widthHeight.x, widthHeight.y, size, originPosition);
		map.Loop(CreateVisualUnit);
	}
	private void CreateVisualUnit(int x, int y) {
		MonoMapUnit monoMapUnit = ModuleVisual.I.GeneratorMapUnit.CreateVisual(prefab);
		monoMapUnit.Settings(map[x, y]);
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
