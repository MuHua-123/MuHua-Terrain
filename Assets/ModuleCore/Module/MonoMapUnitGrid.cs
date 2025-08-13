using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 格子 - 地图单元
/// </summary>
public class MonoMapUnitGrid : MonoMapUnit {
	public override void Settings(MapUnit unit) {
		ManagerMap.TryWorldPosition(unit.xy, out Vector3 position);
		transform.position = position;
	}
}
