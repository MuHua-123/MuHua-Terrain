using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 地图瓦片
/// </summary>
public class MapTile : MonoBehaviour {

	public void Settings(MapUnit unit) {
		ManagerMap.TryWorldPosition(unit.xy, out Vector3 position);
		transform.position = position;
	}

}
