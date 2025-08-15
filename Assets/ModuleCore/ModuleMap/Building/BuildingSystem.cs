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

	/// <summary> 设置 </summary>
	public void Settings(Building building) {
		this.building = building;
	}
	/// <summary> 建造 </summary>
	public bool Build(Vector3 worldPosition) {
		// 判断是否在地图范围内
		if (!ManagerMap.TryMapUnit(worldPosition, out MapUnit unit)) { return false; }
		// 判断建筑空间
		if (!(unit.mapSpace is BuildingSpace buildingSpace)) { return false; }
		// 判断是否有建筑
		if (buildingSpace.building != null) { return false; }
		Building temp = ModuleVisual.I.GeneratorBuilding.CreateVisual(building.transform);
		ManagerMap.TryWorldPosition(unit.xy, out Vector3 position);
		temp.Settings(position);
		buildingSpace.building = temp.transform;
		return true;
	}
	/// <summary> 取消 </summary>
	public void Cancel() {

	}
}
