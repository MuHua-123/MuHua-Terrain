using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图单元 - 生成器
/// </summary>
public class GeneratorMapUnit : VisualGenerator<MonoMapUnit> {

	public override MonoMapUnit CreateVisual(Transform original) {
		return Create<MonoMapUnit>(original, transform);
	}
	public override void UpdateVisual(ref MonoMapUnit visual, Transform original) {
		ReleaseVisual(visual);
		visual = CreateVisual(original);
	}
	public override void ReleaseVisual(MonoMapUnit visual) {
		if (visual != null) { Destroy(visual.gameObject); }
	}
	public override void ReleaseAllVisual() {
		foreach (Transform item in transform) { Destroy(item.gameObject); }
	}
}
