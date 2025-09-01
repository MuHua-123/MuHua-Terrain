using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 建筑系统
/// </summary>
public class BuildingSystem : ModuleSingle<BuildingSystem> {

	private Building building;

	protected override void Awake() => NoReplace(false);

	private void Update() {
		if (building == null) { return; }
		TryWorldPosition(out Vector3 position);
		Vector3 v3 = building.transform.position;
		building.transform.position = Vector3.Lerp(v3, position, Time.deltaTime * 10);
	}

	/// <summary> 设置 </summary>
	public void Settings(Building original) {
		if (original == null) { Cancel(); return; }
		ModuleVisual.I.GeneratorBuilding.UpdateVisual(ref building, original.transform);
		TryWorldPosition(out Vector3 position);
		building.transform.position = position;
	}
	/// <summary> 旋转 </summary>
	public void Rotate(float angle) {
		if (building == null) { return; }
		building.Rotate(angle);
	}
	/// <summary> 建造 </summary>
	public void Build() {
		// 检查建筑是否可用
		if (building == null) { return; }
		// 判断是否在地图范围内
		if (!TryMapUnit(building.Occupy(), out List<MapUnit> mapUnits)) { return; }
		// 生成建筑
		ManagerMap.TryWorldPosition(mapUnits[0].xy, out Vector3 position);
		Building temp = ModuleVisual.I.GeneratorBuilding.CreateVisual(building.transform);
		temp.transform.position = position;
		temp.Initial();
		// 写入建筑空间
		RecordBuild(mapUnits, temp);
	}
	/// <summary> 取消 </summary>
	public void Cancel() {
		ModuleVisual.I.GeneratorBuilding.ReleaseVisual(building);
	}

	/// <summary> 获取地图格子坐标 </summary>
	private bool TryWorldPosition(out Vector3 position) {
		position = transform.position;
		if (!RayTool.GetMouseToWorldPosition(out Vector3 mousePosition)) { return false; }
		return ManagerMap.TryWorldPosition(mousePosition, out position);
	}
	/// <summary> 判断建筑空间 </summary>
	private bool TryMapUnit(List<Vector3> positions, out List<MapUnit> mapUnits) {
		mapUnits = new List<MapUnit>();
		foreach (var position in positions) {
			if (!TryMapUnit(position, out MapUnit mapUnit)) { return false; }
			mapUnits.Add(mapUnit);
		}
		return true;
	}
	/// <summary> 判断建筑空间 </summary>
	private bool TryMapUnit(Vector3 position, out MapUnit mapUnit) {
		mapUnit = null;
		// 判断是否在地图范围内
		if (!ManagerMap.TryMapUnit(position, out MapUnit unit)) { return false; }
		// 判断建筑空间
		if (!(unit.mapSpace is BuildingSpace buildingSpace)) { return false; }
		// 判断是否有建筑
		if (buildingSpace.building != null) { return false; }
		return true;
	}
	/// <summary> 记录建筑 </summary>
	private void RecordBuild(List<MapUnit> mapUnits, Building building) {
		mapUnits.ForEach(mapUnit => RecordBuild(mapUnit, building));
	}
	/// <summary> 记录建筑 </summary>
	private void RecordBuild(MapUnit mapUnit, Building building) {
		if (!(mapUnit.mapSpace is BuildingSpace buildingSpace)) { return; }
		buildingSpace.building = building.transform;
	}
}
