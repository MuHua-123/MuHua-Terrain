using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 资源管理器
/// </summary>
public class AssetsManager : ModuleSingle<AssetsManager> {

	[Header("建筑")]
	/// <summary> 正方形 </summary>
	public List<Building> squares;
	/// <summary> 六边形 </summary>
	public List<Building> hexagons;

	protected override void Awake() => NoReplace(false);

	public Building FindBuilding(int index) {
		bool isSquare = ManagerMap.I.mapType == MapType.Square;
		List<Building> buildings = isSquare ? squares : hexagons;
		index = index % buildings.Count;
		return buildings[index];
	}
}
