using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;
using TMPro;

/// <summary>
/// 地图瓦片
/// </summary>
public class MapTile : MonoBehaviour {

	public TMP_Text gText;
	public TMP_Text hText;
	public TMP_Text fText;
	public TMP_Text xyText;

	private MapUnit unit;

	public void Settings(MapUnit unit) {
		this.unit = unit;
		ManagerMap.TryWorldPosition(unit.xy, out Vector3 position);
		transform.position = position;
	}

	private void LateUpdate() {
		gText.text = Mathf.Abs(unit.GCost) < 10000 ? unit.GCost.ToString() : "0";
		hText.text = Mathf.Abs(unit.HCost) < 10000 ? unit.HCost.ToString() : "0";
		fText.text = Mathf.Abs(unit.FCost) < 10000 ? unit.FCost.ToString() : "0";
		xyText.text = unit.xy.ToString();
	}
}
