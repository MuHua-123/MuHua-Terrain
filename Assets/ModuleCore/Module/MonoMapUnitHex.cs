using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 六边形 - 地图单元
/// </summary>
public class MonoMapUnitHex : MonoMapUnit {
	public override void Settings(MapUnit unit) {
		ManagerMap.TryWorldPosition(unit.xy, out Vector3 position);
		transform.position = position;
	}
}
