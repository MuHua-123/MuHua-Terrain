using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 建筑 - 生成器
/// </summary>
public class GeneratorBuilding : VisualGenerator<Building> {

	public override Building CreateVisual(Transform original) {
		return Create<Building>(original, transform);
	}
	public override void UpdateVisual(ref Building visual, Transform original) {
		ReleaseVisual(visual);
		visual = CreateVisual(original);
	}
	public override void ReleaseVisual(Building visual) {
		if (visual != null) { Destroy(visual.gameObject); }
	}
	public override void ReleaseAllVisual() {
		foreach (Transform item in transform) { Destroy(item.gameObject); }
	}
}
