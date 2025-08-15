using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;
using System;

/// <summary>
/// 地图指示
/// </summary>
public class MapIndicate : MonoBehaviour {
	/// <summary> 正方形 </summary>
	public GameObject square;
	/// <summary> 六边形 </summary>
	public GameObject hexagon;

	private GameObject indicate;

	private void Awake() {
		ManagerMap.OnCreate += ManagerMap_OnCreate;
	}
	private void OnDestroy() {
		ManagerMap.OnCreate -= ManagerMap_OnCreate;
	}

	private void ManagerMap_OnCreate() {
		square.SetActive(false);
		hexagon.SetActive(false);
		if (ManagerMap.I.mapType == MapType.Square) { indicate = square; }
		if (ManagerMap.I.mapType == MapType.Hexagon) { indicate = hexagon; }
	}

	private void Update() {
		bool isShow = TryWorldPosition(out Vector3 position);
		transform.position = position;
		indicate.SetActive(indicate);
	}

	/// <summary> 获取地图格子坐标 </summary>
	private bool TryWorldPosition(out Vector3 position) {
		position = transform.position;
		if (!RayTool.GetMouseToWorldPosition(out Vector3 mousePosition)) { return false; }
		return ManagerMap.TryWorldPosition(mousePosition, out position);
	}
}
