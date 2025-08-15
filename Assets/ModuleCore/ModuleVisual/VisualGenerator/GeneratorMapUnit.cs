using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图单元 - 生成器
/// </summary>
public class GeneratorMapUnit : VisualGenerator<MapTile> {

	/// <summary> 正方预制 </summary>
	public Transform square;
	/// <summary> 六角预制 </summary>
	public Transform hexagon;

	public override MapTile CreateVisual(Transform original) {
		if (original == null) {
			if (ManagerMap.I.mapType == MapType.Square) { original = square; }
			if (ManagerMap.I.mapType == MapType.Hexagon) { original = hexagon; }
		}
		return Create<MapTile>(original, transform);
	}
	public override void UpdateVisual(ref MapTile visual, Transform original) {
		ReleaseVisual(visual);
		visual = CreateVisual(original);
	}
	public override void ReleaseVisual(MapTile visual) {
		if (visual != null) { Destroy(visual.gameObject); }
	}
	public override void ReleaseAllVisual() {
		foreach (Transform item in transform) { Destroy(item.gameObject); }
	}
}
