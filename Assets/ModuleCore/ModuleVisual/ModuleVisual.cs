using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuHua;

/// <summary>
/// 可视化模块
/// </summary>
public class ModuleVisual : ModuleSingle<ModuleVisual> {

	// [Header("控制器")]
	// public VisualController<DataPattern> Pattern;
	// public VisualController<DataPatternUnit> PatternUnit;
	// public VisualController<DataFashionPart> FashionPart;
	// public VisualController<DataFashionUnit> FashionUnit;

	[Header("生成器")]
	public VisualGenerator<MonoMapUnit> GeneratorMapUnit;
	// public VisualGenerator<MonoOrnament> Ornament;
	// public VisualGenerator<MonoLighting> Lighting;
	// public VisualGenerator<MonoSymbol> Symbol;

	protected override void Awake() => NoReplace();

}
